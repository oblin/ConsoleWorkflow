using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Models.LifeCycleEvents;
using WorkflowCore.Models.Search;
using WorkflowCore.Persistence.EntityFramework.Interfaces;
using WorkflowCore.Persistence.EntityFramework.Models;
using WorkflowCore.Persistence.EntityFramework.Services;
using WorkflowCore.Persistence.PostgreSQL;

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
            host.OnLifeCycleEvent += Host_OnLifeCycleEvent;

            var approval = new Approval
            {
                UserList = new System.Collections.Generic.List<User>
                {
                    new User(),
                    new User()
                }
            };

            //var wfId = host.StartWorkflow("TestWorkflow", approval).Result;

            var context = serviceProvider.GetService<IWorkflowDbContextFactory>().Build();
            //var context = contextFactory.Build();
            var ids = context.Set<PersistedWorkflow>()
                .Where(p => p.Status == WorkflowStatus.Complete)
                .Select(s => s.InstanceId.ToString()).ToList();


            // Use efProvider.GetWorkflowInstances
            var efProvider = serviceProvider.GetService<IPersistenceProvider>();
            //PrintPersistance(efProvider);
            //PrintCompleteWorkflow(efProvider);

            // Use UseElasticsearch
            var searchprovice = serviceProvider.GetService<ISearchIndex>();
            var page = searchprovice.Search("", 0, 10, StatusFilter.Equals(WorkflowStatus.Complete)).Result;
            PrintCompleteWorkflow(efProvider, page);

            while (true) { };
        }

        private static void Host_OnLifeCycleEvent(WorkflowCore.Models.LifeCycleEvents.LifeCycleEvent evt)
        {
            if (evt is WorkflowSuspended)
            {
                var type = evt as WorkflowSuspended;
                Console.WriteLine($"Life cycle type: {nameof(WorkflowSuspended)}, workflow id {type.WorkflowInstanceId}");
            }
            else if (evt is WorkflowStarted)
            {
                var type = evt as WorkflowStarted;
                Console.WriteLine($"Life cycle type: {nameof(WorkflowStarted)}, workflow id {type.WorkflowInstanceId}");
            }
            else if (evt is WorkflowCompleted)
            {
                var type = evt as WorkflowCompleted;
                Console.WriteLine($"Life cycle type: {nameof(WorkflowCompleted)}, workflow id {type.WorkflowInstanceId}");
            }
            else if (evt is WorkflowError)
            {
                var type = evt as WorkflowError;
                Console.WriteLine($"Life cycle type: {nameof(WorkflowError)}, workflow id {type.WorkflowInstanceId}, message: {type.Message}, step: {type.StepId}, ExecutionPointerId: {type.ExecutionPointerId}");
            }
            else if (evt is WorkflowResumed)
            {
                var type = evt as WorkflowResumed;
                Console.WriteLine($"Life cycle type: {nameof(WorkflowResumed)}, workflow id {type.WorkflowInstanceId}");
            }
            else if (evt is WorkflowTerminated)
            {
                var type = evt as WorkflowTerminated;
                Console.WriteLine($"Life cycle type: {nameof(WorkflowTerminated)}, workflow id {type.WorkflowInstanceId}");
            }
            else if (evt is StepCompleted)
            {
                var type = evt as StepCompleted;
                Console.WriteLine($"Life cycle type: {nameof(StepCompleted)}, workflow id {type.WorkflowInstanceId}, step: {type.StepId}, ExecutionPointerId: {type.ExecutionPointerId}");
            }
            else if (evt is StepStarted)
            {
                var type = evt as StepStarted;
                Console.WriteLine($"Life cycle type: {nameof(StepStarted)}, workflow id {type.WorkflowInstanceId}, step: {type.StepId}, ExecutionPointerId: {type.ExecutionPointerId}");
            }
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

        private static IEnumerable<WorkflowInstance> PrintCompleteWorkflow(IPersistenceProvider efProvider)
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

        private static void PrintCompleteWorkflow(IPersistenceProvider efProvider,
            Page<WorkflowSearchResult> page)
        {
            Console.WriteLine($"--- Closed Workflow --- total: {page.Total}");

            foreach (var item in page.Data)
            {
                var data = item.Data as Approval;
                Console.WriteLine($"{item.Id}, users: {string.Join(',', data.UserList.Select(u => u.Id))}");
                Console.WriteLine($"create date: {item.CreateTime}, complete date: {item.CompleteTime}");
                Console.WriteLine($"Description: {item.Description}, reference : {item.Reference}");
                Console.WriteLine($"WorkflowDefinitionId: {item.WorkflowDefinitionId}, Status : {Enum.GetName(typeof(WorkflowStatus), item.Status)}");
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(log => log.AddConsole());
            var connectionString = @"Server=localhost;Port=5432;Database=workflow;User Id=postgres;Password=490910;";
            services.AddWorkflow(config => {
                config.
                    UsePostgreSQL(connectionString, true, true);
                config.UseElasticsearch(new Nest.ConnectionSettings(new Uri("http://localhost:9200")), "indexname");
            }
            );

            services.AddSingleton<IWorkflowDbContextFactory>(new PostgresContextFactory(connectionString));
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
