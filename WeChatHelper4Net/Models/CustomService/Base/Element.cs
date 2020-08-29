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
        /// <summary>
        /// 文本内容
        /// </summary>
        [DataMember(IsRequired = true)]
        public string content { get; set; }
    }

    [Serializable]
    [DataContract]
    public class MediaId
    {
        /// <summary>
        /// 媒体ID
        /// </summary>
        [DataMember(IsRequired = true)]
        public string media_id { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Title
    {
        /// <summary>
        /// 标题
        /// </summary>
        [DataMember(IsRequired = true)]
        public string title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [DataMember(IsRequired = true)]
        public string description { get; set; }
    }

    [Serializable]
    [DataContract]
    public class CardId
    {
        /// <summary>
        /// 卡ID
        /// </summary>
        [DataMember(IsRequired = true)]
        public string card_id { get; set; }
    }



    [Serializable]
    [DataContract]
    public class kfAccount
    {
        /// <summary>
        /// 客服账号
        /// </summary>
        [DataMember(IsRequired = true)]
        public string kf_account { get; set; }
    }
}
