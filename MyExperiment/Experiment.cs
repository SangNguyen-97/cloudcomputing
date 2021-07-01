using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyCloudProject.Common;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Akka.Configuration;
using Akka.Util.Internal;

namespace MyExperiment
{
    public class Experiment : IExperiment
    {
        private IStorageProvider storageProvider;

        private ILogger logger;

        private MyConfig config;

        private ExperimentResult result = new ExperimentResult("null", "null");

        public Experiment(IConfigurationSection configSection, IStorageProvider storageProvider, ILogger log)
        {
            this.storageProvider = storageProvider;
            this.logger = log;

            config = new MyConfig();
            configSection.Bind(config);
        }

        /// <summary>
        /// Run the experiment using the specified training and input file and name the output file as specified by the parameter
        /// </summary>
        /// <param name="trainingFile">Training file</param>
        /// <param name="inputFile">Input file</param>
        /// <param name="outputFile">Output file name</param>
        /// <returns></returns>
        public Task<ExperimentResult> Run(string trainingFile, string inputFile, string outputFile)
        {
            // TODO read file

            ExperimentResult res = new ExperimentResult(this.config.GroupId, "not-assigned");

            // Recording start time
            res.StartTimeUtc = DateTime.UtcNow;
            Console.WriteLine("Experiment started...\n--------------\n");

            // Start the experiment
            res.DurationSec = MyMainTestClass.MyTestMethod(trainingFile, inputFile, outputFile);

            // Rcording stop time
            res.EndTimeUtc = DateTime.UtcNow;
            Console.WriteLine("Experiment finished...\n--------------\n");
            return Task.FromResult<ExperimentResult>(res); // TODO...
        }

        /// <inheritdoc/>
        public async Task RunQueueListener(CancellationToken cancelToken)
        {
            CloudQueue queue = await storageProvider.CreateQueueAsync(config.Queue);

            while (cancelToken.IsCancellationRequested == false)
            {
                CloudQueueMessage message = await queue.GetMessageAsync();
                if (message != null)
                {
                    try
                    {
                        this.logger?.LogInformation($"Received the message...\n{message.AsString}\n--------------\n");
                        // Deserialize the received message
                        ExerimentRequestMessage msg = JsonConvert.DeserializeObject<ExerimentRequestMessage>(message.AsString);

                        // Check if the validity of the output filename
                        if (msg.outputFile.Contains("/")) throw new Exception("The output filename is not acceptable. It should not contain any preceding folder in the filename path");

                        // Print out the experiment ID
                        this.logger?.LogInformation("Experiment ID: {0}",msg.experimentId);

                        // Download necessary training file and input file
                        string trainingFileUri = await this.storageProvider.DownloadInputFileAsync(config.TrainingContainer, msg.trainingFile);
                        this.logger?.LogInformation("Finished downloading training file");
                        string inputFileUri = await this.storageProvider.DownloadInputFileAsync(config.TrainingContainer, msg.inputFile);
                        this.logger?.LogInformation("Finished downloading input file");

                        // Run the experiment
                        this.logger?.LogInformation("Experiment is running...");
                        result = await this.Run(msg.trainingFile, msg.inputFile, msg.outputFile);
                        this.logger?.LogInformation("Experiment finished");

                        // Upload result file and update ExperimentResult object
                        result.RowKey = msg.experimentId;
                        result.ExperimentId = msg.experimentId;
                        result.Name = msg.name;
                        result.Description = msg.description;
                        result.trainingFileUri = trainingFileUri;
                        result.inputFileUri = inputFileUri;
                        result.outputFileUri = await this.storageProvider.UploadResultFileAsync(config.ResultContainer, msg.outputFile);
                        this.logger?.LogInformation("Finished uploading result file");

                        // Upload the ExperimentResult object to Azure Table Storage
                        await storageProvider.UploadExperimentResultAsync(config.ResultTable, result);
                        this.logger?.LogInformation("Uploaded result");

                        // Delete the processed message from Azure Queue
                        await queue.DeleteMessageAsync(message);
                        this.logger?.LogInformation("Deleted processesd message from queue");

                        // Delete downloaded and generated files to save space
                        File.Delete(msg.trainingFile);
                        File.Delete(msg.inputFile);
                        File.Delete(msg.outputFile);
                    }
                    catch (Exception ex)
                    {
                        this.logger?.LogError(ex, "Encountered Exception...");
                    }
                }
                else
                    await Task.Delay(1000);
            }

            this.logger?.LogInformation("Cancel pressed. Exiting the listener loop.");
        }


        //////#region Private Methods


        ///////// <summary>
        ///////// Validate the connection string information in app.config and throws an exception if it looks like 
        ///////// the user hasn't updated this to valid values. 
        ///////// </summary>
        ///////// <param name="storageConnectionString">The storage connection string</param>
        ///////// <returns>CloudStorageAccount object</returns>
        //////private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        //////{
        //////    CloudStorageAccount storageAccount;
        //////    try
        //////    {
        //////        storageAccount = CloudStorageAccount.Parse(storageConnectionString);
        //////    }
        //////    catch (FormatException)
        //////    {
        //////        Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
        //////        Console.ReadLine();
        //////        throw;
        //////    }
        //////    catch (ArgumentException)
        //////    {
        //////        Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
        //////        Console.ReadLine();
        //////        throw;
        //////    }

        //////    return storageAccount;
        //////}

        ///////// <summary>
        ///////// Create a queue for the sample application to process messages in. 
        ///////// </summary>
        ///////// <returns>A CloudQueue object</returns>
        //////private static async Task<CloudQueue> CreateQueueAsync(MyConfig config)
        //////{
        //////    // Retrieve storage account information from connection string.
        //////    CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(config.StorageConnectionString);

        //////    // Create a queue client for interacting with the queue service
        //////    CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

        //////    Console.WriteLine("1. Create a queue for the demo");

        //////    CloudQueue queue = queueClient.GetQueueReference(config.Queue);
        //////    try
        //////    {
        //////        await queue.CreateIfNotExistsAsync();
        //////    }
        //////    catch
        //////    {
        //////        Console.WriteLine("If you are running with the default configuration please make sure you have started the storage emulator.  ess the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
        //////        Console.ReadLine();
        //////        throw;
        //////    }

        //////    return queue;
        //////}
        //////#endregion
    }
}
