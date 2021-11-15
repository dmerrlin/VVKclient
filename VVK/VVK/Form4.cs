using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfMath;
using System.IO;

namespace VVK
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

      
        private void Form3_Click(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            if (bt.Text == "sqrt{}" || bt.Text == "log" || bt.Text == "ln" || bt.Text == "exp")
            {
                textBox2.SelectedText += @"\" + bt.Text;
            }
            else
            {
                textBox2.SelectedText += bt.Text;
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }

        private void Button_Click1(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            if (textBox2.Text == "" || textBox2.Text.StartsWith("^") || textBox2.Text.StartsWith("_") || textBox2.Text.StartsWith("'"))
            {

                MessageBox.Show("Ошибка ввода",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.ServiceNotification);
            }
            else
            {

                string latex = textBox2.Text;

                var parser = new TexFormulaParser();
                var formula = parser.Parse(latex);
                var pngBytes = formula.RenderToPng(32.0, 0.0, 0.0, "Arial");
                Form1 Forma = this.Owner as Form1;
                int tabPagesCount = Forma.tabControl1.SelectedIndex;
                PictureBox pb = Forma.tabControl1.TabPages[tabPagesCount].Controls.Find("pb", false).First() as PictureBox;
                TextBox tb = Forma.tabControl1.TabPages[tabPagesCount].Controls.Find("textBox", false).First() as TextBox;
                System.Drawing.Image img = ByteToImage(pngBytes);
                pb.Image = img;

                pb.Size = new Size(img.Width, img.Height);
                pb.Location = new Point((Forma.tabControl1.Width / 2) - (img.Width) / 2, (Forma.tabControl1.Height / 2) - (img.Height) / 2);
                tb.Text = textBox2.Text;

                this.Close();
            }
        }

        private void Btn0_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            string latex = textBox2.Text;
            var parser = new TexFormulaParser();
            if (textBox2.Text.Length == 0)
            {
                string s1 = "Clear";
                var formula = parser.Parse(s1);
                var pngBytes = formula.RenderToPng(22.0, 1.0, 1.0, "Arial");
                System.Drawing.Image img = ByteToImage(pngBytes);
                pictureBox1.Image = img;
            }
            else
            {
                textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length - 1);

                try
                {
                    var formula = parser.Parse(latex);
                    var pngBytes = formula.RenderToPng(12.0, 1.0, 1.0, "Arial");
                    Button pb = this.Controls.Find("bt", false).First() as Button;
                    pb.Enabled = true;
                    System.Drawing.Image img = ByteToImage(pngBytes);
                    pictureBox1.Image = img;
                }
                catch (WpfMath.Exceptions.TexParseException)
                {
                    string s1 = "Incorrect format!";
                    var formula = parser.Parse(s1);
                    Button pb = this.Controls.Find("bt", false).First() as Button;
                    pb.Enabled = false;
                    var pngBytes = formula.RenderToPng(22.0, 1.0, 1.0, "Arial");
                    System.Drawing.Image img = ByteToImage(pngBytes);
                    pictureBox1.Image = img;
                }
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {

            Form1 Forma = this.Owner as Form1;
            int tabPagesCount = Forma.tabControl1.SelectedIndex;
            PictureBox pb = Forma.tabControl1.TabPages[tabPagesCount].Controls.Find("pb", false).First() as PictureBox;
            TextBox tb = Forma.tabControl1.TabPages[tabPagesCount].Controls.Find("textBox", false).First() as TextBox;
            //textBox2.ReadOnly = true;

            string[] str = new string[14] { "*", "+", "-", "/", "=", "(", ")", "sqrt{}", "^", "log", "ln", "!", "x_{n}", "exp" };
            string[] str2 = new string[14] { "Умножение", "Суммирование", "Вычитание", "Деление", "Равно", "Левая скобка", "Правая скобка", "Квадратный корень", "Степень", "Логарифм", "Натуральный логарифм", "Факториал", "Нижний индекс", "Экспонента" };
            Button[] bt = new Button[14];

            int j = 0;
            int k = 0;
            for (int i = 0; i < bt.Count(); i++)
            {

                bt[i] = new Button();
                bt[i].ClientSize = new Size(70, 50);
                bt[i].Location = new Point(50 + k * 70, 200 + j * 50);
                toolTip1.SetToolTip(bt[i], str2[i]);
                bt[i].Text = str[i];

                bt[i].Click += Form3_Click;
                this.Controls.Add(bt[i]);
                k++;

                if ((i + 1) % 4 == 0)
                {
                    j++;
                    k = 0;
                }
            }



            string[] str1 = new string[2] { "C", "<=" };
            Button[] bt1 = new Button[1];


            bt1[0] = new Button();
            bt1[0].ClientSize = new Size(140, 50);
            bt1[0].Location = new Point(190, 350);
            bt1[0].Text = str1[0];
            toolTip1.SetToolTip(bt1[0], "Очистить строку");
            this.Controls.Add(bt1[0]);
            bt1[0].Click += Btn0_Click;



            Button button = new Button() { Dock = Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt", Size = new Size(333, 50) };
            this.Controls.Add(button);
            button.Click += Button_Click1;
            this.ActiveControl = button;
            textBox2.Text = tb.Text;
        }



        private void textBox2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '_' || e.KeyChar == ',' || e.KeyChar == '.' || e.KeyChar == '!' || e.KeyChar == '^' || e.KeyChar == '(' || e.KeyChar == ')' || e.KeyChar == '{' || e.KeyChar == '}' || e.KeyChar == '=' || e.KeyChar == '+' || e.KeyChar == '-' || e.KeyChar == '*' || e.KeyChar == '/' || e.KeyChar == (char)Keys.Back)
            {

            }
            else
            {
                e.Handled = true;
            }
        }

 

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            string latex = textBox2.Text;
            var parser = new TexFormulaParser();
            if (textBox2.Text == "")
            {
                string s1 = "Clear";
                var formula = parser.Parse(s1);
                var pngBytes = formula.RenderToPng(22.0, 1.0, 1.0, "Arial");
                System.Drawing.Image img = ByteToImage(pngBytes);
                pictureBox1.Image = img;
            }
            else
            {


                try
                {
                    var formula = parser.Parse(latex);
                    var pngBytes = formula.RenderToPng(12.0, 1.0, 1.0, "Arial");
                    System.Drawing.Image img = ByteToImage(pngBytes);
                    Button pb = this.Controls.Find("bt", false).First() as Button;
                    pb.Enabled = true;
                    pictureBox1.Image = img;
                }
                catch (WpfMath.Exceptions.TexParseException)
                {
                    string s1 = "Incorrect format!";
                    var formula = parser.Parse(s1);
                    var pngBytes = formula.RenderToPng(22.0, 1.0, 1.0, "Arial");
                     Button pb = this.Controls.Find("bt", false).First() as Button;
                    pb.Enabled = false;
                    System.Drawing.Image img = ByteToImage(pngBytes);
                    pictureBox1.Image = img;
                }


                pictureBox1.Size = new Size(300, 80);
                pictureBox1.Location = new Point(25, 60);
            }
        }
    }
}
