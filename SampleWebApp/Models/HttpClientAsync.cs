using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SampleWebApp.Models
{
    /// <summary>
    /// 异步发送请求（HttpWebRequest）
    /// </summary>
    public class HttpClientAsync
    {

        /// <summary>
        /// 异步发送XML格式的POST请求
        /// </summary>
        /// <param name="url">目标URL</param>
        /// <param name="data">要发送的XML数据</param>
        /// <param name="method">GET，POST</param>
        /// <param name="dataType">要发送的数据类型：xml，json</param>
        /// <param name="timeoutSeconds">超时时间(秒)</param>
        /// <returns>服务器响应内容</returns>
        public static async Task<string> RequestAsync(string url, string data, string method = "POST", string dataType = "xml", int timeoutSeconds = 30)
        {
            try
            {
                // 创建HttpWebRequest对象
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET" == method ? method : "POST";
                switch(dataType)
                {
                    case "xml":
                        request.ContentType = "text/xml; charset=utf-8"; break; // application/xml‌
                    default:
                    case "json":
                        request.ContentType = "application/json; charset=utf-8"; break;
                }
                request.Timeout = timeoutSeconds * 1000; // 转换为毫秒

                // 异步写入请求数据
                using(Stream requestStream = await request.GetRequestStreamAsync())
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(data);
                    await requestStream.WriteAsync(byteArray, 0, byteArray.Length);
                }

                // 异步获取响应
                using(WebResponse response = await request.GetResponseAsync())
                using(Stream responseStream = response.GetResponseStream())
                using(StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch(WebException ex)
            {
                // 处理HTTP错误响应
                if(ex.Response != null)
                {
                    using(var errorResponse = (HttpWebResponse)ex.Response)
                    using(var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string errorDetails = await reader.ReadToEndAsync();
                        throw new Exception($"HTTP Error: {(int)errorResponse.StatusCode} {errorResponse.StatusCode}. Response: {errorDetails}", ex);
                    }
                }
                throw new Exception("网络请求失败: " + ex.Message, ex);
            }
            catch(Exception ex)
            {
                throw new Exception("发送请求时发生错误: " + ex.Message, ex);
            }
        }

        /*
            // 使用示例：异步发送POST请求
            string response = await HttpXmlPostClient.RequestAsync(callbackUrl, data);
            // 重试机制：
// 对于重要请求，可以实现重试逻辑
int retryCount = 0;
while (retryCount < 3)
{
    try
    {
        return await RequestAsync(url, data, timeoutSeconds);
    }
    catch
    {
        retryCount++;
        if (retryCount >= 3) throw;
        await Task.Delay(1000 * retryCount);
    }
}

         */





    }
}