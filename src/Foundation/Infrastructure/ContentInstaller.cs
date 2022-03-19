using EPiServer;
using EPiServer.Authorization;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Enterprise;
using EPiServer.Find.Cms;
using EPiServer.Scheduler;
using EPiServer.Shell.Security;
using EPiServer.Web;
using Foundation.Infrastructure.Cms.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Infrastructure
{
    public class ContentInstaller : IBlockingFirstRequestInitializer
    {
        private readonly UIUserProvider _uIUserProvider;
        private readonly UIRoleProvider _uIRoleProvider;
        private readonly UISignInManager _uISignInManager;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private readonly ContentRootService _contentRootService;
        private readonly IContentRepository _contentRepository;
        private readonly IDataImporter _dataImporter;
        private readonly IScheduledJobExecutor _scheduledJobExecutor;
        private readonly IScheduledJobRepository _scheduledJobRepository;
        private readonly ISettingsService _settingsService;
        private readonly ILanguageBranchRepository _languageBranchRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly EventedIndexingSettings _eventedIndexingSettings;

        public ContentInstaller(UIUserProvider uIUserProvider,
            UISignInManager uISignInManager,
            UIRoleProvider uIRoleProvider,
            ISiteDefinitionRepository siteDefinitionRepository,
            ContentRootService contentRootService,
            IContentRepository contentRepository,
            IDataImporter dataImporter,
            IScheduledJobExecutor scheduledJobExecutor,
            IScheduledJobRepository scheduledJobRepository,
            ISettingsService settingsService,
            ILanguageBranchRepository languageBranchRepository,
            IWebHostEnvironment webHostEnvironment,
            EventedIndexingSettings eventedIndexingSettings)
        {
            _uIUserProvider = uIUserProvider;
            _uISignInManager = uISignInManager;
            _uIRoleProvider = uIRoleProvider;
            _siteDefinitionRepository = siteDefinitionRepository;
            _contentRootService = contentRootService;
            _contentRepository = contentRepository;
            _dataImporter = dataImporter;
            _scheduledJobExecutor = scheduledJobExecutor;
            _scheduledJobRepository = scheduledJobRepository;
            _settingsService = settingsService;
            _languageBranchRepository = languageBranchRepository;
            _webHostEnvironment = webHostEnvironment;
            _eventedIndexingSettings = eventedIndexingSettings;
        }

        public bool CanRunInParallel => false;

        public async Task InitializeAsync(HttpContext httpContext)
        {
            InstallDefaultContent(httpContext);
            _settingsService.InitializeSettings();

            if (await IsAnyUserRegistered())
            {
                return;
            }

            await CreateUser("admin@example.com", "admin@example.com", new[] { Roles.Administrators, Roles.WebAdmins });
        }

        private async Task CreateUser(string username, string email, IEnumerable<string> roles)
        {
            var result = await _uIUserProvider.CreateUserAsync(username, "Episerver123!", email, null, null, true);
            if (result.Status == UIUserCreateStatus.Success)
            {
                foreach (var role in roles)
                {
                    var exists = await _uIRoleProvider.RoleExistsAsync(role);
                    if (!exists)
                    {
                        await _uIRoleProvider.CreateRoleAsync(role);
                    }
                }

                await _uIRoleProvider.AddUserToRolesAsync(result.User.Username, roles);
                var resFromSignIn = await _uISignInManager.SignInAsync(username, "Episerver123!");
            }
        }

        private async Task<bool> IsAnyUserRegistered()
        {
            var res = await _uIUserProvider.GetAllUsersAsync(0, 1).CountAsync();
            return res > 0;
        }

        private void InstallDefaultContent(HttpContext context)
        {
            if (_siteDefinitionRepository.List().Any() || Type.GetType("Foundation.Features.Setup.SetupController, Foundation") != null)
            {
                return;
            }

            var request = context.Request;

            var siteDefinition = new SiteDefinition
            {
                Name = "foundation-mvc-cms",
                SiteUrl = new Uri(request.GetDisplayUrl()),
            };

            siteDefinition.Hosts.Add(new HostDefinition()
            {
                Name = request.Host.Host,
                Type = HostDefinitionType.Primary
            });

            siteDefinition.Hosts.Add(new HostDefinition()
            {
                Name = HostDefinition.WildcardHostName,
                Type = HostDefinitionType.Undefined
            });

            var registeredRoots = _contentRepository.GetItems(_contentRootService.List(), new LoaderOptions());
            var settingsRootRegistered = registeredRoots.Any(x => x.ContentGuid == SettingsFolder.SettingsRootGuid && x.Name.Equals(SettingsFolder.SettingsRootName));

            if (!settingsRootRegistered)
            {
                _contentRootService.Register<SettingsFolder>(SettingsFolder.SettingsRootName + "IMPORT", SettingsFolder.SettingsRootGuid, ContentReference.RootPage);
            }

            CreateSite(new FileStream(Path.Combine(_webHostEnvironment.ContentRootPath, "App_Data", "foundation.episerverdata"),
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read),
                siteDefinition,
                ContentReference.RootPage);

            // RunIndexJob(new Guid("8EB257F9-FF22-40EC-9958-C1C5BA8C2A53"));
        }

        private void RunIndexJob(Guid jobId)
        {
            var job = _scheduledJobRepository.Get(jobId);
            if (job == null)
            {
                return;
            }

            _scheduledJobExecutor.StartAsync(job, new JobExecutionOptions { Trigger = ScheduledJobTrigger.User });
        }

        private void CreateSite(Stream stream, SiteDefinition siteDefinition, ContentReference startPage)
        {
            _eventedIndexingSettings.EventedIndexingEnabled = false;
            _eventedIndexingSettings.ScheduledPageQueueEnabled = false;
            ImportEpiserverContent(stream, startPage, siteDefinition);
            _eventedIndexingSettings.EventedIndexingEnabled = true;
            _eventedIndexingSettings.ScheduledPageQueueEnabled = true;
        }

        public bool ImportEpiserverContent(Stream stream,
            ContentReference destinationRoot,
            SiteDefinition siteDefinition = null)
        {
            var success = false;
            try
            {
                var log = _dataImporter.Import(stream, destinationRoot, new ImportOptions
                {
                    KeepIdentity = true,
                    EnsureContentNameUniqueness = false,
                });

                var status = _dataImporter.Status;

                if (status == null)
                {
                    return false;
                }

                UpdateLanguageBranches(status);
                if (siteDefinition != null && !ContentReference.IsNullOrEmpty(status.ImportedRoot))
                {
                    siteDefinition.StartPage = status.ImportedRoot;
                    _siteDefinitionRepository.Save(siteDefinition);
                    SiteDefinition.Current = siteDefinition;
                    success = true;
                }
            }
            catch (Exception exception)
            {
                success = false;
            }

            return success;
        }

        private void UpdateLanguageBranches(IImportStatus status)
        {
            if (status.ContentLanguages == null)
            {
                return;
            }

            foreach (var languageId in status.ContentLanguages)
            {
                var languageBranch = _languageBranchRepository.Load(languageId);

                if (languageBranch == null)
                {
                    languageBranch = new LanguageBranch(languageId);
                    _languageBranchRepository.Save(languageBranch);
                }
                else if (!languageBranch.Enabled)
                {
                    languageBranch = languageBranch.CreateWritableClone();
                    languageBranch.Enabled = true;
                    _languageBranchRepository.Save(languageBranch);
                }
            }
        }
    }
}
