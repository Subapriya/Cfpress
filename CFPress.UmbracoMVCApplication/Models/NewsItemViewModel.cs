using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CFPress.UmbracoMVCApplication.Models
{
    public class NewsItemViewModel
    {
        public string sectionName { get; set; }
        public string header { get; set; }
        public string teaser { get; set; }
        public string story { get; set; }
        public DateTime createdDate { get; set; }
        public string pictureFilePath { get; set; }
        public string pictureCaption { get; set; }
        public string newsPaperWebsiteName { get; set; }


        //// for now add the method here and put it under umbraco application event start
        public static List<NewsItemViewModel> ReadXmlIntoClass()
        {
            NewsItemViewModel newsItemModel = new NewsItemViewModel();

            string xmlFilePath = "C:\\Users\\snageswaran\\Desktop\\Important\\testxml.xml";
            List<NewsItemViewModel> lstContent = LoadContent(xmlFilePath);

            return lstContent;
        }

        public static List<NewsItemViewModel> LoadContent(string xmlFilePath)
        {
            List<NewsItemViewModel> lstContent = new List<NewsItemViewModel>();
            var com = from p in XElement.Load(xmlFilePath).Elements("content")
                      select new NewsItemViewModel
                      {
                          sectionName = (string)p.Element("section_name"),
                          header = (string)p.Element("header"),
                          teaser = (string)p.Element("teaser"),
                          story = (string)p.Element("story"),
                          createdDate = (DateTime)p.Element("createddate"),
                          pictureCaption = (string) p.Element("picturecaption"),
                          pictureFilePath = (string) p.Element("picture"),
                          newsPaperWebsiteName = (string)p.Element("newspaper_name")
                      };

            foreach (var c in com)
            {

                lstContent.Add(c);
            }
            return lstContent;
        }
    }
}