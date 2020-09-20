using Newtonsoft.Json;
using System.Collections.Generic;
using DCMS_APIServices.Models;

namespace DCMS_APIServices.Services
{
    public class ApproversService
    {
        public string GetApproversSemiColonSeperatedString(List<string> approversArray)
        {
            string approversString = approversArray[0];
            for (var i = 1; i < approversArray.Count; i++)
            {
                approversString = approversString + ";" + approversArray[i];
            }
            return approversString;
        }

        public bool AddApprovers(string approverStackString)
        {
            ApproversModel approversModel = new ApproversModel();
            List<ApproverStack> approverStackArray = JsonConvert.DeserializeObject<List<ApproverStack>>(approverStackString);
            approversModel.DeleteMultipleEntry(approverStackArray[0].RefNo);

            foreach (var approverDetail in approverStackArray)
            {
                approversModel.AddApproverEntry(approverDetail);
            }

            return true;
        }
    }
}