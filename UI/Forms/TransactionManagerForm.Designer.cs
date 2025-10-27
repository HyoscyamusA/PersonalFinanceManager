namespace PersonalFinanceManager.UI.Forms
{
    partial class TransactionManagerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewTransactions;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cmbFilterAccount;
        private System.Windows.Forms.ComboBox cmbFilterType;
        private System.Windows.Forms.DateTimePicker dtpFilterStartDate;
        private System.Windows.Forms.DateTimePicker dtpFilterEndDate;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblSummary;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dataGridViewTransactions = new System.Windows.Forms.DataGridView();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.lblSummary = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpFilterEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFilterStartDate = new System.Windows.Forms.DateTimePicker();
            this.cmbFilterType = new System.Windows.Forms.ComboBox();
            this.cmbFilterAccount = new System.Windows.Forms.ComboBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransactions)).BeginInit();
            this.panelFilters.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewTransactions
            // 
            this.dataGridViewTransactions.AllowUserToAddRows = false;
            this.dataGridViewTransactions.AllowUserToDeleteRows = false;
            this.dataGridViewTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTransactions.Location = new System.Drawing.Point(0, 80);
            this.dataGridViewTransactions.MultiSelect = false;
            this.dataGridViewTransactions.Name = "dataGridViewTransactions";
            this.dataGridViewTransactions.ReadOnly = true;
            this.dataGridViewTransactions.RowHeadersVisible = false;
            this.dataGridViewTransactions.RowTemplate.Height = 23;
            this.dataGridViewTransactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTransactions.Size = new System.Drawing.Size(800, 370);
            this.dataGridViewTransactions.TabIndex = 0;
            // 
            // panelFilters
            // 
            this.panelFilters.Controls.Add(this.lblSummary);
            this.panelFilters.Controls.Add(this.btnFilter);
            this.panelFilters.Controls.Add(this.label11);
            this.panelFilters.Controls.Add(this.label10);
            this.panelFilters.Controls.Add(this.label9);
            this.panelFilters.Controls.Add(this.label8);
            this.panelFilters.Controls.Add(this.dtpFilterEndDate);
            this.panelFilters.Controls.Add(this.dtpFilterStartDate);
            this.panelFilters.Controls.Add(this.cmbFilterType);
            this.panelFilters.Controls.Add(this.cmbFilterAccount);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 0);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new System.Drawing.Size(800, 80);
            this.panelFilters.TabIndex = 1;
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSummary.Location = new System.Drawing.Point(550, 50);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(0, 12);
            this.lblSummary.TabIndex = 9;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(450, 45);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(75, 23);
            this.btnFilter.TabIndex = 8;
            this.btnFilter.Text = "筛选";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(250, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 7;
            this.label11.Text = "至";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(200, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "日期：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(200, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 5;
            this.label9.Text = "类型：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "账户：";
            // 
            // dtpFilterEndDate
            // 
            this.dtpFilterEndDate.CustomFormat = "yyyy-MM-dd";
            this.dtpFilterEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFilterEndDate.Location = new System.Drawing.Point(270, 46);
            this.dtpFilterEndDate.Name = "dtpFilterEndDate";
            this.dtpFilterEndDate.Size = new System.Drawing.Size(150, 21);
            this.dtpFilterEndDate.TabIndex = 3;
            // 
            // dtpFilterStartDate
            // 
            this.dtpFilterStartDate.CustomFormat = "yyyy-MM-dd";
            this.dtpFilterStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFilterStartDate.Location = new System.Drawing.Point(100, 46);
            this.dtpFilterStartDate.Name = "dtpFilterStartDate";
            this.dtpFilterStartDate.Size = new System.Drawing.Size(150, 21);
            this.dtpFilterStartDate.TabIndex = 2;
            // 
            // cmbFilterType
            // 
            this.cmbFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterType.FormattingEnabled = true;
            this.cmbFilterType.Location = new System.Drawing.Point(250, 17);
            this.cmbFilterType.Name = "cmbFilterType";
            this.cmbFilterType.Size = new System.Drawing.Size(150, 20);
            this.cmbFilterType.TabIndex = 1;
            // 
            // cmbFilterAccount
            // 
            this.cmbFilterAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterAccount.FormattingEnabled = true;
            this.cmbFilterAccount.Location = new System.Drawing.Point(70, 17);
            this.cmbFilterAccount.Name = "cmbFilterAccount";
            this.cmbFilterAccount.Size = new System.Drawing.Size(120, 20);
            this.cmbFilterAccount.TabIndex = 0;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnAdd);
            this.panelButtons.Controls.Add(this.btnEdit);
            this.panelButtons.Controls.Add(this.btnDelete);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 450);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(800, 50);
            this.panelButtons.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 13);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "新增";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(93, 13);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "编辑";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(174, 13);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(255, 13);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // TransactionManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.dataGridViewTransactions);
            this.Controls.Add(this.panelFilters);
            this.Controls.Add(this.panelButtons);
            this.Name = "TransactionManagerForm";
            this.Text = "交易记录管理";
            this.Load += new System.EventHandler(this.TransactionManagerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransactions)).EndInit();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
