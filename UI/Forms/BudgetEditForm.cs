
using System;
using System.Windows.Forms;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.BLL;

namespace PersonalFinanceManager.UI.Forms
{
    public partial class BudgetEditForm : Form
    {
        private Budget _budget;
        private BudgetService _budgetService;
        private CategoryService _categoryService;
        private bool _isEditMode;

        public BudgetEditForm()
        {
            InitializeComponent();
            _budgetService = new BudgetService();
            _categoryService = new CategoryService();
            _isEditMode = false;
            _budget = new Budget();
        }

        public BudgetEditForm(Budget budget) : this()
        {
            _budget = budget;
            _isEditMode = true;
            this.Text = "编辑预算";
        }

        private void BudgetEditForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            // 不再调用 LoadYears() 和 LoadMonths()，因为我们在设计器中已经设置了范围

            if (_isEditMode)
            {
                // 编辑模式，填充数据
                numYear.Value = _budget.BudgetYear;
                numMonth.Value = _budget.BudgetMonth;
                cmbCategory.SelectedValue = _budget.CategoryID;
                numBudgetAmount.Value = _budget.BudgetAmount;
                numWarningThreshold.Value = _budget.WarningThreshold;
                cmbStatus.SelectedItem = _budget.Status;
                txtRemark.Text = _budget.Remark ?? "";
            }
            else
            {
                // 新增模式，设置默认值
                numYear.Value = DateTime.Now.Year;
                numMonth.Value = DateTime.Now.Month;
                cmbStatus.SelectedIndex = 0;
                numWarningThreshold.Value = 80;
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _categoryService.GetAllCategories();
                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载分类列表失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                // 更新预算对象
                _budget.BudgetYear = (int)numYear.Value;
                _budget.BudgetMonth = (int)numMonth.Value;
                _budget.CategoryID = (int)cmbCategory.SelectedValue;
                _budget.BudgetAmount = numBudgetAmount.Value;
                _budget.WarningThreshold = numWarningThreshold.Value;
                _budget.Status = cmbStatus.Text;
                _budget.Remark = string.IsNullOrWhiteSpace(txtRemark.Text) ? null : txtRemark.Text.Trim();
                _budget.CreatedBy = "系统"; // 这里可以根据实际需求修改

                bool result = _budgetService.SetBudget(_budget);
                if (result)
                {
                    string message = _isEditMode ? "预算更新成功！" : "预算设置成功！";
                    MessageBox.Show(message, "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    string message = _isEditMode ? "更新预算失败" : "设置预算失败";
                    MessageBox.Show(message, "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("请选择分类", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (numBudgetAmount.Value <= 0)
            {
                MessageBox.Show("预算金额必须大于0", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numBudgetAmount.Focus();
                return false;
            }

            if (numWarningThreshold.Value <= 0 || numWarningThreshold.Value > 100)
            {
                MessageBox.Show("预警阈值必须在1-100之间", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numWarningThreshold.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                MessageBox.Show("请选择状态", "验证错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus.Focus();
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
