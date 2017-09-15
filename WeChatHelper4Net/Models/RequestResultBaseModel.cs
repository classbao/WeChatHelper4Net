using System;
using System.Runtime.Serialization;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models
{
    [Serializable]
    [DataContract]
    public class RequestResult_errmsg
    {
        [DataMember(IsRequired = false)]
        public int errcode { get; set; }

        [DataMember(IsRequired = false)]
        public string errmsg { get; set; }
    }


    [Serializable]
    [DataContract]
    public class RequestResultBaseModel : RequestResult_errmsg
    {
        [DataMember(IsRequired = false)]
        public long msg_id { get; set; }
        [DataMember(IsRequired = false)]
        public long msgid { get; set; }

        [DataMember(IsRequired = false)]
        public string url { get; set; }

        [DataMember(IsRequired = false)]
        public object date { get; set; }

    }
}