using AbhCare.Workflow.Steps;
using Microsoft.Extensions.Configuration;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AbhCare.Workflow
{
    public class ExeWorkflow : IWorkflow<ExeWorkItem>
    {
        public string Id => nameof(ExeWorkflow);

        public int Version => 1;

        public string ExePath { get; set; }

        public void Build(IWorkflowBuilder<ExeWorkItem> builder)
        {
            builder
                // 啟動執行 (Path) 加上參數 (Params)
                .StartWith<GenDocStep>()
                    .Input(s => s.Params, d => d.ComposeParameters())
                    .Input(s => s.WorkItem, d => d)
                .Parallel()
                    .Do(then => then.StartWith(context => ExecutionResult.Next())
                        // DetectFileResult 確認執行後，就會觸動 FileCreated Event
                        .WaitFor("FileCreated", (data, context) => data.Id)
                            .Output(s => s.ForeignKey, step => step.EventData)
                            .Output(s => s.DoneDateTime, _ => DateTime.Now)
                        .Then<NotifyFinishWorkflow>()
                            // 參數必須要是 data object，不可以是其屬性（會無法呼叫）
                            .Input(s => s.WorkItem, d => d)
                            .Output(d => d.IsDone, _ => true)
                    )
                    .Do(then => then.StartWith(_ => ExecutionResult.Next())
                        // 檢查執行超過 30 分鐘，就直接宣告失敗
                        .Delay(_ => TimeSpan.FromMinutes(30))
                        .Then<NofifyWorkflowTimeout>()
                            .Input(s => s.WorkItem, d => d)
                            .Output(d => d.IsDone, _ => true)
                    )
                .Join()
                    // 一旦結束： IsDone = true 就不再執行
                    .CancelCondition(d => d.IsDone, true)
                // 任何步驟失敗就結束此流程（處理機制放在 Workflow_OnStepError）
                .OnError(WorkflowErrorHandling.Terminate)
                .Then<ExecutedResult>()
                    .Input(s => s.DoneTime, d => d.DoneDateTime);
        }
    }
}
