using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;


namespace PersonalizationData.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PersonalizationDataController : ApiController
    {
       [EnableQuery]
        public IEnumerable<UserPersonalizationData> Get()
        {
            using(PersonalizationDataDB entities = new PersonalizationDataDB())
            {
                return entities.UserPersonalizationDatas.ToList();
            }

        }
        public HttpResponseMessage Get(int id)
        {
            using (PersonalizationDataDB entities = new PersonalizationDataDB())
            {
                var entity = entities.UserPersonalizationDatas.FirstOrDefault(e => e.ID == id);
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

        public HttpResponseMessage Post([FromBody] UserPersonalizationData personalizationData)
        {
            try
            {
                using (PersonalizationDataDB entities = new PersonalizationDataDB())
                {
                    entities.UserPersonalizationDatas.Add(personalizationData);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, personalizationData);
                    message.Headers.Location = new Uri(Request.RequestUri + personalizationData.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody] UserPersonalizationData personalizationData)
        {
            try
            {
                using (PersonalizationDataDB entities = new PersonalizationDataDB())
                {
                    var entity = entities.UserPersonalizationDatas.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record Not Found");
                    }
                    else
                    {
                        entity.UserEmail = personalizationData.UserEmail;
                        entity.UserPersonalizationData1 = personalizationData.UserPersonalizationData1;
                        
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, personalizationData);

                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [HttpPatch]
        public HttpResponseMessage Patch(int id, [FromBody] UserPersonalizationDataPatch personalizationData)
        {
            try
            {
                using (PersonalizationDataDB entities = new PersonalizationDataDB())
                {
                    var entity = entities.UserPersonalizationDatas.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record Not Found");
                    }
                    else
                    {
                        entity.UserPersonalizationData1 = personalizationData.UserPersonalizationData1;

                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, personalizationData);

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
