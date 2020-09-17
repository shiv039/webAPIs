using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}