/**
* 命名空间: WeChatHelper4Net.Pay
*
* 功 能： N/A
* 类 名： XmlPostHelper
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/13 16:25:59 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2025 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;

namespace WeChatHelper4Net.Pay
{
    /// <summary>
    /// 调用支付API必须遵守协议规则
    /// </summary>
    public static class XmlPostHelper
    {
        /*
协议规则 更新时间：2024.11.18 商户接入微信支付，调用API必须遵循以下规则：

传输方式：为保证交易安全性，采用HTTPS传输

提交方式：采用POST方法提交

数据格式：提交和返回数据都为XML格式，根节点名为xml

字符编码：微信支付API v2仅支持UTF-8字符编码的一个子集：使用一至三个字节编码的字符。也就是说，不支持Unicode辅助平面中的四至六字节编码的字符。

签名算法：MD5/HMAC-SHA256

签名要求：请求和接收数据均需要校验签名，详细方法请参考安全规范-签名算法

证书要求：调用申请退款、撤销订单、红包接口等需要商户api证书，各api接口文档均有说明。

判断逻辑：先判断协议字段返回，再判断业务返回，最后判断交易状态

特别提示：必须严格按照API的说明进行一单一支付，一单一红包，一单一付款，在未得到支付系统明确的回复之前不要换单，防止重复支付或者重复付款
         */

        private static readonly X509Certificate2 Certificate;
        private static readonly Encoding Utf8Encoding = Encoding.UTF8;

        // 静态构造函数，初始化证书（只加载一次）
        static XmlPostHelper()
        {
            try
            {
                // 加载证书
                //X509Certificate2 certificate = new X509Certificate2(certPath, certPassword);

                // 根据实际情况优化
                var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                var certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, "xxx有限公司", false);
                Certificate = certCollection.Count > 0 ? certCollection[0] : null;
                store.Close();
            }
            catch(Exception ex)
            {
                LogHelper.Save(ex);
                Certificate = null;
            }
        }

        /*
         * 使用示例：
                string xmlData = "<Request><Data>测试数据</Data></Request>";
                string apiUrl = "https://api.example.com/endpoint";
        
                XmlDocument response = XmlPostHelper.Post(apiUrl, xmlData, true);
         */

        /// <summary>
        /// 调用支付API必须遵守协议规则
        /// </summary>
        /// <param name="url">微信支付API地址</param>
        /// <param name="xmlDoc">提交和返回数据都为XML格式，根节点名为xml </param>
        /// <param name="useClientCertificate">默认不需要客户端证书。调用申请退款、撤销订单、红包接口等需要商户api证书，各api接口文档均有说明。</param>
        /// <returns></returns>
        public static XmlDocument Post(string url, string xmlDoc, bool useClientCertificate = false)
        {
            // 验证输入
            if(string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));
            if(string.IsNullOrWhiteSpace(xmlDoc))
                throw new ArgumentNullException(nameof(xmlDoc));

            // 准备请求数据（避免多次编码转换）
            byte[] requestData = Utf8Encoding.GetBytes(xmlDoc);

            // 创建并配置请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            ConfigureRequest(request, requestData.Length, useClientCertificate);

            try
            {
                // 异步写入请求数据（避免阻塞线程池线程）
                using(Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(requestData, 0, requestData.Length);
                }

                // 获取响应
                using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using(Stream responseStream = response.GetResponseStream())
                {
                    XmlDocument xmlResponse = new XmlDocument();

                    // 使用XmlTextReader提高加载性能
                    using(XmlTextReader reader = new XmlTextReader(responseStream))
                    {
                        reader.ProhibitDtd = true; // 禁用DTD解析防止XXE攻击
                        xmlResponse.Load(reader);
                    }

                    return xmlResponse;
                }
            }
            catch(WebException webEx)
            {
                // 特殊处理WebException以获取更多错误信息
                HandleWebException(webEx);
                throw webEx; // 重新抛出异常
            }
            catch(Exception ex)
            {
                LogHelper.Save(ex);
                throw ex;
            }
        }

        private static void ConfigureRequest(HttpWebRequest request, int contentLength, bool useClientCertificate)
        {
            request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/xml; charset=utf-8"; // 修正为正确的XML内容类型
            request.ContentLength = contentLength;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.0; .NET CLR 1.1.4322; .NET CLR 2.0.50215;)";
            request.Timeout = 30000; // 30秒超时
            request.ReadWriteTimeout = 60000; // 60秒读写超时

            // 配置证书
            if(useClientCertificate && Certificate != null)
            {
                request.ClientCertificates.Add(Certificate);
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            }

            // 性能优化设置
            request.ServicePoint.Expect100Continue = false; // 禁用100-Continue
            request.KeepAlive = true; // 保持连接
        }

        private static void HandleWebException(WebException webEx)
        {
            try
            {
                if(webEx.Response != null)
                {
                    using(var errorResponse = (HttpWebResponse)webEx.Response)
                    using(var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        string errorDetails = reader.ReadToEnd();
                        LogHelper.Save(new Exception($"HTTP Error {(int)errorResponse.StatusCode}: {errorDetails}", webEx));
                    }
                }
                else
                {
                    LogHelper.Save(webEx);
                }
            }
            catch(Exception logEx)
            {
                LogHelper.Save(logEx);
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors errors)
        {
            if(errors == System.Net.Security.SslPolicyErrors.None)
                return true; // 仅用于测试环境，生产环境中应实现严格的证书验证逻辑
            return false;
        }


    }
}




