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
    [EnableCors(origins: "*", headers:"*", methods:"*")]
    public class UploadDocumentsController : ApiController
    {
        static readonly string WebURL = ConfigurationManager.AppSettings["MemoWebURL"];

        [ActionName("UploadDocs")]
        public string UploadDocs(AttachmentModel attachmentData)
        
        {
            try
            {
                Microsoft.SharePoint.Client.File uploadFile = AttachmentService.UploadFiletoLibrary(attachmentData);
                if(uploadFile == null)
                {
                    throw new Exception("File Name Title is null");
                }
                var fileLocation = WebURL + "/" + "CommunicationAttachments/" + attachmentData.RefID + "/" + attachmentData.Title;
                return "{\"status\":\"Success\", \"Link\":\"" + fileLocation + "\", \"FileName\":\"" + attachmentData.Title + "\"}";
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "failure";
            }
        }
    }
}
