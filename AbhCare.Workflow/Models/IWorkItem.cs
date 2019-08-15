using System;

namespace AbhCare.Workflow
{
    public interface IWorkItem
    {
        string Id { get; set; }
        string WorkflowId { get; }
    }

    public abstract class WorkItem : IWorkItem
    {
        public event EventHandler StartHandler;
        public event EventHandler ErrorHandler;
        public event EventHandler FinishHandler;

        private string _workflowId;

        public string Id { get; set; }

        public string WorkflowId => _workflowId;

        public void RaiseErrorEvent(string step, string message)
        {
            ErrorHandler?.Invoke(this, new WorkItemEventArgs {
                Id = Id,
                WorkflowId = WorkflowId,
                Step = step,
                ErrorMessage = message
            });
        }

        public void RaiseStartEvent()
        {
            StartHandler?.Invoke(this, new WorkItemEventArgs
            {
                Id = Id,
                WorkflowId = WorkflowId
            });
        }

        public void RaiseFinishEvent(string step, string message)
        {
            FinishHandler?.Invoke(this, new WorkItemEventArgs
            {
                Id = Id,
                WorkflowId = WorkflowId,
                Step = step,
                ErrorMessage = message
            });
        }

        public void SetWorkflowId(string id)
        {
            this._workflowId = id;
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