using EPiServer.Cms.Shell;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using Foundation.Infrastructure.Find.Facets;
using System;
using Foundation.Features.CustomPageFolder;
using EPiServer.Logging;


namespace Foundation.Features.CustomInitializationModule
{
    [InitializableModule]
    public class CustomInitializationModule : IInitializableModule
    {
        private readonly Lazy<IContentEvents> _contentEvents = new Lazy<IContentEvents>(() => ServiceLocator.Current.GetInstance<IContentEvents>());
        private readonly Lazy<IContentRepository> _contentRepository = new Lazy<IContentRepository>(() => ServiceLocator.Current.GetInstance<IContentRepository>());

        public void Initialize(InitializationEngine context)
        {
            Console.WriteLine("Initialize site custom");

            //_logger.Log(Level.Information, "Hello on initialize");

            _contentEvents.Value.PublishedContent += OnPublishedContent;
        }
        
        public void Uninitialize(InitializationEngine context)
        {
            Console.WriteLine("Uninitialize site custom");

            _contentEvents.Value.PublishedContent -= OnPublishedContent;
        }
        
        void OnPublishedContent(object sender, ContentEventArgs contentEventArgs)
        {
            var content = contentEventArgs.Content;
            if(content is CustomPage)
            {
                var parent = _contentRepository.Value.Get<CustomPage>(contentEventArgs.Content.ContentGuid);
                CustomPage clone = (CustomPage)parent.CreateWritableClone();
                var value = Int32.Parse(clone.PublishCounterValue);
                value++;
                clone.PublishCounterValue = value.ToString();
                _contentRepository.Value.Save(clone, SaveAction.Save);
            }
        }
    }
}