using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCMS_APIServices.Services;

namespace DCMS_APIServices.Models
{
    public class ApproversModel
    {
        public string GetApprovers(string refNo)
        {
            using (var ctx = new ITFC_DCMSEntities())
            {
                List<ApproverStack> approverStack = (from x in ctx.ApproverStacks
                                                     where x.RefNo == refNo
                                                     select x).ToList();

                return JsonConvert.SerializeObject(approverStack);
            }
        }

        public string GetApproverEmails(string refNo, string role, string status)
        {
            using (var ctx = new ITFC_DCMSEntities())
            {
                List<string> approvers = (from x in ctx.ApproverStacks
                                              where (x.RefNo == refNo) && (x.RoleCode == role) && (x.Status == status)
                                          select x.Email).ToList<string>();

                if(approvers.Count>0)
                {
                    ApproversService service = new ApproversService();
                    return service.GetApproversSemiColonSeperatedString(approvers);
                } else
                {
                    return "";
                }                
            }
        }
    }
}