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
    /// 验证URL有效性，即验证消息的确来自微信服务器，申请成为微信公众账号开发者的时候使用
    /// </summary>
    public class ValidationToken
    {
        /// <summary>
        /// 将Token、timestamp、nonce三个参数值按字典排序，拼接成一条字符串，然后SHA1方式加密（utf-8编码），结果与signature完全一致则校验成功
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
		private static bool CheckToken(string signature, string timestamp, string nonce, string Token)
        {
            if(string.IsNullOrWhiteSpace(signature) || string.IsNullOrWhiteSpace(timestamp) || string.IsNullOrWhiteSpace(nonce)) return false;

            string[] ArrTmp = { Token, timestamp, nonce };
            System.Array.Sort(ArrTmp);  
            string tmpStr = string.Join("", ArrTmp);
            //tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = Common.SHA1Encrypt(tmpStr);
            //tmpStr = tmpStr.ToLower();
            if(tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证URL有效性，即验证消息的确来自微信服务器，开发者提交信息后，微信服务器将发送GET请求到填写的URL上，GET请求携带四个参数。而Token的值则是在微信开发平台上自定义配置的。
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        /// <param name="Token">Token</param>
        /// <returns>随机字符串echostr</returns>
        public static string Validation(string signature, string timestamp, string nonce, string echostr, string Token)
        {
            if(string.IsNullOrWhiteSpace(echostr) || string.IsNullOrWhiteSpace(Token)) return string.Empty;
            if(CheckToken(signature, timestamp, nonce, Token))
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