using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;

namespace NotificationHandler.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotificationController : ApiController
    {
        static readonly string UserName = ConfigurationManager.AppSettings["AdminUserName"];
        static readonly string Password = ConfigurationManager.AppSettings["Password"];
        [EnableQuery]
        public IEnumerable<Notification> Get()
        {
            using (ITFC_DCMS_DEVEntities entities = new ITFC_DCMS_DEVEntities())
            {
                return entities.Notifications.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (ITFC_DCMS_DEVEntities entities = new ITFC_DCMS_DEVEntities())
            {
                var entity = entities.Notifications.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not found");
                }
            }
        }

        public HttpResponseMessage Post([FromBody] Notification notification)
        {
            try
            {
                using (ITFC_DCMS_DEVEntities entities = new ITFC_DCMS_DEVEntities())
                {
                    entities.Notifications.Add(notification);
                    entities.SaveChanges();

                    if (notification.EmailNotification == "yes")
                    {
                        
                        MailMessage mailMessage = new MailMessage("vinay@isdbdev.org", notification.EmailTo);
                        mailMessage.Body = notification.EmailBody;
                        mailMessage.Subject = "Notification";

                        
                        //SmtpClient smtpClient = new SmtpClient("SMTP.office365.com", 587);
                        SmtpClient smtpClient = new SmtpClient("AZDMSSPD01.isdbdev.org");

                        /*smtpClient.Credentials = new System.Net.NetworkCredential()
                        {
                            UserName = UserName,
                            Password = Password
                        };*/
                        smtpClient.UseDefaultCredentials = true;
                        smtpClient.EnableSsl = true;
                        
                        smtpClient.Send(mailMessage);

                    }
                    

                    var message = Request.CreateResponse(HttpStatusCode.Created, notification);
                    message.Headers.Location = new Uri(Request.RequestUri + notification.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody] Notification notification)
        {
            try
            {
                using (ITFC_DCMS_DEVEntities entities = new ITFC_DCMS_DEVEntities())
                {
                    var entity = entities.Notifications.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record Not Found");
                    }
                    else
                    {
                        entity.UserName = notification.UserName;
                        entity.NotifMsg = notification.NotifMsg;
                        entity.ReadStatus = notification.ReadStatus;
                        entity.Category = notification.Category;
                        entity.CreatedDate = notification.CreatedDate;
                        entity.CreatedTime = notification.CreatedTime;
                        entity.UserEmail = notification.UserEmail;
                        entity.EmailNotification = notification.EmailNotification;
                        entity.ItemURL = notification.ItemURL;
                        entity.NotificationSource = notification.NotificationSource;
                        entity.EmailBody = notification.EmailBody;
                        entity.EmailCc = notification.EmailCc;
                        entity.EmailTo = notification.EmailTo;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, notification);

                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpPatch]
        public HttpResponseMessage Patch(int id, [FromBody] NotificationPatch notification)
        {
            try
            {
                using (ITFC_DCMS_DEVEntities entities = new ITFC_DCMS_DEVEntities())
                {
                    var entity = entities.Notifications.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record Not Found");
                    }
                    else
                    {
                        entity.ReadStatus = notification.ReadStatus;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, notification);

                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

    }
}
