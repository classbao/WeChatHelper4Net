using System;
using System.Web;
using System.Text;
using WeChatHelper4Net.Models;

/*
 * 微信开放平台网站应用API
 * 作者：熊学浩
 * 时间：2015-6-29
 */
namespace WeChatHelper4Net.OpenWeb
{
	/// <summary>
	/// 开放平台网站应用授权
	/// </summary>
	public class OAuth
	{
		/// <summary>
		/// 引导用户进入授权页面同意授权，获取code
		/// 如果用户同意授权，页面将跳转至 redirect_uri/?code=CODE&amp;state=STATE。
		/// 若用户禁止授权，则重定向后不会带上code参数，仅会带上state参数redirect_uri?state=STATE
		/// （注意：重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值）
		/// </summary>
		/// <param name="redirect_uri">目标页面URL（不包含页面参数）</param>
		/// <param name="scope">应用授权作用域</param>
		/// <param name="state">重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值</param>
		/// <returns></returns>
		public static string OAuthToURL(string redirect_uri, scope scope, string state)
		{
			if (string.IsNullOrEmpty(redirect_uri))
			{
				throw new Exception("redirect_uri 为必填参数！");
			}
			if (scope == null)
			{
				scope = scope.snsapi_login;
			}
			if (string.IsNullOrEmpty(state))
			{
				state = string.Empty;
			}

			return string.Format("https://open.weixin.qq.com/connect/qrconnect?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect"
				, Privacy.AppId
				, HttpUtility.UrlEncode(redirect_uri)
				, scope.ToString()
				, state
				);
		}

		/// <summary>
		/// 通过code换取网页授权access_token。
		/// 请注意，这里通过code换取的网页授权access_token,与基础支持中的access_token不同。
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public static AccessTokenModel GetOAuth(string code)
		{
			if (string.IsNullOrEmpty(code))
			{
				throw new Exception("code 为必填参数！");
			}

			string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", Privacy.AppId, Privacy.AppSecret, code);
			try
			{
				string result = HttpRequestHelper.Request(url);
                PageOAuth_AccessToken model = JsonHelper.DeSerialize<PageOAuth_AccessToken>(result);
				if (model == null || string.IsNullOrEmpty(model.openid))
				{
					StringBuilder msg = new StringBuilder("获取授权Openid失败，url=" + url);
					msg.AppendLine(" ");
					msg.AppendLine("result=" + result);
					if (model == null)
						msg.Append("AccessTokenModel=" + model);
					else
						msg.AppendFormat("errcode={0},errmsg={1}", model.errcode, model.errmsg);
					//LogHelper.Save(msg.ToString(), "GetOAuth", LogType.Error, LogTime.day);
				}
				return model;
			}
			catch (Exception Ex)
			{
				LogHelper.Save(Ex);
				throw Ex;
			}
		}

		/// <summary>
		/// 通过refresh_token刷新或续期access_token。
		/// </summary>
		/// <param name="refresh_token"></param>
		/// <returns></returns>
		public static AccessTokenModel RefreshToken(string refresh_token)
		{
			if (string.IsNullOrEmpty(refresh_token))
			{
				throw new Exception("refresh_token 为必填参数！");
			}

			string url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", Privacy.AppId, refresh_token);
			try
			{
				string result = HttpRequestHelper.Request(url);
                PageOAuth_AccessToken model = JsonHelper.DeSerialize<PageOAuth_AccessToken>(result);
				if (model == null || string.IsNullOrEmpty(model.openid))
				{
					StringBuilder msg = new StringBuilder("刷新access_token有效期失败，url=" + url);
					msg.AppendLine(" ");
					msg.AppendLine("result=" + result);
					if (model == null)
						msg.Append("AccessTokenModel=" + model);
					else
						msg.AppendFormat("errcode={0},errmsg={1}", model.errcode, model.errmsg);
					//LogHelper.Save(msg.ToString(), "GetOAuth", LogType.Error, LogTime.day);
				}
				return model;
			}
			catch (Exception Ex)
			{
				LogHelper.Save(Ex);
				throw Ex;
			}
		}

		/// <summary>
		/// 检验授权凭证（access_token）是否有效
		/// </summary>
		/// <param name="access_token">访问令牌</param>
		/// <param name="openid"></param>
		/// <returns></returns>
		public static Boolean CheckToken(string access_token, string openid)
		{
			if (string.IsNullOrEmpty(access_token) || string.IsNullOrEmpty(openid))
			{
				return false;
			}

			string url = string.Format("https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}", access_token, openid);
			try
			{
				string result = HttpRequestHelper.Request(url);
				RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
				if (model == null)
				{
					if (model.errcode == 0)
						return true;
				}
			}
			catch (Exception Ex)
			{
				LogHelper.Save(Ex);
				throw Ex;
			}
			return false;
		}

		#region 获取用户基本信息
		/// <summary>
		/// 获取用户基本信息，返回JSON数据包
		/// </summary>
		/// <param name="access_token">访问令牌</param>
		/// <param name="openid">普通用户的标识，对当前公众号唯一</param>
		/// <returns>UserModel</returns>
		public static UserModel GetUserInfo(string access_token, string openid)
		{
			if (string.IsNullOrEmpty(access_token) || string.IsNullOrEmpty(openid)) return new UserModel();
			try
			{
				string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", access_token, openid);
				string result = HttpRequestHelper.Request(url, HttpRequestHelper.Method.GET);
				if (!string.IsNullOrEmpty(result))
				{
					return JsonHelper.DeSerialize<UserModel>(result);
				}
			}
			catch (Exception Ex)
			{
				LogHelper.Save(Ex, "", LogTime.day);
				throw Ex;
			}
			return new UserModel();
		}
		#endregion

	}
}
