using System;
using System.Configuration;
using System.Net;
using System.Text;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    HttpStatusCode result;
    string contentType;

    result = HttpStatusCode.BadRequest;

    contentType = req.Content.Headers?.ContentType?.MediaType;

    if(contentType == "image/jpg")
    {
        string body;

        body = await req.Content.ReadAsStringAsync();

        if(!string.IsNullOrEmpty(body))
        {
            string name;

            name = Guid.NewGuid().ToString("n");

            await CreateBlob(name + ".jpg", body, log);

            result = HttpStatusCode.OK;
        }
    }

    return req.CreateResponse(result, string.Empty);
}

private async static Task CreateBlob(string name, string data, TraceWriter log)
{
    string accessKey;
    string accountName;
    string connectionString;
    CloudStorageAccount storageAccount;
    CloudBlobClient client;
    CloudBlobContainer container;
    CloudBlockBlob blob;

    connectionString = "DefaultEndpointsProtocol=https;AccountName=storage2349;AccountKey=I1F+6O9FeF469O5j9QOm8tbZXymXC2Ayo3JwG9dcmfSRlyrgKtbNGgT8Dke9n52BGwlIVscoJDIrAtKlYUjJ1w==;EndpointSuffix=core.windows.net";
    storageAccount = CloudStorageAccount.Parse(connectionString);

    client = storageAccount.CreateCloudBlobClient();
    
    container = client.GetContainerReference("test-images");
   
    await container.CreateIfNotExistsAsync();
    
    blob = container.GetBlockBlobReference(name);
    blob.Properties.ContentType = "image/jpg";

    using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
    {
        await blob.UploadFromStreamAsync(stream);
    }
}