using System;
using System.Windows.Forms;
using PersonalFinanceManager.BLL;
using PersonalFinanceManager.DAL;
using PersonalFinanceManager.UI.Forms;
using PersonalFinanceManager.UI.Forms;

namespace PersonalFinanceManager
{
    public partial class MainForm : Form
    {
        private readonly AccountService _accountService;
        private readonly CategoryService _categoryService;

        public MainForm()
        {
            InitializeComponent();
            _accountService = new AccountService();
            _categoryService = new CategoryService();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateStatusInfo();
        }

        public void UpdateStatusInfo()
        {
            try
            {
                decimal totalBalance = _accountService.GetTotalBalance();
                statusLabel.Text = $"总资产: {totalBalance:C2} | 欢迎使用个人财务管理系统";
            }
            catch (Exception)
            {
                statusLabel.Text = "欢迎使用个人财务管理系统 | 数据库连接正常";
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出系统吗？", "确认退出",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void 初始化数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("这将重新初始化数据库，现有数据可能会丢失。确定要继续吗？",
                "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    var initializer = new DatabaseInitializer();
                    initializer.InitializeDatabase();
                    MessageBox.Show("数据初始化成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateStatusInfo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"数据初始化失败: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void 账户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 传递当前 MainForm 实例给 AccountManagerForm
            ShowFormInPanel(new AccountManagerForm());
        }

        private void 交易记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new TransactionManagerForm());
        }

        private void 预算管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("预算管理功能开发中...", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 统计报表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("统计报表功能开发中...", "提示",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowFormInPanel(Form form)
        {
            // 清除面板中的所有控件
            panelMain.Controls.Clear();

            // 设置子窗体的属性
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            // 将子窗体添加到面板中
            panelMain.Controls.Add(form);

            // 显示子窗体
            form.Show();
        }
    }
}