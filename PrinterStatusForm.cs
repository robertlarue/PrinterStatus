using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace PrinterStatus
{
    public partial class PrinterStatusForm : Form
    {
        delegate void RefreshPrinterGridViewCallback();
        delegate void SetProgressBarCallback(int percentDone);
        delegate void SetStatusTextCallback(string status);
        delegate void StopUpdatingCallback();
        delegate void StartUpdatingCallback();
        public static DataGridViewSelectedCellCollection selectedCells;
        public static DataGridViewColumn sortingColumn;
        public static ListSortDirection sortingDirection = ListSortDirection.Ascending;



        public PrinterStatusForm()
        {
            InitializeComponent();
        }
        private void ConditionalFormatting()
        {
            foreach (DataGridViewRow printerGridViewRow in PrinterGridView.Rows)
            {
                DataGridViewCell tonerCell = printerGridViewRow.Cells[4];
                string tonerStatus = "";
                if(tonerCell.Value != null)
                {
                    tonerStatus = tonerCell.Value.ToString();
                }
                if (tonerStatus != "")
                {
                    MatchCollection matches = Regex.Matches(tonerStatus, @"(\d{1,3})%.+", RegexOptions.Multiline);
                    bool tonerCritical = false;
                    bool tonerWarning = false;
                    
                    foreach (Match match in matches)
                    {
                        int percentUsed = int.Parse(match.Groups[1].ToString());
                        if (percentUsed < Program.tonerCriticalThreshold)
                        {
                            tonerCritical = true;
                            break;
                        }
                        else if (percentUsed < Program.tonerWarningThreshold)
                        {
                            tonerWarning = true;
                        }
                    }
                    if (tonerCritical)
                    {
                        tonerCell.Style.BackColor = Color.LightCoral;
                    }
                    else if(tonerWarning){
                        tonerCell.Style.BackColor = Color.Khaki;
                    }
                    else
                    {
                        tonerCell.Style = tonerCell.OwningColumn.DefaultCellStyle;
                    }
                }

                DataGridViewCell paperCell = printerGridViewRow.Cells[5];
                string paperStatus = paperCell.Value.ToString();
                if (paperStatus != "")
                {
                    Match matchEmpty = Regex.Match(paperStatus, @".*Empty.*", RegexOptions.Singleline);
                    Match matchOpen = Regex.Match(paperStatus, @".*Tray Open.*", RegexOptions.Singleline);
                    if (matchEmpty.Success)
                    {
                        paperCell.Style.BackColor = Color.LightCoral;
                    }
                    else if (matchOpen.Success)
                    {
                        paperCell.Style.BackColor = Color.Khaki;
                    }
                    else
                    {
                        paperCell.Style = paperCell.OwningColumn.DefaultCellStyle;
                    }
                }
            }
        }
        //public void RefreshPrinterGridView()
        //{
        //    // InvokeRequired required compares the thread ID of the
        //    // calling thread to the thread ID of the creating thread.
        //    // If these threads are different, it returns true.
        //    if (this.PrinterGridView.InvokeRequired)
        //    {
        //        RefreshPrinterGridViewCallback d = new RefreshPrinterGridViewCallback(RefreshPrinterGridView);
        //        try
        //        {
        //            this.Invoke(d, new object[] {});
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine(ex.StackTrace);
        //        }
        //    }
        //    else
        //    {
        //        //while (Program.updatingTable)
        //        //{
        //        //    Thread.Sleep(100);
        //        //}
        //        //Program.printerBindingSource.ResetBindings(false);
        //        PrinterGridView.Sort(this.printerNameDataGridViewTextBoxColumn, ListSortDirection.Ascending);
        //        ConditionalFormatting();
                
        //    }
        //}
        public void SetProgressBar(int percentDone)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.statusStrip.InvokeRequired)
            {
                SetProgressBarCallback d = new SetProgressBarCallback(SetProgressBar);
                try
                {
                    this.Invoke(d, new object[] {percentDone});
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Application.Exit();
                }
            }
            else
            {
                this.progressBar.Value = percentDone;
            }
        }

        public void StopUpdating()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.statusStrip.InvokeRequired)
            {
                StopUpdatingCallback d = new StopUpdatingCallback(StopUpdating);
                try
                {
                    this.Invoke(d, new object[] { });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Application.Exit();
                }
            }
            else
            {
                //Program.printerBindingSource.DataSource = Program.printerTableLast;
                //if (!Program.updatingTable)
                //{
                Program.updatingTable = true;
                PrinterGridView.DataSource = Program.printerTableLast;
                //PrinterGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                if (sortingColumn != null)
                {
                    PrinterGridView.Sort(sortingColumn, sortingDirection);
                }
                ConditionalFormatting();
                SelectCell(Program.selectedY, Program.selectedX);
                //SelectCellCollection();

                Program.updatingTable = false;
                //}
                //PrinterGridView.Sort(this.printerNameDataGridViewTextBoxColumn, ListSortDirection.Ascending);
            }
        }

        public void StartUpdating()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.statusStrip.InvokeRequired)
            {
                StartUpdatingCallback d = new StartUpdatingCallback(StartUpdating);
                try
                {
                    this.Invoke(d, new object[] { });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Application.Exit();
                }
            }
            else
            {
                //if (!Program.updatingTable)
                //{
                Program.updatingTable = true;
                PrinterGridView.DataSource = Program.printerTable;
                //PrinterGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                if (sortingColumn != null)
                {
                    PrinterGridView.Sort(sortingColumn, sortingDirection);
                }
                ConditionalFormatting();
                SelectCell(Program.selectedY, Program.selectedX);
                //SelectCellCollection();
                Program.updatingTable = false;
                //}
            }
        }
        private void SelectCell(int row, int col)
        {
            if(row == -1 && col == -1)
            {
                PrinterGridView.CurrentCell.Selected = false;
                return;
            }
            if (row < PrinterGridView.Rows.Count)
            {
                if(col < PrinterGridView.Rows[row].Cells.Count)
                {
                    PrinterGridView.CurrentCell = PrinterGridView.Rows[row].Cells[col];
                }
            }
        }
        private void SelectCellCollection()
        {
            if (selectedCells != null)
            {
                foreach (DataGridViewCell cell in selectedCells)
                {
                    if (cell.RowIndex > 0 && cell.RowIndex < PrinterGridView.Rows.Count)
                    {
                        if (cell.ColumnIndex > 0 && cell.ColumnIndex < PrinterGridView.Rows[cell.RowIndex].Cells.Count)
                        {
                            PrinterGridView.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Selected = true;
                        }
                    }
                }
            }
        }
        public void SetStatusText(string status)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.statusStrip.InvokeRequired)
            {
                SetStatusTextCallback d = new SetStatusTextCallback(SetStatusText);
                try
                {
                    this.Invoke(d, new object[] { status });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Application.Exit();
                }
            }
            else
            {
                this.statusText.Text = status;
            }
        }

        private void PrinterStatusForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void PrinterGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!Program.updatingTable)
            {
                Program.selectedX = e.ColumnIndex;
                Program.selectedY = e.RowIndex;
            }
        }

        private void PrinterGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!Program.selectionChanged)
            {
                PrinterGridView.ClearSelection();
                Program.selectedX = -1;
                Program.selectedY = -1;
                Program.selectionChanged = true;
            }
            else
            {
                Program.selectionChanged = false;
            }
        }

        private void PrinterGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (!Program.updatingTable)
            {
                Program.selectionChanged = true;
            }
        }

        private void PrinterGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            sortingColumn = PrinterGridView.SortedColumn;
            if(PrinterGridView.SortOrder == SortOrder.Ascending)
            {
                sortingDirection = ListSortDirection.Ascending;
            }
            else
            {
                sortingDirection = ListSortDirection.Descending;
            }
            ConditionalFormatting();
        }

        private void PrinterStatusForm_Load(object sender, EventArgs e)
        {
            sortingColumn = PrinterGridView.Columns[0];
        }
    }
}
