using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;

namespace OnionsV2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    class COM
    {
        public SerialPort port = null;
        public Queue<string> packets = new Queue<string>();

        public COM()
        {
            foreach (string name in SerialPort.GetPortNames())
            {
                Console.WriteLine(name);
                try
                {
                    this.port = new SerialPort(name, 115200);
                    this.port.Open();
                    this.port.ReadTimeout = 500;
                    this.write("hello");
                    string recv = this.read();
                    if (recv.StartsWith("hi"))
                    {
                        return;
                    }
                }
                catch { }
            }
        }
        public string read()
        {
            byte[] buffer = new byte[16];
            this.port.Read(buffer, 0, 16);
            return System.Text.Encoding.UTF8.GetString(buffer);

        }

        public void write(string text)
        {
            this.port.Write(text);
            Thread.Sleep(100);
        }
    }
}
