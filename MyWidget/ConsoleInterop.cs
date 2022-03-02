using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UJSAPP
{
    /// <summary>
    /// 提供与控制台程序的交互。
    /// </summary>
    public class ConsoleInterop
    {
        /// <summary>
        /// 关闭控制台程序。
        /// </summary>
        /// <param name="process">要关闭的控制台程序的进程实例。</param>
        /// <param name="timeoutInMilliseconds">如果不希望一直等待进程自己退出，则可以在此参数中设置超时。你可以在超时未推出候采取强制杀掉进程的策略。</param>
        /// <returns>如果进程成功退出，则返回 true；否则返回 false。</returns>
        public static bool StopConsoleProgram(Process process, int? timeoutInMilliseconds = null)
        {
            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            if (process.HasExited)
            {
                return true;
            }

            // 尝试将我们自己的进程附加到指定进程的控制台（如果有的话）。
            if (AttachConsole((uint)process.Id))
            {
                // 我们自己的进程需要忽略掉 Ctrl+C 信号，否则自己也会退出。
                SetConsoleCtrlHandler(null, true);

                // 将 Ctrl+C 信号发送到前面已关联（附加）的控制台进程中。
                GenerateConsoleCtrlEvent(CtrlTypes.CTRL_C_EVENT, 0);

                // 拾前面已经附加的控制台。
                FreeConsole();

                bool hasExited;
                // 由于 Ctrl+C 信号只是通知程序关闭，并不一定真的关闭。所以我们等待一定时间，如果仍未关闭，则超时不处理。
                // 业务可以通过判断返回值来角是否进行后续处理（例如强制杀掉）。
                if (timeoutInMilliseconds == null)
                {
                    // 如果没有超时处理，则一直等待，直到最终进程停止。
                    process.WaitForExit();
                    hasExited = true;
                }
                else
                {
                    // 如果有超时处理，则超时候返回。
                    hasExited = process.WaitForExit(timeoutInMilliseconds.Value);
                }

                // 重新恢复我们自己的进程对 Ctrl+C 信号的响应。
                SetConsoleCtrlHandler(null, false);

                return hasExited;
            }
            else
            {
                return false;
            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate? HandlerRoutine, bool Add);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GenerateConsoleCtrlEvent(CtrlTypes dwCtrlEvent, uint dwProcessGroupId);

        enum CtrlTypes : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        private delegate bool ConsoleCtrlDelegate(CtrlTypes CtrlType);
    }
}
