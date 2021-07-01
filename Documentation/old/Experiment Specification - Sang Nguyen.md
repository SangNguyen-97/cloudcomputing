

# VNGroup: ML19/20-5.12. Investigate SpatialPooler's noise robustnes against Additive Gaussian Noise

## Experiment Description
Our experiment deals with the investigation of Hierarchical-Temporal-Memory (HTM) Spatial Pooler (SP)'s robustness against Additive Gaussian Noise. The experiment consists of two phases: training and testing.

In training phase, the SP is trained with a noise-free input file. Specifically, a training file describing the fluctuation of a sinusoidal waveform is first encoded by HTM Encoder and then fed to the SP (with common settings specified in the document linked below). The final output representing the column distribution of the SP's output with noise-free input after training phase is recorded.

In testing phase, the SP will instead be fed noisy versions of the original training file. These noisy files are also encoded by the Encoder before being fed to the SP. The outputs of the SP with these noisy files as input will be recorded and compared with the noise-free output.

About constructing the noisy versions of the original training file: a random variable with Gaussian distribution and mean value 0 is generated and then added to every value in the original noise-free file. Different noisy versions are generated with different levels of the random variable's fluctuation, which is, in this experiment, standard deviation of the random variable.

By comparing the noise-free and noisy output files, some conclusions about robustness of the SP with the specified settings against the additive Gaussian noise are drawn.

Here is the URL to the experiment's document:
[https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2019-2020/blob/VNGroup/Source/MyProject/Documentation/SangNguyen_1185021_SE-19-20_SpatialPooler_NoiseRobustness.pdf](https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2019-2020/blob/VNGroup/Source/MyProject/Documentation/SangNguyen_1185021_SE-19-20_SpatialPooler_NoiseRobustness.pdf)

## Azure Cloud Settings

### "appsettings.json"

~~~json
{
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },

  "MyConfig": {
    "GroupId": "prof",
    "StorageConnectionString" : "*** hidden-due-to-security-reason ***",
    "TrainingContainer": "vngroup-training-files",
    "ResultContainer": "vngroup-result-files",
    "ResultTable": "vngroupresultstable",
    "Queue": "vngroup-trigger-queue"
  }
}
~~~


## How to run experiment

### Training files

1. "sinusoidal.csv"

Currently, there is only this single training file available

### Testing files

1. "Noisy_N-0-0p1_sinusoidal.csv"
2. "Noisy_N-0-0p2_sinusoidal.csv"
3. "Noisy_N-0-0p3_sinusoidal.csv"
4. "Noisy_N-0-0p4_sinusoidal.csv"
5. "Noisy_N-0-0p5_sinusoidal.csv"
6. "Noisy_N-0-0p6_sinusoidal.csv"
7. "Noisy_N-0-0p7_sinusoidal.csv"
8. "Noisy_N-0-0p8_sinusoidal.csv"
9. "Noisy_N-0-0p9_sinusoidal.csv"
10. "Noisy_N-0-1_sinusoidal.csv" 
11. "Noisy_N-0-1p5_sinusoidal.csv"
12. "Noisy_N-0-2_sinusoidal.csv"
13. "Noisy_N-0-2p5_sinusoidal.csv"
14. "Noisy_N-0-3_sinusoidal.csv"
15. "Noisy_N-0-3p5_sinusoidal.csv"
16. "Noisy_N-0-4_sinusoidal.csv"

*Remarks:
For example, file "Noisy_N-0-0p1_sinusoidal.csv" means this file is a noisy input file with noise level 0.1*

### Output files

The name of the output files are to be specified by the person, who requested the experiment.

IMPORTANT: PLEASE USE ONLY FILE NAME WITHOUT PRECEDING DIRECTORY.
FOR EXAMPLE, DO NOT NAME THE OUTPUT FILE: "somefolder/output.csv"
INSTEAD, NAME IT MERELY: "output.csv"

### Experiment Request Message

For example, an Azure Queue experiment request message, which will trigger the experiment, has the following format:

~~~json
 {
"experimentId" : "123456789",
"trainingFile" : "sinusoidal.csv",
"inputFile" : "Noisy_N-0-1_sinusoidal.csv",
"outputFile" : "output_Noisy_N-0-1_sinusoidal.csv",
"name" : "Sang Nguyen",
"description" : "debugging"
}
~~~

Message format explanation:

1. "experimentId" : identifier of experiment and should be assigned uniquely to each run of the experiment.
2. "trainingFile" : name of the blob, which will be downloaded and used as the training file for the experiment.
3. "inputFile" : name of the blob, which will be downloaded and used as the testing file for the experiment.
4. "outputFile" : name of the blob, which will be uploaded as the output of the experiment.
5. "name" : Name of the person who requested this experiment.
6. "description" : Description of this experiment request.

### Experiment Result Table Entity

For example, an Azure Table entity, which will be uploaded as result of the experiment, has the following format:

|PartitionKey|RowKey|TimeStamp|Description|DurationSec|EndTimeUtc|ExperimentId|Name|StartTimeUtc|inputFileUri|outputFileUri|trainingFileUri|
|-|-|-|-|-|-|-|-|-|-|-|-|
| prof|123456789|2020-08-09T21:50:33.947Z|debugging|3|2020-08-09T21:50:32.463Z|123456789|Sang Nguyen|2020-08-09T21:50:29.027Z|https://webapplearningstorage.blob.core.windows.net/vngroup-training-files/Noisy_N-0-1_sinusoidal.csv|https://webapplearningstorage.blob.core.windows.net/vngroup-result-files/MySPInput/MyEncoderOut_robustness.csv|https://webapplearningstorage.blob.core.windows.net/vngroup-training-files/sinusoidal.csv|

Azure Table entity for experiment result's format explanation:
1. **PartitionKey** : required keyword for a table entity. This takes the value of "GroupId" key in "appsettings.json".
2. **RowKey** : required keyword for a table entity. This takes the value of "ExperimentId" key from the Azure Queue experiment request message.
3. **TimeStamp** : property of Azure Table entity.
4. **Description** : description of the experiment, provided by the person, who requested to run the experiment. This takes the value of "description" key from the Azure Queue experiment request message.
5. **DurationSec** : duration of the experiment.
6. **EndTimeUtc** : the time, at which the experiment is finished.
7. **ExperimentId** : identifier of a specific run of the experiment. This is the same as RowKey. This takes the value of "ExperimentId" key from the Azure Queue experiment request message.
8. **Name** : name of the person, who requested this run of the experiment. This takes the value of "name" key from the Azure Queue experiment request message.
9. **StartTimeUtc** : the time, at which the experiment is started.
10. **inputFileUri** : URI of the Azure Blob Storage blob, which is used as the testing file for the experiment. This is the URI of the blob, whose name is specified at "inputFile" key from the Azure Queue experiment request message and whose Azure Blob Storage container is specified at "TrainingContainer" key in "appsettings.json".
11. **outputFileUri** : URI of the Azure Blob Storage blob, which is uploaded as the output file of the experiment. This is the URI of the blob, whose name is specified at "outputFile" key from the Azure Queue experiment request message and whose Azure Blob Storage container is specified at "ResultContainer" key in "appsettings.json".
12. **trainingFileUri** : URI of the Azure Blob Storage blob, which is used as the training file for the experiment. This is the URI of the blob, whose name is specified at "trainingFile" key from the Azure Queue experiment request message and whose Azure Blob Storage container is specified at "TrainingContainer" key in "appsettings.json".