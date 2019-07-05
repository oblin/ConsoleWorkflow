using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;
using WorkflowCore.Interface;

namespace ConsoleWorkflow
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider serviceProvider = ConfigureServices();
            var host = serviceProvider.GetService<IWorkflowHost>();
            host.RegisterWorkflow<TestWorkflow, WorkItem>();
            host.Start();

            var data1 = new WorkItem { Id = "A" };
            var wfId1 = host.StartWorkflow("TestWorkflow", data1).Result;

            var data2 = new WorkItem { Id = "B" };
            var wfId2 = host.StartWorkflow("TestWorkflow", data2).Result;

            Console.WriteLine("press enter to trigger");
            Console.ReadLine();
            host.PublishEvent("Trigger", wfId2, null);

            Console.ReadLine();
            host.Stop();
        }

        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddWorkflow();

            return services.BuildServiceProvider();
        }
    }

}
