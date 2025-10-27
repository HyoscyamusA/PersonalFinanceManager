using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.BLL;

namespace PersonalFinanceManager.UI.Forms
{
    public partial class TransactionManagerForm : Form
    {
        private TransactionService _transactionService;
        private AccountService _accountService;
        private List<Transaction> _transactions;

        public TransactionManagerForm()
        {
            InitializeComponent();
            _transactionService = new TransactionService();
            _accountService = new AccountService();
            SetupDataGridView();
            SetupFilters();
        }

        private void SetupDataGridView()
        {
            // 设置列
            dataGridViewTransactions.Columns.Clear();
            dataGridViewTransactions.Columns.Add("TransactionID", "ID");
            dataGridViewTransactions.Columns.Add("TransactionTime", "交易时间");
            dataGridViewTransactions.Columns.Add("AccountName", "账户");
            dataGridViewTransactions.Columns.Add("CategoryName", "分类");
            dataGridViewTransactions.Columns.Add("TransactionType", "类型");
            dataGridViewTransactions.Columns.Add("Amount", "金额");
            dataGridViewTransactions.Columns.Add("TargetAccount", "目标账户");
            dataGridViewTransactions.Columns.Add("Remark", "备注");
            dataGridViewTransactions.Columns.Add("Location", "地点");

            // 隐藏ID列
            dataGridViewTransactions.Columns["TransactionID"].Visible = false;

            // 设置金额格式
            dataGridViewTransactions.Columns["Amount"].DefaultCellStyle.Format = "C2";
            dataGridViewTransactions.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // 设置日期格式
            dataGridViewTransactions.Columns["TransactionTime"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
        }

        private void SetupFilters()
        {
            // 设置交易类型筛选
            cmbFilterType.Items.Clear();
            cmbFilterType.Items.AddRange(new object[] { "所有类型", "收入", "支出", "转账" });
            cmbFilterType.SelectedIndex = 0;

            // 设置日期范围
            dtpFilterStartDate.Value = DateTime.Now.AddMonths(-1);
            dtpFilterEndDate.Value = DateTime.Now;

            // 设置账户筛选 - 使用不同的方法
            SetupAccountFilter();
        }

        private void SetupAccountFilter()
        {
            try
            {
                var accounts = _accountService.GetAllAccounts();

                // 创建一个包含"所有账户"选项的新列表
                var filterItems = new List<object>();
                filterItems.Add("所有账户");
                filterItems.AddRange(accounts);

                cmbFilterAccount.DisplayMember = "AccountName";
                cmbFilterAccount.ValueMember = "AccountID";

                // 设置数据源
                cmbFilterAccount.DataSource = filterItems;

                // 设置显示成员和值成员的处理
                cmbFilterAccount.Format += (sender, e) =>
                {
                    if (e.ListItem is string)
                    {
                        e.Value = e.ListItem.ToString();
                    }
                    else if (e.ListItem is Account account)
                    {
                        e.Value = account.AccountName;
                    }
                };

                // 处理选择值
                cmbFilterAccount.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载账户筛选列表失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TransactionManagerForm_Load(object sender, EventArgs e)
        {
            LoadTransactions();
        }

        private void LoadTransactions()
        {
            try
            {
                DateTime startDate = dtpFilterStartDate.Value.Date;
                DateTime endDate = dtpFilterEndDate.Value.Date.AddDays(1).AddSeconds(-1);

                // 获取筛选条件
                int? accountId = null;
                if (cmbFilterAccount.SelectedItem is Account selectedAccount)
                {
                    accountId = selectedAccount.AccountID;
                }

                string transactionType = null;
                if (cmbFilterType.SelectedIndex > 0)
                {
                    transactionType = cmbFilterType.Text;
                }

                // 获取交易记录
                var allTransactions = _transactionService.GetTransactionsByDateRange(startDate, endDate);
                _transactions = new List<Transaction>();

                // 应用筛选
                foreach (var transaction in allTransactions)
                {
                    bool include = true;

                    // 账户筛选
                    if (accountId.HasValue && transaction.AccountID != accountId.Value)
                    {
                        include = false;
                    }

                    // 交易类型筛选
                    if (!string.IsNullOrEmpty(transactionType) && transaction.TransactionType != transactionType)
                    {
                        include = false;
                    }

                    if (include)
                    {
                        _transactions.Add(transaction);
                    }
                }

                dataGridViewTransactions.Rows.Clear();
                foreach (var transaction in _transactions)
                {
                    dataGridViewTransactions.Rows.Add(
                        transaction.TransactionID,
                        transaction.TransactionTime,
                        transaction.AccountName,
                        transaction.CategoryName,
                        transaction.TransactionType,
                        transaction.Amount,
                        transaction.TargetAccount ?? "",
                        transaction.Remark ?? "",
                        transaction.Location ?? ""
                    );
                }

                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载交易记录失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary()
        {
            try
            {
                DateTime startDate = dtpFilterStartDate.Value.Date;
                DateTime endDate = dtpFilterEndDate.Value.Date.AddDays(1).AddSeconds(-1);

                decimal totalIncome = _transactionService.GetTotalIncome(startDate, endDate);
                decimal totalExpense = _transactionService.GetTotalExpense(startDate, endDate);

                lblSummary.Text = $"收入: {totalIncome:C2} | 支出: {totalExpense:C2} | 净额: {(totalIncome - totalExpense):C2}";
            }
            catch (Exception ex)
            {
                lblSummary.Text = "统计信息加载失败";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new TransactionEditForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadTransactions();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加交易记录失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewTransactions.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要编辑的交易记录", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int transactionId = Convert.ToInt32(dataGridViewTransactions.SelectedRows[0].Cells["TransactionID"].Value);
                var transaction = _transactionService.GetTransactionById(transactionId);

                if (transaction != null)
                {
                    using (var form = new TransactionEditForm(transaction))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadTransactions();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"编辑交易记录失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewTransactions.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的交易记录", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int transactionId = Convert.ToInt32(dataGridViewTransactions.SelectedRows[0].Cells["TransactionID"].Value);
                string transactionInfo = $"{dataGridViewTransactions.SelectedRows[0].Cells["TransactionTime"].Value} - {dataGridViewTransactions.SelectedRows[0].Cells["Amount"].Value}";

                if (MessageBox.Show($"确定要删除交易记录 '{transactionInfo}' 吗？", "确认删除",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_transactionService.DeleteTransaction(transactionId))
                    {
                        LoadTransactions();
                        MessageBox.Show("交易记录删除成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除交易记录失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTransactions();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadTransactions();
        }
    }
}