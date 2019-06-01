# DevelopingDotNetOnMSAzureGettingStarted
Code for Pluralsight course by Scott Allen

If I really want to learn a subject, watching the videos is not enough, I have to follow along, type code, and try to make it work on my 
own.
This course is relatively recent, so it was not hard to make things work, even on such a dynamic environment as Azure. Most of the 
differences are in Azure portal UI, but figuring out what changed was pretty straightforward most of the times. There were occasions that
code or procedures were changed, though.
Here are a few places that come to mind:
1. Some Azure resource names are required to be unique not only within the Azure account, but within Azure itself, like domain names for
app services of Cosmos DB. Of course, these names had to be changed since the original ones were taken.
2. Cosmos DB does not have the "Fixed size" option anymore, now requiring the definition of a partition key. For this I added a "Subject" 
property to the Course model, and used it as the partition key, also populating it the the specific code section.
3. The section about Cosmos DB has code that was not typed or displayed in the videos, so I copied some sections from the final code from
the module 5 Exercise Files.
4. The final code for the module 5 Exercise files contains additional code, probably added through some VS tool or Nuget package, to 
manage
users, like a Manage Controller, an Extensions folder with a UrlHelper and EmailSender, and Manage and Account View Models. I assumed 
these elements were not essential to the use of the application nor to the use of Cosmos DB or Azure, so I left them out of the project 
(or I added and later deleted them, I don't remember now).
5. Since I was already creating and maintaining the code directly in Github, I deployed to Azure from Github, instead of using an Azure 
remote from the local repo. I even created a separate release branch to deploy into staging for the deployment slots, while master deploys 
directly to the production slot.
6. Of course, URIs and Azure keys in configuration files and/or configuration code had to be updated with the equivalent for my Azure 
environment.

## Unresolved issue with Cosmos DB

After the "Cosmos DB" commit, the code builds and deploys successfully, and the home page displays correctly (even though I forgot to 
add a menu item for "Courses".
When I try the url `https://pashelloazure.azurewebsites.net/Courses`, that correctly requests Courses/Index, I received an error page
(response code 500).
I looked into Application Insights, and saw that a System.InvalidOperationException was being thrown when the CoursesController attempeted
to activate the CourseStore, as stated in the following message:
```Unable to resolve service for type 'PsHelloAzure.Services.CourseStore' while attempting to activate 'PsHelloAzure.Controllers.CoursesController'```
AI also says the problemId was `System.InvalidOperationException at lambda_method`.
I am suspicious this has to do with the partition key defined. I entered the partition key as `/courses/subject`.
I did some research and implemented the solution suggested in the course discussion forum, recreating the container with the partition
key set to `/subject`, adding feed options with EnableCrossPartitionQuery to the DocumentQuery, and setting the partition key
JsonProperty to "subject" in the Course Model. I still got the same results, same exception, same messages in AI.
