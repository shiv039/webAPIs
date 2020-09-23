using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;

namespace DCMS_APIServices.Controllers
{
   
    [EnableCors(origins: "https://dmcs-dev.itfc-idb.org", headers: "*", methods: "*")]
    public class NotificationController : ApiController
    {
        [EnableQuery]
        public IEnumerable<Notification> Get()
        {
            using (ITFC_DCMSEntities entities = new ITFC_DCMSEntities())
            {
                return entities.Notifications.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (ITFC_DCMSEntities entities = new ITFC_DCMSEntities())
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
               using (ITFC_DCMSEntities entities = new ITFC_DCMSEntities())
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
                using (ITFC_DCMSEntities entities = new ITFC_DCMSEntities())
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
