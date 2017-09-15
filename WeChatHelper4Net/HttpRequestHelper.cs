using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// HttpRequest，处理HTTP请求
    /// </summary>
    public class HttpRequestHelper
    {
        /// <summary>
        /// HttpRequest请求方式
        /// </summary>
        public enum Method
        {
            /// <summary>
            /// GET方式
            /// </summary>
            GET,
            /// <summary>
            /// POST方式
            /// </summary>
            POST
        }

        #region HTTP请求
        public static string Get(string url, string contentType = "application/x-www-form-urlencoded", CookieCollection cookies = null, string accept = "*/*")
        {
            string responseString;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                webRequest.ContentType = contentType;
            }
            if (cookies != null)
            {
                webRequest.CookieContainer = new CookieContainer();
                webRequest.CookieContainer.Add(cookies);
            }
            webRequest.Accept = accept;
            //webRequest.AllowAutoRedirect = true;
            //webRequest.Method = "GET";
            //webRequest.ContentType = "application/x-www-form-urlencoded";
            using (WebResponse response = webRequest.GetResponse() as HttpWebResponse)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        responseString = reader.ReadToEnd();
                    }
                }
            }
            return responseString;
        }

        /// <summary>
        /// HTTP请求（get方式）默认utf-8编码
        /// 作者：熊学浩
        /// 时间：2014-5-27
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public static string Request(string url)
        {
            return Request(url, string.Empty, Method.GET, System.Text.Encoding.UTF8);
        }
        /// <summary>
        /// HTTP请求，默认utf-8编码
        /// 作者：熊学浩
        /// 时间：2014-5-27
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <returns></returns>
        public static string Request(string url, Method method)
        {
            return Request(url, string.Empty, method, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// HTTP请求
        /// 作者：熊学浩
        /// 时间：2014-5-27
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string Request(string url, string data, Method method, System.Text.Encoding encode)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(url)) throw new Exception("目标url不能为空！");
            method = method != null ? method : Method.GET; //默认GET方式提交请求
            encode = encode != null ? encode : System.Text.Encoding.UTF8; //默认utf-8编码

            try
            {
                url = Uri.UnescapeDataString(url);
            }
            catch { };
            Regex re = new Regex("(?<h>[^\x00-\xff]+)");
            Match mc = re.Match(url);
            if (mc.Success)
            {
                string han = mc.Groups["h"].Value;
                //url = url.Replace(han, System.Web.HttpUtility.UrlEncode(han, Encoding.GetEncoding("GB2312")));
                url = url.Replace(han, System.Web.HttpUtility.UrlEncode(han, encode));
            }

			ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate; //解决“基础连接已经关闭: 未能为SSL/TLS 安全通道建立信任关系”
			//LogHelper.Save("总是接受", "ValidateServerCertificate", LogType.Report, LogTime.day);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UseDefaultCredentials = true;
            request.Method = method.ToString();

            if (method == Method.GET)
            {
                request.ContentType = "text/html;charset=" + (encode == System.Text.Encoding.UTF8 ? "UTF-8" : "GB2312");
            }
            else if (method == Method.POST)
            {
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] byteArray = encode.GetBytes(data);
                request.ContentLength = byteArray.Length;
                System.IO.Stream newStream = request.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);
                newStream.Close();
            }
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.50215;)";
            request.CookieContainer = new CookieContainer();
            request.AllowAutoRedirect = true;
            request.MaximumAutomaticRedirections = 3;
			if (method == Method.GET)
			{
				//request.KeepAlive = true;
				request.Timeout = 10000; //10秒
			}
			else
			{
				request.KeepAlive = true;
				request.Timeout = 50000; //50秒
			}

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //网页编码
                string CharSet = string.Empty;
				if (response.ContentType.ToLower().IndexOf("charset=") != -1)
                {
					try
					{
						CharSet = response.ContentType.Substring(response.ContentType.ToLower().IndexOf("charset=") + "charset=".Length);
					}
					catch
					{
						CharSet = Encoding.Default.BodyName;
					}
                }
				else if (response.ContentType.ToLower().IndexOf("encoding=") != -1)
				{
					try
					{
						CharSet = response.ContentType.Substring(response.ContentType.ToLower().IndexOf("encoding=") + "encoding=".Length);
					}
					catch
					{
						CharSet = Encoding.Default.BodyName;
					}
				}
				else
				{
					CharSet = response.CharacterSet;
					if (string.Compare(CharSet, "ISO-8859-1") == 0)
						CharSet = Encoding.UTF8.BodyName;
				}

                var buffer = GetBytes(response);
                result = GetStringFromBuffer(buffer, ref CharSet);

                response.Close();
                request.Abort();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #region 不同网页编码处理
        /// <summary>
        /// 内存流，将流拷贝到byte数组中保存起来。
        /// 把流拷贝到内存里面可以重复使用。
        /// 作者：熊学浩
        /// 时间：2014-03-27
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static byte[] GetBytes(WebResponse response)
        {
            var length = (int)response.ContentLength;
            byte[] data;

            using (var memoryStream = new MemoryStream())
            {
                var buffer = new byte[0x100];
                using (var rs = response.GetResponseStream())
                {
                    for (var i = rs.Read(buffer, 0, buffer.Length); i > 0; i = rs.Read(buffer, 0, buffer.Length))
                    {
                        memoryStream.Write(buffer, 0, i);
                    }
                }
                data = memoryStream.ToArray();
            }

            return data;
        }
        /// <summary>
        /// 从字节数组编码字符串
        /// 作者：熊学浩
        /// 时间：2014-03-27
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="charSet"></param>
        /// <returns></returns>
        private static string GetStringFromBuffer(byte[] buffer, ref string charSet)
        {
            if (string.IsNullOrEmpty(charSet) || string.Compare(charSet.Trim().ToUpper(), "ISO-8859-1") == 0)
            {
                charSet = GetEncodingFromBody(buffer);
            }

			try
			{
				var encoding = Encoding.GetEncoding(charSet);
				var str = encoding.GetString(buffer);
				return str;
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
        }
        /// <summary>
		/// 当无法从Http Header中获得编码信息时就用ASCII编码从buffer中获得字符串。我们知道HTML的标签都是字母，使用ASCII编码虽然中文或者其他双字节字符会出现乱码，但是HTML标签还是能够解析出来。这样我们就可以检测HTML的meta标签从而获得charset。
        /// 作者：熊学浩
        /// 时间：2014-03-27
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static string GetEncodingFromBody(byte[] buffer)
        {
            Regex regex = new Regex("charset=(?<encoding>[^=]+)?\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var str = Encoding.ASCII.GetString(buffer);
            var regMatch = regex.Match(str);
            if (regMatch.Success)
            {
                var charSet = regMatch.Groups["encoding"].Value.Trim();
                return charSet.Replace("\"", "").Trim();
            }

            return Encoding.ASCII.BodyName;
        }
        #endregion

		/// <summary>
		/// 解决“请求被中止: 未能创建 SSL/TLS 安全通道。”的问题
		/// http://radiumwong.iteye.com/blog/684118
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="certificate"></param>
		/// <param name="chain"></param>
		/// <param name="sslPolicyErrors"></param>
		/// <returns></returns>
		private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			//总是接受
			return true;
		}
        #endregion
    }
}
