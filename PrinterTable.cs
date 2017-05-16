using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace PrinterStatus
{
    public class PrinterTable : DataTable
    {
        protected override Type GetRowType()
        {
            return typeof(PrinterRow);
        }
        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new PrinterRow(builder);
        }
        public PrinterTable()
        {
            Columns.Add(new DataColumn("PrinterName", typeof(string)));
            Columns.Add(new DataColumn("PrinterStatus", typeof(string)));
            Columns.Add(new DataColumn("PrinterModel", typeof(string)));
            Columns.Add(new DataColumn("PrinterIP", typeof(string)));
            Columns.Add(new DataColumn("TonerStatus", typeof(string)));
            Columns.Add(new DataColumn("PaperStatus", typeof(string)));
            Columns.Add(new DataColumn("PrinterDisplay", typeof(string)));
        }
        public PrinterRow this[int idx]
        {
            get { return (PrinterRow)Rows[idx]; }
        }
        public void Add(PrinterRow row)
        {
            Rows.Add(row);
        }
        public void Remove(PrinterRow row)
        {
            Rows.Remove(row);
        }
        public PrinterRow GetNewRow()
        {
            PrinterRow row = (PrinterRow)NewRow();

            return row;
        }
    }
    public class PrinterRow : DataRow
    {
        internal PrinterRow(DataRowBuilder builder) : base(builder)
        {
            PrinterName = String.Empty;
            PrinterModel = String.Empty;
            PrinterIP = String.Empty;
            TonerStatus = String.Empty;
            PaperStatus = String.Empty;
            PrinterDisplay = String.Empty;
        }
        public string PrinterName
        {
            get { return (string)base["PrinterName"]; }
            set { base["PrinterName"] = value; }
        }

        public string PrinterStatus
        {
            get { return (string)base["PrinterStatus"]; }
            set { base["PrinterStatus"] = value; }
        }

        public string PrinterModel
        {
            get { return (string)base["PrinterModel"]; }
            set { base["PrinterModel"] = value; }
        }

        public string PrinterIP
        {
            get { return (string)base["PrinterIP"]; }
            set { base["PrinterIP"] = value; }
        }

        public string TonerStatus
        {
            get { return (string)base["TonerStatus"]; }
            set { base["TonerStatus"] = value; }
        }

        public string PaperStatus
        {
            get { return (string)base["PaperStatus"]; }
            set { base["PaperStatus"] = value; }
        }

        public string PrinterDisplay
        {
            get { return (string)base["PrinterDisplay"]; }
            set { base["PrinterDisplay"] = value; }
        }
    }
}
