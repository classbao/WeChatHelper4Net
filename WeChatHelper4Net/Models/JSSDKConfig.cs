using System;
using System.Runtime.Serialization;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models.JSSDK
{
    /// <summary>
    /// JS-SDK的页面配置信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class JSSDKConfig
    {
        [DataMember(IsRequired = false)]
        public string appId { get { return Privacy.AppId; } }

        [DataMember(IsRequired = false)]
        public int timestamp { get; set; }

        [DataMember(IsRequired = false)]
        public string nonceStr { get; set; }

        [DataMember(IsRequired = false)]
        public string signature { get; set; }
    }

    /// <summary>
    /// 微信分享参数配置
    /// </summary>
    [Serializable]
    [DataContract]
    public class WeiXinShareData
    {
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string desc { get; set; }
        [DataMember]
        public string link { get; set; }
        [DataMember]
        public string imgUrl { get; set; }

        /// <summary>
        /// 分享类型,music、video或link，不填默认为link
        /// </summary>
        [DataMember]
        public string type { get; set; }
        /// <summary>
        /// 如果type是music或video，则要提供数据链接，默认为空
        /// </summary>
        [DataMember]
        public string dataUrl { get; set; }
    }

    /// <summary>
    /// 使用微信内置地图查看位置接口
    /// </summary>
    [Serializable]
    [DataContract]
    public class LocationData
    {
        [DataMember]
        public decimal latitude { get; set; }

        [DataMember]
        public decimal longitude { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string address { get; set; }

        /// <summary>
        /// 地图缩放级别,整形值,范围从1~28。默认为最大
        /// </summary>
        [DataMember]
        public int scale { get; set; }

        /// <summary>
        /// 在查看位置界面底部显示的超链接,可点击跳转
        /// </summary>
        [DataMember]
        public string infoUrl { get; set; }
    }

}