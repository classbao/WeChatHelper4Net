using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Security;
using WeChatHelper4Net.Models;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// 常用公共方法与属性
    /// </summary>
    public class Common
    {
        private Common() { }

        /// <summary>
        /// 字符串：success
        /// </summary>
        public static string success { get { return "success"; } }
        /// <summary>
        /// 字符串：error
        /// </summary>
        public static string error { get { return "error"; } }

        #region 公众微信号相关信息
        /// <summary>
        /// 微信公众号域名（结尾不包含“/”）
        /// </summary>
        public static string WeiXinDomainName { get { return Privacy.wxDomainName; } }
        /// <summary>
        /// 公众微信号名称
        /// </summary>
        public static string WeiXinName { get { return Privacy.wxName; } }
        /// <summary>
        /// 公众微信号
        /// </summary>
        public static string WeiXinNumber { get { return Privacy.wxNumber; } }
        /// <summary>
        /// 公众微信号（原始ID）
        /// </summary>
        public static string WeiXinId { get { return Privacy.wxId; } }
        /// <summary>
        /// 开发者ID：AppID(应用ID，微信公众号身份的唯一标识)
        /// </summary>
        public static string AppId { get { return Privacy.AppId; } }

        /// <summary>
        /// 商户号（MCHID）
        /// </summary>
        public static string PartnerID { get { return Privacy.PartnerID; } }
        #endregion

        #region ApiUrl
        /// <summary>
        /// https://api.weixin.qq.com/
        /// 通用域名，使用该域名将访问官方指定就近的接入点
        /// </summary>
        public static string ApiHost { get { return "https://api.weixin.qq.com/"; } }

        /// <summary>
        /// WeChatHelper4Net公用的URL地址
        /// https://api.weixin.qq.com/cgi-bin/
        /// 注意调用所有微信接口时均需使用https协议
        /// </summary>
        public static string ApiUrl { get { return string.Concat(ApiHost, "cgi-bin/"); } }

        #endregion

        #region 辅助方法
        /// <summary>
        /// 正则表达式替换字符串（不区分大小写）
        /// </summary>
        /// <param name="input">原内容文本</param>
        /// <param name="pattern">要替换的正则表达式匹配</param>
        /// <param name="replacement">用来替换的内容</param>
        /// <returns></returns>
        public static string Replace(string input, string pattern, string replacement)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern) || replacement == null) return string.Empty;
            return Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
        }

        #region 时间戳
        /// <summary>
        /// 时间戳转换为标准时间格式（基准时间为"1970-1-1 08:00:00"）
        /// </summary>
        /// <param name="TimeStamp">时间戳</param>
        /// <returns>标准时间格式</returns>
        public static DateTime ConvertTime(int TimeStamp)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, TimeStamp);
            DateTime baseTime = Convert.ToDateTime("1970-1-1 08:00:00");
            DateTime now = baseTime + ts;

            return now;
        }
        /// <summary>
        /// 标准时间格式转换为时间戳（基准时间为"1970-1-1 08:00:00"）
        /// </summary>
        /// <param name="time">标准时间格式</param>
        /// <returns>时间戳</returns>
        public static int ConvertTime(DateTime time)
        {
            //基准为"1970-1-1 08:00:00"时间转整数
            DateTime baseTime = Convert.ToDateTime("1970-1-1 08:00:00");
            TimeSpan ts = time - baseTime;
            int TimeStamp = (int)ts.TotalSeconds;

            return TimeStamp;
        }
        #endregion

        #region Url编码与解码
        /// <summary>
        /// url编码，添加空格转成%20
        /// </summary>
        /// <param name="parameterValue">参数值</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string UrlEncode(string parameterValue, System.Text.Encoding encoding)
        {
            string UrlEncode = "";
            UrlEncode = HttpUtility.UrlEncode(parameterValue, encoding);
            UrlEncode = UrlEncode.Replace("+", "%20");
            return UrlEncode;
        }
        /// <summary>
        /// url编码，添加空格转成%20（默认UTF-8编码）
        /// </summary>
        /// <param name="parameterValue">参数值</param>
        /// <returns></returns>
        public static string UrlEncode(string parameterValue)
        {
            return UrlEncode(parameterValue, System.Text.Encoding.UTF8);
        }
        #endregion

        /// <summary>
        /// UTF8编码的字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetUTF8(string text)
        {
            byte[] utf8bytes = System.Text.Encoding.Default.GetBytes(text);
            byte[] utf8bytes2 = System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, utf8bytes);
            return System.Text.Encoding.Default.GetString(utf8bytes2);
        }

        #region 加密与解密
        /// <summary>
        /// SHA1方式加密字符串的方法
        /// </summary>
        /// <param name="text">要进行加密的字符串</param>
        /// <param name="encoding">字符串编码格式</param>
        /// <returns>加密后的字符串</returns>
        public static string SHA1Encrypt(string text, System.Text.Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            try
            {
                byte[] cleanBytes = encoding.GetBytes(text);
                byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
                StringBuilder returnResult = new StringBuilder();
                foreach (var b in hashedBytes)
                {
                    returnResult.AppendFormat("{0:x2}", b);
                }
                return returnResult.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1方式加密字符串失败。错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// SHA1方式加密字符串的方法（utf-8编码）
        /// </summary>
        /// <param name="text">要进行加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string SHA1Encrypt(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            return SHA1Encrypt(text, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// MD5方式加密字符串的方法
        /// </summary>
        /// <param name="text">要进行加密的字符串</param>
        /// <param name="encoding">字符串编码格式</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5Encrypt(string text, System.Text.Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] t = md5.ComputeHash(encoding.GetBytes(text));
                for (int i = 0; i < t.Length; i++)
                {
                    sb.Append(t[i].ToString("x").PadLeft(2, '0'));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("MD5方式加密字符串失败。错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// MD5方式加密字符串的方法（utf-8编码）
        /// </summary>
        /// <param name="text">要进行加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5Encrypt(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;
            return MD5Encrypt(text, System.Text.Encoding.UTF8);
        }
        #endregion

        #region XML与参数字典
        #region Xml转义
        /*
		* CDATA标签用于说明数据不被XML解析器解析。
		* 下面是五个在XML文件中预定义好的实体：
		* &lt;	< 小于号
		* &gt; > 大于号
		* &amp; & 和
		* &apos; ' 单引号
		* &quot; " 双引号
		* 实体必须以符号"&"开头，以符号";"结尾。
		* 注意: 只有"<" 字符和"&"字符对于XML来说是严格禁止使用的。剩下的都是合法的，为了减少出错，使用实体是个好习惯。
		*/

        /// <summary>
        /// Xml值转义标签字符，将标签等特殊字符转义成预定义实体
        /// </summary>
        /// <param name="XmlText"></param>
        /// <returns></returns>
        public static string XmlEscape(string XmlText)
        {
            if (string.IsNullOrWhiteSpace(XmlText)) return string.Empty;
            return XmlText
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("&", "&amp;")
                .Replace("'", "&apos;")
                .Replace("\"", "&quot;");
        }
        /// <summary>
        /// Xml值反转义标签字符，将预定义实体反转义成原标签等特殊字符
        /// </summary>
        /// <param name="XmlText"></param>
        /// <returns></returns>
        public static string XmlNoEscape(string XmlText)
        {
            if (string.IsNullOrWhiteSpace(XmlText)) return string.Empty;
            return XmlText
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&amp;", "&")
                .Replace("&apos;", "'")
                .Replace("&quot;", "\"");
        }
        #endregion

        //获取Json某一key值：
        /// <summary>
        /// 从json字符串中获取key的值
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetJosnValue(string jsonString, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonString))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonString.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值

                    int end = jsonString.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonString.IndexOf('}', index);
                    }
                    //index = json.IndexOf('"', index + key.Length + 1) + 1;
                    result = jsonString.Substring(index, end - index);
                    //过滤引号或空格
                    result = result.Trim(new char[] { '"', ' ', '\'' });
                }
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 返回url（不包含#及其后面部分） 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CleanUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;
            return Replace(url, @"(#.*|&[^=]*)$", ""); //清理url结尾的#……与以及多余的&
        }

        /// <summary>
        /// 微信公众号唯一的OpenID
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static bool IsOpenID(string openid)
        {
            if (!string.IsNullOrWhiteSpace(openid))
                return Regex.IsMatch(openid, @"^[0-9a-zA-Z_\-]{28}$");
            return false;
        }
        /// <summary>
        /// 微信开放平台UnionID
        /// </summary>
        /// <param name="UnionID"></param>
        /// <returns></returns>
        public static bool IsUnionID(string unionid)
        {
            if (!string.IsNullOrWhiteSpace(unionid))
                return Regex.IsMatch(unionid, @"^[0-9a-zA-Z_\-]{28,29}$");
            return false;
        }

        /// <summary>
        /// 用户头像来自微信URL
        /// </summary>
        /// <param name="headimgurl"></param>
        /// <returns></returns>
        public static bool IsHeadimgurl(string headimgurl)
        {
            if (!string.IsNullOrWhiteSpace(headimgurl))
                return headimgurl.StartsWith("http://wx.qlogo.cn/");
            return false;
        }

        /// <summary>
        /// 获取请求的微信客户端UserAgent对象
        /// </summary>
        /// <param name="UserAgent"></param>
        /// <returns></returns>
        public static MicroMessengerUserAgent MicroMessengerUserAgent(string UserAgent)
        {
            if (string.IsNullOrWhiteSpace(UserAgent)) return null;
            Match m = Regex.Match(UserAgent, @"MicroMessenger/(\d(\.\d+)+)", RegexOptions.IgnoreCase);
            if (m != null && !string.IsNullOrWhiteSpace(m.Value))
            {
                MicroMessengerUserAgent model = new MicroMessengerUserAgent()
                {
                    IsMicroMessenger = true,
                    Version = m.Value.ToLower().Replace("micromessenger/", "")
                };
                Match mNetType = Regex.Match(UserAgent, @"NetType/\w+", RegexOptions.IgnoreCase);
                if (mNetType != null && !string.IsNullOrWhiteSpace(mNetType.Value))
                {
                    model.NetType = mNetType.Value.ToLower().Replace("nettype/", "");
                }
                return model;
            }
            return null;
        }

        /// <summary>
        /// 正确时的返回JSON数据包
        /// {"errcode":0,"errmsg":"ok"}
        /// </summary>
        /// <param name="resultJsonString"></param>
        /// <returns></returns>
        public static bool ReturnJSONisOK(string resultJsonString)
        {
            if (!string.IsNullOrWhiteSpace(resultJsonString))
                return resultJsonString.Contains("{\"errcode\":0,\"errmsg\":\"ok\"}"); //{"errcode":0,"errmsg":"ok"}
            return false;
        }
        #endregion

    }

}