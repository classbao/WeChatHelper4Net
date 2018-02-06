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
    public class Click : SingleButton, ISingleButton
    {
        private static readonly string _type = "click";
        public Click() : base(_type, "") { }
        public Click(string name, string key) : base(_type, name) { this.key = key; }

        /// <summary>
        /// click等点击类型必须	菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        [DataMember(IsRequired = true)]
        public string key { get; set; }
    }

    [Serializable]
    [DataContract]
    public class View : SingleButton, ISingleButton
    {
        private static readonly string _type = "view";
        public View() : base(_type, "") { }
        public View(string name, string url) : base(_type, name) { this.url = url; }

        /// <summary>
        /// view类型必须	网页链接，用户点击菜单可打开链接，不超过1024字节
        /// </summary>
        [DataMember(IsRequired = true)]
        public string url { get; set; }
    }


    [Serializable]
    [DataContract]
    public class MiniProgram : SingleButton, ISingleButton
    {
        private static readonly string _type = "miniprogram";
        public MiniProgram() : base(_type, "") { }
        public MiniProgram(string name, string url, string appid, string pagepath) : base(_type, name) { this.url = url; this.appid = appid; this.pagepath = pagepath; }

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
    public class Scancode_waitmsg : absSubSubButton, ISubButton, ISingleButton
    {
        private static readonly string _type = "scancode_waitmsg";
        //public Scancode_waitmsg() : base(_type, "") { }
        public Scancode_waitmsg(string name, string key, IList<SingleButton> sub_button) : base(_type, name, key, sub_button) { }
    }

    [Serializable]
    [DataContract]
    public class Scancode_push : absSubSubButton, ISubButton, ISingleButton
    {
        private static readonly string _type = "scancode_push";
        //public Scancode_push() : base(_type, "") { }
        public Scancode_push(string name, string key, IList<SingleButton> sub_button) : base(_type, name, key, sub_button) { }
    }

    [Serializable]
    [DataContract]
    public class Pic_sysphoto : absSubSubButton, ISubButton, ISingleButton
    {
        private static readonly string _type = "pic_sysphoto";
        //public Pic_sysphoto() : base(_type, "") { }
        public Pic_sysphoto(string name, string key, IList<SingleButton> sub_button) : base(_type, name, key, sub_button) { }
    }

    [Serializable]
    [DataContract]
    public class Pic_photo_or_album : absSubSubButton, ISubButton, ISingleButton
    {
        private static readonly string _type = "pic_photo_or_album";
        //public Pic_photo_or_album() : base(_type, "") { }
        public Pic_photo_or_album(string name, string key, IList<SingleButton> sub_button) : base(_type, name, key, sub_button) { }
    }

    [Serializable]
    [DataContract]
    public class Pic_weixin : absSubSubButton, ISubButton, ISingleButton
    {
        private static readonly string _type = "pic_weixin";
        //public Pic_weixin() : base(_type, "") { }
        public Pic_weixin(string name, string key, IList<SingleButton> sub_button) : base(_type, name, key, sub_button) { }
    }

    [Serializable]
    [DataContract]
    public class Location_select : SingleButton, ISingleButton
    {
        private static readonly string _type = "location_select";
        public Location_select() : base(_type, "") { }
        public Location_select(string name, string key) : base(_type, name) { this.key = key; }

        /// <summary>
        /// click等点击类型必须	菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        [DataMember(IsRequired = false)]
        public string key { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Media_id : SingleButton, ISingleButton
    {
        private static readonly string _type = "media_id";
        public Media_id() : base(_type, "") { }
        public Media_id(string name, string media_id) : base(_type, name) { this.media_id = media_id; }

        /// <summary>
        /// media_id类型和view_limited类型必须	调用新增永久素材接口返回的合法media_id
        /// </summary>
        [DataMember(IsRequired = false)]
        public string media_id { get; set; }
    }

    [Serializable]
    [DataContract]
    public class View_limited : SingleButton, ISingleButton
    {
        private static readonly string _type = "view_limited";
        public View_limited() : base(_type, "") { }
        public View_limited(string name, string media_id) : base(_type, name) { this.media_id = media_id; }

        /// <summary>
        /// media_id类型和view_limited类型必须	调用新增永久素材接口返回的合法media_id
        /// </summary>
        [DataMember(IsRequired = false)]
        public string media_id { get; set; }
    }

    /// <summary>
    /// 二级菜单
    /// </summary>
    [Serializable]
    [DataContract]
    public class SubButton : absSubButton, ISubButton
    {
        //public SubButton() : base() { }
        //public SubButton(string name) : base(name) { }
        public SubButton(string name, IList<SingleButton> sub_button) : base(name, sub_button) { }
    }

    /// <summary>
    /// 一级菜单
    /// </summary>
    [Serializable]
    [DataContract]
    public class Button : absButton
    {
        public Button() : base() { }
        public Button(IList<Base> button) : base(button) { }
    }

}
