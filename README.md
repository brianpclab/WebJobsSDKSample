# WebJobsSDKSample
This web job uses a TimerTrigger, Application Insights and Serilog.
Issues to be aware of.
You might have to setup Azurite see the link below:
when running the web job in development ensure you always end the job with <control> + c otherwise
it seems like it hold a lock open within the storage emulator.

Be sure to add a connection string in the appservice for the AzureWebJobsDashboard otherwise the web job will not run in the cloud.
it does not seem to be necessary for it to run in the development environment.

Also will probably want to add the AzureWebJobsStorage connection string in the appservice because the one in appsettings.json is for Azurite.

Notice that it uses dependency injection which should always be a requirement.
I created this because for whatever reason the one I developed for my job is not working so this is my template to get that one functional.

Azurite Links
https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?toc=%2Fazure%2Fstorage%2Fblobs%2Ftoc.json&bc=%2Fazure%2Fstorage%2Fblobs%2Fbreadcrumb%2Ftoc.json&tabs=visual-studio#http-connection-strings

Serilog Links
Application Insights
https://github.com/serilog-contrib/serilog-sinks-applicationinsights
Generic Hosting
https://github.com/serilog/serilog-extensions-hosting

Azure Functions Timer Trigger
https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer?tabs=in-process&pivots=programming-language-csharp

Application Insights
https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Logging.ApplicationInsights

Microsoft Tutorial
https://learn.microsoft.com/en-us/azure/app-service/webjobs-sdk-get-started
