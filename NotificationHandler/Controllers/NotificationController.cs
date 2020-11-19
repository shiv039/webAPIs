using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;

namespace NotificationHandler.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotificationController : ApiController
    {
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
