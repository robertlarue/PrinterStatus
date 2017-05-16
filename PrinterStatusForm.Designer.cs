namespace PrinterStatus
{
    partial class PrinterStatusForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterStatusForm));
            this.PrinterGridView = new System.Windows.Forms.DataGridView();
            this.printerStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printerModelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.scanProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.printerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printerIPDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tonerStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paperStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printerDisplayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.printerTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PrinterGridView)).BeginInit();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printerTableBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // PrinterGridView
            // 
            this.PrinterGridView.AllowUserToAddRows = false;
            this.PrinterGridView.AllowUserToDeleteRows = false;
            this.PrinterGridView.AllowUserToOrderColumns = true;
            this.PrinterGridView.AutoGenerateColumns = false;
            this.PrinterGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PrinterGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PrinterGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.PrinterGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PrinterGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.printerNameDataGridViewTextBoxColumn,
            this.printerStatusDataGridViewTextBoxColumn,
            this.printerModelDataGridViewTextBoxColumn,
            this.printerIPDataGridViewTextBoxColumn,
            this.tonerStatusDataGridViewTextBoxColumn,
            this.paperStatusDataGridViewTextBoxColumn,
            this.printerDisplayDataGridViewTextBoxColumn});
            this.PrinterGridView.DataSource = this.printerTableBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PrinterGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.PrinterGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PrinterGridView.Location = new System.Drawing.Point(0, 0);
            this.PrinterGridView.MultiSelect = false;
            this.PrinterGridView.Name = "PrinterGridView";
            this.PrinterGridView.ReadOnly = true;
            this.PrinterGridView.RowHeadersVisible = false;
            this.PrinterGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.PrinterGridView.Size = new System.Drawing.Size(700, 293);
            this.PrinterGridView.TabIndex = 0;
            this.PrinterGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PrinterGridView_CellClick);
            this.PrinterGridView.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.PrinterGridView_CellEnter);
            this.PrinterGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.PrinterGridView_ColumnHeaderMouseClick);
            this.PrinterGridView.SelectionChanged += new System.EventHandler(this.PrinterGridView_SelectionChanged);
            // 
            // printerStatusDataGridViewTextBoxColumn
            // 
            this.printerStatusDataGridViewTextBoxColumn.DataPropertyName = "PrinterStatus";
            this.printerStatusDataGridViewTextBoxColumn.FillWeight = 116.9543F;
            this.printerStatusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.printerStatusDataGridViewTextBoxColumn.Name = "printerStatusDataGridViewTextBoxColumn";
            this.printerStatusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // printerModelDataGridViewTextBoxColumn
            // 
            this.printerModelDataGridViewTextBoxColumn.DataPropertyName = "PrinterModel";
            this.printerModelDataGridViewTextBoxColumn.FillWeight = 116.9543F;
            this.printerModelDataGridViewTextBoxColumn.HeaderText = "Model";
            this.printerModelDataGridViewTextBoxColumn.Name = "printerModelDataGridViewTextBoxColumn";
            this.printerModelDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scanProgressLabel,
            this.progressBar,
            this.statusText});
            this.statusStrip.Location = new System.Drawing.Point(0, 271);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(700, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // scanProgressLabel
            // 
            this.scanProgressLabel.Name = "scanProgressLabel";
            this.scanProgressLabel.Size = new System.Drawing.Size(80, 17);
            this.scanProgressLabel.Text = "Scan Progress";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(65, 17);
            this.statusText.Text = "Scanning...";
            // 
            // printerNameDataGridViewTextBoxColumn
            // 
            this.printerNameDataGridViewTextBoxColumn.DataPropertyName = "PrinterName";
            this.printerNameDataGridViewTextBoxColumn.FillWeight = 116.9543F;
            this.printerNameDataGridViewTextBoxColumn.HeaderText = "Printer";
            this.printerNameDataGridViewTextBoxColumn.Name = "printerNameDataGridViewTextBoxColumn";
            this.printerNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // printerIPDataGridViewTextBoxColumn
            // 
            this.printerIPDataGridViewTextBoxColumn.DataPropertyName = "PrinterIP";
            this.printerIPDataGridViewTextBoxColumn.FillWeight = 116.9543F;
            this.printerIPDataGridViewTextBoxColumn.HeaderText = "Printer IP";
            this.printerIPDataGridViewTextBoxColumn.Name = "printerIPDataGridViewTextBoxColumn";
            this.printerIPDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tonerStatusDataGridViewTextBoxColumn
            // 
            this.tonerStatusDataGridViewTextBoxColumn.DataPropertyName = "TonerStatus";
            this.tonerStatusDataGridViewTextBoxColumn.FillWeight = 116.9543F;
            this.tonerStatusDataGridViewTextBoxColumn.HeaderText = "Toner Status";
            this.tonerStatusDataGridViewTextBoxColumn.Name = "tonerStatusDataGridViewTextBoxColumn";
            this.tonerStatusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // paperStatusDataGridViewTextBoxColumn
            // 
            this.paperStatusDataGridViewTextBoxColumn.DataPropertyName = "PaperStatus";
            this.paperStatusDataGridViewTextBoxColumn.FillWeight = 116.9543F;
            this.paperStatusDataGridViewTextBoxColumn.HeaderText = "Paper Status";
            this.paperStatusDataGridViewTextBoxColumn.Name = "paperStatusDataGridViewTextBoxColumn";
            this.paperStatusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // printerDisplayDataGridViewTextBoxColumn
            // 
            this.printerDisplayDataGridViewTextBoxColumn.DataPropertyName = "PrinterDisplay";
            this.printerDisplayDataGridViewTextBoxColumn.FillWeight = 116.9543F;
            this.printerDisplayDataGridViewTextBoxColumn.HeaderText = "Printer Display";
            this.printerDisplayDataGridViewTextBoxColumn.Name = "printerDisplayDataGridViewTextBoxColumn";
            this.printerDisplayDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // printerTableBindingSource
            // 
            this.printerTableBindingSource.DataSource = typeof(PrinterStatus.PrinterTable);
            // 
            // PrinterStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 293);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.PrinterGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrinterStatusForm";
            this.Text = "Printer Status";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PrinterStatusForm_FormClosed);
            this.Load += new System.EventHandler(this.PrinterStatusForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PrinterGridView)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.printerTableBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView PrinterGridView;
        private System.Windows.Forms.BindingSource printerTableBindingSource;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel scanProgressLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.DataGridViewTextBoxColumn printerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn printerStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn printerModelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn printerIPDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tonerStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paperStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn printerDisplayDataGridViewTextBoxColumn;
    }
}