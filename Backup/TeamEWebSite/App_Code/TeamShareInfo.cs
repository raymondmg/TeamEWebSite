using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamEWebSite.App_Code
{
    /// <summary>
    /// 团队信息分享
    /// </summary>
    public class TeamShareInfo
    {
        private string imgSrc;

        public string ImgSrc
        {
            get { return imgSrc; }
            set { imgSrc = value; }
        }
        private string articleTitle;

        public string ArticleTitle
        {
            get { return articleTitle; }
            set { articleTitle = value; }
        }
        private string articleContent;

        public string ArticleContent
        {
            get { return articleContent; }
            set { articleContent = value; }
        }
        private string publishDate;

        public string PublishDate
        {
            get { return publishDate; }
            set { publishDate = value; }
        }

        /// <summary>
        /// 含参构造
        /// </summary>
        /// <param name="img"></param>
        /// <param name="title"></param>
        /// <param name="info"></param>
        /// <param name="date"></param>
        public TeamShareInfo(string img,string title,string info,string date)
        {
            ImgSrc = img;
            ArticleTitle = title;
            ArticleContent = info;
            PublishDate = date;
        }
        
    }


    /// <summary>
    /// 团队成员信息
    /// </summary>
    public class TeamMemberInfo
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string memberTitle;

        public string MemberTitle
        {
            get { return memberTitle; }
            set { memberTitle = value; }
        }
        private string sayContent;

        public string SayContent
        {
            get { return sayContent; }
            set { sayContent = value; }
        }
        private string imgSrc;

        public string ImgSrc
        {
            get { return imgSrc; }
            set { imgSrc = value; }
        }

        public TeamMemberInfo(string n,string imgS,string title,string content)
        {
            Name = n;
            ImgSrc = imgS;
            MemberTitle = title;
            SayContent = content;
        }
    }
}