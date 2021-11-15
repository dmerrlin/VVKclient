using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using WpfMath;
using System.IO;

namespace VVK
{

    public partial class Form1 : Form
    {
        public string user_token;
        public string user_id;
        public string test_id;
        int[] question_id = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        dynamic[] question;
        int questionNum = 0;
        int questionKol = 1;
        int[] id_variant;
        int mainQuestionNum = 1;
        public BitMatrix matrixResult;
        public BitMatrix matrixThis;
        public int col_vertex = 0;
        void createChoiseWindow(string[] answerChoise, string answer, int number, int idQuestion) {

            RadioButton[] radioButton;
            radioButton = new RadioButton[answerChoise.Length];
            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос "+ number.ToString());
            tabControl1.TabPages[tabPagesCount].Name = idQuestion.ToString();
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true, Dock = Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });
            
            for (int i = 0; i < radioButton.Count(); i++)
            {
                radioButton[i] = new RadioButton();
                radioButton[i].ClientSize = new Size(500, 50);
                radioButton[i].Location = new Point(20, 45 + i * 40);
                radioButton[i].Text = answerChoise[i];
                radioButton[i].Name = "radioButton";
                tabControl1.TabPages[tabPagesCount].Controls.Add(radioButton[i]);
            }
           
            Button button = new Button() { Dock = Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt" };
            button.Click += Button_Click1;
            tabControl1.TabPages[tabPagesCount].Controls.Add(button);
        }

        private void Button_Click1(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;
            Control[] con = tabControl1.TabPages[tabPagesCount].Controls.Find("radioButton", false);
            string str = "";
            int k = 0;
            foreach (var item in con)
            {
               k++;
               RadioButton rb = item as RadioButton;
                if (rb.Checked)
                    str += id_variant[k-1] + " ";
                //rb.Enabled = false;
            }

            sendingResponse(test_id, question_id[questionNum - 1].ToString(), str);


        }

        void createChoisesWindow(string[] answerChoise, string answer, int number, int idQuestion)
        {
            CheckBox[] checkBox;
            checkBox = new CheckBox[answerChoise.Length];
            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true,  Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });
            tabControl1.TabPages[tabPagesCount].Name = idQuestion.ToString();
            for (int i = 0; i < checkBox.Count(); i++)
            {
                checkBox[i] = new CheckBox();
                checkBox[i].ClientSize = new Size(500, 50);
                checkBox[i].Location = new Point(20, 45 + i * 40);
                checkBox[i].Text = answerChoise[i];
                checkBox[i].Name = "checkBox";
                tabControl1.TabPages[tabPagesCount].Controls.Add(checkBox[i]);
            }

            Button button = new Button() { Dock = Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt" };
            button.Click += Button_Click2;
            tabControl1.TabPages[tabPagesCount].Controls.Add(button);
          
        }

        private void Button_Click2(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;
            Control[] con = tabControl1.TabPages[tabPagesCount].Controls.Find("checkBox", false);
            string str = "";
            int k = 0;
            foreach (var item in con)
            {
                k++;
                CheckBox cb = item as CheckBox;
                if (cb.Checked)
                    str += id_variant[k - 1] + " ";
                //cb.Enabled = false;
            }

            sendingResponse(test_id, question_id[questionNum - 1].ToString(), str);

        }

        void createPuttWindow(string answer, int number, int idQuestion)
        {
           
            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true,  Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { Location = new Point(0, 60) , Text = "", Size = new Size(767, 20), Name = "textBox" });
            tabControl1.TabPages[tabPagesCount].Name = idQuestion.ToString();
            Button button = new Button() { Dock = Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt" };
            button.Click += Button_Click3;
            tabControl1.TabPages[tabPagesCount].Controls.Add(button);
          
        }

        private void Button_Click3(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;
          TextBox tb = tabControl1.TabPages[tabPagesCount].Controls.Find("textBox", false).First() as TextBox;
            sendingResponse(test_id, question_id[questionNum - 1].ToString(), tb.Text);
        }

        void createPutNubmerWindow(string answer, int number, int idQuestion)
        {

            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true, Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });
            tabControl1.TabPages[tabPagesCount].Name = idQuestion.ToString();
            TextBox tb = new TextBox() { Location = new Point(0, 60), Text = "", Size = new Size(767, 20), Name = "textBox" };
            tb.KeyPress += Tb_KeyPress;
            tabControl1.TabPages[tabPagesCount].Controls.Add(tb);
            Button button = new Button() { Dock = Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt" };
            button.Click += Button_Click5;

            tabControl1.TabPages[tabPagesCount].Controls.Add(button);
           
        }

        private void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
           (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void Button_Click5(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;
            TextBox tb = tabControl1.TabPages[tabPagesCount].Controls.Find("textBox", false).First() as TextBox;
           
            try
            {
                double answer = Double.Parse(tb.Text);
                //tb.Enabled = false;

                sendingResponse(test_id, question_id[questionNum - 1].ToString(), tb.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат");
            }
            
        }
        void createPuttFormulasWindow(string answer, int number, int idQuestion)
        {

            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            tabControl1.TabPages[tabPagesCount].Name = idQuestion.ToString();

            tabControl1.TabPages[tabPagesCount].Controls.Add(new PictureBox() { Location = new Point(tabControl1.Width / 2, tabControl1.Height / 2), Name = "pb" });
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true, Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Location = new Point(0, 60), Text = "", Size = new Size(767, 20), Name = "textBox" });

            Button btn1 = new Button() { Location = new Point(330, 300), Size = new Size(100, 40), Name = "btn1", Text = "Ввести формулу" };
            tabControl1.TabPages[tabPagesCount].Controls.Add(btn1);
            btn1.Click += Btn1_Click;
            Button button = new Button() { Dock = Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt" };
            button.Click += Button_Click6;
            tabControl1.TabPages[tabPagesCount].Controls.Add(button);

        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;
            PictureBox pb = tabControl1.TabPages[tabPagesCount].Controls.Find("pb", false).First() as PictureBox;
            pb.Image = null;
            Form4 Calc = new Form4();
            Calc.Owner = this;
            Calc.ShowDialog();
        }

        private void Button_Click6(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;
            TextBox tb = tabControl1.TabPages[tabPagesCount].Controls.Find("textBox", false).First() as TextBox;
            // tb.Enabled = false;


            //   bt = tabControl1.TabPages[tabPagesCount].Controls.Find("btn1", false).First() as Button;
            //  bt.Enabled = false;
            sendingResponse(test_id, question_id[questionNum - 1].ToString(), tb.Text);
        }

        void createSortWindow(string[] answerChoise, string answer, int number, int idQuestion)
        {
            ComboBox[] сomboBox;
            Label[] label;
            label = new Label[answerChoise.Length];
            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            tabControl1.TabPages[tabPagesCount].Name = idQuestion.ToString();
            сomboBox = new ComboBox[answerChoise.Length];
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true, Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });

            for (int i = 0; i < сomboBox.Count(); i++)
            {
                label[i] = new Label();
                label[i].ClientSize = new Size(20, 20);
                label[i].Location = new Point(5, 63 + i * 40);
                label[i].Text = (i+1).ToString() + ")";

                tabControl1.TabPages[tabPagesCount].Controls.Add(label[i]);
            }

            for (int i = 0; i < сomboBox.Count(); i++)
            {
                сomboBox[i] = new ComboBox();
                сomboBox[i].ClientSize = new Size(500, 50);
                сomboBox[i].Location = new Point(25, 60 + i * 40);
                сomboBox[i].DropDownStyle = ComboBoxStyle.DropDownList;
                сomboBox[i].Name = "сomboBox";
                for (int j = 0; j < сomboBox.Count(); j++)
                    сomboBox[i].Items.Add(answerChoise[j]);
                сomboBox[i].SelectedIndex = 0;
                tabControl1.TabPages[tabPagesCount].Controls.Add(сomboBox[i]);
            }
            Button button = new Button() { Dock = Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt" };
            button.Click += Button_Click; 
           
            tabControl1.TabPages[tabPagesCount].Controls.Add(button);
           
        }

        private void Button_Click(object sender, EventArgs e)
        {

            int tabPagesCount = tabControl1.SelectedIndex;
            Control[] con = tabControl1.TabPages[tabPagesCount].Controls.Find("сomboBox", false);
            string str = "";
            foreach (var item in con)
            { 
                ComboBox сomboBox = item as ComboBox;
               // сomboBox.Enabled = false;
                str += сomboBox.Text;
            }


            sendingResponse(test_id, question_id[questionNum - 1].ToString(), str);
        }

        void createRatioWindow(string[] answerChoise, string[] answerRatio, string answer, int number)
        {
            ComboBox[] сomboBox;
            Label[] label;
            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            сomboBox = new ComboBox[answerChoise.Length];
            label = new Label[answerChoise.Length];
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true, Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });

            for (int i = 0; i < сomboBox.Count(); i++)
            {
                label[i] = new Label();
                label[i].ClientSize = new Size(100, 25);
                label[i].Location = new Point(20, 60 + i * 40);
                label[i].Text = answerRatio[i];
                label[i].Name = "label";
                tabControl1.TabPages[tabPagesCount].Controls.Add(label[i]);
            }

            for (int i = 0; i < сomboBox.Count(); i++)
            {
                сomboBox[i] = new ComboBox();
                сomboBox[i].ClientSize = new Size(200, 50);
                сomboBox[i].Location = new Point(200, 60 + i * 40);
                сomboBox[i].Name = "сomboBox";
                сomboBox[i].DropDownStyle = ComboBoxStyle.DropDownList;
                for (int j = 0; j < сomboBox.Count(); j++)
                    сomboBox[i].Items.Add(answerChoise[j]);
                сomboBox[i].SelectedIndex = 0;
                tabControl1.TabPages[tabPagesCount].Controls.Add(сomboBox[i]);
            }

            Button button = new Button() { Dock = Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt" };
            button.Click += Button_Click4; ;

            tabControl1.TabPages[tabPagesCount].Controls.Add(button);
           
        }

        void createSchemeWindow(string answer, int number)
        {

            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true, Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { Location = new Point(0, 60), Text = "", Size = new Size(767, 200), Name = "textBox", Multiline = true }) ;
            Button button = new Button() { Location = new Point(20, 280), Size = new Size(730, 50),  Text = "Редактор схем"};
            Button button1 = new Button() { Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt", };
            button1.Click += Button_Click7;
            button.Click += Button_Click8;
            tabControl1.TabPages[tabPagesCount].Controls.Add(button);
            tabControl1.TabPages[tabPagesCount].Controls.Add(button1);
          
        }

        private void Button_Click8(object sender, EventArgs e)
        {
           Form2 form = new Form2();
            form.Owner = this;
            form.ShowDialog();
        }

        private void Button_Click7(object sender, EventArgs e)
        {
           
        }

        private void Button_Click4(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;
            Control[] con = tabControl1.TabPages[tabPagesCount].Controls.Find("сomboBox", false);
            Control[] con1 = tabControl1.TabPages[tabPagesCount].Controls.Find("label", false);
            string str = "";
            int l = 0;
            foreach (var item in con)
            {
                ComboBox сomboBox = item as ComboBox;
               // сomboBox.Enabled = false;
              
                Label lb = con1[l] as Label;
                str += lb.Text + сomboBox.Text;
                l++;
            }
            sendingResponse(test_id, question_id[questionNum - 1].ToString(), str);
        }

        private static readonly HttpClient client = new HttpClient();
        public async void startTest()
        {
            var values = new Dictionary<string, string>
                {
                    { "user_id", user_id },
                  
                };

            var content = new FormUrlEncodedContent(values);



            var response = await client.PostAsync("http://localhost:8000/start_test/", content);

            var responseString = await response.Content.ReadAsStringAsync();



            dynamic jsonDe = JsonConvert.DeserializeObject(responseString);
            test_id = jsonDe.test_id;
            int kol_q = 1;
            foreach (dynamic typeStr in jsonDe.questions)
            {
                question_id[kol_q - 1] = typeStr.question.id;
                dynamic[] dop = new dynamic[kol_q - 1];
                for (int i = 0; i < kol_q - 1; i++)
                    dop[i] = question[i];
                question = new dynamic[kol_q];
                for (int i = 0; i < kol_q - 1; i++)
                    question[i] = dop[i];
                question[kol_q - 1] = typeStr;
                kol_q++;
            }
            questionOutput(question[questionNum], questionNum+1);
           questionNum++;
        }
        public Form1()
        {
            InitializeComponent();
        }
        int qusestionStrNum = 1;
        string[] questionMain = new string[0];
        int colParametr;
       string[] anwerMain = new string[0];
        void scenarioLab7() {
           string dopStr = "";
            switch (mainQuestionNum)
            {
                case 1:
                    createSchemeWindowMain(questionMain[qusestionStrNum], mainQuestionNum);
                    qusestionStrNum++;
                    break;
                case 2:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createChoiseWindowMain(dopStr, mainQuestionNum);
               
                    break;
                case 3:
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createChoiseWindowMain("Укажите граф передачи стохастической сети", mainQuestionNum, 4);
                  
                    break;
                case 4:
                    dopStr = questionMain[qusestionStrNum];
               
                    qusestionStrNum++;
                    anwerMain = new string[0];
                    for (int i = 0; i < colParametr; i++  )
                    {
                        Array.Resize(ref anwerMain, anwerMain.Length + 1);
                        anwerMain[anwerMain.Length - 1] = questionMain[qusestionStrNum];
                        qusestionStrNum++;
                    }
                    createMainQuestionWindow(dopStr, mainQuestionNum, colParametr, "Лямда");
                    break;
                case 5:
                    dopStr = questionMain[qusestionStrNum];

                    qusestionStrNum++;
                    anwerMain = new string[0];
                    for (int i = 0; i < colParametr; i++)
                    {
                        Array.Resize(ref anwerMain, anwerMain.Length + 1);
                        anwerMain[anwerMain.Length - 1] = questionMain[qusestionStrNum];
                        qusestionStrNum++;
                    }
                    createMainQuestionWindow(dopStr, mainQuestionNum, colParametr, "Альфа");
                    break;
                case 6:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttWindowMain(dopStr, mainQuestionNum);
                    break;
                case 7:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttFormulasWindowMain(dopStr, mainQuestionNum);
                    break;
                case 8:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[0];
                    for (int i = 0; i < colParametr; i++)
                    {
                        Array.Resize(ref anwerMain, anwerMain.Length + 1);
                        anwerMain[anwerMain.Length - 1] = questionMain[qusestionStrNum];
                        qusestionStrNum++;
                    }
                    createMainQuestionWindow(dopStr, mainQuestionNum, colParametr, "Бэта");
                    break;
                case 9:
                    dopStr = questionMain[qusestionStrNum];
                   
                    qusestionStrNum++;
                    anwerMain = new string[0];
                    for (int i = 0; i < colParametr; i++)
                    {
                        Array.Resize(ref anwerMain, anwerMain.Length + 1);
                        anwerMain[anwerMain.Length - 1] = questionMain[qusestionStrNum];
                        qusestionStrNum++;
                    }
                    createMainQuestionWindow(dopStr, mainQuestionNum, colParametr, "Пи");
                    break;
                case 10:
                    dopStr = questionMain[qusestionStrNum];
                  
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttFormulasWindowMain(dopStr, mainQuestionNum);
                    break;
                case 11:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttWindowMain(dopStr, mainQuestionNum);

                    break;
                case 12:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttFormulasWindowMain(dopStr, mainQuestionNum);

                    break;
                case 13:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[0];
                    for (int i = 0; i < colParametr; i++)
                    {
                        Array.Resize(ref anwerMain, anwerMain.Length + 1);
                        anwerMain[anwerMain.Length - 1] = questionMain[qusestionStrNum];
                        qusestionStrNum++;
                    }
                    createMainQuestionWindow(dopStr, mainQuestionNum, colParametr, "L");

                    break;
                case 14:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttFormulasWindowMain(dopStr, mainQuestionNum);

                    break;
                case 15:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[0];
                    for (int i = 0; i < colParametr; i++)
                    {
                        Array.Resize(ref anwerMain, anwerMain.Length + 1);
                        anwerMain[anwerMain.Length - 1] = questionMain[qusestionStrNum];
                        qusestionStrNum++;
                    }
                    createMainQuestionWindow(dopStr, mainQuestionNum, colParametr, "m");
                    break;
                case 16:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttFormulasWindowMain(dopStr, mainQuestionNum);

                    break;
                case 17:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[0];
                    for (int i = 0; i < colParametr; i++)
                    {
                        Array.Resize(ref anwerMain, anwerMain.Length + 1);
                        anwerMain[anwerMain.Length - 1] = questionMain[qusestionStrNum];
                        qusestionStrNum++;
                    }
                    createMainQuestionWindow(dopStr, mainQuestionNum, colParametr, "W");

                    break;
                case 18:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttFormulasWindowMain(dopStr, mainQuestionNum);

                    break;
                case 19:
                    dopStr = questionMain[qusestionStrNum];
                
                    qusestionStrNum++;
                    anwerMain = new string[0];
                    for (int i = 0; i < colParametr; i++)
                    {
                        Array.Resize(ref anwerMain, anwerMain.Length + 1);
                        anwerMain[anwerMain.Length - 1] = questionMain[qusestionStrNum];
                        qusestionStrNum++;
                    }
                    createMainQuestionWindow(dopStr, mainQuestionNum, colParametr, "U");
                    break;
                case 20:
                    dopStr = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttWindowMain(dopStr, mainQuestionNum);

                    break;
                case 21:
                    dopStr = questionMain[qusestionStrNum];
                   
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttWindowMain(dopStr, mainQuestionNum);
                    break;
                case 22:
                    dopStr = questionMain[qusestionStrNum];
                    
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttWindowMain(dopStr, mainQuestionNum);
                    break;
                case 23:
                    dopStr = questionMain[qusestionStrNum];
                  
                    qusestionStrNum++;
                    anwerMain = new string[1];
                    anwerMain[0] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                    createPuttWindowMain(dopStr, mainQuestionNum);
                    break;
                default:
                    MessageBox.Show("Вопросов больше нет!");
                    break;
            }
            mainQuestionNum++;
        }

        public string numLab = "3";
        public string option = "1";
        private void Form1_Load(object sender, EventArgs e)
        {

            this.Hide();

            Form3 fm3 = new Form3();
            fm3.Owner = this;
            fm3.ShowDialog();

            //Form5 fm5 = new Form5();
            //fm5.Owner = this;
            //fm5.ShowDialog();


            //switch (numLab)
            //{
            //    case "3":
            //        using (StreamReader sr = new StreamReader($"lab{numLab}\\Сценарий_лаб{numLab}{option}.txt", System.Text.Encoding.Default))
            //        {
            //            string st = "";
            //            while ((st = sr.ReadLine()) != null)
            //            {
            //                Array.Resize(ref questionMain, questionMain.Length + 1);
            //                questionMain[questionMain.Length - 1] = st;

            //            }
            //        }
            //        colParametr = Int32.Parse(questionMain[0]);
            //        scenarioLab7();
            //        break;
            //    case "6":
            //        using (StreamReader sr = new StreamReader($"lab{numLab}\\Сценарий_лаб{numLab}{option}.txt", System.Text.Encoding.Default))
            //        {
            //            string st = "";
            //            while ((st = sr.ReadLine()) != null)
            //            {
            //                Array.Resize(ref questionMain, questionMain.Length + 1);
            //                questionMain[questionMain.Length - 1] = st;

            //            }
            //        }
            //        colParametr = Int32.Parse(questionMain[0]);
            //        scenarioLab6();
            //        break;
            //}

        }

        void questionOutput(dynamic typeStr, int questionNumber)
        {
            string str = typeStr.question.task_type;
            switch (str)
            {
                case "v":
                    int kol_var = 1;
                    string[] answer = new string[kol_var - 1];
                    id_variant = new int[kol_var - 1];
                    foreach (dynamic typeStr1 in typeStr.question.test_variant)
                    {

                        string[] dop = new string[kol_var - 1];
                        int[] dop_v = new int[kol_var - 1];

                        for (int i = 0; i < kol_var - 1; i++)
                        {
                            dop[i] = answer[i];
                            dop_v[i] = id_variant[i];

                        }
                           
                        answer = new string[kol_var];
                        id_variant = new int[kol_var];
                        for (int i = 0; i < kol_var - 1; i++)
                        {
                            answer[i] = dop[i];
                            id_variant[i] = dop_v[i];
                        }

                        id_variant[kol_var - 1] = typeStr1.id;
                        answer[kol_var - 1] = typeStr1.question_variant;
                        kol_var++;

                    }
                    createChoiseWindow(answer, typeStr.question.test_description.ToString(), questionNumber, (int)typeStr.question.id);

                    break;
                    case "mv":
                    int kol_var1 = 1;
                      id_variant = new int[kol_var1 - 1];
                    string[] answer1 = new string[kol_var1 - 1];
                    foreach (dynamic typeStr1 in typeStr.question.test_variant)
                    {

                        string[] dop = new string[kol_var1 - 1];
                        int[] dop_v1 = new int[kol_var1 - 1];
                        for (int i = 0; i < kol_var1 - 1; i++)
                        {
                            dop[i] = answer1[i];
                            dop_v1[i] = id_variant[i];
                        }
                       answer1 = new string[kol_var1];
                        id_variant = new int[kol_var1];
                        for (int i = 0; i < kol_var1 - 1; i++)
                        {
                            answer1[i] = dop[i];
                            id_variant[i] = dop_v1[i];
                        }
                        id_variant[kol_var1 - 1] = typeStr1.id;
                        answer1[kol_var1 - 1] = typeStr1.question_variant;
                        kol_var1++;

                    }
                    createChoisesWindow(answer1, typeStr.question.test_description.ToString(), questionNumber, (int)typeStr.question.id);
                    break;
                case "o":
                    createPuttWindow(typeStr.question.test_description.ToString(), questionNumber, (int)typeStr.question.id);
                    break;
                case "f":
                    createPuttFormulasWindow(typeStr.question.test_description.ToString(), questionNumber, (int)typeStr.question.id);
                    break;
                default: break;

            }
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void createPuttFormulasWindowMain(string answer, int number)
        {

            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            tabControl1.TabPages[tabPagesCount].Controls.Add(new PictureBox() { Location = new Point(tabControl1.Width / 2, tabControl1.Height / 2), Name = "pb" });
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true, Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Location = new Point(0, 60), Text = anwerMain[0], Size = new Size(767, 20), Name = "textBox" });

            Button btn1 = new Button() { Location = new Point(330, 300), Size = new Size(100, 40), Name = "btn1", Text = "Ввести формулу" };
            tabControl1.TabPages[tabPagesCount].Controls.Add(btn1);
            btn1.Click += Btn1_Click_Main;
            Button button = new Button() { Dock = Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt" };
            button.Click += Button_Click6_Main;
            tabControl1.TabPages[tabPagesCount].Controls.Add(button);

        }

        private void Btn1_Click_Main(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;
            PictureBox pb = tabControl1.TabPages[tabPagesCount].Controls.Find("pb", false).First() as PictureBox;
            pb.Image = null;
            Form4 Calc = new Form4();
            Calc.Owner = this;
            Calc.ShowDialog();
        }

        private void Button_Click6_Main(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;
            TextBox tb = tabControl1.TabPages[tabPagesCount].Controls.Find("textBox", false).First() as TextBox;
            if (tb.Text == anwerMain[0])
            {
                tb.Enabled = false;
                Button bt = tabControl1.TabPages[tabPagesCount].Controls.Find("bt", false).First() as Button;
                bt.Enabled = false;
                scenarioLab7();
            }
            else MessageBox.Show("Неверно!");
        }

        void createPuttWindowMain(string answer, int number)
        {

            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true, Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { Location = new Point(0, 60), Text = anwerMain[0], Size = new Size(767, 20), Name = "textBox" });
            Button button = new Button() { Dock = Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt" };
            button.Click += Button_Click3_Main;
            tabControl1.TabPages[tabPagesCount].Controls.Add(button);

            if (numLab == "6")
            { 
                Bitmap image = new Bitmap($"lab{numLab}\\Задание10\\{option}.png");
                tabControl1.TabPages[tabPagesCount].Controls.Add(new PictureBox() { Location = new Point(20, 100), Size = new Size(767, 300), Name = "pb", Image = image }) ;
                Button button1 = new Button() { Location = new Point(320, 400), Size = new Size(150, 25), Text = "Дополнительное окно", Name = "bt1" };
                tabControl1.TabPages[tabPagesCount].Controls.Add(button1);
                button1.Click += Button1_Click; ;
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Form6 fm6 = new Form6();
            fm6.Owner = this;
            int[,] table = new int[16,120];
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 120; j++)
                    table[i, j] = 0;

            for (int j = 0; j < 120; j++)
            {
                var column = new DataGridViewTextBoxColumn();
                column.HeaderText = (j+1).ToString();
                fm6.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { column});
            }
            for (int i = 0; i < 16; i++)
                fm6.dataGridView1.Rows.Add();

          
                fm6.Show();
        }

        private void Button_Click3_Main(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;
            TextBox tb = tabControl1.TabPages[tabPagesCount].Controls.Find("textBox", false).First() as TextBox;
            if (tb.Text == anwerMain[0])
            {
                tb.Enabled = false;
                Button bt = tabControl1.TabPages[tabPagesCount].Controls.Find("bt", false).First() as Button;
                bt.Enabled = false;

                if (numLab == "3")
                    scenarioLab7();
                else if (numLab == "6")
                    scenarioLab6();

            }
            else MessageBox.Show("Неверно!");
        
        }

        void createChoiseWindowMain(string answer, int number, int kol = 2)
        {
            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true, Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });
            Button button = new Button() { Dock = Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt" };
            button.Click += Button_Click4_Main;
            tabControl1.TabPages[tabPagesCount].Controls.Add(button);
            RadioButton[] radioButton;
            radioButton = new RadioButton[kol];

            if (radioButton.Count() == 2)
            {
                for (int i = 0; i < radioButton.Count(); i++)
                {
                    radioButton[i] = new RadioButton();
                    radioButton[i].ClientSize = new Size(300, 300);
                    radioButton[i].Location = new Point(100 + i * 350, 100);
                    Bitmap image = new Bitmap($"lab{numLab}\\Задание2\\{option}{i + 1}.png");
                    radioButton[i].Image = image;
                    radioButton[i].Name = "radioButton";
                    tabControl1.TabPages[tabPagesCount].Controls.Add(radioButton[i]);
                }
            }
            else
            {
                int j = 0;
                int k = 0;
                for (int i = 0; i < radioButton.Count(); i++)
                {
                    radioButton[i] = new RadioButton();
                    radioButton[i].ClientSize = new Size(250, 190);
                    radioButton[i].Location = new Point(50 + k * 300, 50 + j * 190);
                    Bitmap image = new Bitmap($"lab{numLab}\\Задание3\\{option}{i + 1}.png");
                    radioButton[i].Image = image;
                    radioButton[i].Name = "radioButton";
                    tabControl1.TabPages[tabPagesCount].Controls.Add(radioButton[i]);
                    if (i == 1) { j++; k = 0; } else k++;
                }


            }
        }

        private void Button_Click4_Main(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;

            Control[] con = tabControl1.TabPages[tabPagesCount].Controls.Find("radioButton", false);
            string str = "";
            foreach (var item in con)
            {
                RadioButton rb = item as RadioButton;
                if (rb.Checked)
                    str += "1";
                else str += "0";
            }
            if (str == anwerMain[0])
            {
                Button bt = tabControl1.TabPages[tabPagesCount].Controls.Find("bt", false).First() as Button;
                bt.Enabled = false;
                scenarioLab7();
            }
            else MessageBox.Show("Неверно!");

        }

        void createSchemeWindowMain(string answer, int number)
        {

            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true, Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 50) });
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { Location = new Point(0, 60), Text = "", Size = new Size(767, 200), Name = "textBox", Multiline = true });
            Button button = new Button() { Location = new Point(20, 280), Size = new Size(730, 50), Text = "Редактор схем" };
            Button button1 = new Button() { Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt", };
            button1.Click += Button_Click7_Main;
            button.Click += Button_Click8_Main;
            tabControl1.TabPages[tabPagesCount].Controls.Add(button);
            tabControl1.TabPages[tabPagesCount].Controls.Add(button1);

        }

        private void Button_Click8_Main(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Owner = this;
            form.loadCheme($"lab{numLab}\\Задание1\\Задание1_{option}.txt");
            form.ShowDialog();


        }

        private void Button_Click7_Main(object sender, EventArgs e)
        {
            if (matrixThis != null)
            {
                bool cheak = true;
                for (int i = 0; i < col_vertex; i++)
                    for (int j = 0; j < col_vertex; j++)
                        if (matrixThis.GetValue(i, j) != matrixResult.GetValue(i, j)) cheak = false;

                if (cheak)
                {
                   
                }

                else
                    MessageBox.Show("Схема составлена неверно");
            }
            else
                MessageBox.Show("Схема составлена неверно");
            int tabPagesCount = tabControl1.SelectedIndex;
            scenarioLab7();
            Button bt = tabControl1.TabPages[tabPagesCount].Controls.Find("bt", false).First() as Button;
            bt.Enabled = false;
        }

        void createMainQuestionWindow(string answer, int number, int col , string parametr) {
            int tabPagesCount = tabControl1.TabPages.Count;
            tabControl1.TabPages.Add("Вопрос " + number.ToString());
            tabControl1.TabPages[tabPagesCount].Controls.Add(new TextBox() { ReadOnly = true, Multiline = true, Dock = System.Windows.Forms.DockStyle.Top, Text = answer, Size = new Size(50, 75) });
            TextBox[] textBox;
            Label[] label;
            textBox = new TextBox[col];
            label = new Label[col];

            string[] lab6Question = new string[6] { "Размер сети:", "Порядок узла:", "Диаметр:", " Число связей:", "Ширина бисекции:", "Симметричность:" };

            for (int i = 0; i < textBox.Count(); i++)
            {
                label[i] = new Label();
                label[i].ClientSize = new Size(300, 25);
                label[i].Location = new Point(20, 80 + i * 40);
                if (numLab == "3")
                    label[i].Text = $"Рассчитать вероятность простоя {parametr}{i+1} для для S1,S2,...Sn.";
                else if (numLab == "6")
                    label[i].Text = lab6Question[i];
                label[i].Name = "label";
                tabControl1.TabPages[tabPagesCount].Controls.Add(label[i]);

                textBox[i] = new TextBox();
                textBox[i].ClientSize = new Size(200, 50);
                textBox[i].Location = new Point(350, 80 + i * 40);
                textBox[i].Name = "textBox";
                textBox[i].Text = anwerMain[i];
                tabControl1.TabPages[tabPagesCount].Controls.Add(textBox[i]);
            }



            Button button = new Button() { Dock = System.Windows.Forms.DockStyle.Bottom, Text = "Принять", Name = "bt", };
            button.Click += Button_Click9; ;
            tabControl1.TabPages[tabPagesCount].Controls.Add(button);
        }

        private void Button_Click9(object sender, EventArgs e)
        {
            int tabPagesCount = tabControl1.SelectedIndex;
            Control[] con = tabControl1.TabPages[tabPagesCount].Controls.Find("textBox", false);
             int k = 0;
            bool cheack = true;
            foreach (var item in con)
            {

                TextBox tb = item as TextBox;
                if (tb.Text == anwerMain[k].ToString())
                {
                    tb.Enabled = false;
                }
                else cheack = false;
                k++;
            }

            if (cheack == true)
            {
                if (numLab == "3")
                    scenarioLab7();
                else if (numLab == "6")
                    scenarioLab6();

                Button bt = tabControl1.TabPages[tabPagesCount].Controls.Find("bt", false).First() as Button;
                bt.Enabled = false;
            }
            else MessageBox.Show("Не все ответы верны");
        }

        async void sendingResponse(string id_test, string id_question, string question_response)
        {
            var values = new Dictionary<string, string>
                {
                    { "user_test_id", id_test },
                    { "test_question_id", id_question },
                    {"question_response", question_response.Trim() }
                };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://localhost:8000/response/", content);

            var responseString = await response.Content.ReadAsStringAsync();

            dynamic jsonDe = JsonConvert.DeserializeObject(responseString);

            if ((int)response.StatusCode != 200)
            {
                string errors = "";
                foreach (dynamic error in jsonDe)
                {
                    errors += error.Name + ": " + error.Value[0] + "\n";
                }
                MessageBox.Show(errors);
            }
            else
            {
                questionNum++;
                if (question.Length >= questionNum)
                {
                    questionOutput(question[questionNum - 1], questionNum);
                }
                else
                {
                       
                         values = new Dictionary<string, string>
                        {
                            { "user_test_id", id_test },
                           
                        };

                         content = new FormUrlEncodedContent(values);

                         response = await client.PostAsync("http://localhost:8000/result/", content);

                         responseString = await response.Content.ReadAsStringAsync();

                         jsonDe = JsonConvert.DeserializeObject(responseString);
                         MessageBox.Show("Время: "+ jsonDe.time + "\r\n Вопросов " + jsonDe.valid_question + " из " + jsonDe.question + "\r\n Допуск: "+ jsonDe.status);
                    if (jsonDe.status == "True") createSchemeWindow("Суммарное количество каналов между всеми узлами сети это...", 8);
                }
                int tabPagesCount = tabControl1.SelectedIndex;
                Button bt = tabControl1.TabPages[tabPagesCount].Controls.Find("bt", false).First() as Button;
                bt.Enabled = false;

            }
        }

        void scenarioLab6()
        {
            if (mainQuestionNum > 0 && mainQuestionNum < 10)
            {
                string dopStr = questionMain[qusestionStrNum];
                qusestionStrNum++;
                anwerMain = new string[0];
                for (int i = 0; i < colParametr; i++)
                {
                    Array.Resize(ref anwerMain, anwerMain.Length + 1);
                    anwerMain[anwerMain.Length - 1] = questionMain[qusestionStrNum];
                    qusestionStrNum++;
                }
                createMainQuestionWindow(dopStr, mainQuestionNum, colParametr, "Лямда");
            }
            else if (mainQuestionNum > 9 && mainQuestionNum < 15)
            {
                string dopStr = questionMain[qusestionStrNum];
                qusestionStrNum++;
                anwerMain = new string[1];
                anwerMain[0] = questionMain[qusestionStrNum];
                qusestionStrNum++;
                createPuttWindowMain(dopStr, mainQuestionNum);
            }
            else MessageBox.Show("Вопросов больше нет");
            mainQuestionNum++;

        }

    }
}
