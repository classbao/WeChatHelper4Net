using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// 验证URL有效性与Token，申请成为微信公众账号开发者的时候使用
    /// </summary>
    public class ValidationToken
    {
        /// <summary>
        /// 验证URL有效性，开发者提交信息后，微信服务器将发送GET请求到填写的URL上，GET请求携带四个参数。
        /// </summary>
        /// <param name="signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
		/// <param name="Token">Token</param>
		/// <returns></returns>
		private static bool CheckToken(string signature, string timestamp, string nonce, string Token)
		{
			if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce)) return false;
			if (string.IsNullOrEmpty(Token)) return false;

			string[] ArrTmp = { Token, timestamp, nonce };
			System.Array.Sort(ArrTmp); //字典排序  
			string tmpStr = string.Join("", ArrTmp);
			//tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
			tmpStr = Common.SHA1Encrypt(tmpStr); //字符串进行sha1加密
			//tmpStr = tmpStr.ToLower();
			if (tmpStr == signature)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        /// <summary>
        /// 验证URL有效性，开发者提交信息后，微信服务器将发送GET请求到填写的URL上，GET请求携带四个参数。
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        /// <param name="Token">Token</param>
        /// <returns>随机字符串echostr</returns>
        public static string Validation(string signature, string timestamp, string nonce, string echostr, string Token)
        {
            if (string.IsNullOrEmpty(echostr) || string.IsNullOrEmpty(Token)) return string.Empty;
            if (CheckToken(signature, timestamp, nonce, Token))
            {
                return echostr;
            }
            else
            {
                return string.Empty;
            }
        }

    }
}