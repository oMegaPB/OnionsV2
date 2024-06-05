using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnionsV2
{
    public partial class Form1 : Form
    {
        private int activePad = 1;
        private int circlesAmount = 1;
        private List<Label> speedLabels = new List<Label>();
        private int usableScene = 1; // use label house or timer
        private Dictionary<string, bool> connections = new Dictionary<string, bool>();
        private bool metricsEnabled = false;
        private Thread timerLabelsThread = null;
        private COM com = new COM();
        private double time = 0.00;
        public Form1()
        {
            InitializeComponent();
        }

        private Control GetControlByName(string name)
        {
            foreach (Control c in Controls)
            {
                if (c.Name == name)
                {
                    return c;
                }
            }
            return null;
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeft, int nTop, int nRight, int nBottom, int nWidthEllipse, int nHeightEllipse);

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            foreach (Control c in Controls)
            {
                if (c.Name.StartsWith("RoundedElement"))
                {
                    char arg = c.Name[c.Name.Length-1];
                    int value;
                    if (new List<char>(new char[] {'3', '4', '5'}).Contains(arg)) {value = 15;} else {value = 30;}
                    c.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, c.Width, c.Height, value, value));
                }
            }
            this.BackgroundImage = new Bitmap(Properties.Resources._1background, this.Width, this.Height);
            Control c_label = GetControlByName("ConnectLBL");
            c_label.BackgroundImage = new Bitmap(Properties.Resources.image_6, c_label.Width, c_label.Height);
            Control speed_label = GetControlByName("SpeedLBL");
            speed_label.BackgroundImage = new Bitmap(Properties.Resources.image_7, speed_label.Width, speed_label.Height);
            Control use_label = GetControlByName("UseLBL");
            use_label.BackgroundImage = new Bitmap(Properties.Resources.image_8, use_label.Width, use_label.Height);
            foreach (Control c in new Control[] { GetControlByName("RoundedElement4"), GetControlByName("RoundedElement5") })
            {
                c.Visible = false;
            }
            foreach (Button c in new Control[] { GetControlByName("RoundedElement66"), GetControlByName("RoundedElement67"), GetControlByName("RoundedElement68") })
            {
                c.FlatStyle = FlatStyle.Flat;
                c.FlatAppearance.BorderSize = 0;
            }
            this.GetControlByName("RoundedElement67").BackgroundImage = Properties.Resources.dark_blue;
            this.GetControlByName("RoundedElement67").ForeColor = Color.White;
            this.GetControlByName("RoundedElement68").BackgroundImage = Properties.Resources.dark_blue;
            this.GetControlByName("RoundedElement68").ForeColor = Color.White;
            this.RoundedElement8.BackgroundImage = Properties.Resources.gray;
            this.RenderCircles();
            this.textBox1.Visible = false;
            this.button1.Visible = false;
            this.label5.Hide();
            this.RoundedElement10.Hide();
            this.RoundedElement11.Hide();
            this.RoundedElement12.Hide();
            this.label9.Hide();
            this.RoundedElement9.Hide();
            this.label7.Hide();
            this.button2.BackgroundImage = new Bitmap(Properties.Resources.house_white, button2.Width, button2.Height);
            this.button2.Hide();
            this.button3.BackgroundImage = new Bitmap(Properties.Resources.sekundomer_black, button3.Width, button3.Height);
            this.button3.Hide();
            this.RoundedElement13.Hide();
            this.RoundedElement14.Hide();
            this.button4.Hide();
            this.button5.Hide();
            this.button6.Hide();

            this.label6.Hide();
            Thread pollingThread_ = new Thread(this.pollingThread);
            pollingThread_.Start();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void RoundedElement66_MouseEnter(object sender, EventArgs e)
        {
            if (this.activePad == 1)
            {
                this.SuspendLayout();
                this.GetControlByName("RoundedElement66").BackgroundImage = Properties.Resources.gray;
                this.GetControlByName("label1").BackgroundImage = Properties.Resources.gray;
                this.ResumeLayout(false);
            }
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            this.RoundedElement66_MouseEnter(sender, e);
        }

        private void RoundedElement66_MouseLeave(object sender, EventArgs e)
        {
            if (this.activePad == 1)
            {
                this.SuspendLayout();
                this.GetControlByName("RoundedElement66").BackgroundImage = Properties.Resources.white;
                this.GetControlByName("label1").BackgroundImage = Properties.Resources.white;
                this.ResumeLayout(false);
            }
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            this.RoundedElement66_MouseLeave(sender, e);
        }

        private void RoundedElement67_MouseEnter(object sender, EventArgs e)
        {
            if (this.activePad == 2)
            {
                this.SuspendLayout();
                this.GetControlByName("RoundedElement67").BackgroundImage = Properties.Resources.gray;
                this.GetControlByName("label2").BackgroundImage = Properties.Resources.gray;
                this.ResumeLayout(false);
            }
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            this.RoundedElement67_MouseEnter(sender, e);
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            if (this.activePad == 2)
            {
                this.SuspendLayout();
                this.GetControlByName("RoundedElement67").BackgroundImage = Properties.Resources.white;
                this.GetControlByName("label2").BackgroundImage = Properties.Resources.white;
                this.ResumeLayout(false);
            }
        }

        private void RoundedElement67_MouseLeave(object sender, EventArgs e)
        {
            this.label2_MouseLeave(sender, e);
        }

        private void RoundedElement66_Click(object sender, EventArgs e)
        {
            if (this.metricsEnabled)
            {
                return;
            }
            this.activePad = 1;
            this.BackgroundImage = new Bitmap(Properties.Resources._1background, this.Width, this.Height);
            this.GetControlByName("RoundedElement67").BackgroundImage = Properties.Resources.dark_blue;
            this.GetControlByName("RoundedElement67").ForeColor = Color.White;
            this.GetControlByName("RoundedElement68").BackgroundImage = Properties.Resources.dark_blue;
            this.GetControlByName("RoundedElement68").ForeColor = Color.White;
            this.GetControlByName("label2").BackgroundImage = Properties.Resources.dark_blue;
            this.GetControlByName("label3").BackgroundImage = Properties.Resources.dark_blue;
            this.GetControlByName("RoundedElement66").ForeColor = Color.FromArgb(61, 82, 161);
            this.GetControlByName("RoundedElement66").BackgroundImage = Properties.Resources.gray;
            this.GetControlByName("label1").BackgroundImage = Properties.Resources.gray;
            this.GetControlByName("RoundedElement5").Visible = false;
            this.GetControlByName("RoundedElement4").Visible = false;
            this.GetControlByName("RoundedElement3").Visible = true;
            this.RenderCircles();
            this.RoundedElement8.Text = "Порядковый номер/название устройства.";
            this.label4.Visible = true;
            this.textBox1.Visible = false;
            this.button1.Visible = false;
            this.label5.Hide();
            this.RoundedElement10.Hide();
            this.RoundedElement11.Hide();
            this.RoundedElement12.Hide();
            this.label9.Hide();
            this.button2.Hide();
            this.button3.Hide();
            this.RoundedElement9.Hide();
            this.label7.Hide();
            this.RoundedElement13.Hide();
            this.RoundedElement14.Hide();
            this.button4.Hide();
            this.button5.Hide();
            this.button6.Hide();
            this.disposeSpeedLabels();
            this.label5.Size = new System.Drawing.Size(272, 43);
        }

        private void RoundedElement67_Click(object sender, EventArgs e)
        {
            if (this.metricsEnabled)
            {
                return;
            }
            this.activePad = 2;
            this.BackgroundImage = new Bitmap(Properties.Resources._2background, this.Width, this.Height);
            this.RoundedElement66.BackgroundImage = Properties.Resources.dark_blue;
            this.RoundedElement66.ForeColor = Color.White;
            this.RoundedElement68.BackgroundImage = Properties.Resources.dark_blue;
            this.RoundedElement68.ForeColor = Color.White;
            this.label1.BackgroundImage = Properties.Resources.dark_blue;
            this.label3.BackgroundImage = Properties.Resources.dark_blue;
            this.RoundedElement67.ForeColor = Color.FromArgb(61, 82, 161);
            this.RoundedElement67.BackgroundImage = Properties.Resources.gray;
            this.label2.BackgroundImage = Properties.Resources.gray;
            this.RoundedElement5.Visible = false;
            this.RoundedElement4.Visible = true;
            this.RoundedElement3.Visible = false;
            this.DisposeCircles();
            this.RoundedElement8.Text = "Укажите расстояние от кольца в метрах:";
            this.label4.Visible = false;
            this.textBox1.Visible = true;
            this.button1.Show();
            this.label5.Text = "Результат в м/с: ";
            this.label5.Show();
            this.RoundedElement10.Show();
            this.RoundedElement11.Show();
            this.RoundedElement12.Show();
            this.label9.Show();
            this.button2.Hide();
            this.button3.Hide();
            this.RoundedElement9.Hide();
            this.label7.Hide();
            this.RoundedElement13.Hide();
            this.RoundedElement14.Hide();
            this.button4.Hide();
            this.button5.Hide();
            this.button6.Hide();
            this.disposeSpeedLabels();
            this.label5.Size = new System.Drawing.Size(272, 43);
        }

        private void RoundedElement68_Click(object sender, EventArgs e)
        {
            if (this.metricsEnabled)
            {
                return;
            }
            this.activePad = 3;
            this.BackgroundImage = new Bitmap(Properties.Resources._3background, this.Width, this.Height);
            this.GetControlByName("RoundedElement66").BackgroundImage = Properties.Resources.dark_blue;
            this.GetControlByName("RoundedElement66").ForeColor = Color.White;
            this.GetControlByName("RoundedElement67").BackgroundImage = Properties.Resources.dark_blue;
            this.GetControlByName("RoundedElement67").ForeColor = Color.White;
            this.GetControlByName("label1").BackgroundImage = Properties.Resources.dark_blue;
            this.GetControlByName("label2").BackgroundImage = Properties.Resources.dark_blue;
            this.GetControlByName("RoundedElement68").ForeColor = Color.FromArgb(61, 82, 161);
            this.GetControlByName("RoundedElement68").BackgroundImage = Properties.Resources.gray;
            this.GetControlByName("label3").BackgroundImage = Properties.Resources.gray;
            this.GetControlByName("RoundedElement5").Visible = true;
            this.GetControlByName("RoundedElement4").Visible = false;
            this.GetControlByName("RoundedElement3").Visible = false;
            this.DisposeCircles();
            int cnt = connections.Values.Sum(x => x ? 1 : 0);
            this.RoundedElement8.Text = $"Подключенные устройства: {cnt}";
            this.label4.Visible = false;
            this.textBox1.Visible = false;
            this.button1.Show();
            this.label5.Text = "Номер круга/время в секундах";
                foreach (Control c in new Control[] {
                this.RoundedElement10, this.RoundedElement11, this.RoundedElement12, this.RoundedElement13, this.RoundedElement14,
                this.button4, this.button5, this.button6, this.label5
            }) { if (this.usableScene == 2) { c.Hide(); } else { c.Show(); } }
            this.label9.Hide();
            this.button2.Show();
            this.button3.Show();
            this.RoundedElement9.Show();
            this.label7.Show();

            this.disposeSpeedLabels();
            this.label5.Size = new System.Drawing.Size(417, 43);
        }

        private void RoundedElement68_MouseEnter(object sender, EventArgs e)
        {
            if (this.activePad == 3)
            {
                this.SuspendLayout();
                this.GetControlByName("label3").BackgroundImage = Properties.Resources.gray;
                this.GetControlByName("RoundedElement68").BackgroundImage = Properties.Resources.gray;
                this.ResumeLayout(false);
            }
        }

        private void RoundedElement68_MouseLeave(object sender, EventArgs e)
        {
            if (this.activePad == 3)
            {
                this.SuspendLayout();
                this.GetControlByName("label3").BackgroundImage = Properties.Resources.white;
                this.GetControlByName("RoundedElement68").BackgroundImage = Properties.Resources.white;
                this.ResumeLayout(false);
            }
        }
        private Dictionary<int, string> GetCircles()
        {
            this.com.write("getCircles");
            string circles;
            try
            {
                circles = this.com.read();
            } catch (TimeoutException)
            {
                circles = this.com.packets.First();
            }
            if (!circles.StartsWith("1"))
            {
                circles = "1" + circles;
            }
            Dictionary<int, string> hashMap = new Dictionary<int, string>();
            foreach (string circle in circles.Split(';'))
            {
                string[] values = circle.Split(':');
                try
                {
                    Convert.ToInt32(values[0]);
                }
                catch { break; }
                if (!connections.ContainsKey(values[0]))
                {
                    connections.Add(values[0], true);
                }
                hashMap.Add(Convert.ToInt32(values[0]), "ESP2866");
            }
            return hashMap;
        }

        private void DisposeCircles()
        {
            foreach (Control c in Controls)
            {
                if (c.Name == "DisposableESPS")
                {
                    c.Visible = false;
                }
            }
        }

        private void RenderCircles()
        {
            Point point = new System.Drawing.Point(208, 84);
            Point btn_loc = new System.Drawing.Point(766, 84);
            Dictionary<int, string> circles = GetCircles();
            ResumeLayout(true);
            foreach (var item in circles)
            {
                Label label = new Label();
                label.BackColor = Color.FromArgb(240, 240, 240);
                label.Location = point;
                point.Y += 42;
                label.TabIndex = 5 + item.Key;
                label.Size = new System.Drawing.Size(510, 33);
                label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                label.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                label.Text = $"№{item.Key}/{item.Value}";
                label.Name = "DisposableESPS";
                label.AutoSize = false;
                Controls.Add(label);
                label.BringToFront();
                Button button = new Button();
                button.BackColor = Color.FromArgb(240, 240, 240);
                button.Location = btn_loc;
                btn_loc.Y += 42;
                button.TabIndex = 5 + item.Key;
                button.Size = new System.Drawing.Size(172, 33);
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));

                bool value = connections.TryGetValue(item.Value, out var c);
                if (c)
                {
                    button.Text = "Выключить";
                }
                else
                {
                    button.Text = "Включить";
                }
                button.Name = "DisposableESPS";
                button.Click += (s, e) => {
                    connections.TryGetValue(item.Value, out var a);
                    connections[item.Value] = !a;
                    if (a)
                    {
                        button.Text = "Включить";
                        this.com.write($"{item.Key}:on");
                    }
                    else
                    {
                        button.Text = "Выключить";
                        this.com.write($"{item.Key}:off");
                    }
                };
                Controls.Add(button);
                button.BringToFront();
            }
            ResumeLayout(false);

        }
        private void pollingThread()
        {
            while (true)
            {
                try
                {
                    string prolet = this.com.read();
                    if (prolet.EndsWith("prolet"))
                    {
                        if (this.activePad == 3)
                        {
                            this.renderNewSpeedLabel(Math.Round(this.time, 2).ToString());
                        }
                    } else
                    {
                        this.com.packets.Enqueue(prolet);
                    }

                } catch { }
                Thread.Sleep(100);
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            this.disposeSpeedLabels();
            if (this.metricsEnabled)
            {
                if (this.timerLabelsThread != null)
                {
                    this.timerLabelsThread.Abort();
                    this.RoundedElement10.BackColor = Color.White;
                    this.RoundedElement10.ForeColor = Color.Black;
                    this.RoundedElement11.BackColor = Color.White;
                    this.RoundedElement11.ForeColor = Color.Black;
                    this.RoundedElement12.BackColor = Color.White;
                    this.RoundedElement12.ForeColor = Color.Black;
                }
                this.button1.Text = "Начать";
                this.metricsEnabled = false;
                this.time = 0.00;
                return;
            }
            ((Button)sender).Text = "Закончить";
            this.metricsEnabled = true;
            if (this.activePad == 2)
            {
                foreach (Control c in Controls)
                {
                    if (c.Name == "textBox1")
                    {
                        int val;
                        try
                        {
                            val = int.Parse(c.Text);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Что то пошло не так...");
                            return;
                        }
                        MessageBox.Show($"Начинаю измерение с {val} метров");
                        // логика измерения
                        this.label9.Text = $"{val} Ceкунд";
                        return;
                    }
                }
            }
            ((Button)sender).Text = "Закончить";
            this.metricsEnabled = true;
            void InvokeColor(Control с, Color color, bool fore)
            {
                с.Invoke((MethodInvoker)delegate
                {
                    if (!fore)
                    {
                        с.BackColor = color;
                    }
                    else
                    {
                        с.ForeColor = color;
                    }
                });
            }
            void updateTimerLabels()
            {
                int step = 1;
                while (true)
                {
                    if (step == 4)
                    {
                        Thread thread = new Thread(this.updateTimer);
                        thread.Start();
                        InvokeColor(this.RoundedElement12, Color.White, false);
                        InvokeColor(this.RoundedElement12, Color.Black, true);
                        return;
                    }
                    if (step == 1)
                    {
                        InvokeColor(this.RoundedElement10, Color.FromArgb(61, 82, 161), false);
                        InvokeColor(this.RoundedElement10, Color.White, true);
                        step = 2;
                    }
                    else if (step == 2)
                    {
                        InvokeColor(this.RoundedElement11, Color.FromArgb(61, 82, 161), false);
                        InvokeColor(this.RoundedElement11, Color.White, true);
                        InvokeColor(this.RoundedElement10, Color.White, false);
                        InvokeColor(this.RoundedElement10, Color.Black, true);
                        step = 3;
                    }
                    else if (step == 3)
                    {
                        InvokeColor(this.RoundedElement12, Color.FromArgb(61, 82, 161), false);
                        InvokeColor(this.RoundedElement12, Color.White, true);
                        InvokeColor(this.RoundedElement11, Color.White, false);
                        InvokeColor(this.RoundedElement11, Color.Black, true);
                        step = 4;
                    }
                    Thread.Sleep(1000);
                }
            }
            if (this.usableScene == 2)
            {
                Thread thread = new Thread(this.updateTimer);
                thread.Start();
            } else
            {
                Thread labelThread = new Thread(updateTimerLabels);
                labelThread.Start();
                this.timerLabelsThread = labelThread;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.metricsEnabled)
            {
                return;
            }
            this.usableScene = 1;
            foreach (Control c in new Control[] {
                this.RoundedElement10, this.RoundedElement11, this.RoundedElement12, this.RoundedElement13, this.RoundedElement14,
                this.button4, this.button5, this.button6, this.label5
            }) { c.Show(); }
            Button btn = (Button)sender;
            btn.BackgroundImage = new Bitmap(Properties.Resources.house_white, btn.Width, btn.Height);
            this.button3.BackgroundImage = new Bitmap(Properties.Resources.sekundomer_black, this.button3.Width, this.button3.Height);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.metricsEnabled)
            {
                return;
            }
            this.usableScene = 2;
            foreach (Control c in new Control[] {
                this.RoundedElement10, this.RoundedElement11, this.RoundedElement12, this.RoundedElement13, this.RoundedElement14,
                this.button4, this.button5, this.button6, this.label5
            }) { c.Hide(); }
            Button btn = (Button)sender;
            btn.BackgroundImage = new Bitmap(Properties.Resources.sekundomer_white, btn.Width, btn.Height);
            this.button2.BackgroundImage = new Bitmap(Properties.Resources.house_black, this.button2.Width, this.button2.Height);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.metricsEnabled)
            {
                return;
            }
            this.disposeSpeedLabels();
            this.circlesAmount = 5;
            button6.BackColor = Color.FromArgb(61, 82, 161);
            button6.ForeColor = Color.White;
            button5.BackColor = Color.White;
            button5.ForeColor = Color.Black;
            button4.BackColor = Color.White;
            button4.ForeColor = Color.Black;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.metricsEnabled)
            {
                return;
            }
            this.disposeSpeedLabels();
            this.circlesAmount = 3;
            button5.BackColor = Color.FromArgb(61, 82, 161);
            button5.ForeColor = Color.White;
            button6.BackColor = Color.White;
            button6.ForeColor = Color.Black;
            button4.BackColor = Color.White;
            button4.ForeColor = Color.Black;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.metricsEnabled)
            {
                return;
            }
            this.circlesAmount = 1;
            button4.BackColor = Color.FromArgb(61, 82, 161);
            button4.ForeColor = Color.White;
            button5.BackColor = Color.White;
            button5.ForeColor = Color.Black;
            button6.BackColor = Color.White;
            button6.ForeColor = Color.Black;
        }

        private void renderNewSpeedLabel(string text)
        {
            Point default_loc = new System.Drawing.Point(211, 338);
            if (this.activePad == 3 && this.speedLabels.Count < this.circlesAmount)
            {
                default_loc.Y += 32 * this.speedLabels.Count;
                Label label = new Label();
                this.label6.Invoke((MethodInvoker)delegate
                {
                    label.AutoSize = false;
                    label.Location = default_loc;
                    label.Name = "DisposableSpeedLabels";
                    label.Size = new System.Drawing.Size(272, 23);
                    label.Text = $"№{this.speedLabels.Count + 1}/{text}";
                    label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                    label.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    this.Controls.Add(label);
                    label.BringToFront();
                    label.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, label.Width, label.Height, 30, 30));
                });
                this.speedLabels.Add(label);
                if (this.speedLabels.Count == this.circlesAmount)
                {
                    this.label6.Invoke((MethodInvoker)delegate
                    {
                        this.label6.Show();
                        this.calculateAvgTimeAndShowIt();
                        this.button1.Text = "Начать";
                    });
                    this.metricsEnabled = false;
                    if (this.timerLabelsThread != null)
                    {
                        this.timerLabelsThread.Abort();
                    }
                    this.time = 0.00;
                }
            }
        }

        private void calculateAvgTimeAndShowIt()
        {
            double total = 0.0;
            int count = 0;
            foreach (Label l in this.speedLabels)
            {
                string text = l.Text.Split('/')[1];
                total += double.Parse(text);
                count += 1;
            }
            this.label6.Text = $"Итоговое время: {total / (double)count} сек";
            this.time = 0.00;
        }

        private void disposeSpeedLabels()
        {
            foreach (Label c in this.speedLabels)
            {
                c.Dispose();
            }
            this.speedLabels.Clear();
            this.label6.Hide();
            this.time = 0.00;
        }

        private void updateTimer()
        {
            while (true)
            {
                string str, after;
                try
                {
                    this.time += 0.05;
                    if (this.metricsEnabled)
                    {
                        str = Math.Round(this.time, 2).ToString();
                        if (!str.Contains(","))
                        {
                            str = $"{str},00";
                        } else
                        {
                            after = str.Split(',')[1];
                            if (after.Length == 1)
                            {
                                str = $"{str}0";
                            }
                        }
                        try
                        {
                            button1.Invoke((MethodInvoker)delegate {
                                button1.Text = $"Закончить\n{str}";
                            });
                        } catch (InvalidAsynchronousStateException)
                        {
                        }
                    }
                    else
                    {
                        this.time = 0.00;
                        button1.Invoke((MethodInvoker)delegate {
                            button1.Text = "Начать";
                        });
                        return;
                    }
                    Thread.Sleep(35);
                } catch (InvalidOperationException) { return; }
            }
        }
    }
}
