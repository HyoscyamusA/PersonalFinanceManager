using System;
using System.Windows.Forms;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.BLL;

namespace PersonalFinanceManager.UI.Forms
{
    public partial class AccountEditForm : Form
    {
        private Account _account;
        private AccountService _accountService;
        private bool _isEditMode;

        public AccountEditForm()
        {
            InitializeComponent();
            _accountService = new AccountService();
            _isEditMode = false;
            _account = new Account();
        }

        public AccountEditForm(Account account) : this()
        {
            _account = account;
            _isEditMode = true;
            this.Text = "编辑账户";
        }

        private void AccountEditForm_Load(object sender, EventArgs e)
        {
            if (_isEditMode)
            {
                // 编辑模式，填充数据
                txtAccountName.Text = _account.AccountName;
                cmbAccountType.Text = _account.AccountType;
                txtInitialAmount.Text = _account.InitialAmount.ToString("F2");
                txtCurrency.Text = _account.Currency;
                txtBankName.Text = _account.BankName ?? "";
                txtCardNumber.Text = _account.CardNumber ?? "";
                txtDescription.Text = _account.Description ?? "";
            }
            else
            {
                // 新增模式，设置默认值
                cmbAccountType.SelectedIndex = 0;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                // 更新账户对象
                _account.AccountName = txtAccountName.Text.Trim();
                _account.AccountType = cmbAccountType.Text;
                _account.InitialAmount = decimal.Parse(txtInitialAmount.Text);
                _account.Currency = txtCurrency.Text.Trim();
                _account.BankName = string.IsNullOrWhiteSpace(txtBankName.Text) ? null : txtBankName.Text.Trim();
                _account.CardNumber = string.IsNullOrWhiteSpace(txtCardNumber.Text) ? null : txtCardNumber.Text.Trim();
                _account.Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim();

                if (_isEditMode)
                {
                    // 更新现有账户
                    if (_accountService.UpdateAccount(_account))
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("更新账户失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // 新增账户
                    _accountService.AddAccount(_account);
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtAccountName.Text))
            {
                MessageBox.Show("请输入账户名称", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAccountName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbAccountType.Text))
            {
                MessageBox.Show("请选择账户类型", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbAccountType.Focus();
                return false;
            }

            if (!decimal.TryParse(txtInitialAmount.Text, out decimal amount) || amount < 0)
            {
                MessageBox.Show("初始金额必须为有效的非负数", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtInitialAmount.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCurrency.Text))
            {
                MessageBox.Show("请输入币种", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCurrency.Focus();
                return false;
            }

            return true;
        }
    }
}