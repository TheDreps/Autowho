using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Autowho
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Config.Create();

            int log = StartLog();

            [DllImport("user32.dll")]
            static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

            Console.WriteLine("Running in background...");


            while (true)
            {
                var fileStream = File.Open(Config.getLogLocation(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                List<String> logFile = new List<string>();
                using (var stream = fileStream)
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        logFile.Add(line);
                    }
                }
                logFile.RemoveRange(0, log);
                for (int i = 0; i < logFile.Count; i++)
                {
                    try
                    {
                        string o = logFile[i];
                        string[] words = o.Split();
                        if ((o.Substring(39, o.Length - 39) == "        "))
                        {
                            [DllImport("User32.dll")]
                            static extern int SetForegroundWindow(IntPtr point);

                            Process p = Process.GetProcessesByName("javaw")[0];
                            if (p != null)
                            {
                                IntPtr h = p.MainWindowHandle;
                                SetForegroundWindow(h);

                                Thread.Sleep(1500);

                                const uint KEYUP = 0x0002;
                                byte key = Config.GetAutoWhoKey();

                                keybd_event(key, 0, 0, 0);
                                Thread.Sleep(500);
                                keybd_event(key, 0, KEYUP, 0);
                            }
                        }
                    }
                    catch (Exception t)
                    {
                        Debug.WriteLine(t);
                    }
                }
                log += logFile.Count;
            }
        }

        public static int StartLog()
        {
            var lineCount = 0;
            var fileStream = File.Open(Config.getLogLocation(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (var stream = fileStream)
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }
            return lineCount;
        }
    }
}