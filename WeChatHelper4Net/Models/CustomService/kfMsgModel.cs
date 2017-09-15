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
    public class kfTextMsg : TextMsg
    {
        [DataMember(IsRequired = true)]
        public Base.kfAccount customservice { get; set; }
    }

    [Serializable]
    [DataContract]
    public class kfImageMsg : ImageMsg
    {
        [DataMember(IsRequired = true)]
        public Base.kfAccount customservice { get; set; }
    }

    [Serializable]
    [DataContract]
    public class kfVoiceMsg : VoiceMsg
    {
        [DataMember(IsRequired = true)]
        public Base.kfAccount customservice { get; set; }
    }

    [Serializable]
    [DataContract]
    public class kfVideoMsg : VideoMsg
    {
        [DataMember(IsRequired = true)]
        public Base.kfAccount customservice { get; set; }
    }

    [Serializable]
    [DataContract]
    public class kfMusicMsg : MusicMsg
    {
        [DataMember(IsRequired = true)]
        public Base.kfAccount customservice { get; set; }
    }

    [Serializable]
    [DataContract]
    public class kfNewsMsg : NewsMsg
    {
        [DataMember(IsRequired = true)]
        public Base.kfAccount customservice { get; set; }
    }

    [Serializable]
    [DataContract]
    public class kfmpNewsMsg : mpNewsMsg
    {
        [DataMember(IsRequired = true)]
        public Base.kfAccount customservice { get; set; }
    }

    [Serializable]
    [DataContract]
    public class kfwxCardMsg : wxCardMsg
    {
        [DataMember(IsRequired = true)]
        public Base.kfAccount customservice { get; set; }
    }

}
