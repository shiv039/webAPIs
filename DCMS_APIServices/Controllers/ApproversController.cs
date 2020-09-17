using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using DCMS_APIServices.Models;

namespace DCMS_APIServices.Controllers
{
    public class ApproversController : ApiController
    {
        [HttpGet]
        public string GetApproversDetails(string refNo)
        {
            ApproversModel approvers = new ApproversModel();

            return approvers.GetApprovers(refNo);
        }

        [HttpGet]
        public string GetApprovers(string refNo, string role, string status)
        {
            ApproversModel approvers = new ApproversModel();

            return approvers.GetApproverEmails(refNo, role, status);
        }

        [HttpPost]
        public string AddApprovers()
        {
            return "PushApprovers";
        }

        public string UpdateApproverStatus(string refNo, int orderNo)
        {
            return "UpdateApproverStatus";
        }
    }
}
