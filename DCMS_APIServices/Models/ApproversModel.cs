using System.Collections.Generic;
using System.Linq;
using DCMS_APIServices.Services;

namespace DCMS_APIServices.Models
{
    public class ApproversModel
    {
        public List<ApproverStack> GetApprovers(string refNo)
        {
            using (var ctx = new ITFC_DCMSEntities())
            {
                List<ApproverStack> approverStacks = (from x in ctx.ApproverStacks
                                                      where x.RefNo == refNo
                                                      select x).ToList();

                return approverStacks;
            }
        }

        public string GetApproverEmails(string refNo, string role, string status)
        {
            using (var ctx = new ITFC_DCMSEntities())
            {
                List<string> approvers = (from x in ctx.ApproverStacks
                                          where (x.RefNo == refNo) && (x.RoleCode == role) && (x.Status == status)
                                          select x.Email).ToList<string>();

                if (approvers.Count > 0)
                {
                    ApproversService service = new ApproversService();
                    return service.GetApproversSemiColonSeperatedString(approvers);
                }
                else
                {
                    return "";
                }
            }
        }

        public bool AddApproverEntry(ApproverStack approverStack)
        {
            using (var ctx = new ITFC_DCMSEntities())
            {
                ctx.ApproverStacks.Add(approverStack);
                ctx.SaveChanges();

                ctx.Dispose();

                return true;
            }
        }

        public bool DeleteMultipleEntry(string refNo)
        {
            using (var ctx = new ITFC_DCMSEntities())
            {
                List<ApproverStack> approverStack = (from x in ctx.ApproverStacks
                                                     where x.RefNo == refNo
                                                     select x).ToList();

                if (approverStack.Count > 0)
                {
                    ctx.ApproverStacks.RemoveRange(approverStack);
                    ctx.SaveChanges();
                }

                return true;
            }
        }

        public bool UpdateStatus(string refNo, string role, string email)
        {
            using (var ctx = new ITFC_DCMSEntities())
            {
                var approverStack = (from x in ctx.ApproverStacks
                                     where (x.RefNo == refNo) && (x.RoleCode == role) && (x.Email == email)
                                     select x).First();

                if (approverStack != null)
                {
                    approverStack.Status = "Completed";
                    ctx.SaveChanges();
                }

                return true;
            }
        }

        public bool Reset(string refNo)
        {
            using (var ctx = new ITFC_DCMSEntities())
            {
                var approverStack = ctx.ApproverStacks.Where(x => x.RefNo == refNo).ToList();

                approverStack.ForEach(x => x.Status = "Pending");
                ctx.SaveChanges();
            }

            return true;
        }
    }
}