/**
* 命名空间: WeChatHelper4Net
*
* 功 能： N/A
* 类 名： WeChatSignature
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/12 21:11:11 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2025 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WeChatHelper4Net
{
    /// <summary>
    /// 签名算法（微信开放平台）
    /// </summary>
    public static class WeChatSignature
    {
        /*
安全规范
更新时间：2024.12.11
1、签名算法
（签名校验工具）

签名生成的通用步骤如下：

第一步，设所有发送或者接收到的数据为集合M，将集合M内非空参数值的参数按照参数名ASCII码从小到大排序（字典序），使用URL键值对的格式（即key1=value1&key2=value2…）拼接成字符串stringA。

特别注意以下重要规则：

◆ 参数名ASCII码从小到大排序（字典序）；

◆ 如果参数的值为空不参与签名；

◆ 参数名区分大小写；

◆ 验证调用返回或微信主动通知签名时，传送的sign参数不参与签名，将生成的签名与该sign值作校验。

◆ 微信接口可能增加字段，验证签名时必须支持增加的扩展字段

第二步，在stringA最后拼接上key得到stringSignTemp字符串，并对stringSignTemp进行MD5运算，再将得到的字符串所有字符转换为大写，得到sign值signValue。 注意：密钥的长度为32个字节。

◆ key设置路径：微信商户平台-->账户中心-->账户设置-->API安全-->设置API密钥

举例：

假设传送的参数如下：

appid： wxd930ea5d5a258f4f
mch_id： 10000100
device_info： 1000
body： test
nonce_str： ibuaiVcKdpRxkhJA
第一步：对参数按照key=value的格式，并按照参数名ASCII字典序排序如下：

stringA="appid=wxd930ea5d5a258f4f&body=test&device_info=1000&mch_id=10000100&nonce_str=ibuaiVcKdpRxkhJA";
第二步：拼接API密钥：

MD5签名方式：
stringSignTemp=stringA+"&key=192006250b4c09247ec02edce69f6a2d" //注：key为商户平台设置的密钥key
sign=MD5(stringSignTemp).toUpperCase()="9A0A8659F005D6984697E2CA0A9CF3B7" //注：MD5签名方式
HMAC-SHA256签名方式：
stringSignTemp=stringA+"&key=192006250b4c09247ec02edce69f6a2d" //注：key为商户平台设置的密钥key
sign=hash_hmac("sha256",stringSignTemp,key).toUpperCase()="6A9AE1657590FD6257D693A078E1C3E4BB6BA4DC30B23E0EE2496E54170DACD6" //注：HMAC-SHA256签名方式，部分语言的hmac方法生成结果二进制结果，需要调对应函数转化为十六进制字符串。
最终得到最终发送的数据：

<xml>
<appid>wxd930ea5d5a258f4f</appid>
<mch_id>10000100</mch_id>
<device_info>1000</device_info>
<body>test</body>
<nonce_str>ibuaiVcKdpRxkhJA</nonce_str>
<sign>9A0A8659F005D6984697E2CA0A9CF3B7</sign>
</xml>


2、生成随机数算法
微信支付API接口协议中包含字段nonce_str（随机字符串，不长于32位。），主要保证签名不可预测。我们推荐生成随机数算法如下：调用随机数函数生成，将得到的值转换为字符串。
         */

        /// <summary>
        /// 生成微信签名。支持 SHA1、MD5 和 HMAC-SHA256 三种签名方式的高性能实现，完全符合微信开放平台规范，适合高并发环境使用。
        /// </summary>
        /// <param name="parameters">参数字典</param>
        /// <param name="apiKey">API密钥</param>
        /// <param name="signType">签名类型 MD5/SHA1</param>
        /// <returns>签名结果</returns>
        public static string Generate(IDictionary<string, string> parameters, string apiKey, string signType = "MD5")
        {
            if(parameters == null) throw new ArgumentNullException(nameof(parameters));
            if(string.IsNullOrEmpty(apiKey)) throw new ArgumentException("API密钥不能为空");

            // 1. 过滤空值并排序
            var sortedParams = parameters
                .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                .ToList();

            // 2. 拼接键值对
            string stringA = BuildQueryString(sortedParams, apiKey);

            // 3. 计算签名
            switch(signType.ToUpper())
            {
                case "SHA1": return ComputeSha1(stringA);
                case "MD5": return ComputeMd5(stringA);
                case "HMAC-SHA256": return ComputeHmacSha256(stringA, apiKey);
                default: throw new ArgumentException("不支持的签名类型，请使用MD5/SHA1/HMAC-SHA256");
            };
        }

        /// <summary>
        /// MD5信息摘要算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string ComputeMd5(string input)
        {
            using(var md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }
        }

        /// <summary>
        /// SHA1加密散列算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string ComputeSha1(string input)
        {
            using(var sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }
        }

        /// <summary>
        /// HMAC-SHA256：基于哈希函数的消息认证码算法，结合了SHA-256哈希算法和密钥派生机制，用于验证消息的完整性和真实性。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string ComputeHmacSha256(string input, string key)
        {
            using(var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }
        }

        /// <summary>
        /// 使用URL键值对的格式（即key1=value1&key2=value2…），并在stringA最后拼接上key
        /// </summary>
        /// <param name="sortedParams"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        private static string BuildQueryString(List<KeyValuePair<string, string>> sortedParams, string apiKey)
        {
            // 使用预分配的StringBuilder提高性能
            var sb = new StringBuilder(256);

            foreach(var param in sortedParams)
            {
                sb.Append(param.Key)
                  .Append('=')
                  .Append(param.Value)
                  .Append('&');
            }

            sb.Append("key=").Append(apiKey);
            return sb.ToString();
        }

        /// <summary>
        /// 使用URL键值对的格式（即key1=value1&key2=value2…），并在stringA最后拼接上key（如果需要处理URL编码）
        /// </summary>
        /// <param name="sortedParams"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        private static string BuildQueryStringForUrlEncode(List<KeyValuePair<string, string>> sortedParams, string apiKey)
        {
            var sb = new StringBuilder(256);

            foreach(var param in sortedParams)
            {
                sb.Append(HttpUtility.UrlEncode(param.Key))
                  .Append('=')
                  .Append(HttpUtility.UrlEncode(param.Value))
                  .Append('&');
            }

            sb.Append("key=").Append(HttpUtility.UrlEncode(apiKey));
            return sb.ToString();
        }


        /// <summary>
        /// 随机字符串：获取无连字符的Guid（32位），性能优化版本（适用于高性能场景）
        /// </summary>
        /// <returns></returns>
        public static string GuidWithoutHyphens()
        {
            return Guid.NewGuid().ToString("N");
        }


        /// <summary>
        /// 验证签名
        /// </summary>
        public static bool Verify(IDictionary<string, string> parameters, string apiKey, string receivedSign,
            string signType = "MD5")
        {
            string generatedSign = Generate(parameters, apiKey, signType);
            return string.Equals(generatedSign, receivedSign, StringComparison.Ordinal);
        }

    }
}
