using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2018-09-09
 */
namespace WeChatHelper4Net.Models.Multimedia
{
    /// <summary>
    /// 获取视频(永久)
    /// </summary>
    [Serializable]
    [DataContract]
    public class ForeverVideo
    {
        /*
         {"title":"第一期-关闭分页预览","description":"","down_url":"http:\/\/mp.weixin.qq.com\/mp\/mp\/video?__biz=MzIxMzc4MTMzMA==&mid=100001728&sn=b9e6258f2abb37028af88bad81976879&vid=o1345teloaj&idx=1&vidsn=5efab83eb152c43d3cfff638d39cc909&fromid=1#rd"}
         */

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember(IsRequired = true)]
        public string title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [DataMember(IsRequired = false)]
        public string description { get; set; }
        /// <summary>
        /// 下载地址
        /// </summary>
        [DataMember(IsRequired = true)]
        public string down_url { get; set; }

    }
}
