using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleWebApp.Models
{

    /// <summary>
    /// 自定义的微信分享参数模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeChatCustomShareData<T> where T : class
    {
        /// <summary>
        /// 微信分享类容
        /// </summary>
        public WeChatHelper4Net.Models.JSSDK.WeiXinShareData ShareData { get; set; }

        /// <summary>
        /// 提供分享的站内用户信息
        /// </summary>
        public T CurrentUser { get; set; }
        /// <summary>
        /// 转发分享站内媒体ID
        /// </summary>
        public string metaid { get; set; }
        /// <summary>
        /// 转发分享站内媒体类型
        /// </summary>
        public int metatype { get; set; }

        public WeChatCustomShareData(T currentUser, string metaid, int metatype, string url, string title, string desc, string link = "", string imgUrl = "", string type = "", string dataUrl = "")
        {
            this.CurrentUser = currentUser;
            this.metaid = metaid;
            this.metatype = metatype;
            ShareData = new WeChatHelper4Net.Models.JSSDK.WeiXinShareData()
            {
                title = title,
                desc = desc,
                link = !string.IsNullOrWhiteSpace(link) ? link : url,
                imgUrl = imgUrl,
                type = type,
                dataUrl = dataUrl
            };
            ShareData.link = HttpUtility.UrlPathEncode(this.ShareData.link);
        }
    }
}