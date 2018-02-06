using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2016-12-16
 */
namespace WeChatHelper4Net.Models.Menu.Base
{
    /// <summary>
    /// 菜单基础抽象
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class BaseButton : IBaseButton
    {
        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过60个字节
        /// </summary>
        [DataMember(IsRequired = true)]
        public string name { get; set; }

        private BaseButton() { }
        public BaseButton(string name) { this.name = name; }
        public string ToJson()
        {
            switch(this.GetType().Name)
            {
                case "Scancode_waitmsg":
                    return ((Scancode_waitmsg)this).ToJson();
                case "Scancode_push":
                    return ((Scancode_push)this).ToJson();
                case "Pic_sysphoto":
                    return ((Pic_sysphoto)this).ToJson();
                case "Pic_photo_or_album":
                    return ((Pic_photo_or_album)this).ToJson();
                case "Pic_weixin":
                    return ((Pic_weixin)this).ToJson();
                default:
                    return MenuJson.Serialize(this);
            }
        }
    }

    /// <summary>
    /// 单个菜单
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class SingleButton : BaseButton, ISingleButton
    {
        /// <summary>
        /// 构造函数内部将会初始化赋值
        /// </summary>
        [DataMember(IsRequired = true)]
        public string type { get; set; }

        //private SingleButton() : base() { }
        //public SingleButton(string name) : base(name) { }
        public SingleButton(string type, string name) : base(name) { this.type = type; }
    }

    /// <summary>
    /// 二级菜单
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class absSubButton : BaseButton, ISubButton
    {
        /// <summary>
        /// 二级菜单组合
        /// </summary>
        public IList<SingleButton> sub_button { get; set; }

        //private absSubButton() : base() { }
        //public absSubButton(string name) : base(name) { }
        public absSubButton(string name, IList<SingleButton> sub_button) : base(name) { this.sub_button = sub_button; }

        public new string ToJson()
        {
            StringBuilder sb = new StringBuilder("{\"name\":\"" + name + "\",\"sub_button\":[");
            for(int i = 0; i < sub_button.Count; i++)
            {
                sb.Append(sub_button[i].ToJson());
                if(i < sub_button.Count - 1)
                    sb.Append(",");
            }
            sb.Append("]}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// 二级菜单的子菜单
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class absSubSubButton : SingleButton, ISubButton, ISingleButton
    {
        /// <summary>
        /// click等点击类型必须	菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        [DataMember(IsRequired = true)]
        public string key { get; set; }
        /// <summary>
        /// 二级菜单的子菜单组合
        /// </summary>
        public IList<SingleButton> sub_button { get; set; }

        //private absSubSubButton() : base() { }
        //public absSubSubButton(string name) : base(name) { }
        public absSubSubButton(string type, string name, string key, IList<SingleButton> sub_button) : base(type, name) { this.key = key; this.sub_button = sub_button; }

        public new string ToJson()
        {
            StringBuilder sb = new StringBuilder(string.Concat("{\"type\":\"", type, "\",\"name\":\"", name, "\",\"key\":\"", key, "\",\"sub_button\":["));
            for(int i = 0; i < sub_button.Count; i++)
            {
                sb.Append(sub_button[i].ToJson());
                if(i < sub_button.Count - 1)
                    sb.Append(",");
            }
            sb.Append("]}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// 自定义菜单(根级)
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class absButton
    {
        /// <summary>
        /// 一级菜单组合
        /// </summary>
        public IList<BaseButton> button { get; set; }

        public absButton() { }
        public absButton(IList<BaseButton> button) { this.button = button; }

        public string ToJson()
        {
            StringBuilder sb = new StringBuilder("{\"button\":[");
            for(int i = 0; i < button.Count; i++)
            {
                if(button[i].GetType().Name == "SubButton")
                {
                    sb.Append(((SubButton)button[i]).ToJson());
                }
                else
                {
                    sb.Append(button[i].ToJson());
                }

                if(i < button.Count - 1)
                    sb.Append(",");
            }
            sb.Append("]}");
            return sb.ToString();
        }
    }

    /// <summary>
    /// 自定义菜单转换成Json字符串
    /// </summary>
    public class MenuJson
    {
        /// <summary>
        /// 将对象序列化成json字符串
        /// </summary>
        /// <param name="o">对象实例</param>
        /// <returns>json字符串</returns>
        public static string Serialize(object o)
        {
            if(o == null || o == System.DBNull.Value) return string.Empty;
            string result = string.Empty;
            using(MemoryStream memoryStream = new MemoryStream())
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(o.GetType());
                json.WriteObject(memoryStream, o);
                result = Encoding.UTF8.GetString(memoryStream.ToArray());

                memoryStream.Close();
            }
            return result;
        }
    }

}
