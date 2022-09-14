using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace ProjeHaber
{
    public partial class Form1 : Form
    {
        static Connection con = new Connection();
        public Form1()
        {
            InitializeComponent();
        }
        string wbtHbr="", wbtHbrctrl = "a", mynetHaber="", mynetHaberctrl = "a", milliyetHaber="", milliyetHabercrtl = "a", trtHaber="", trtHaberctrl = "a";
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "0";
            timer1.Enabled = true;

        }
        private void webtekno()
        {
            WebRequest SiteyeBaglantiTalebi = HttpWebRequest.Create("https://www.webtekno.com/");
            WebResponse GelenCevap = SiteyeBaglantiTalebi.GetResponse();
            StreamReader CevapOku = new StreamReader(GelenCevap.GetResponseStream());
            string KaynakKodlar = CevapOku.ReadToEnd();
            //KaynakKodlar.IndexOf("<a class=\"headline__blocks__link\" href=")
            var linkText = KaynakKodlar.Substring(KaynakKodlar.IndexOf("<a class=\"headline__blocks__link\" href="));
            string lnk = linkText.Substring(48, linkText.IndexOf("title=") - 50);
            wbtHbr = linkText.Substring(linkText.IndexOf("title=") + 6, linkText.IndexOf("onclick=") - linkText.IndexOf("title=") - 6);
            string imgUrl = KaynakKodlar.Substring(KaynakKodlar.IndexOf("<div class=\"headline__blocks__image\" style=\"background-image: url(")+66,KaynakKodlar.IndexOf("<a class=\"headline__blocks__link\" href=")- KaynakKodlar.IndexOf("<div class=\"headline__blocks__image\" style=\"background-image: url(")-76);
            if (wbtHbrctrl!=wbtHbr)
            {
                con.client = new FireSharp.FirebaseClient(con.config);
                con.response = con.client.Get("newsProje/");
                var data = con.response.Body;
                List<haber> rst = JsonConvert.DeserializeObject<List<haber>>(data);
                var id = Convert.ToString(Convert.ToInt16(rst.LastOrDefault().id) + 1);
                haber h = new haber
                {
                    id = id,
                    baslik = wbtHbr,
                    imgLink = imgUrl,
                    link = lnk,
                    site = "Webtekno"
                };
                con.client.Update("newsProje/" + id, h);
                wbtHbrctrl = wbtHbr;
            }

        }
        private void mynet()
        {
            WebRequest SiteyeBaglantiTalebi = HttpWebRequest.Create("https://www.mynet.com/haber/rss/sondakika");
            WebResponse GelenCevap = SiteyeBaglantiTalebi.GetResponse();
            StreamReader CevapOku = new StreamReader(GelenCevap.GetResponseStream());
            string KaynakKodlar = CevapOku.ReadToEnd();
            KaynakKodlar = KaynakKodlar.Substring(KaynakKodlar.IndexOf("<item>"));
            mynetHaber = KaynakKodlar.Substring(KaynakKodlar.IndexOf("![CDATA[")+8,KaynakKodlar.IndexOf("]]>")- KaynakKodlar.IndexOf("![CDATA[")-8);
            string img = KaynakKodlar.Substring(KaynakKodlar.IndexOf("img300x300")+11,KaynakKodlar.IndexOf("</img300x300>")- KaynakKodlar.IndexOf("img300x300")-11);
            string url = KaynakKodlar.Substring(KaynakKodlar.IndexOf("link")+5,KaynakKodlar.IndexOf("/link") -KaynakKodlar.IndexOf("link")-6);
            if (mynetHaber!=mynetHaberctrl)
            {
                con.client = new FireSharp.FirebaseClient(con.config);
                con.response = con.client.Get("newsProje/");
                var data = con.response.Body;
                List<haber> rst = JsonConvert.DeserializeObject<List<haber>>(data);
                var id = Convert.ToString(Convert.ToInt16(rst.LastOrDefault().id) + 1);
                haber h = new haber
                {
                    id = id,
                    baslik = mynetHaber,
                    imgLink = img,
                    link = url,
                    site = "Webtekno"
                };
                con.client.Update("newsProje/" + id, h);
                mynetHaberctrl = mynetHaber;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                webtekno();
               
            }
            finally
            {
                System.Threading.Thread.Sleep(2000);
            }
            try
            {
                mynet();
            }
            finally
            {
                System.Threading.Thread.Sleep(2000);
            }
            try
            {
                milliyet();
            }
            finally
            {
                System.Threading.Thread.Sleep(2000);
            }
            try
            {
                trt();
            }
            finally
            {
                System.Threading.Thread.Sleep(2000);
            }
            label1.Text = Convert.ToString(Convert.ToInt32(label1.Text) + 1);
        }

       
        private void milliyet()
        {
            WebRequest SiteyeBaglantiTalebi = HttpWebRequest.Create("https://www.milliyet.com.tr/rss/rssnew/sondakikarss.xml");
            WebResponse GelenCevap = SiteyeBaglantiTalebi.GetResponse();
            StreamReader CevapOku = new StreamReader(GelenCevap.GetResponseStream());
            string KaynakKodlar = CevapOku.ReadToEnd();
            milliyetHaber = KaynakKodlar.Substring(KaynakKodlar.IndexOf("![CDATA[")+8,KaynakKodlar.IndexOf("]]>")-KaynakKodlar.IndexOf("![CDATA[")-8);
            string link = KaynakKodlar.Substring(KaynakKodlar.IndexOf("<atom:link"),150);
            link = link.Substring(17,link.IndexOf("/>")-18);
            string img = KaynakKodlar.Substring(KaynakKodlar.IndexOf("<img src=")+10,KaynakKodlar.IndexOf("/><p>")- KaynakKodlar.IndexOf("<img src=")-11);
            if (milliyetHabercrtl!=milliyetHaber)
            {
                con.client = new FireSharp.FirebaseClient(con.config);
                con.response = con.client.Get("newsProje/");
                var data = con.response.Body;
                List<haber> rst = JsonConvert.DeserializeObject<List<haber>>(data);
                var id = Convert.ToString(Convert.ToInt16(rst.LastOrDefault().id) + 1);
                haber h = new haber
                {
                    id = id,
                    baslik = milliyetHaber,
                    imgLink = img,
                    link = link,
                    site = "Milliyet"
                };
                con.client.Update("newsProje/" + id, h);
                milliyetHabercrtl = milliyetHaber;

            }
        }
        private void trt()
        {
            WebRequest SiteyeBaglantiTalebi = HttpWebRequest.Create("http://www.trt.net.tr/rss/gundem.rss");
            WebResponse GelenCevap = SiteyeBaglantiTalebi.GetResponse();
            StreamReader CevapOku = new StreamReader(GelenCevap.GetResponseStream());
            string KaynakKodlar = CevapOku.ReadToEnd();
            KaynakKodlar = KaynakKodlar.Substring(KaynakKodlar.IndexOf("<item>"));
            trtHaber = KaynakKodlar.Substring(13,KaynakKodlar.IndexOf("</title>")-13);
            string url = KaynakKodlar.Substring(KaynakKodlar.IndexOf("<link>")+6,KaynakKodlar.IndexOf("</link>")- KaynakKodlar.IndexOf("<link>")-6);
            string img = KaynakKodlar.Substring(KaynakKodlar.IndexOf("<enclosure"));
            img = img.Substring(img.IndexOf("url")+5,img.IndexOf("/>")- img.IndexOf("url")-7);
            if (trtHaber!=trtHaberctrl)
            {
                con.client = new FireSharp.FirebaseClient(con.config);
                con.response = con.client.Get("newsProje/");
                var data = con.response.Body;
                List<haber> rst = JsonConvert.DeserializeObject<List<haber>>(data);
                var id = Convert.ToString(Convert.ToInt16(rst.LastOrDefault().id) + 1);
                haber h = new haber
                {
                    id = id,
                    baslik = trtHaber,
                    imgLink = img,
                    link = url,
                    site = "Trt"
                };
                con.client.Update("newsProje/" + id, h);
                trtHaberctrl = trtHaber;
            }
        }
        private class haber
        {
            public string id { get; set; }
            public string baslik { get; set; }
            public string link { get; set; }
            public string imgLink { get; set; }
            public string site { get; set; }
        }
    }
}
