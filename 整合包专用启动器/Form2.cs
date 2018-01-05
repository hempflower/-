
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 整合包专用启动器
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        

        private void Form2_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(System.Environment.CurrentDirectory + @"\7z.exe x -aoa " + System.Environment.CurrentDirectory + @"\update.7z");
            Thread t = new Thread(new ThreadStart(this.Dl));
            t.Name = "Dl";
            t.Start();



            // cmd(System.Environment.CurrentDirectory + @"\7z.exe x -aoa update.7z");
            // Process.Start(System.Environment.CurrentDirectory + @"\7z.exe x -aoa "+ System.Environment.CurrentDirectory + @"\update.7z");


        }
        
        private void Dl()
            
          
        {
            if (File.Exists("update.7z"))
            {
                File.Delete("update.7z");
            }
            DownloadFile("http://www.ministudio.tk:81/minecraft/pk-001.7z","update.7z",progressBar1,label2);
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            
            
            p.StartInfo.FileName = System.Environment.CurrentDirectory + @"\7z.exe";//需要启动的程序名       
            p.StartInfo.Arguments = "x -aoa update.7z";//启动参数       
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;

            p.Start();//启动       
            if (p.HasExited)//判断是否运行结束       
                p.Kill();
            System.Environment.Exit(0);

        }
        public void DownloadFile(string URL, string filename, System.Windows.Forms.ProgressBar prog, System.Windows.Forms.Label label1)
        {
            float percent = 0;
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(URL);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                if (prog != null)
                {
                    prog.Maximum = (int)totalBytes;
                }
                System.IO.Stream st = myrp.GetResponseStream();
                System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[8192];
                int osize = st.Read(by, 0, (int)by.Length);
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    System.Windows.Forms.Application.DoEvents();
                    so.Write(by, 0, osize);
                    if (prog != null)
                    {
                        prog.Value = (int)totalDownloadedByte;
                    }
                    osize = st.Read(by, 0, (int)by.Length);

                    percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                    //label1.Text = "当前进度:" + percent.ToString() + "%";
                    //System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                    //delay(100);
                }
                so.Close();
                st.Close();
            }
            catch (System.Exception)
            {
                MessageBox.Show("下载失败");
                this.Close();
                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Dispose();
            System.Environment.Exit(0);
        }
        /// <summary>  
        /// 解压缩文件  
        /// </summary>  
        /// <param name="GzipFile">压缩包文件名</param>  
        /// <param name="targetPath">解压缩目标路径</param>         
       private void cmd(String command)
        {
            string str = Console.ReadLine();

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();


            
        }
        private void delay(int millisecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) <= millisecond)
                Application.DoEvents();
        }

    }
}
