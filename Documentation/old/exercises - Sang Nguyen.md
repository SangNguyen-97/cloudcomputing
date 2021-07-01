## Sang Nguyen's exercises

#### Exercise 1

Login and check Azure account infos

$ az login -u <user-name> # Type in password from standard input in order to improve security
$ az account list # List all subscriptions in the current directory
$ az account set -s <Subscription-name/ID> # Switch to a specific subscription
$ az logout



#### Exercise 2

1. URL to docker file: 
	https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2019-2020/blob/SangNguyen/MyWork/Cloud%20Exercise/Exercise%203/MyExercise3/Dockerfile
2. URL to Docker Hub image: 
	https://hub.docker.com/repository/docker/sangnguyenfrauas/exercise2
3. URL to Azure Container Registry image: 
	webapplearningregistry.azurecr.io/exercise04:latest



#### Exercise 3

1. URL of the Web App: 
	https://webapplearningsite.azurewebsites.net/
2. URL to the Web App's source code:
	https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2019-2020/tree/SangNguyen/MyWork/Cloud%20Exercise/Exercise%203/MyExercise3
3. Command to deploy source code to the Web App from zip file:
$ az webapp deployment source config-zip --src mysite.zip --resource-group webapplearning-rg --name webapplearningsite

Please contact me before validating this exercise. Azure Web App service does not work well sometimes. Therefore, I have to recreate a new Web App every times.  



#### Exercise 4

1. URL to the image (private Azure Container Registry):
	webapplearningregistry.azurecr.io/exercise04:latest
2. URL to the Web App: 
	https://webapplearningsite.azurewebsites.net/

Please contact me before validating this exercise. Azure Web App service does not work well sometimes. Therefore, I have to recreate a new Web App every times.  



#### Exercise 5

The blob used for this exercise is "test.txt" in container "sangnguyen-exercise05"

URL of container "sangnguyen-exercise05": https://webapplearningstorage.blob.core.windows.net/sangnguyen-exercise05

Blob "test.txt" SAS token: sp=r&st=2020-08-15T12:30:08Z&se=2021-08-15T12:30:08Z&spr=https&sv=2019-12-12&sr=b&sig=TOFa9kpxnYyVy61afTlBmC84N%2F9jlLpo5A%2F6QPRgDTU%3D

Blob "test.txt" SAS URL: https://webapplearningstorage.blob.core.windows.net/sangnguyen-exercise05/test.txt?sp=r&st=2020-08-15T12:30:08Z&se=2021-08-15T12:30:08Z&spr=https&sv=2019-12-12&sr=b&sig=TOFa9kpxnYyVy61afTlBmC84N%2F9jlLpo5A%2F6QPRgDTU%3D

URL to Visual Studio C# project demonstrating sample operation on Azure Blob Storage service:
https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2019-2020/tree/SangNguyen/MyWork/Cloud%20Exercise/Exercise%205/AzureBlobStorageSample



#### Exercise 6

URL to Visual Studio C# project demonstrating sample operation on Azure Table Storage service:
https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2019-2020/tree/SangNguyen/MyWork/Cloud%20Exercise/Exercise%206/Cosmos%20DB%20table%20sample



#### Exercise 7

URL to Visual Studio C# project demonstrating sample operation on Azure Queue Storage service:
https://github.com/UniversityOfAppliedSciencesFrankfurt/se-cloud-2019-2020/tree/SangNguyen/MyWork/Cloud%20Exercise/Exercise%207/StorageQueueSample