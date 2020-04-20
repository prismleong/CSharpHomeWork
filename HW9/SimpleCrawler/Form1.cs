using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleCrawler
{
    public partial class Form1 : Form
    {
        bool status_flag;//0-stopped,1-started
        Crawler crawler;
        
        public Form1()
        {
            InitializeComponent();
            status_flag = false;
            crawler = new Crawler();
            crawler.PageDownloaded += Crawler_PageDownloaded;
            crawler.CrawlerStopped += Crawler_CrawlerStopped;
        }
        private void Crawler_CrawlerStopped(Crawler obj)
        {
            Action action = () => listBox1.Items.Add("爬虫已停止");
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void Crawler_PageDownloaded(Crawler crawler, string url, string info)
        {
            //var pageInfo = new { Index = resultBindingSource.Count + 1, URL = url, Status = info };
            Action action = () => { listBox1.Items.Add("正在爬取:"+ crawler.current); };
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            crawler.StartURL = textBox1.Text;

            Match match = Regex.Match(crawler.StartURL, Crawler.urlParseRegex);
            if (match.Length == 0) return;
            string host = match.Groups["host"].Value;
            crawler.HostFilter = "^" + host + "$";
            crawler.FileFilter = ".html?$";

            //Task task = Task.Run(() => crawler.Start());
            crawler.Start();
            listBox1.Items.Add("爬虫已启动....");
        }
    }
}
