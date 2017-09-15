using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Security;
using WeChatHelper4Net.Models;

/*
 * 微信开放平台网站应用API
 * 作者：熊学浩
 * 时间：2015-6-29
 */
namespace WeChatHelper4Net.OpenWeb
{
	/// <summary>
	/// 常用公共方法与属性
	/// </summary>
	public class Common
	{
		#region 微信开放平台网站应用相关信息
		/// <summary>
		/// 微信开放平台网站应用-开发者ID：AppID
		/// </summary>
		public static string AppId { get { return Privacy.AppId; } }
		#endregion

	}
}
