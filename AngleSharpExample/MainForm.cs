using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
//using System.Net

//Author: Nikolai Kekish
//Web: http://mydataprovider.com
//Email: sales@mydataprovider.com

namespace AngleSharpExample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            //http://www.amazon.com/s/ref=nb_sb_noss_2?url=search-alias%3Daps&field-keywords=iphone+6s
            try
            {
                textBoxOutput.Text = "";
                var url = "http://www.amazon.com/s/ref=nb_sb_noss_2?url=search-alias%3Daps&field-keywords=" + System.Web.HttpUtility.UrlEncode(textBoxInput.Text);
                using (var client = new System.Net.Http.HttpClient())
                {
                    var res = client.GetAsync(url).Result.Content.ReadAsStreamAsync().Result;
                    var doc = AngleSharp.DocumentBuilder.Html(res); //AngleSharp.DOM.Html.HTMLDocument.LoadFromSource(html);
                    var a = doc.QuerySelectorAll("//div[@class=\"s-item-container\"]//a");
                    if (a != null)
                    {
                        textBoxOutput.Text = a[0].Attributes["href"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                textBoxOutput.Text = ex.Message;
            }
        }
    }
}
