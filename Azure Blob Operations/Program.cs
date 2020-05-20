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
           
            if(!CloudStorageAccount.TryParse("<Connection String>", out CloudStorageAccount storageAccount))
            {
                Console.WriteLine("Unable to parse connection string");
                return;
            }

            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference("blobcrud");
            bool created = await blobContainer.CreateIfNotExistsAsync();

            Console.WriteLine(created ? "Created the Blob Container" : "Blob container already exits");

            //upload
            var blob = blobContainer.GetBlockBlobReference("spiderman.png");
            string filename = "<location>\\spiderman.png";
            await blob.UploadFromFileAsync(filename);

            //checking if blob exists
            if (blob.Exists())
                Console.WriteLine("blob exists");

            //list all blobs in the container
            Console.WriteLine("List of blobs in the container");
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var results = await blobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);

                blobContinuationToken = results.ContinuationToken;
                foreach (IListBlobItem item in results.Results)
                {
                    Console.WriteLine(item.Uri);
                }

            } while (blobContinuationToken != null);

            //download blob

            string destinationFile = filename.Replace(".png", "_downloaded.png");
            Console.WriteLine("Downloading blob to {0}", destinationFile);
            await blob.DownloadToFileAsync(destinationFile, FileMode.Create);

            //delete container
            Console.WriteLine("Do you want to delete the container?(yes/no)");
            if(Console.ReadLine() == "yes")
            {
                Console.WriteLine("deleting the container");
                if(blobContainer != null)
                {
                    await blobContainer.DeleteIfExistsAsync();
                }
                
                Console.WriteLine("deleting the downloaded file");
                File.Delete(destinationFile);
            }

        }
    }
}
