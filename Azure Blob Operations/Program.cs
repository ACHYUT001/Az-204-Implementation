using System;
using System.IO;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;

namespace Azure_Blob_Operations
{
    class Program
    {
        static async Task Main(string[] args)
        {   
           
            if(!CloudStorageAccount.TryParse("<connection string>", out CloudStorageAccount storageAccount))
            {
                Console.WriteLine("Unable to parse connection string");
                return;
            }

            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference("blobcrud");
            bool created = await blobContainer.CreateIfNotExistsAsync();

            Console.WriteLine(created ? "Created the Blob Container" : "Blob container already exits");
            
        }
    }
}
