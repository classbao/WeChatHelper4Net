using System;
using System.Web;
using WeChatHelper4Net.Models;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2016-12-11
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// 微信网页授权
    /// </summary>
    public class OAuth
	{
        private OAuth() { }

        /// <summary>
        /// 微信网页授权，如果用户同意授权，页面将跳转至 redirect_uri/?code=CODE&amp;state=STATE。
        /// </summary>
        /// <param name="redirect_uri">授权后重定向的回调链接地址</param>
        /// <param name="state">重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值，最多128字节</param>
        /// <param name="scope">应用授权作用域</param>
        /// <returns></returns>
        public static string OAuthToURL(string redirect_uri, string state = "", scope scope = scope.snsapi_base)
        {
            if (string.IsNullOrWhiteSpace(redirect_uri))
                throw new ArgumentException("参数值无效", nameof(redirect_uri));

            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect"
                , Privacy.AppId
                , HttpUtility.UrlEncode(redirect_uri)
                , scope.ToString()
                , state
                );
        }

        /// <summary>
        /// 通过code换取网页授权access_token。
        /// 如果网页授权的作用域为snsapi_base，则本步骤中获取到网页授权access_token的同时，也获取到了openid，snsapi_base式的网页授权流程即到此为止。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static PageOAuth_AccessToken GetAccessToken(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("参数值无效", nameof(code));

            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", Privacy.AppId, Privacy.AppSecret, code);
            string result = HttpRequestHelper.Request(url);
            PageOAuth_AccessToken accessToken = JsonHelper.DeSerialize<PageOAuth_AccessToken>(result);
            return accessToken;
        }

        /// <summary>
        /// 刷新access_token
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public static PageOAuth_AccessToken RefreshAccessToken(string refresh_token)
        {
            if (string.IsNullOrWhiteSpace(refresh_token))
                throw new ArgumentException("参数值无效", nameof(refresh_token));

            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", Privacy.AppId, refresh_token);
            string result = HttpRequestHelper.Request(url);
            PageOAuth_AccessToken accessToken = JsonHelper.DeSerialize<PageOAuth_AccessToken>(result);
            return accessToken;
        }

        /// <summary>
        /// 检验授权凭证（access_token）是否有效
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static bool CheckAccessToken(string access_token, string openid)
        {
            if (string.IsNullOrWhiteSpace(access_token) || string.IsNullOrWhiteSpace(openid))
                return false;

            string url = string.Format("https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}", access_token, openid);
            string result = HttpRequestHelper.Request(url);
            RequestResult_errmsg resultEntity = JsonHelper.DeSerialize<RequestResult_errmsg>(result);
            if (null != resultEntity && 0 == resultEntity.errcode)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 拉取用户信息(需scope为 snsapi_userinfo)
        /// </summary>
        /// <param name="access_token">网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同</param>
        /// <param name="openid"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(string access_token, string openid, string lang = "zh_CN")
        {
            if (string.IsNullOrWhiteSpace(access_token) || string.IsNullOrWhiteSpace(openid))
                return null;

            string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}", access_token, openid, lang);
            string result = HttpRequestHelper.Request(url);
            // {"openid":"od2Uu1IvIX1ypbmqnkqGTKNtHBXQ","nickname":"熊仔其人","sex":1,"language":"zh_CN","city":"海淀","province":"北京","country":"中国","headimgurl":"http:\/\/thirdwx.qlogo.cn\/mmopen\/vi_32\/Q0j4TwGTfTKOticpMHZNfGZstl5MNz9sqFwQ9bSmNbF2T2f9qlfb3mcx8pVnF7jwODBTVnNdBxekR77QBiaCLTNA\/132","privilege":[]}
            UserInfo resultEntity = JsonHelper.DeSerialize<UserInfo>(result);
            return resultEntity;
        }

    }
}