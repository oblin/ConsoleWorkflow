using AbhCare.Workflow;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Testing;
using Xunit;

namespace TestExeWorkflow
{
    public class UnitTest1 : WorkflowTest<ExeWorkflow, ExeWorkItem>
    {
        private readonly IUdWorkflowConfig _fileService;
        private readonly InvokeUdExe _fakeExeStep;
        private readonly IStepExecutionContext _exeutionContext;

        public UnitTest1()
        {
            Setup();


            _fileService = Substitute.For<IUdWorkflowConfig>();
            _fakeExeStep = Substitute.For<InvokeUdExe>(_fileService);
            _fakeExeStep.Run(_exeutionContext).Returns(ExecutionResult.Next());

            _exeutionContext = Substitute.For<IStepExecutionContext>();

            //IServiceCollection services = new ServiceCollection();
            //services.AddTransient<InvokeUdExe>();   // 將 InvokedUdExe 加入才可以進行 DI
            //ConfigureServices(services);
        }

        [Fact]
        public void Test1()
        {
            var workflowId = StartWorkflow(new ExeWorkItem { Id = "1", Params = new string[] { "This", "Is", "a Test" } });
            WaitForEventSubscription("FileCreated", "1", TimeSpan.FromSeconds(10));


            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            GetStatus(workflowId).Should().Be(WorkflowStatus.Complete);
            UnhandledStepErrors.Count.Should().Be(0);
        }
    }
}
