using AttachmentUpload.Models;
using AttachmentUpload.Services;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AttachmentUpload.Controllers
{
    [EnableCors(origins: "*", headers:"*", methods:"*")]
    public class UploadDocumentsController : ApiController
    {
        
        [ActionName("UploadDocs")]
        public string UploadDocs(AttachmentModel attachmentData)
        
        {
            try
            {
                AttachmentService.UploadFiletoLibrary(attachmentData);
                return "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "failure";
            }
        }
    }
}
