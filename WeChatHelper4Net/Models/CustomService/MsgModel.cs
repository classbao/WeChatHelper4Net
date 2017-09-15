using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2016-12-12
 */
namespace WeChatHelper4Net.Models.CustomService
{
    [Serializable]
    [DataContract]
    public class TextMsg : Base.Base
    {
        public TextMsg() { msgtype = "text"; }

        [DataMember(IsRequired = true)]
        public Base.Text text { get; set; }
    }


    [Serializable]
    [DataContract]
    public class ImageMsg : Base.Base
    {
        public ImageMsg() { msgtype = "image"; }

        [DataMember(IsRequired = true)]
        public Base.MediaId image { get; set; }
    }

    [Serializable]
    [DataContract]
    public class VoiceMsg : Base.Base
    {
        public VoiceMsg() { msgtype = "voice"; }

        [DataMember(IsRequired = true)]
        public Base.MediaId voice { get; set; }
    }


    [Serializable]
    [DataContract]
    public class Video : Base.Title
    {
        [DataMember(IsRequired = true)]
        public string media_id { get; set; }

        [DataMember(IsRequired = true)]
        public string thumb_media_id { get; set; }
    }

    [Serializable]
    [DataContract]
    public class VideoMsg : Base.Base
    {
        public VideoMsg() { msgtype = "video"; }

        [DataMember(IsRequired = true)]
        public Video video { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Music : Base.Title
    {
        [DataMember(IsRequired = true)]
        public string musicurl { get; set; }

        [DataMember(IsRequired = true)]
        public string hqmusicurl { get; set; }

        [DataMember(IsRequired = true)]
        public string thumb_media_id { get; set; }
    }


    [Serializable]
    [DataContract]
    public class MusicMsg : Base.Base
    {
        public MusicMsg() { msgtype = "music"; }

        [DataMember(IsRequired = true)]
        public Music music { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Article : Base.Title
    {
        [DataMember(IsRequired = true)]
        public string url { get; set; }

        [DataMember(IsRequired = true)]
        public string picurl { get; set; }
    }
    
    [Serializable]
    [DataContract]
    public class News
    {
        [DataMember(IsRequired = true)]
        public List<Article> articles { get; set; }
    }

    [Serializable]
    [DataContract]
    public class NewsMsg : Base.Base
    {
        public NewsMsg() { msgtype = "news"; }

        [DataMember(IsRequired = true)]
        public News news { get; set; }
    }

    [Serializable]
    [DataContract]
    public class mpNewsMsg : Base.Base
    {
        public mpNewsMsg() { msgtype = "mpnews"; }

        [DataMember(IsRequired = true)]
        public Base.MediaId mpnews { get; set; }
    }
    
    [Serializable]
    [DataContract]
    public class wxCardMsg : Base.Base
    {
        public wxCardMsg() { msgtype = "wxcard"; }

        [DataMember(IsRequired = true)]
        public Base.CardId wxcard { get; set; }
    }
    
}
