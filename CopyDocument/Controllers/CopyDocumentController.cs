using System;
using CopyDocument.Models;
using CopyDocument.Services;
using Microsoft.SharePoint.Client;
using System.Configuration;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Threading;


namespace CopyDocument.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CopyDocumentController : ApiController
    {
        public string CopyDoc(CopyDocumentModel copyDocument)

        {
            try
            {
                string copyDocumenttoLibrary = CopyDocumentService.copyDocumenttoLibrary(copyDocument);

                return "success";

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "failure";
            }
        }

    }
}
