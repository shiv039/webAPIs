using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DCMS_APIServices.Models
{
    public class AuditTrialModel
    {
        public List<AuditTrial> GetAuditTrials(string refNo)
        {
            using (var ctx = new ITFC_DCMSEntities())
            {
                List<AuditTrial> auditTrials = (from x in ctx.AuditTrials
                                                where x.RefNo == refNo
                                                select x).ToList();

                return auditTrials;
            }
        }

        public bool AddAudit(string auditString)
        {
            AuditTrial auditTrial = JsonConvert.DeserializeObject<AuditTrial>(auditString);

            using (var ctx = new ITFC_DCMSEntities())
            {
                ctx.AuditTrials.Add(auditTrial);
                ctx.SaveChanges();

                return true;
            }
        }

        public bool ClearAudits(string refNo)
        {
            using (var ctx = new ITFC_DCMSEntities())
            {
                List<AuditTrial> auditTrials = ctx.AuditTrials.Where(x => x.RefNo == refNo).ToList();

                ctx.AuditTrials.RemoveRange(auditTrials);
                ctx.SaveChanges();
            }

            return true;
        }
    }
}