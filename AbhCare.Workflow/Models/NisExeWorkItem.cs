using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbhCare.Workflow
{
    public class NisExeWorkItem : ExeWorkItem
    {
        public NisExeWorkItem(string orgNo, string feeNo, string regNo, string priceCode, string priceName, string startDate, string startTime, 
            string takeTime, string orderId, int qty, string operation, string outputFolder,
            string endDate = null, string endTime = null)
        {
            this.OrgNo = orgNo;
            FeeNo = feeNo;
            RegNo = regNo;
            PriceCode = priceCode;
            PriceName = priceName;
            StartDate = startDate;
            StartTime = startTime;
            TakeTime = takeTime;
            Qty = qty;
            Operation = operation;
            EndDate = endDate ?? string.Empty;
            EndTime = endTime ?? string.Empty;
            FilePath = outputFolder;
            FileName = orderId;
            Id = orderId;
        }

        public override string[] ComposeParameters()
        {
            //if (string.IsNullOrEmpty(WorkflowId)) throw new ArgumentNullException();

            //FileName = WorkflowId;

            return new string[]
            {
                //",,01,SUPER01",     // a.	權限：請固定填入→,,01,SUPER01
                //"nis_order",        // b.	作業代碼：此為開立護囑指令的辨識代碼
                GetSeperatedParameters()  //  c.  參數共15個，中間用分號隔開
            };
        }

        private string GetSeperatedParameters()
        {
            return OrgNo + ";"
                + FeeNo + ";"
                + RegNo + ";"
                + PriceCode + ";"
                + PriceName + ";"
                + StartDate + ";"
                + StartTime + ";"
                + EndDate + ";"
                + EndTime + ";"
                + Qty.ToString() + ";"
                + TakeTime + ";"
                + Id + ";"
                + Operation + ";"
                + FilePath + ";"
                + FileName;
        }

        /// <summary>
        /// 參數1：機構別
        /// </summary>
        public string OrgNo { get; set; }
        /// <summary>
        /// 參數2：當次就診序號
        /// </summary>
        public string FeeNo { get; set; }
        /// <summary>
        /// 參數3：住民號（病歷號）
        /// </summary>
        public string RegNo { get; set; }
        /// <summary>
        /// 參數4：批價代碼（或護囑代碼，NE開頭4碼）（可以空白）
        /// </summary>
        public string PriceCode { get; set; }
        /// <summary>
        /// 參數5：批價名稱（或護囑名稱）（不可空白）
        /// </summary>
        public string PriceName { get; set; }
        /// <summary>
        /// 參數6：起始日期（yyyymmdd）
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 參數7：起始時間（hhmm）
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 參數8：結束日期（yyyymmdd）（可空白）
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 參數9：結束時間（hhmm）（可空白）
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 參數10：次量（數字，不可為0或負數）
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 參數11：頻率
        /// </summary>
        public string TakeTime { get; set; }
        /// <summary>
        /// TODO: 修正改為使用 Id
        /// 參數12：來源護囑的uuid欄位
        /// </summary>
        [Obsolete]
        public string OrderId { get; set; }
        /// <summary>
        /// 參數13：此次護囑指令動作（新增護囑:A 修改原護囑:E 刪除護囑:D）
        /// </summary>
        public string Operation { get; set; }
        /// <summary>
        /// 參數14：產生路徑
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 參數15：產生的檔案名稱（例如： e3ef9280-5f72-458a-a33f-0de38d1ffc62）
        /// </summary>
        public string FileName { get; set; }
    }
}
