using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CloudDataService.Helper
{
    public class BlobstorageFileHandler
    {
        string _accountName = string.Empty;
        string _accountKey = string.Empty;
        string _containerName = string.Empty;
        private CloudBlobClient _blobStorage = null;

        private CloudBlobContainer Container
        {
            get
            {
                return _blobStorage.GetContainerReference(_containerName);
            }
        }

        public BlobstorageFileHandler(string accountName, string accountKey, string containerName)
        {
            _accountName = accountName;
            _accountKey = accountKey;
            _containerName = containerName;
            StorageCredentialsAccountAndKey myaccount = new StorageCredentialsAccountAndKey(_accountName, _accountKey);
            CloudStorageAccount storageAccount = new CloudStorageAccount(myaccount, true);
            _blobStorage = storageAccount.CreateCloudBlobClient();
            _blobStorage.Timeout = TimeSpan.FromMinutes(5);
        }

        void CreateContainer()
        {
            //
            // Azure Blob Storage requires some delay between deleting and recreating containers.  This loop 
            // compensates for that delay if the first attempt to creat the container fails then wait 30 
            // seconds and try again.  While the service actually returns error 409 with the correct description
            // of the failure the storage client interprets that as the container existing and the
            // call to CreateContainer returns false.  For now just work around this by attempting to create
            // a test blob in the container bofore proceeding.
            //
            //
            //
            for (int i = 1; i <= 10; i++)
            {
                CloudBlobContainer blobContainer = Container;
                blobContainer.CreateIfNotExist();

                MemoryStream stream = new MemoryStream();
                try
                {
                    blobContainer.GetBlobReference("__testblob").UploadFromStream(stream);
                    blobContainer.GetBlobReference("__testblob").Delete();

                    var permissions = blobContainer.GetPermissions();
                    permissions.PublicAccess = BlobContainerPublicAccessType.Off;
                    blobContainer.SetPermissions(permissions);

                    break;
                }
                catch (StorageClientException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Failed creating container, retrying in 30 seconds...");

                    // Wait 30 seconds and retry
                    System.Threading.Thread.Sleep(30000);
                    if (i == 10)
                    {
                        // Only retry 10 times
                        throw;
                    }
                }
            }
        }

        public List<string> FileList(string filter)
        {
            List<string> myreturn = new List<string>();
            BlobRequestOptions opts = new BlobRequestOptions();
            opts.UseFlatBlobListing = true;
            foreach (IListBlobItem o in Container.ListBlobs(opts).OfType<CloudBlob>().Where(b => b.Name.StartsWith(filter)))
            {
                var blob = o as CloudBlob;
                myreturn.Add(blob.Name);
            }

            return myreturn;
        }

        public List<CloudBlob> FileListWithDetails(string filter)
        {
            List<CloudBlob> myreturn = new List<CloudBlob>();
            BlobRequestOptions opts = new BlobRequestOptions();
            opts.UseFlatBlobListing = true;
            foreach (IListBlobItem o in Container.ListBlobs(opts).OfType<CloudBlob>().Where(b => b.Name.StartsWith(filter)))
            {
                var blob = o as CloudBlob;
                myreturn.Add(blob);
            }

            return myreturn;
        }

        public List<CloudBlob> FileListWithDetails()
        {
            List<CloudBlob> myreturn = new List<CloudBlob>();
            BlobRequestOptions opts = new BlobRequestOptions();
            opts.UseFlatBlobListing = true;
            foreach (IListBlobItem o in Container.ListBlobs(opts).OfType<CloudBlob>())
            {
                var blob = o as CloudBlob;
                myreturn.Add(blob);
            }

            return myreturn;
        }

        public List<string> FileList()
        {
            List<string> myreturn = new List<string>();
            BlobRequestOptions opts = new BlobRequestOptions();
            opts.UseFlatBlobListing = true;
            foreach (IListBlobItem o in Container.ListBlobs(opts))
            {
                myreturn.Add(o.Uri.ToString());
            }

            return myreturn;
        }

        public byte[] DownloadFile(string Uri)
        {

            BlobRequestOptions opts = new BlobRequestOptions();
            opts.UseFlatBlobListing = true;
            CloudBlob blob = Container.GetBlobReference(Uri);

            try
            {
                return blob.DownloadByteArray();
            }
            catch (Exception e)
            {
                //MessageBox.Show("Bilder: " + e.Message + "\r\nInnerException: " + e.InnerException);
            }
            return null;
        }

        public bool MoveFileToArchive(string UriSource)
        {
            CloudBlob blob = Container.GetBlobReference(UriSource);

            var dir = Container.GetDirectoryReference("ARCHIV");

            var copyblob = dir.GetBlobReference(blob.Name);

            copyblob.CopyFromBlob(blob);

            blob.Delete();

            return true;
        }

        public void UploadFile(Stream file, string filename)
        {
            CloudBlob blob = Container.GetBlobReference(filename);
            blob.UploadFromStream(file);
        }
        public void UploadFile(string filepath, string filename)
        {
            CloudBlob blob = Container.GetBlobReference(filename);
            blob.UploadFile(filepath);
        }
    }
}