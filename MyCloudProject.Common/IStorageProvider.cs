using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

namespace MyCloudProject.Common
{
    public interface IStorageProvider
    {
        Task<string> DownloadInputFileAsync(string downloadBlobContainerName, string blobToDownload);

        Task<string> UploadResultFileAsync(string uploadBlobContainerName, string blobToUpload);

        Task UploadExperimentResultAsync(string resultTableName, ExperimentResult entity);

        Task<CloudQueue> CreateQueueAsync(string queueName);
    }
}
