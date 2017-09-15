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

        public static string StreamToString(Stream stream)
        {
            if (null != stream)
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                return Encoding.UTF8.GetString(bytes);
            }
            return string.Empty;
        }

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
