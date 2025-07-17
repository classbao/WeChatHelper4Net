using System.IO;
using System.Text;
using System.Text.RegularExpressions;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// 响应微信服务器消息推送，接收微信服务器推送来的请求或消息或事件或结果
    /// </summary>
    public class ReceivingMessage
    {
        private ReceivingMessage() { }

        /// <summary>
        /// Stream转换为字符串（可以用于接收POST请求完整数据）
        /// </summary>
        /// <param name="stream">Request.InputStream</param>
        /// <returns>返回：接收到POST请求完整数据字符串</returns>
        public static string StreamToString(System.IO.Stream stream)
        {
            if (null != stream)
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                return Encoding.UTF8.GetString(bytes);
            }
            return string.Empty;
        }

        /// <summary>
        /// 接收POST请求完整数据并转换为字符串
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>返回：接收到POST请求完整数据字符串</returns>
        public static string PostDataToString(System.Web.HttpRequestBase Request)
        {
            // 读取POST数据
            using(System.IO.StreamReader reader = new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)) // HttpRequest.InputStream 是一次性的流:只能读取一次：一旦读取后，流的位置会到达末尾，再次读取将得不到数据
            {
                string postData = reader.ReadToEnd();
                return postData;
            }
        }


        //public static string DecryptWithAES(string xmlString, string EncodingAESKey)
        //{

        //}

        public static string GetMsgType(string xmlString)
        {
            Regex reg = new Regex(@"(?<=(\<MsgType\>\<\!\[CDATA\[))[\w\d]+(?=(\]\]\>\<\/MsgType\>))", RegexOptions.IgnoreCase); // <MsgType><![CDATA[text]]></MsgType>
            Match m = reg.Match(xmlString);
            if (null != m)
                return m.Value;
            return string.Empty;
        }

        public static string GetEvent(string xmlString)
        {
            Regex reg = new Regex(@"(?<=(\<Event\>\<\!\[CDATA\[))[\w\d]+(?=(\]\]\>\<\/Event\>))", RegexOptions.IgnoreCase); // <Event><![CDATA[subscribe]]></Event>
            Match m = reg.Match(xmlString);
            if (null != m)
                return m.Value;
            return string.Empty;
        }

        public static bool ContainQrscene(string xmlString)
        {
            return xmlString.Contains("qrscene_");
        }
    }
}
