using System;
using WeChatHelper4Net.Extend;

/*
 * 微信开放平台API-网站应用
 * 作者：熊学浩
 * 时间：2015-6-29
 */
namespace WeChatHelper4Net.OpenWeb
{
	/// <summary>
	/// 微信开放平台网站应用开发者凭据，隐私设置
	/// </summary>
	internal sealed class Privacy
	{
        #region 微信开放平台网站应用开发者凭据
        /// <summary>
        /// 开发者ID：AppID(应用ID，微信开放平台应用身份的唯一标识)
        /// </summary>
        public static readonly string AppId = ConfigHelper.GetAppSetting("WeiXinOpenWebAppId");
		/// <summary>
		/// 开发者ID：AppSecret(应用密钥)
		/// </summary>
		public static readonly string AppSecret = ConfigHelper.GetAppSetting("WeiXinOpenWebAppSecret");

        #endregion

    }
}
