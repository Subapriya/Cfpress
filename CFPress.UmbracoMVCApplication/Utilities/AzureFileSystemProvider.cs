using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Umbraco.Core.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CFPress.UmbracoMVCApplication.Utilities
{
    [FileSystemProvider("media")]
    public class AzureFileSystemProvider : IFileSystem
    {
       
        CloudStorageAccount storageAccount;
        CloudBlobClient blobClient;
        CloudBlobContainer container;

        public AzureFileSystemProvider(string rootUrl)
        {
      
            var constring = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;

            // Retrieve storage account from connection string.
            storageAccount = CloudStorageAccount.Parse(constring);

            // Create the blob client.
            blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            container = blobClient.GetContainerReference("images");

            //container.CreateIfNotExists();
            //container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            rootUrl = "https://cfpressstaticfiles.blob.core.windows.net/images";
        }

        public void AddFile(string path, System.IO.Stream stream, bool overrideIfExists)
        {
            throw new NotImplementedException();
        }

        public void AddFile(string path, System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetCreated(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFiles(string path, string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFiles(string path)
        {
            throw new NotImplementedException();
        }

        public string GetFullPath(string path)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetLastModified(string path)
        {
            throw new NotImplementedException();
        }

        public string GetRelativePath(string fullPathOrUrl)
        {
            throw new NotImplementedException();
        }

        public string GetUrl(string path)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream OpenFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}