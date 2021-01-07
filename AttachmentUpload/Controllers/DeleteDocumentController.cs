using AttachmentUpload.Models;
using AttachmentUpload.Services;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;


namespace AttachmentUpload.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DeleteDocumentController : ApiController
    {
        static readonly string WebURL = ConfigurationManager.AppSettings["MemoWebURL"];

        [ActionName("DeleteDocs")]
        public string DeleteDocs(AttachmentModel attachmentData)

        {
            try
            {
                AttachmentService.DeleteFilefromLibrary(attachmentData);
                var fileLocation = WebURL + "/" + "CommunicationAttachments/" + attachmentData.RefID + "/" + attachmentData.Title;
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
