using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Models.Search;
using WorkflowCore.Persistence.EntityFramework.Interfaces;
using WorkflowCore.Persistence.EntityFramework.Models;
using WorkflowCore.Persistence.EntityFramework.Services;

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

            //var wfId = host.StartWorkflow("TestWorkflow", approval).Result;

            var efProvider = serviceProvider.GetService<IPersistenceProvider>();
            PrintPersistance(efProvider);

            //var context = serviceProvider.GetService<IWorkflowDbContextFactory>().Build();
            ////var context = contextFactory.Build();
            //var ids = context.Set<PersistedWorkflow>()
            //    .Where(p => p.Status == WorkflowStatus.Complete)
            //    .Select(s => s.InstanceId.ToString()).ToList();

            var searchProvice = serviceProvider.GetService<ISearchIndex>();
            //var ids = searchProvice.Search("Status", 0, 10, StatusFilter.Equals(WorkflowStatus.Complete)).Result;
            var ids = searchProvice.Search("", 0, 10, DateRangeFilter.After(x => x.CreateTime, new DateTime(2019, 7, 10))).Result;
            var items = PrintCompleteWorkflow(efProvider, null);

            while (true) { };
        }

        private static void PrintPersistance(IPersistenceProvider efProvider)
        {
            var instances = efProvider.GetRunnableInstances(DateTime.Now).Result;

            foreach (var i in instances)
                Console.WriteLine($"instance: {i}");

            Console.WriteLine("--- Current Workflow ----");
            var items = efProvider.GetWorkflowInstances(instances).Result;
            foreach (var item in items)
            {
                var data = item.Data as Approval;
                Console.WriteLine($"{item.Id}, users: {string.Join(',', data.UserList.Select(u => u.Id))}");
                Console.WriteLine($"create date: {item.CreateTime}, complete date: {item.CompleteTime}");
                Console.WriteLine($"next execution: {item.NextExecution}, reference : {item.Reference}");
                Console.WriteLine($"WorkflowDefinitionId: {item.WorkflowDefinitionId}, Status : {Enum.GetName(typeof(WorkflowStatus), item.Status)}");
            }

        }

        private static IEnumerable<WorkflowInstance> PrintCompleteWorkflow(IPersistenceProvider efProvider,
            IEnumerable<string> ids)
        {
            var instances = efProvider.GetWorkflowInstances(WorkflowStatus.Complete, "", null, null, 0, 10).Result;
            System.Collections.Generic.IEnumerable<WorkflowInstance> items;
            Console.WriteLine($"--- Closed Workflow --- total: {instances.Count()}");

            items = efProvider.GetWorkflowInstances(instances.Select(s => s.Id)).Result;
            foreach (var item in items)
            {
                var data = item.Data as Approval;
                Console.WriteLine($"{item.Id}, users: {string.Join(',', data.UserList.Select(u => u.Id))}");
                Console.WriteLine($"create date: {item.CreateTime}, complete date: {item.CompleteTime}");
                Console.WriteLine($"next execution: {item.NextExecution}, reference : {item.Reference}");
                Console.WriteLine($"WorkflowDefinitionId: {item.WorkflowDefinitionId}, Status : {Enum.GetName(typeof(WorkflowStatus), item.Status)}");
            }

            return items;
        }

        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(log => log.AddConsole());
            
            services.AddWorkflow(config => {
                config.
                    UsePostgreSQL(@"Server=localhost;Port=5432;Database=workflow;User Id=postgres;Password=490910;", true, true);
                config.UseElasticsearch(new Nest.ConnectionSettings(new Uri("http://localhost:9200")), "indexname");
                    }
            );

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
