using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

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

    public class SignalReader
    {
        Socket reader = null;
        Queue<byte[]> packets = new Queue<byte[]>();
        public SignalReader()
        {
            this.reader = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            try
            {
                this.reader.Connect(new IPEndPoint(ipAddress, 26789));
            }
            catch
            {
                MessageBox.Show("Refused");
                System.Environment.Exit(1);
            }
        }
        public byte[] read(int n)
        {
            byte[] buffer = new byte[n];
            if (this.reader != null)
            {
                this.reader.Receive(buffer);
                return buffer;
            }
            return null;

        }
    }
}
