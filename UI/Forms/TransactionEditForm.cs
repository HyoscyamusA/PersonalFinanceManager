using System;
using System.Windows.Forms;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.BLL;

namespace PersonalFinanceManager.UI.Forms
{
    public partial class TransactionEditForm : Form
    {
        private Transaction _transaction;
        private TransactionService _transactionService;
        private AccountService _accountService;
        private CategoryService _categoryService;
        private bool _isEditMode;

        public TransactionEditForm()
        {
            InitializeComponent();
            _transactionService = new TransactionService();
            _accountService = new AccountService();
            _categoryService = new CategoryService();
            _isEditMode = false;
            _transaction = new Transaction();
        }

        public TransactionEditForm(Transaction transaction) : this()
        {
            _transaction = transaction;
            _isEditMode = true;
            this.Text = "编辑交易记录";
        }

        private void TransactionEditForm_Load(object sender, EventArgs e)
        {
            // 先加载静态数据（交易类型）
            LoadTransactionTypes();

            // 再加载动态数据（账户和分类）
            LoadAccounts();
            LoadCategories();

            if (_isEditMode)
            {
                // 编辑模式，填充数据
                cmbAccount.SelectedValue = _transaction.AccountID;
                cmbCategory.SelectedValue = _transaction.CategoryID;
                cmbTransactionType.SelectedItem = _transaction.TransactionType;
                dtpTransactionTime.Value = _transaction.TransactionTime;
                numAmount.Value = _transaction.Amount;
                txtTargetAccount.Text = _transaction.TargetAccount ?? "";
                txtRemark.Text = _transaction.Remark ?? "";
                txtLocation.Text = _transaction.Location ?? "";
                txtTags.Text = _transaction.Tags ?? "";
            }
            else
            {
                // 新增模式，设置默认值
                cmbTransactionType.SelectedIndex = 0;
                dtpTransactionTime.Value = DateTime.Now;
                numAmount.Value = 0;
            }
        }

        private void LoadAccounts()
        {
            try
            {
                var accounts = _accountService.GetAllAccounts();

                // 创建一个新的 BindingSource 来避免修改原始集合
                var accountSource = new BindingSource();
                accountSource.DataSource = accounts;

                cmbAccount.DataSource = accountSource;
                cmbAccount.DisplayMember = "AccountName";
                cmbAccount.ValueMember = "AccountID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载账户列表失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _categoryService.GetAllCategories();

                // 创建一个新的 BindingSource 来避免修改原始集合
                var categorySource = new BindingSource();
                categorySource.DataSource = categories;

                cmbCategory.DataSource = categorySource;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载分类列表失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTransactionTypes()
        {
            // 在设置任何 DataSource 之前先加载交易类型
            cmbTransactionType.Items.Clear();
            cmbTransactionType.Items.AddRange(new object[] { "收入", "支出", "转账" });
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                // 更新交易记录对象
                _transaction.AccountID = (int)cmbAccount.SelectedValue;
                _transaction.CategoryID = (int)cmbCategory.SelectedValue;
                _transaction.TransactionType = cmbTransactionType.Text;
                _transaction.TransactionTime = dtpTransactionTime.Value;
                _transaction.Amount = numAmount.Value;
                _transaction.TargetAccount = string.IsNullOrWhiteSpace(txtTargetAccount.Text) ? null : txtTargetAccount.Text.Trim();
                _transaction.Remark = string.IsNullOrWhiteSpace(txtRemark.Text) ? null : txtRemark.Text.Trim();
                _transaction.Location = string.IsNullOrWhiteSpace(txtLocation.Text) ? null : txtLocation.Text.Trim();
                _transaction.Tags = string.IsNullOrWhiteSpace(txtTags.Text) ? null : txtTags.Text.Trim();

                bool result;
                if (_isEditMode)
                {
                    result = _transactionService.UpdateTransaction(_transaction);
                    if (result)
                    {
                        MessageBox.Show("交易记录更新成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("更新交易记录失败", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    result = _transactionService.AddTransaction(_transaction);
                    if (result)
                    {
                        MessageBox.Show("交易记录添加成功！", "成功",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("添加交易记录失败", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (cmbAccount.SelectedValue == null)
            {
                MessageBox.Show("请选择账户", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbAccount.Focus();
                return false;
            }

            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("请选择分类", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbTransactionType.Text))
            {
                MessageBox.Show("请选择交易类型", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTransactionType.Focus();
                return false;
            }

            if (numAmount.Value <= 0)
            {
                MessageBox.Show("金额必须大于0", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numAmount.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}