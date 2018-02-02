using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.IO;

namespace runwx
{
    public partial class Form1 : Form
    {
        public string configFilePath;
        public string wxPath="";
        public string launchesNum="5";
        public XmlDocument XMLDoc;

        public Form1()
        {
            InitializeComponent();
            //把配置文件路径存储在变量里面
            configFilePath = Application.StartupPath + "\\config.xml";
            XMLDoc = new XmlDocument();
            if (!File.Exists(configFilePath))
            {
                //如果不存在，则创建该文件。
                //File.Create(configFilePath);
                //建立根节点
                XmlElement config = XMLDoc.CreateElement("config");
                XMLDoc.AppendChild(config);
                //写入配置信息,变量在声明的时候已经初始化
                config.SetAttribute("wxpath", wxPath);
                config.SetAttribute("launchesnum", launchesNum);
                XMLDoc.Save("config.xml");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //计算机\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\WeChat
            wxPath = textBox1.Text;//从编辑框里获取填写的微信路径
            wxPath = wxPath.Replace("\"", "");//替换掉路径里的双引号
            launchesNum = textBox2.Text;//编辑框里的启动数量

            //判断微信程序路径是否正确
            if (!File.Exists(wxPath))
            {
                MessageBox.Show("填写的微信路径不正确。","错误");
                return;
            }

            XMLDoc.Load("config.xml");
            //选择节点
            XmlElement config = (XmlElement)XMLDoc.SelectSingleNode("config");
            //写入配置信息
            config.SetAttribute("wxpath", wxPath);
            config.SetAttribute("launchesnum", launchesNum);
            XMLDoc.Save("config.xml");

            int n = Int16.Parse(launchesNum);
            for (int i = 0; i < n; i++)
            {
                new Thread(RunWeiXin).Start();
            }

        }
        public void RunWeiXin()
        {
            //TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            //Console.WriteLine(tss.TotalMilliseconds);
            Process.Start(wxPath);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://zhidao.baidu.com/question/497868258393934324.html");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //读入配置文件数据
            XMLDoc.Load("config.xml");
            //选择节点，"config/xxxxx/xxx"
            XmlElement config = (XmlElement)XMLDoc.SelectSingleNode("config");
            //取得节点里的属性值
            wxPath = config.GetAttribute("wxpath");
            launchesNum = config.GetAttribute("launchesnum");
            //赋值到编辑框里，展示给用户看。
            textBox1.Text = wxPath;
            textBox2.Text = launchesNum;
        }
    }
}
