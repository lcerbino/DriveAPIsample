using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DriveQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] Scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };
        static string ApplicationName = "Drive API .NET Quickstart";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()  
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            //// Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 30;
            listRequest.Fields = "nextPageToken, files(id, name)";

            //// List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var f in files)
                {
                    Console.WriteLine("{0} ({1})", f.Name, f.Id);
                }
            }

            ////ADD Data

            //var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            //{
            //    Name = "photo.jpg",
            //    Parents = new List<string>() { "1SSHCUcMp1oTBwD1JSVaXyiKQkjV2h9bV" }
            //};

            //FilesResource.CreateMediaUpload request;
            //using (var stream = new System.IO.FileStream("files/photo.jpg",
            //                        System.IO.FileMode.Open))
            //{
            //    request = service.Files.Create(
            //        fileMetadata, stream, "image/jpeg");
            //    request.Fields = "id";
            //   var temp = request.Upload();
            //}

            ////Update data
            //Update a file & rename

            Google.Apis.Drive.v3.Data.File fil = files.FirstOrDefault(z => z.Name.Equals("photo.jpg") /*&& z.Parents.Contains("1SSHCUcMp1oTBwD1JSVaXyiKQkjV2h9bV")*/);
            //string id = fil.Id;
            //fil.Name = $"Image_Photo_2";
            //fil.Id = null;

            //FilesResource.UpdateRequest request = service.Files.Update(fil, id);
            //request.Execute();

            ////Delete a File

            Google.Apis.Drive.v3.Data.File afil = files.FirstOrDefault(z => z.Name.Equals("Image_Photo_2") /*&& z.Parents.Contains("1J9Z0y4rvwVeZT5Rooyd4R9lmsuevcxkK")*/);
            service.Files.Delete(afil.Id).Execute();

            Console.Read();
        }
    }
}