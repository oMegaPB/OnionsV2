using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnionsV2
{
    public partial class Form1 : Form
    {
        private int activePad = 1;
        private Dictionary<string, bool> connections = new Dictionary<string, bool>();
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
            this.label6.Hide();
            this.label7.Hide();
            this.label8.Hide();
            this.label9.Hide();
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
            this.label6.Hide();
            this.label7.Hide();
            this.label8.Hide();
            this.label9.Hide();
        }

        private void RoundedElement67_Click(object sender, EventArgs e)
        {
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
            this.button1.Visible = true;
            this.label5.Show();
            this.label6.Show();
            this.label7.Show();
            this.label8.Show();
            this.label9.Show();
        }

        private void RoundedElement68_Click(object sender, EventArgs e)
        {
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
            this.button1.Visible = false;
            this.label5.Hide();
            this.label6.Hide();
            this.label7.Hide();
            this.label8.Hide();
            this.label9.Hide();
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
            Dictionary<int, string> hashMap = new Dictionary<int, string>();
            List<string> names = new List<string>(new string[] { "ESP2866", "ESP2867", "ESP2868", "ESP2869" });
            int cnt = 1;
            foreach (string name in names)
            {
                if (!connections.ContainsKey(name))
                {
                    connections.Add(name, true);
                }
                hashMap.Add(cnt, name);
                cnt += 1;
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
                button.BackColor = Color.FromArgb(240, 240, 240); ;
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
                        // Логика Выключения кольца
                    }
                    else
                    {
                        button.Text = "Выключить";
                        // Логика Включения кольца
                    }
                };
                Controls.Add(button);
                button.BringToFront();
            }
            ResumeLayout(false);

        }

        private void button1_Click(object sender, EventArgs e)
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

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
