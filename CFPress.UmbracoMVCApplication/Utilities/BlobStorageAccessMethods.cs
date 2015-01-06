using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CFPress.UmbracoMVCApplication.Utilities
{
   public class BlobStorageAccessMethods
    {
        //these variables are used throughout the class
        protected internal string ContainerName { get; set; }
        protected internal CloudBlobContainer cloudBlobContainer { get; set; }
 
        //this is the only public constructor; can't use this class without this info
       protected internal BlobStorageAccessMethods(string storageAccountName, string storageAccountKey, string containerName)
       {
       cloudBlobContainer = SetUpContainer(storageAccountName, storageAccountKey, containerName);
       ContainerName = containerName;
       }

       /// <summary>
       /// Set up container method 
       /// </summary>
       /// <param name="storageAccountName"></param>
       /// <param name="storageAccountKey"></param>
       /// <param name="containerName"></param>
       /// <returns></returns>
       private CloudBlobContainer SetUpContainer(string storageAccountName, string storageAccountKey, string containerName)
       {
           string connectionString = string.Format(@"DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",storageAccountName, storageAccountKey);
           CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
           CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
           CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
           return cloudBlobContainer;

       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="storageAccountName"></param>
       /// <param name="storageAccountKey"></param>
       /// <param name="containerName"></param>
       public void RunAtAppStartup(string storageAccountName, string storageAccountKey, string containerName)
       {
           cloudBlobContainer = SetUpContainer(storageAccountName, storageAccountKey, containerName);
           //just in case, check to see if the container exists,
           //  and create it if it doesn't
           cloudBlobContainer.CreateIfNotExists();

           //set access level to "blob", which means user can access the blob 
           //  but not look through the whole container
           //this means the user must have a URL to the blob to access it
           BlobContainerPermissions permissions = new BlobContainerPermissions();
           permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
           cloudBlobContainer.SetPermissions(permissions);
       }

    }
}