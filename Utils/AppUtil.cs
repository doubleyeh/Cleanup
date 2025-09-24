using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Cleanup.Utils
{
    public static class AppUtil
    {

        /// <summary>
        /// 判断当前程序是否以管理员权限运行
        /// </summary>
        public static bool IsRunningAsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// 以管理员权限重新运行
        /// </summary>
        public static void RestartAsAdministrator()
        {
            var exeName = Environment.ProcessPath;
            if(null == exeName)
            {
                return;
            }
            var psi = new System.Diagnostics.ProcessStartInfo(exeName)
            {
                UseShellExecute = true,
                Verb = "runas" // 以管理员权限启动
            };

            try
            {
                System.Diagnostics.Process.Start(psi);
                Environment.Exit(0); // 关闭当前非管理员进程
            }
            catch
            {
                // 用户取消了 UAC
            }
        }

    }
}
