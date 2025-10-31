//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace PersonalFinanceManager
//{
//    internal static class Program
//    {
//        /// <summary>
//        /// 应用程序的主入口点。
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new Form1());
//        }
//    }
//}
using System;
using System.Windows.Forms;
using PersonalFinanceManager.DAL;

namespace PersonalFinanceManager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 禁用数据库初始化的调试消息
            DatabaseInitializer.ShowDebugMessages = false;
            // 初始化数据库
            InitializeDatabase();

            Application.Run(new MainForm());
        }

        static void InitializeDatabase()
        {
            try
            {
                var initializer = new DatabaseInitializer();
                initializer.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"数据库初始化失败: {ex.Message}\n" +
                    "请确保：\n" +
                    "1. MySQL服务正在运行\n" +
                    "2. 连接字符串中的用户名和密码正确\n" +
                    "3. 有创建数据库的权限",
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}