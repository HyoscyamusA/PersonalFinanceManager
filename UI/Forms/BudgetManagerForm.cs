using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.BLL;

namespace PersonalFinanceManager.UI.Forms
{
    public partial class BudgetManagerForm : Form
    {
        private BudgetService _budgetService;
        private List<Budget> _budgets;

        public BudgetManagerForm()
        {
            InitializeComponent();
            _budgetService = new BudgetService();
            SetupDataGridView();
            SetupFilters();
        }

        private void SetupDataGridView()
        {
            // 设置自动生成列为false
            dataGridViewBudgets.AutoGenerateColumns = false;
            dataGridViewBudgets.Columns.Clear();

            // 创建数据绑定列
            var columns = new[]
            {
                new { Name = "BudgetID", Header = "ID", DataProperty = "BudgetID", Visible = false, Width = 0 },
                new { Name = "CategoryName", Header = "分类", DataProperty = "CategoryName", Visible = true, Width = 120 },
                new { Name = "BudgetAmount", Header = "预算金额", DataProperty = "BudgetAmount", Visible = true, Width = 100 },
                new { Name = "ActualAmount", Header = "实际支出", DataProperty = "ActualAmount", Visible = true, Width = 100 },
                new { Name = "CompletionRate", Header = "完成率", DataProperty = "CompletionRate", Visible = true, Width = 80 },
                new { Name = "WarningThreshold", Header = "预警阈值", DataProperty = "WarningThreshold", Visible = true, Width = 80 },
                new { Name = "Status", Header = "状态", DataProperty = "Status", Visible = true, Width = 60 },
                new { Name = "Remark", Header = "备注", DataProperty = "Remark", Visible = true, Width = 150 }
            };

            foreach (var col in columns)
            {
                dataGridViewBudgets.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    Name = col.Name,
                    HeaderText = col.Header,
                    DataPropertyName = col.DataProperty,
                    Visible = col.Visible,
                    Width = col.Width
                });
            }

            // 设置金额格式
            dataGridViewBudgets.Columns["BudgetAmount"].DefaultCellStyle.Format = "C2";
            dataGridViewBudgets.Columns["BudgetAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewBudgets.Columns["ActualAmount"].DefaultCellStyle.Format = "C2";
            dataGridViewBudgets.Columns["ActualAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // 设置百分比格式
            dataGridViewBudgets.Columns["CompletionRate"].DefaultCellStyle.Format = "0.##'%'";
            dataGridViewBudgets.Columns["CompletionRate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewBudgets.Columns["WarningThreshold"].DefaultCellStyle.Format = "0'%'";
            dataGridViewBudgets.Columns["WarningThreshold"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // 设置行样式
            dataGridViewBudgets.RowPrePaint += DataGridViewBudgets_RowPrePaint;
        }

        private void DataGridViewBudgets_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewBudgets.Rows.Count)
            {
                var row = dataGridViewBudgets.Rows[e.RowIndex];
                if (row.DataBoundItem != null)
                {
                    var budget = (Budget)row.DataBoundItem;

                    // 根据完成率设置颜色
                    if (budget.CompletionRate >= 100)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral; // 超预算
                    }
                    else if (budget.CompletionRate >= budget.WarningThreshold)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightYellow; // 预警
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White; // 正常
                    }
                }
            }
        }

        private void SetupFilters()
        {
            // 设置年份
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear - 1; year <= currentYear + 1; year++)
            {
                cmbYear.Items.Add(year);
            }
            cmbYear.SelectedItem = currentYear;

            // 设置月份
            for (int month = 1; month <= 12; month++)
            {
                cmbMonth.Items.Add(month);
            }
            cmbMonth.SelectedItem = DateTime.Now.Month;
        }

        private void BudgetManagerForm_Load(object sender, EventArgs e)
        {
            LoadBudgets();
        }

        private void LoadBudgets()
        {
            try
            {
                int year = (int)cmbYear.SelectedItem;
                int month = (int)cmbMonth.SelectedItem;

                _budgets = _budgetService.GetBudgetsByMonth(year, month);

                // 绑定数据
                dataGridViewBudgets.DataSource = null;
                dataGridViewBudgets.DataSource = _budgets;

                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载预算数据失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary()
        {
            if (_budgets == null || _budgets.Count == 0)
            {
                lblSummary.Text = "暂无预算数据";
                return;
            }

            decimal totalBudget = 0;
            decimal totalActual = 0;
            int warningCount = 0;
            int exceededCount = 0;

            foreach (var budget in _budgets)
            {
                totalBudget += budget.BudgetAmount;
                totalActual += budget.ActualAmount;

                if (budget.CompletionRate >= budget.WarningThreshold)
                {
                    warningCount++;
                }

                if (budget.CompletionRate >= 100)
                {
                    exceededCount++;
                }
            }

            lblSummary.Text = $"总预算: {totalBudget:C2} | 总支出: {totalActual:C2} | 预警: {warningCount} | 超支: {exceededCount}";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new BudgetEditForm())
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadBudgets();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加预算失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewBudgets.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要编辑的预算", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var selectedBudget = dataGridViewBudgets.SelectedRows[0].DataBoundItem as Budget;
                if (selectedBudget != null)
                {
                    using (var form = new BudgetEditForm(selectedBudget))
                    {
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            LoadBudgets();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"编辑预算失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewBudgets.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的预算", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var selectedBudget = dataGridViewBudgets.SelectedRows[0].DataBoundItem as Budget;
                if (selectedBudget != null)
                {
                    string budgetInfo = $"{selectedBudget.CategoryName} - {selectedBudget.BudgetAmount:C2}";

                    if (MessageBox.Show($"确定要删除预算 '{budgetInfo}' 吗？", "确认删除",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bool result = _budgetService.DeleteBudget(selectedBudget.BudgetID);
                        if (result)
                        {
                            MessageBox.Show("删除成功", "提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadBudgets();
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除预算失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBudgets();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadBudgets();
        }

        private void btnViewWarnings_Click(object sender, EventArgs e)
        {
            try
            {
                var warnings = _budgetService.GetBudgetWarnings();
                if (warnings.Count == 0)
                {
                    MessageBox.Show("暂无预算预警", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string warningMessage = "预算预警列表：\n\n";
                    foreach (var warning in warnings)
                    {
                        warningMessage += $"{warning.CategoryName}: {warning.CompletionRate:0.##}% (阈值: {warning.WarningThreshold}%)\n";
                    }

                    MessageBox.Show(warningMessage, "预算预警",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取预算预警失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}