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

namespace VVK
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        

        private static readonly HttpClient client = new HttpClient();
        private async void button1_Click(object sender, EventArgs e)
        {


                var values = new Dictionary<string, string>
                {
                    { "username", textBox1.Text },
                    { "password", textBox2.Text }
                };

            var content = new FormUrlEncodedContent(values);

          

            var response = await client.PostAsync("http://localhost:8000/api-token-auth/", content);

            var responseString = await response.Content.ReadAsStringAsync();


            dynamic jsonDe = JsonConvert.DeserializeObject(responseString);

            if ((int)response.StatusCode != 200)
            {
                string errors = "";
                foreach(dynamic error in jsonDe)
                {
                    errors += error.Name +": "+ error.Value[0]+ "\n";
                }
                MessageBox.Show(errors);
            } else
            {
                Form1 main = this.Owner as Form1;
                main.user_id = jsonDe.user_id;
                main.user_token = jsonDe.token;
                main.Visible = true;
                Console.WriteLine(jsonDe.token);
                Console.WriteLine(jsonDe.user_id);
                Console.WriteLine(jsonDe.email);
                main.startTest();
                this.Hide();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
