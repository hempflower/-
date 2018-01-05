using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;

namespace 整合包专用启动器
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        String server = "http://www.ministudio.tk:81";

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = File.ReadAllText(@"version.txt");
            }
            catch
            {
                MessageBox.Show("读取本地版本信息失败");
            }
            try
            {
                textBox2.Text = HttpGet(server+"/minecraft/pk-001.upd", "");
            }
            catch
            {
                MessageBox.Show("我们未能成功的获取版本信息！！");
            }
            if (textBox1.Text != textBox2.Text && textBox2.Text != "")
            {
                button1.Visible = true;
                label3.Text = "我们发现了一个新版本，建议你立即更新，以便体验到最新的特性！！！";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(HttpGet(server + "/minecraft/pk-001.con", ""));
            }
            catch
            {
                MessageBox.Show("我们未能成功的获取更新内容！");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
            
        }

    }
}
