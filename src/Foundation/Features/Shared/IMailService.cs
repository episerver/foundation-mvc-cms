using EPiServer.Core;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Foundation.Features.Shared
{
    public interface IMailService 
    {
        void Send(string subject, string body, string toEmail);
        void Send(MailMessage message);
        Task SendAsync(ContentReference mailReference, NameValueCollection nameValueCollection, string toEmail, string language);
        Task SendAsync(MailMessage message);
        Task<string> GetHtmlBodyForMailAsync(ContentReference mailReference, NameValueCollection nameValueCollection, string language);
    }
}