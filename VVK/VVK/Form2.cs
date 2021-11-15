using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Drawing.Drawing2D;

namespace VVK
{
    public partial class Form2 : Form
    {
        public int col_vertex = 0;
         BitMatrix matrixMain;
        public Form2()
        {
            InitializeComponent();
        }

        private object currObject = null;
        private void button1_Click(object sender, EventArgs e)
        {
            createElements(0);
            paint = false;
            Delpaint = false;
        }

        bool delete = false;
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && delete)
            {
                
                PictureBox pb = sender as PictureBox;
                BitMatrix matrixDop = new BitMatrix(col_vertex);
                for (int i = 0; i < col_vertex; i++)
                    for (int j = 0; j < col_vertex; j++)
                        if (matrixMain.GetValue(i, j)) matrixDop.SetValue(i, j, true);

                matrixMain = new BitMatrix(col_vertex-1);
                int k = 0;
                for (int i = 0; i < col_vertex; i++)
                {
                    if (Int32.Parse(pb.Name) < i) k = 1;
                    int m = 0;
                    for (int j = 0; j < col_vertex; j++)
                    {
                        if (Int32.Parse(pb.Name) < j) m = 1;
                             if (matrixDop.GetValue(i, j) && Int32.Parse(pb.Name) != j && Int32.Parse(pb.Name) != i) matrixMain.SetValue(i-k, j-m, true);
                    }
                }

                Label lb = pictureBox1.Controls.Find("label" + pb.Name, false).First() as Label;

                for (int i = Int32.Parse(pb.Name)+1; i < col_vertex; i++)
                {
                    PictureBox pb1 = pictureBox1.Controls.Find(i.ToString(), false).First() as PictureBox;
                    Label lb1 = pictureBox1.Controls.Find("label" + i.ToString(), false).First() as Label;
                    pb1.Name = (Int32.Parse(pb1.Name) - 1).ToString();
                    lb1.Name = "label" + (Int32.Parse(pb1.Name)).ToString();
                    StringBuilder strB = new StringBuilder(lb1.Text);
                    strB[lb1.Text.Length - 1] = (Char)(Int32.Parse(pb1.Name)+49);
                    lb1.Text = strB.ToString();
                }
                col_vertex--;
                pb.Dispose();
                lb.Dispose();
                shiftConnection();

              

            }
            base.OnMouseClick(e);
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox pb = sender as PictureBox;
                if (((pb.Left + e.X - Location.X) <= 560 && (pb.Left + e.X - Location.X) >= 0) && ((pb.Top + e.Y - Location.Y) <= 380) && (pb.Top + e.Y - Location.Y) >= 0)
                {
                    Label lb = pictureBox1.Controls.Find("label"+pb.Name, false).First() as Label;
                    lb.Left += e.X - Location.X;
                    lb.Top += e.Y - Location.Y;
                    pb.Left += e.X - Location.X;
                    pb.Top += e.Y - Location.Y;
                    shiftConnection();
                }
                   
            }
        }

        Point Location;

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Location = e.Location;
            base.OnMouseDown(e);
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            currObject = sender;
        }



        private void moveblePicrureBox1_Click(object sender, EventArgs e)
        {

        }
        bool oneClick = false;
        int x1;
        int y1;
        string name = "";

        private void button2_Click(object sender, EventArgs e)
        {
            createElements(1);
            paint = false;
            Delpaint = false;
        }
        Bitmap bmp;

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (paint)
            {
                if (oneClick)
                {
                    PictureBox pb = sender as PictureBox;
                    if (name != pb.Name)
                    {
                        oneClick = false;
                        matrixMain.SetValue(Int32.Parse(name), Int32.Parse(pb.Name), true);
                        matrixMain.SetValue(Int32.Parse(pb.Name), Int32.Parse(name), true);
                        shiftConnection();
                    }
                    
                }
                else 
                {
                    PictureBox pb = sender as PictureBox;
                    x1 = pb.Location.X;
                    y1 = pb.Location.Y;
                    name = pb.Name;
                    oneClick = true;
                }
            } else if (Delpaint)
            {
                if (oneClick)
                {
                    PictureBox pb = sender as PictureBox;
                    if (name != pb.Name)
                    {
                        oneClick = false;
                        matrixMain.SetValue(Int32.Parse(name), Int32.Parse(pb.Name), false);
                        matrixMain.SetValue(Int32.Parse(pb.Name), Int32.Parse(name), false);
                        shiftConnection();
                    }
                }
                else
                {
                    PictureBox pb = sender as PictureBox;
                    x1 = pb.Location.X;
                    y1 = pb.Location.Y;
                    name = pb.Name;
                    oneClick = true;
                }
            }
                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            createElements(2);
            paint = false;
            Delpaint = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
          bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graph = Graphics.FromImage(bmp);
            SolidBrush brush = new SolidBrush(Color.White);
            graph.FillRectangle(brush, 0, 0, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;

            shiftConnection();

        }
        bool paint = false;
        private void button4_Click(object sender, EventArgs e)
        {
            paint = true;
            Delpaint = false;
        }

        bool Delpaint = false;
        private void button5_Click(object sender, EventArgs e)
        {
            Delpaint = true;
            paint = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

       void createElements(int num)
        {
            PictureBox pictureBox = new PictureBox(); 
            Label label = new Label();
            switch (num)
            {
                case 0: 
                    label.Text = "Пр." + (col_vertex + 1);
                    break;
                case 1: 
                    label.Text = "УВВ." + (col_vertex + 1);
                    break;
                case 2:
                    label.Text = "П." + (col_vertex + 1);
                    break;
                case 3:
                    label.Text = "ВЗУ." + (col_vertex + 1);
                    break;
                case 4:
                    label.Text = "МК." + (col_vertex + 1);
                    break;
                case 5:
                    label.Text = "СК." + (col_vertex + 1);
                    break;
                default: break;
                  
            }
            label.Name = "label" + col_vertex;
            label.Location = new Point(30, 30);
            label.ClientSize = new Size(40, 25);
            label.BackColor = Color.White;
            pictureBox.ClientSize = new Size(50, 50);
            pictureBox.Location = new Point(20, 45);
            pictureBox.Image = imageList1.Images[num];
            pictureBox.Click += PictureBox_Click;
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseClick += PictureBox_MouseClick;
            pictureBox.Name = col_vertex.ToString();
            pictureBox1.Controls.Add(pictureBox);
            pictureBox1.Controls.Add(label);
            col_vertex++;
            if (col_vertex == 1)
            {
                matrixMain = new BitMatrix(col_vertex);
            }
            else
            {
                BitMatrix matrixDop = new BitMatrix(col_vertex - 1);
                for (int i = 0; i < col_vertex - 1; i++)
                    for (int j = 0; j < col_vertex - 1; j++)
                        if (matrixMain.GetValue(i, j)) matrixDop.SetValue(i, j, true);

                matrixMain = new BitMatrix(col_vertex);
                for (int i = 0; i < col_vertex - 1; i++)
                    for (int j = 0; j < col_vertex - 1; j++)
                        if (matrixDop.GetValue(i, j)) matrixMain.SetValue(i, j, true);
            }
        } 
      public  void shiftConnection()
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graph = Graphics.FromImage(bmp);
            SolidBrush brush = new SolidBrush(Color.White);
            graph.FillRectangle(brush, 0, 0, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < col_vertex ; i++)
                for (int j = 0; j < col_vertex ; j++)
                    if (matrixMain.GetValue(i, j))
                    {
                        PictureBox pb = pictureBox1.Controls.Find(i.ToString(), false).First() as PictureBox;
                        PictureBox pb1 = pictureBox1.Controls.Find(j.ToString(), false).First() as PictureBox;
                        graph.DrawLine(pen, pb1.Location.X + 25, pb1.Location.Y + 25, pb.Location.X + 25, pb.Location.Y + 25);
                        pictureBox1.Image = bmp;
                    }


        }
        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog OPF = new SaveFileDialog();
            OPF.InitialDirectory = "d:\\";
            OPF.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(OPF.FileName, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(col_vertex);
                    for (int i = 0; i < col_vertex; i++)
                    {
                        PictureBox pb = pictureBox1.Controls.Find(i.ToString(), false).First() as PictureBox;
                        Label lb = pictureBox1.Controls.Find("label" + pb.Name, false).First() as Label;
                        sw.WriteLine(lb.Text);
                        sw.WriteLine(lb.Location.X);
                        sw.WriteLine(lb.Location.Y);
                        sw.WriteLine(pb.Location.X);
                        sw.WriteLine(pb.Location.Y);

                    }
                    for (int i = 0; i < col_vertex; i++)
                        for (int j = 0; j < col_vertex; j++)
                            if (matrixMain.GetValue(i, j)) sw.WriteLine("1");
                            else sw.WriteLine("0");
                }
                   
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog OPF = new OpenFileDialog();
            OPF.InitialDirectory = "d:\\";
            OPF.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(OPF.FileName, System.Text.Encoding.Default))
                {

                    for (int i = 0; i < col_vertex; i++)
                    {
                        PictureBox pb = pictureBox1.Controls.Find(i.ToString(), false).First() as PictureBox;
                        Label lb = pictureBox1.Controls.Find("label" + pb.Name, false).First() as Label;
                        pb.Dispose();
                        lb.Dispose();
                    }
                        col_vertex = Int32.Parse(sr.ReadLine());
                


                    for (int i = 0; i < col_vertex; i++)
                    {
                        PictureBox pictureBox = new PictureBox();
                        Label label = new Label();



                        label.Name = "label" + i;
                        label.Text = sr.ReadLine();
                        int x = Int32.Parse(sr.ReadLine());
                        int y = Int32.Parse(sr.ReadLine());
                        label.Location = new Point(x, y);
                        x = Int32.Parse(sr.ReadLine());
                        y = Int32.Parse(sr.ReadLine());
                        label.ClientSize = new Size(40, 25);
                        label.BackColor = Color.White;
                        pictureBox.ClientSize = new Size(50, 50);
                        pictureBox.Name = i.ToString();
                       pictureBox.Location = new Point(x, y);
                        string str = "";
                        bool yes = false;
                        for (int j = 0; j < label.Text.Length; j++)
                            if (label.Text[j].ToString() != Convert.ToString((char)('.')) && yes == false)
                            {
                                str += label.Text[j].ToString();
                            }
                            else yes = true;
                        switch (str)
                        {
                            case "Пр":
                                pictureBox.Image = imageList1.Images[0];
                                break;
                            case "УВВ":
                                pictureBox.Image = imageList1.Images[1];
                                break;
                            case "П":
                                pictureBox.Image = imageList1.Images[2];
                                break;
                            case "ВЗУ":
                                pictureBox.Image = imageList1.Images[3];
                                break;
                            case "МК":
                                pictureBox.Image = imageList1.Images[4];
                                break;
                            case "СК":
                                pictureBox.Image = imageList1.Images[5];
                                break;
                            default: break;

                        }

                        pictureBox.Click += PictureBox_Click;
                        pictureBox.MouseDown += PictureBox_MouseDown;
                        pictureBox.MouseMove += PictureBox_MouseMove;
                        pictureBox.MouseClick += PictureBox_MouseClick;
                     
                        pictureBox1.Controls.Add(pictureBox);
                        pictureBox1.Controls.Add(label);
                    }

                    matrixMain = new BitMatrix(col_vertex);
                    for (int i = 0; i < col_vertex; i++)
                        for (int j = 0; j < col_vertex; j++)
                            if (sr.ReadLine() == "1") matrixMain.SetValue(i, j, true);
                            else matrixMain.SetValue(i, j, false);

                    shiftConnection();
                }

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            int tabPagesCount = main.tabControl1.SelectedIndex;
            string str = "";
            for (int i = 0; i < col_vertex; i++)
            {
                Label lb = pictureBox1.Controls.Find("label" + i, false).First() as Label;
                str += lb.Text + " ";
            }
                TextBox tb = main.tabControl1.TabPages[tabPagesCount].Controls.Find("textBox", false).First() as TextBox;
            str += "\r\n";

            for (int i = 0; i < col_vertex; i++)
            {
                for (int j = 0; j < col_vertex; j++)
                    if (matrixMain.GetValue(i, j)) str += "1 "; else str += "0 ";
                str += "\r\n";
            }

            main.matrixThis = matrixMain;
            tb.Text = str;
            this.Dispose();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            createElements(3);
            paint = false;
            Delpaint = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            createElements(4);
            paint = false;
            Delpaint = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            createElements(5);
            paint = false;
            Delpaint = false;
        }

        public void loadCheme(string path)
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                col_vertex = Int32.Parse(sr.ReadLine());



                for (int i = 0; i < col_vertex; i++)
                {
                    PictureBox pictureBox = new PictureBox();
                    Label label = new Label();



                    label.Name = "label" + i;
                    label.Text = sr.ReadLine();
                    int x = Int32.Parse(sr.ReadLine());
                    int y = Int32.Parse(sr.ReadLine());
                    label.Location = new Point(x, y);
                    x = Int32.Parse(sr.ReadLine());
                    y = Int32.Parse(sr.ReadLine());
                    label.ClientSize = new Size(40, 25);
                    label.BackColor = Color.White;
                    pictureBox.ClientSize = new Size(50, 50);
                    pictureBox.Name = i.ToString();
                    pictureBox.Location = new Point(x, y);
                    string str = "";
                    bool yes = false;
                    for (int j = 0; j < label.Text.Length; j++)
                        if (label.Text[j].ToString() != Convert.ToString((char)('.')) && yes == false)
                        {
                            str += label.Text[j].ToString();
                        }
                        else yes = true;
                    switch (str)
                    {
                        case "Пр":
                            pictureBox.Image = imageList1.Images[0];
                            break;
                        case "УВВ":
                            pictureBox.Image = imageList1.Images[1];
                            break;
                        case "П":
                            pictureBox.Image = imageList1.Images[2];
                            break;
                        case "ВЗУ":
                            pictureBox.Image = imageList1.Images[3];
                            break;
                        case "МК":
                            pictureBox.Image = imageList1.Images[4];
                            break;
                        case "СК":
                            pictureBox.Image = imageList1.Images[5];
                            break;
                        default: break;

                    }

                    pictureBox.Click += PictureBox_Click;
                    pictureBox.MouseDown += PictureBox_MouseDown;
                    pictureBox.MouseMove += PictureBox_MouseMove;
                    pictureBox.MouseClick += PictureBox_MouseClick;

                   pictureBox1.Controls.Add(pictureBox);
                    pictureBox1.Controls.Add(label);
                }

               matrixMain = new BitMatrix(col_vertex);
                Form1 fm1 = this.Owner as Form1;
                fm1.matrixResult = new BitMatrix(col_vertex);
                matrixMain = new BitMatrix(col_vertex);
                for (int i = 0; i < col_vertex; i++)
                    for (int j = 0; j < col_vertex; j++)
                        if (sr.ReadLine() == "1") fm1.matrixResult.SetValue(i, j, true);
                        else fm1.matrixResult.SetValue(i, j, false);
                

                if (fm1.matrixThis != null) 
                    matrixMain = fm1.matrixThis;
                else fm1.matrixThis = new BitMatrix(col_vertex);

                fm1.col_vertex = col_vertex;
            }
            shiftConnection();
        }
    }

}
