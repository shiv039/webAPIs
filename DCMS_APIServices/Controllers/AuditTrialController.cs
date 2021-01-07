using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DCMS_APIServices.Models;
using Newtonsoft.Json;

namespace DCMS_APIServices.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuditTrialController : ApiController
    {
        [HttpGet]
        public string Get(string refNo)
        {
            AuditTrialModel audit = new AuditTrialModel();

            List<AuditTrial> auditTrials = audit.GetAuditTrials(refNo);

            return JsonConvert.SerializeObject(auditTrials);
        }

        [HttpPost]
        public void Post(HttpRequestMessage data)
        {
            string json = data.Content.ReadAsStringAsync().Result;

            AuditTrialModel audit = new AuditTrialModel();
            audit.AddAudit(json);
        }

        [HttpGet]
        public void Clear(string refNo)
        {
            AuditTrialModel audit = new AuditTrialModel();

            audit.ClearAudits(refNo);
        }
    }
}
