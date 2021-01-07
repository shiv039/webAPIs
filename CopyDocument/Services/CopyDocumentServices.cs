using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.DocumentSet;
using System.Configuration;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Threading;
using CopyDocument.Models;

namespace CopyDocument.Services
{
    public static class CopyDocumentService 
    {
        static readonly string WebURL = ConfigurationManager.AppSettings["MemoWebURL"];
        static readonly string contentID = ConfigurationManager.AppSettings["MemoDocSetID"];
        static readonly string UserName = ConfigurationManager.AppSettings["AdminUserName"];
        static readonly string Password = ConfigurationManager.AppSettings["Password"];
        static readonly string AttachmentLocalPath = ConfigurationManager.AppSettings["AttachmentLocalPath"];


        public static string copyDocumenttoLibrary(CopyDocumentModel copyDocument)

        {
            {
                Microsoft.SharePoint.Client.FileCollection uploadFile = null;
                ClientContext context = new ClientContext(WebURL)
                {
                    Credentials = new NetworkCredential(UserName, Password)
                };
                List library = context.Web.Lists.GetByTitle("CommunicationAttachments");
                Folder Parentfolder = library.RootFolder;

                try
                {
                    ContentType ct = context.Web.ContentTypes.GetById(contentID);
                    context.Load(ct);
                    context.ExecuteQuery();
                    DocumentSet.Create(context, Parentfolder, copyDocument.destRefID, ct.Id);
                    try
                    {
                        context.ExecuteQuery();
                    }
                    catch (ServerException ex)
                    {
                         throw ex;
                    }
                    string docSetUrl = copyDocument.srcRefID;
                    uploadFile = library.RootFolder.Folders.GetByUrl(docSetUrl).Files;
          
                    context.Load(uploadFile);
                    context.ExecuteQuery();

                    foreach (var file in uploadFile)
                    {
                        FileInformation fileInfo = File.OpenBinaryDirect(context, file.ServerRelativeUrl);
                        context.ExecuteQuery();
                        string pathString = AttachmentLocalPath;
                        var filePath = pathString + file.Name;
                        using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                        {
                            fileInfo.Stream.CopyTo(fileStream);
                        }

                    }


                    return "success";
                }
                 catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return ex.Message;
                }

            }
        }
    }
}