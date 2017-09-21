using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2016-12-12
 */
namespace WeChatHelper4Net.Models.Menu.Base
{
    [Serializable]
    [DataContract]
    public class Click : Base
    {
        public Click() { type = "click"; }

        /// <summary>
        /// click等点击类型必须	菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        [DataMember(IsRequired = true)]
        public string key { get; set; }
    }

    [Serializable]
    [DataContract]
    public class View : Base
    {
        public View() { type = "view"; }

        /// <summary>
        /// view类型必须	网页链接，用户点击菜单可打开链接，不超过1024字节
        /// </summary>
        [DataMember(IsRequired = true)]
        public string url { get; set; }
    }


    [Serializable]
    [DataContract]
    public class MiniProgram : Base
    {
        public MiniProgram() { type = "miniprogram"; }

        /// <summary>
        /// view、miniprogram类型必须	网页链接，用户点击菜单可打开链接，不超过1024字节。type为miniprogram时，不支持小程序的老版本客户端将打开本url。
        /// </summary>
        [DataMember(IsRequired = true)]
        public string url { get; set; }
        /// <summary>
        /// miniprogram类型必须	小程序的appid（仅认证公众号可配置）
        /// </summary>
        [DataMember(IsRequired = true)]
        public string appid { get; set; }
        /// <summary>
        /// miniprogram类型必须	小程序的页面路径
        /// </summary>
        [DataMember(IsRequired = true)]
        public string pagepath { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Scancode_waitmsg : Base
    {
        public Scancode_waitmsg() { type = "scancode_waitmsg"; }

        [DataMember(IsRequired = false)]
        public string key { get; set; }


        /// <summary>
        /// 二级菜单数组，个数应为1~5个
        /// </summary>
        [DataMember(IsRequired = false)]
        public List<string> sub_button { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Scancode_push : Base
    {
        public Scancode_push() { type = "scancode_push"; }

        [DataMember(IsRequired = false)]
        public string key { get; set; }

        /// <summary>
        /// 二级菜单数组，个数应为1~5个
        /// </summary>
        [DataMember(IsRequired = false)]
        public List<string> sub_button { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Pic_sysphoto : Base
    {
        public Pic_sysphoto() { type = "pic_sysphoto"; }

        [DataMember(IsRequired = false)]
        public string key { get; set; }

        /// <summary>
        /// 二级菜单数组，个数应为1~5个
        /// </summary>
        [DataMember(IsRequired = false)]
        public List<string> sub_button { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Pic_photo_or_album : Base
    {
        public Pic_photo_or_album() { type = "pic_photo_or_album"; }

        [DataMember(IsRequired = false)]
        public string key { get; set; }

        /// <summary>
        /// 二级菜单数组，个数应为1~5个
        /// </summary>
        [DataMember(IsRequired = false)]
        public List<string> sub_button { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Pic_weixin : Base
    {
        public Pic_weixin() { type = "pic_weixin"; }

        [DataMember(IsRequired = false)]
        public string key { get; set; }

        /// <summary>
        /// 二级菜单数组，个数应为1~5个
        /// </summary>
        [DataMember(IsRequired = false)]
        public List<string> sub_button { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Location_select : Base
    {
        public Location_select() { type = "location_select"; }

        [DataMember(IsRequired = false)]
        public string key { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Media_id : Base
    {
        public Media_id() { type = "media_id"; }

        /// <summary>
        /// media_id类型和view_limited类型必须	调用新增永久素材接口返回的合法media_id
        /// </summary>
        [DataMember(IsRequired = false)]
        public string media_id { get; set; }
    }

    [Serializable]
    [DataContract]
    public class View_limited : Base
    {
        public View_limited() { type = "view_limited"; }

        /// <summary>
        /// media_id类型和view_limited类型必须	调用新增永久素材接口返回的合法media_id
        /// </summary>
        [DataMember(IsRequired = false)]
        public string media_id { get; set; }
    }

}
