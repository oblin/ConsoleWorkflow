using Microsoft.Extensions.DependencyInjection;
using System;
using WorkflowCore.Interface;

namespace ConsoleUserWorkflow
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceProvider serviceProvider = ConfigureServices();
            var host = serviceProvider.GetService<IWorkflowHost>();
            host.RegisterWorkflow<TestWorkflow, Approval>();
            host.Start();

            var approval = new Approval
            {
                UserList = new System.Collections.Generic.List<User>
                {
                    new User(),
                    new User()
                }
            };

            var wfId = host.StartWorkflow("TestWorkflow", approval).Result;

            while (true) { };
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
