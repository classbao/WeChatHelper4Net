using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2019-01-26
 */
namespace WeChatHelper4Net.Models.Multimedia
{
    /// <summary>
    /// 获取视频(临时)
    /// </summary>
    [Serializable]
    [DataContract]
    public class TempVideo
    {
        /*
        {
         "video_url":DOWN_URL
        }         
        */

        /// <summary>
        /// DOWN_URL
        /// </summary>
        [DataMember(IsRequired = true)]
        public string video_url { get; set; }

    }
}
