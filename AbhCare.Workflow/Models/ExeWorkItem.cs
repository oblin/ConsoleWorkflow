using System;

namespace AbhCare.Workflow
{
    public class ExeWorkItem : WorkItem
    {
        public string Path { get; set; }

        public string[] Params => this.ComposeParameters();

        public string ForeignKey { get; set; }

        public bool HadDone { get; set; }
        public bool IsDone { get; set; }
        public DateTime? DoneDateTime { get; set; }

        public string JsonResult { get; set; }

        protected virtual string[] ComposeParameters()
        {
            throw new NotImplementedException();
        }
    }
}