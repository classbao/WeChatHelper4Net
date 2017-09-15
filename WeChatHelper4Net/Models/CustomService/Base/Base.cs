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
    public class Base
    {
        /// <summary>
        /// 普通用户openid
        /// </summary>
        [DataMember(IsRequired = true)]
        public string touser { get; set; }

        /// <summary>
        /// 构造函数内部将会初始化赋值
        /// </summary>
        [DataMember(IsRequired = true)]
        public string msgtype { get; set; }
    }
}
