using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using DCMS_APIServices.Models;
using DCMS_APIServices.Services;
using Newtonsoft.Json;

namespace DCMS_APIServices.Controllers
{
    public class ApproversController : ApiController
    {
        [HttpGet]
        
        public string GetApproversDetails(string refNo)
        {
            ApproversModel approvers = new ApproversModel();
            List<ApproverStack> approverStacks = approvers.GetApprovers(refNo);

            return JsonConvert.SerializeObject(approverStacks);
        }

        [HttpGet]
        public string GetApprovers(string refNo, string role, string status)
        {
            ApproversModel approvers = new ApproversModel();

            return approvers.GetApproverEmails(refNo, role, status);
        }

        [HttpPost]
        public void AddApprovers(HttpRequestMessage approversData)
        {
            string json = approversData.Content.ReadAsStringAsync().Result;
            ApproversService approversService = new ApproversService();
            approversService.AddApprovers(json);
        }

        [HttpGet]
        public void UpdateApproverStatus(string refNo, string role, string email)
        {
            ApproversModel approvers = new ApproversModel();
            approvers.UpdateStatus(refNo, role, email);
        }

        [HttpGet]
        public void ResetApprovers(string refNo)
        {
            ApproversModel approvers = new ApproversModel();
            approvers.Reset(refNo);
        }
    }
}
