using AbhCare.Workflow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using WorkflowCore.Interface;

namespace ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider serviceProvider = ConfigureServices();

            var hostService = serviceProvider.GetService<ExeWorkflowService>();
            // TODO: make it @ appsettings
            hostService.Start("d:\\Temp\\", "FileCreated");

            ExeWorkItem exeParam = hostService.Add(Guid.NewGuid().ToString(), new string[] { "fk100", "AYN" });
            exeParam.StartHandler += ExeParam_StartHandler;
            exeParam.ErrorHandler += ExeParam_ErrorHandler;
            exeParam.FinishHandler += ExeParam_FinishHandler;

            Console.ReadLine();
            Console.ReadLine();
            hostService.Stop();
        }

        private static void ExeParam_FinishHandler(object sender, EventArgs e)
        {
            var item = e as WorkItemEventArgs;
            Console.WriteLine($"Work item {item.Id} had been Finished, foreign key: {item.ErrorMessage}");
        }

        private static void ExeParam_StartHandler(object sender, EventArgs e)
        {
            var item = e as WorkItemEventArgs;
            Console.WriteLine($"Work item {item.Id} had been Started");
        }
        private static void ExeParam_ErrorHandler(object sender, EventArgs e)
        {
            var item = e as WorkItemEventArgs;
            Console.WriteLine($"Work item {item.Id} at {item.Step} step had been cancel, message: {item.ErrorMessage}");
        }

        private static void Workflow_OnStepError(WorkflowCore.Models.WorkflowInstance workflow, WorkflowCore.Models.WorkflowStep step, Exception exception)
        {
            var exeParam = workflow.Data as ExeWorkItem;
            
            Console.WriteLine($"workflow id: {workflow.Id} FAILED! exe params = {exeParam.Id} - {String.Join(',', exeParam.Params)}");
            Console.WriteLine($"Step: {step.Name}, reason: {exception.Message}");
        }


        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(builder => builder.AddDebug());
            services.AddWorkflow();
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));
            services.AddSingleton<ExeWorkflowService>();

            return services.BuildServiceProvider();
        }
    }
}
