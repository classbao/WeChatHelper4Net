using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2016-12-08
 */
namespace WeChatHelper4Net.Models.PassiveReply
{
    [Serializable]
    //[DataContract(Name = "xml")]
    //[XmlRoot("xml")]
    public class PassiveReplyModel
    {
        public PassiveReplyModel() { FromUserName = Common.WeChatId; }

        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(ToUserName))]
        public string ToUserName { get; set; }

        /// <summary>
        /// 构造函数内部将会初始化赋值
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(FromUserName))]
        public string FromUserName { get; set; }

        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(CreateTime))]
        public long CreateTime { get; set; }

        /// <summary>
        /// 构造函数内部将会初始化赋值
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(MsgType))]
        public string MsgType { get; set; }
    }

    [Serializable]
    //[DataContract(Name = "MediaId")]
    //[XmlRoot("MediaId")]
    public class MediaIdMember
    {
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(MediaId))]
        public string MediaId { get; set; }
    }

    [Serializable]
    //[DataContract(Name = "Title")]
    //[XmlRoot("Title")]
    public class TitleMember
    {
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Title))]
        public string Title { get; set; }

        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Description))]
        public string Description { get; set; }
    }



    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReplyTextModel : PassiveReplyModel, IXmlSerializable
    {
        public ReplyTextModel() { MsgType = "text"; }

        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Content))]
        public string Content { get; set; }

        #region CDATA
        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(ReplyTextModel));
            ToUserName = reader.ReadElementContentAsString(nameof(ToUserName), null);
            FromUserName = reader.ReadElementContentAsString(nameof(FromUserName), null);
            CreateTime = reader.ReadElementContentAsInt(nameof(CreateTime), null);
            MsgType = reader.ReadElementContentAsString(nameof(MsgType), null);
            /*** ***/

            Content = reader.ReadElementContentAsString(nameof(Content), null);
            reader.ReadEndElement();
        }
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(ToUserName));
            writer.WriteCData(ToUserName);
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(FromUserName));
            writer.WriteCData(FromUserName);
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(CreateTime));
            writer.WriteValue(CreateTime);
            writer.WriteEndElement();

            writer.WriteStartElement(nameof(MsgType));
            writer.WriteCData(MsgType);
            writer.WriteEndElement();
            /*** ***/

            writer.WriteStartElement(nameof(Content));
            writer.WriteValue(Content);
            writer.WriteEndElement();
        }
        #endregion
    }


    [Serializable]
    //[DataContract(Name = "Image")]
    //[XmlRoot("Image")]
    public class Image : MediaIdMember
    {
    }

    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReplyImageModel : PassiveReplyModel
    {
        public ReplyImageModel() { MsgType = "image"; }

        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Image))]
        public Image Image { get; set; }
    }

    [Serializable]
    //[DataContract(Name = "Voice")]
    //[XmlRoot("Voice")]
    public class Voice : MediaIdMember
    {
    }

    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReplyVoiceModel : PassiveReplyModel
    {
        public ReplyVoiceModel() { MsgType = "voice"; }

        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Voice))]
        public Voice Voice { get; set; }
    }


    [Serializable]
    //[DataContract(Name = "Video")]
    //[XmlRoot("Video")]
    public class Video : TitleMember
    {
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(MediaId))]
        public string MediaId { get; set; }
    }

    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReplyVideoModel : PassiveReplyModel
    {
        public ReplyVideoModel() { MsgType = "video"; }

        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Video))]
        public Video Video { get; set; }
    }


    [Serializable]
    //[DataContract(Name = "Music")]
    //[XmlRoot("Music")]
    public class Music : TitleMember
    {
        [DataMember(IsRequired = false)]
        [XmlElement(ElementName = nameof(MusicUrl))]
        public string MusicUrl { get; set; }

        [DataMember(IsRequired = false)]
        [XmlElement(ElementName = nameof(HQMusicUrl))]
        public string HQMusicUrl { get; set; }

        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(ThumbMediaId))]
        public string ThumbMediaId { get; set; }
    }

    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReplyMusicModel : PassiveReplyModel
    {
        public ReplyMusicModel() { MsgType = "music"; }

        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Music))]
        public Music Music { get; set; }
    }


    [Serializable]
    //[DataContract(Name = "item")]
    //[XmlRoot("item")]
    public class ArticleItem : TitleMember
    {
        [DataMember(IsRequired = false)]
        [XmlElement(ElementName = nameof(PicUrl))]
        public string PicUrl { get; set; }

        [DataMember(IsRequired = false)]
        [XmlElement(ElementName = nameof(Url))]
        public string Url { get; set; }
    }

    [Serializable]
    //[DataContract(Name = "Articles")]
    //[XmlRoot("Articles")]
    public class Articles
    {
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = "item")]
        public List<ArticleItem> list { get; set; }
    }

    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReplyNewsModel : PassiveReplyModel
    {
        public ReplyNewsModel() { MsgType = "news"; }

        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(ArticleCount))]
        public int ArticleCount { get { return (null != Articles && null != Articles.list) ? Articles.list.Count : 0; } set { } }

        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Articles))]
        public Articles Articles { get; set; }
    }


    /*
    WeChatHelper4Net.Models.PassiveReply.ReplyMusicModel music = new WeChatHelper4Net.Models.PassiveReply.ReplyMusicModel()
    {
        ToUserName = "11111",
        CreateTime = WeChatHelper4Net.Common.ConvertTime(DateTime.Now),
        Music = new WeChatHelper4Net.Models.PassiveReply.Music()
        {
            Title = "三个Ian抗旱额可能",
            Description = "安设万天目湖朋克头我今后人生里面看到",
            MusicUrl = "http://wx.newsasker.com/xxx/getprojects?FLID=20",
            HQMusicUrl = "http://wx.newsasker.com/xxx/getprojects?FLID=20",
            ThumbMediaId = "sasd15135153"
        }
    };
    WeChatHelper4Net.PassiveReply.Music(music, DateTime.Now);

    WeChatHelper4Net.Models.PassiveReply.ReplyNewsModel news = new WeChatHelper4Net.Models.PassiveReply.ReplyNewsModel()
    {
        ToUserName = "11111",
        CreateTime = WeChatHelper4Net.Common.ConvertTime(DateTime.Now),
        Articles = new WeChatHelper4Net.Models.PassiveReply.Articles()
        {
            list = new List<WeChatHelper4Net.Models.PassiveReply.ArticleItem>()
                {
                    new WeChatHelper4Net.Models.PassiveReply.ArticleItem() { Title="标题AAA", Description="描述AAA", PicUrl="http://wx.newsasker.com/xxx/getprojects?FLID=20", Url="http://wx.newsasker.com/xxx/getprojects?FLID=20"  },
                    new WeChatHelper4Net.Models.PassiveReply.ArticleItem() { Title="标题BBB", Description="描述BBB", PicUrl="http://wx.newsasker.com/xxx/getprojects?FLID=20", Url="http://wx.newsasker.com/xxx/getprojects?FLID=20"  }
                }
        }
    };
    WeChatHelper4Net.PassiveReply.News(news, DateTime.Now);
    */


}
