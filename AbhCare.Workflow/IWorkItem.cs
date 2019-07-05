using System;

namespace AbhCare.Workflow
{
    public interface IWorkItem
    {
        string Id { get; set; }
        string WorkflowId { get; set; }
    }

    public abstract class WorkItem : IWorkItem
    {
        public event EventHandler StartHandler;
        public event EventHandler ErrorHandler;
        public event EventHandler FinishHandler;

        public string Id { get; set; }

        public string WorkflowId { get; set; }

        public void RaiseError(string step, string message)
        {
            ErrorHandler?.Invoke(this, new WorkItemEventArgs {
                Id = Id,
                WorkflowId = WorkflowId,
                Step = step,
                ErrorMessage = message
            });
        }

        public void RaiseStart()
        {
            StartHandler?.Invoke(this, new WorkItemEventArgs
            {
                Id = Id,
                WorkflowId = WorkflowId
            });
        }

        public void RaiseFinish(string step, string message)
        {
            FinishHandler?.Invoke(this, new WorkItemEventArgs
            {
                Id = Id,
                WorkflowId = WorkflowId,
                Step = step,
                ErrorMessage = message
            });
        }
    }

    public class WorkItemEventArgs : EventArgs, IWorkItem
    {
        public string Id { get; set; }
        public string WorkflowId { get; set; }

        public string Step { get; set; }
        public string ErrorMessage { get; set; }
    }
}