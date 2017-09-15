using System;
using System.Runtime.Serialization;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2016-12-16
 */
namespace WeChatHelper4Net.Models.Menu.Base
{
    [Serializable]
    [DataContract]
    public class Base
    {
        /// <summary>
        /// 构造函数内部将会初始化赋值
        /// </summary>
        [DataMember(IsRequired = true)]
        public string type { get; set; }

        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过60个字节
        /// </summary>
        [DataMember(IsRequired = true)]
        public string name { get; set; }
    }
}
