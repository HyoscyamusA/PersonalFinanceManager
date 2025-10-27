using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.BLL;

namespace PersonalFinanceManager.UI.Forms
{
    public partial class AccountManagerForm : Form
    {
        private AccountService _accountService;
        private List<Account> _accounts;

        public AccountManagerForm()
        {
            InitializeComponent();
            _accountService = new AccountService();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            // 设置列
            dataGridViewAccounts.Columns.Clear();
            dataGridViewAccounts.Columns.Add("AccountID", "ID");
            dataGridViewAccounts.Columns.Add("AccountName", "账户名称");
            dataGridViewAccounts.Columns.Add("AccountType", "账户类型");
            dataGridViewAccounts.Columns.Add("CurrentBalance", "当前余额");
            dataGridViewAccounts.Columns.Add("Currency", "币种");
            dataGridViewAccounts.Columns.Add("BankName", "银行");
            dataGridViewAccounts.Columns.Add("CreateTime", "创建时间");

            // 隐藏ID列
            dataGridViewAccounts.Columns["AccountID"].Visible = false;

            // 设置金额格式
            dataGridViewAccounts.Columns["CurrentBalance"].DefaultCellStyle.Format = "C2";
        }

        private void AccountManagerForm_Load(object sender, EventArgs e)
        {
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            try
            {
                _accounts = _accountService.GetAllAccounts();
                dataGridViewAccounts.Rows.Clear();

                foreach (var account in _accounts)
                {
                    dataGridViewAccounts.Rows.Add(
                        account.AccountID,
                        account.AccountName,
                        account.AccountType,
                        account.CurrentBalance,
                        account.Currency,
                        account.BankName,
                        account.CreateTime.ToString("yyyy-MM-dd")
                    );
                }

                // 更新状态
                UpdateStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载账户数据失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatus()
        {
            if (dataGridViewAccounts.Parent is Panel panel && panel.Parent is MainForm mainForm)
            {
                decimal totalBalance = _accountService.GetTotalBalance();
                mainForm.UpdateStatusInfo();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // 创建新增账户对话框
                using (var form = new AccountEditForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadAccounts();
                        MessageBox.Show("账户添加成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加账户失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewAccounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要编辑的账户", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int accountId = Convert.ToInt32(dataGridViewAccounts.SelectedRows[0].Cells["AccountID"].Value);
                var account = _accountService.GetAccountById(accountId);

                if (account != null)
                {
                    using (var form = new AccountEditForm(account))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadAccounts();
                            MessageBox.Show("账户更新成功！", "成功",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"编辑账户失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewAccounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的账户", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int accountId = Convert.ToInt32(dataGridViewAccounts.SelectedRows[0].Cells["AccountID"].Value);
                string accountName = dataGridViewAccounts.SelectedRows[0].Cells["AccountName"].Value.ToString();

                if (MessageBox.Show($"确定要删除账户 '{accountName}' 吗？", "确认删除",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (_accountService.DeleteAccount(accountId))
                    {
                        LoadAccounts();
                        MessageBox.Show("账户删除成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除账户失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAccounts();
        }
    }
}