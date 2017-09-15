using System;
using System.Runtime.Serialization;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2016-12-12
 */
namespace WeChatHelper4Net.Models.CustomService.Base
{
    [Serializable]
    [DataContract]
    public class Text
    {
        [DataMember(IsRequired = true)]
        public string content { get; set; }
    }

    [Serializable]
    [DataContract]
    public class MediaId
    {
        [DataMember(IsRequired = true)]
        public string media_id { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Title
    {
        [DataMember(IsRequired = true)]
        public string title { get; set; }

        [DataMember(IsRequired = true)]
        public string description { get; set; }
    }

    [Serializable]
    [DataContract]
    public class CardId
    {
        [DataMember(IsRequired = true)]
        public string card_id { get; set; }
    }



    [Serializable]
    [DataContract]
    public class kfAccount
    {
        [DataMember(IsRequired = true)]
        public string kf_account { get; set; }
    }
}
