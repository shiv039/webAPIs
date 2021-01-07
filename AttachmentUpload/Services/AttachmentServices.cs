using AttachmentUpload.Models;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.DocumentSet;
using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace AttachmentUpload.Services
{
    
    public static class AttachmentService
    {
        static readonly string WebURL = ConfigurationManager.AppSettings["MemoWebURL"]; 
        static readonly string contentID = ConfigurationManager.AppSettings["MemoDocSetID"];
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        static readonly string UserName = ConfigurationManager.AppSettings["AdminUserName"];
        static readonly string Password = ConfigurationManager.AppSettings["Password"];
        static readonly string AttachmentLocalPath = ConfigurationManager.AppSettings["AttachmentLocalPath"];


        public static Microsoft.SharePoint.Client.File UploadFiletoLibrary(AttachmentModel attachmentData)
        {

            ClientContext context = new ClientContext(WebURL)
            {
                Credentials = new NetworkCredential(UserName, Password)
            };


            FileStream fs = null;
            Microsoft.SharePoint.Client.File uploadFile = null;
            string fileName = attachmentData.Title;
            if(fileName !="" && fileName != null)
            {
                long fileSize;
                int fileChunkSizeInMB = 3;

                int blockSize = fileChunkSizeInMB * 1024 * 1024;
                
                List library = context.Web.Lists.GetByTitle("CommunicationAttachments");
                Guid uploadId = Guid.NewGuid();
                string pathString = AttachmentLocalPath;
                pathString = pathString + "\\" + fileName;
                Folder Parentfolder = library.RootFolder;
                System.IO.File.WriteAllBytes(pathString, Convert.FromBase64String(attachmentData.Filebase));
                string filebasenames = pathString;
                fileSize = new FileInfo(filebasenames).Length;
                string uniqueFileName = Path.GetFileName(filebasenames);

                ContentType ct = context.Web.ContentTypes.GetById(contentID);
                context.Load(ct);
                context.ExecuteQuery();

                DocumentSet.Create(context, Parentfolder, attachmentData.RefID, ct.Id);
                try
                {
                    Logger.Info("Hello world");
                    context.ExecuteQuery();
                }
                catch (ServerException ex)
                {
                    Logger.Error(ex, "Goodbye cruel world");
                    if (ex.Message.Contains("A document, folder or document set with the name '" + attachmentData.RefID + "' already exists."))
                    {

                    }
                    else
                    {
                        throw ex;
                    }
                }

                if (fileSize <= blockSize)
                {
                    // Use regular approach.
                    using (fs = new FileStream(filebasenames, FileMode.Open))
                    {
                        FileCreationInformation fileInfo = new FileCreationInformation
                        {
                            ContentStream = fs,
                            Url = uniqueFileName,
                            Overwrite = true
                        };


                        fileInfo.Url = WebURL + "/" + "CommunicationAttachments/" + attachmentData.RefID + "/" + fileName;

                        uploadFile = library.RootFolder.Files.Add(fileInfo);
                        context.Load(uploadFile);
                        //Return the file object for the uploaded file.


                        ListItem lstItem = uploadFile.ListItemAllFields;

                        context.Load(lstItem);

                        lstItem["FileLeafRef"] = attachmentData.Title;
                        lstItem["Title"] = attachmentData.Title;

                        lstItem.Update();
                        context.ExecuteQuery();
                        return uploadFile;
                    }
                }
                else
                {
                    try
                    {
                        fs = System.IO.File.Open(filebasenames, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            byte[] buffer = new byte[blockSize];
                            byte[] lastBuffer = null;
                            long fileoffset = 0;
                            long totalBytesRead = 0;
                            int bytesRead;
                            bool first = true;
                            bool last = false;

                            // Read data from filesystem in blocks
                            while ((bytesRead = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                totalBytesRead += bytesRead;

                                // We've reached the end of the file
                                if (totalBytesRead == fileSize)
                                {
                                    last = true;
                                    // Copy to a new buffer that has the correct size
                                    lastBuffer = new byte[bytesRead];
                                    Array.Copy(buffer, 0, lastBuffer, 0, bytesRead);
                                }


                                ClientResult<long> bytesUploaded;
                                if (first)
                                {
                                    using (MemoryStream contentStream = new MemoryStream())
                                    {
                                        // Add an empty file.
                                        FileCreationInformation fileInfo = new FileCreationInformation
                                        {
                                            ContentStream = contentStream,
                                            Url = uniqueFileName,
                                            Overwrite = true
                                        };

                                        fileInfo.Url = WebURL + "/" + "CommunicationAttachments/" + attachmentData.RefID + "/" + fileName;
                                        uploadFile = library.RootFolder.Files.Add(fileInfo);

                                        // Start upload by uploading the first slice.
                                        using (MemoryStream s = new MemoryStream(buffer))
                                        {
                                            // Call the start upload method on the first slice

                                            bytesUploaded = uploadFile.StartUpload(uploadId, s);

                                            context.ExecuteQuery();

                                            // fileoffset is the pointer where the next slice will be added
                                            fileoffset = bytesUploaded.Value;
                                        }

                                        ListItem lstItem = uploadFile.ListItemAllFields;

                                        context.Load(lstItem);

                                        lstItem["FileLeafRef"] = attachmentData.Title;
                                        lstItem["Title"] = attachmentData.Title;

                                        lstItem.Update();
                                        // we can only start the upload once
                                        first = false;
                                    }
                                }
                                else
                                {
                                    // Get a reference to our file

                                    if (last)
                                    {
                                        // Is this the last slice of data?
                                        using (MemoryStream s = new MemoryStream(lastBuffer))
                                        {
                                            // End sliced upload by calling FinishUpload
                                            uploadFile = uploadFile.FinishUpload(uploadId, fileoffset, s);
                                            context.ExecuteQuery();
                                            // return the file object for the uploaded file
                                            return uploadFile;
                                        }
                                    }
                                    else
                                    {
                                        using (MemoryStream s = new MemoryStream(buffer))
                                        {
                                            // Continue sliced upload
                                            bytesUploaded = uploadFile.ContinueUpload(uploadId, fileoffset, s);
                                            context.ExecuteQuery();
                                            // update fileoffset for the next slice
                                            fileoffset = bytesUploaded.Value;
                                        }
                                    }
                                }

                            }
                        }

                    }
                    finally
                    {
                        if (fs != null)
                        {

                            fs.Dispose();
                        }
                    }
                }
                return uploadFile;
            } else
            {
                return uploadFile;
            }
        }
    
        public static bool DeleteFilefromLibrary(AttachmentModel attachmentData)
        {
            bool returnVal = false;
                Microsoft.SharePoint.Client.FileCollection uploadFile = null;

            //ClientContext context = new ClientContext(new Uri(WebURL));
            ClientContext context = new ClientContext(WebURL)
            {
                Credentials = new NetworkCredential(UserName, Password)
            };
            // clientcontext.Web.Lists.GetById - This option also can be used to get the list using List GUID
            // This value is NOT List internal name
            List library = context.Web.Lists.GetByTitle("CommunicationAttachments");
            string docSetUrl = attachmentData.RefID;

            uploadFile = library.RootFolder.Folders.GetByUrl(docSetUrl).Files;
            // Option 1: Get Item by ID
            context.Load(uploadFile);
            context.ExecuteQuery();
            foreach (var file in uploadFile)
            {
                
                if (file.Title == attachmentData.Title)
                {
                    file.DeleteObject();
                    context.ExecuteQuery();
                    return true;                }
                
            }

            return returnVal;
        }
    }
}