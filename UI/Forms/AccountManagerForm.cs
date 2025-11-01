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

            // 绑定事件
            dataGridViewAccounts.CellDoubleClick += DataGridViewAccounts_CellDoubleClick;
        }

        private void SetupDataGridView()
        {
            // 设置列
            dataGridViewAccounts.Columns.Clear();
            dataGridViewAccounts.Columns.Add("AccountID", "ID");
            dataGridViewAccounts.Columns.Add("AccountName", "账户名称");
            dataGridViewAccounts.Columns.Add("AccountType", "账户类型");
            dataGridViewAccounts.Columns.Add("InitialAmount", "初始金额");
            dataGridViewAccounts.Columns["InitialAmount"].DefaultCellStyle.Format = "C2"; // 显示为货币
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

        //private void LoadAccounts()
        //{
        //    try
        //    {
        //        _accounts = _accountService.GetAllAccounts();
        //        dataGridViewAccounts.Rows.Clear();

        //        foreach (var account in _accounts)
        //        {
        //            dataGridViewAccounts.Rows.Add(
        //                account.AccountID,
        //                account.AccountName,
        //                account.AccountType,
        //                account.InitialAmount,      //新增初始金额列
        //                account.CurrentBalance,
        //                account.Currency,
        //                account.BankName,
        //                account.CreateTime.ToString("yyyy-MM-dd")
        //            );
        //        }

        //        // 更新状态
        //        UpdateStatus();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"加载账户数据失败: {ex.Message}", "错误",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private void LoadAccounts()
        {
            try
            {
                _accounts = _accountService.GetAllAccounts();
                dataGridViewAccounts.Rows.Clear();

                foreach (var account in _accounts)
                {
                    // 日志记录加载的账户信息
                    Console.WriteLine($"[LoadAccounts] 加载账户：{account.AccountName}，初始金额：{account.InitialAmount}，当前余额：{account.CurrentBalance}");

                    dataGridViewAccounts.Rows.Add(
                        account.AccountID,
                        account.AccountName,
                        account.AccountType,
                        account.InitialAmount,      //新增初始金额列
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
                Console.WriteLine($"[LoadAccounts] 加载账户失败: {ex.Message}");
                MessageBox.Show($"加载账户数据失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //  双击进入编辑模式
        private void DataGridViewAccounts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // 避免标题行

            try
            {
                int accountId = Convert.ToInt32(dataGridViewAccounts.Rows[e.RowIndex].Cells["AccountID"].Value);
                var account = _accountService.GetAccountById(accountId);

                if (account != null)
                {
                    using (var form = new AccountEditForm(account))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadAccounts();
                            OnAccountUpdated?.Invoke(); // 通知主窗体更新总余额
                            MessageBox.Show("账户已更新！", "成功",
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

        private void UpdateStatus()
        {
            if (dataGridViewAccounts.Parent is Panel panel && panel.Parent is MainForm mainForm)
            {
                decimal totalBalance = _accountService.GetTotalBalance();
                mainForm.UpdateStatusInfo();
            }
        }


        public event Action OnAccountUpdated; // 事件定义

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new AccountEditForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadAccounts();
                        MessageBox.Show("账户添加成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        try
                        {
                            var newAccount = _accountService.GetLatestAccount(); //  获取刚新增的账户
                            if (newAccount != null && newAccount.CurrentBalance > 0)
                            {
                                var transactionService = new TransactionService();

                                transactionService.AddTransaction(new Transaction
                                {
                                    AccountID = newAccount.AccountID,
                                    TransactionTime = DateTime.Now,
                                    Amount = newAccount.CurrentBalance,
                                    TransactionType = "收入",
                                    CategoryID = 1, // 期初余额分类ID（请确保数据库里有）
                                    Remark = "账户创建时自动生成期初记录",
                                    Status = "正常",
                                    CreateTime = DateTime.Now
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"账户添加成功，但创建交易记录失败: {ex.Message}", "警告",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }


                        OnAccountUpdated?.Invoke(); // 触发事件，通知更新总余额
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加账户失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // 创建新增账户对话框
        //        using (var form = new AccountEditForm())
        //        {
        //            if (form.ShowDialog() == DialogResult.OK)
        //            {
        //                LoadAccounts();
        //                MessageBox.Show("账户添加成功！", "成功",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                UpdateStatus();  // 调用更新总余额的函数
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"添加账户失败: {ex.Message}", "错误",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

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
                            // 编辑完账户后，同步交易记录
                            try
                            {
                                var updatedAccount = _accountService.GetAccountById(accountId);
                                var transactionService = new TransactionService();

                                if (updatedAccount != null)
                                {
                                    transactionService.AddTransaction(new Transaction
                                    {
                                        AccountID = updatedAccount.AccountID,
                                        TransactionTime = DateTime.Now,
                                        Amount = updatedAccount.CurrentBalance,
                                        TransactionType = "调整",
                                        CategoryID = 2, // 可选：调整类分类
                                        Remark = "账户编辑后余额调整",
                                        Status = "正常",
                                        CreateTime = DateTime.Now
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"账户更新成功，但记录调整交易失败: {ex.Message}", "警告",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            OnAccountUpdated?.Invoke(); // 触发事件，通知更新总余额
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

        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    if (dataGridViewAccounts.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要删除的账户", "提示",
        //            MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    try
        //    {
        //        int accountId = Convert.ToInt32(dataGridViewAccounts.SelectedRows[0].Cells["AccountID"].Value);
        //        string accountName = dataGridViewAccounts.SelectedRows[0].Cells["AccountName"].Value.ToString();

        //        if (MessageBox.Show($"确定要删除账户 '{accountName}' 吗？", "确认删除",
        //            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //        {
        //            if (_accountService.DeleteAccount(accountId))
        //            {
        //                LoadAccounts();
        //                MessageBox.Show("账户删除成功！", "成功",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"删除账户失败: {ex.Message}", "错误",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    if (dataGridViewAccounts.SelectedRows.Count == 0)
        //    {
        //        MessageBox.Show("请选择要删除的账户", "提示",
        //            MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    try
        //    {
        //        int accountId = Convert.ToInt32(dataGridViewAccounts.SelectedRows[0].Cells["AccountID"].Value);
        //        string accountName = dataGridViewAccounts.SelectedRows[0].Cells["AccountName"].Value.ToString();

        //        if (MessageBox.Show($"确定要删除账户 '{accountName}' 吗？", "确认删除",
        //            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //        {
        //            if (_accountService.DeleteAccount(accountId))
        //            {
        //                LoadAccounts();  // 加载账户数据
        //                //UpdateStatus();   // 更新总余额显示

        //                MessageBox.Show("账户删除成功！", "成功",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                OnAccountUpdated?.Invoke(); // 触发事件，通知更新总余额
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"删除账户失败: {ex.Message}", "错误",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
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

                if (MessageBox.Show($"确定要删除账户 '{accountName}' 及其所有交易记录吗？", "确认删除",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //  先删除该账户的所有交易记录
                    TransactionService transactionService = new TransactionService();
                    transactionService.DeleteTransactionsByAccountId(accountId);

                    //  再删除账户本身
                    if (_accountService.DeleteAccount(accountId))
                    {
                        LoadAccounts(); // 刷新账户列表

                        MessageBox.Show("账户及相关交易记录已删除。", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        OnAccountUpdated?.Invoke(); // 通知外部更新余额显示
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
            try
            {
                LoadAccounts(); // 刷新账户数据
                OnAccountUpdated?.Invoke(); // 通知主窗体同步更新总余额等
                MessageBox.Show("账户数据已刷新。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新账户失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}