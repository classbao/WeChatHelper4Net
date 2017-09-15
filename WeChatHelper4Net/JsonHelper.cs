using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// JSON助手
    /// </summary>
    public class JsonHelper
    {
        #region JSON序列化
        /// <summary>
        /// 将对象序列化成json字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象实例</param>
        /// <returns>json字符串</returns>
        public static string Serialize<T>(T o)
        {
            if (o == null) return string.Empty;
            string result = string.Empty;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
                json.WriteObject(memoryStream, o);
                result = Encoding.UTF8.GetString(memoryStream.ToArray());

                memoryStream.Close();
            }
            return result;
        }

        /// <summary>
        /// 将对象序列化成json字符串
        /// </summary>
        /// <param name="o">对象实例</param>
        /// <returns>json字符串</returns>
        public static string Serialize(object o)
        {
            if (o == null || o == System.DBNull.Value) return string.Empty;
            string result = string.Empty;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(o.GetType());
                json.WriteObject(memoryStream, o);
                result = Encoding.UTF8.GetString(memoryStream.ToArray());

                memoryStream.Close();
            }
            return result;
        }
        #endregion

        #region JSON反序列化
        /// <summary>
        /// 将json字符串反序列化成对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonText">json字符串文本</param>
        /// <returns>对象</returns>
        public static T DeSerialize<T>(string jsonText)
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonText)))
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
                T o = (T)json.ReadObject(memoryStream);

                memoryStream.Close();
                return o;
            }
            return default(T);
        }

        #endregion
    }
}
