using MyCloudProject.Common;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading;
using MyExperiment;

namespace MyCloudProject
{
    class Program
    {
        /// <summary>
        /// Your project ID from the last semester.
        /// </summary>
        private static string projectName = "ML19/20-5.12. Investigate SpatialPooler's noise robustnes against Additive Gaussian Noise";


        static void Main(string[] args)
        {
            // Create cancellation token to interrupt the program in runtime (by pressing "Crt" + "C")
            CancellationTokenSource tokeSrc = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                tokeSrc.Cancel();
            };

            // Initialize configuration section for the Experiment
            var cfgRoot = InitHelpers.InitConfiguration(args);
            var cfgSec = cfgRoot.GetSection("MyConfig");

            // Initialize logging infrastructure
            var logFactory = InitHelpers.InitLogging(cfgRoot);
            var logger = logFactory.CreateLogger("Train.Console");
            logger?.LogInformation($"{DateTime.Now} -  Started experiment: {projectName}");


            Console.WriteLine($"Started experiment: {projectName}");

            // Initialize client for Azure Storage Account
            IStorageProvider storageProvider = new AzureStorageProvider(cfgSec);

            // Create and run the experiment
            Experiment experiment = new Experiment(cfgSec, storageProvider, logger/* put some additional config here */);
            experiment.RunQueueListener(tokeSrc.Token).Wait();


            Console.WriteLine($"Finished experiment: {projectName}");

            // Log runtime info
            logger?.LogInformation($"{DateTime.Now} -  Experiment exit: {projectName}");
        }


    }
}
