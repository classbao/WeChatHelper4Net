using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChatHelper4Net.Extend
{
    /// <summary>
    /// Dictionary 扩展属性与方法
    /// </summary>
    public static class DictionaryExtend
	{

        #region 参数字典排序
        /// <summary>
        /// 将集合内非空参数值的参数按照参数名ASCII码从小到大排序（字典序），使用URL键值对的格式（即key1=value1&amp;key2=value2…）拼接成字符串stringA
        /// </summary>
        /// <param name="dictionarys">参数（键值对）字典</param>
        /// <param name="isAsc">参数（键值对）字典的主键排序</param>
        /// <returns></returns>
        [Obsolete("已经永久迁移到：WeChatSignature.Generate")]
        public static Dictionary<string, string> Sort(this Dictionary<string, string> dictionarys, bool isAsc = true)
		{
			Dictionary<string, string> newDictionarys = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> item in dictionarys)
			{
				if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value))
				{
					newDictionarys.Add(item.Key, item.Value);
				}
			}
			if (isAsc)
				return newDictionarys.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value); //将集合内非空参数值的参数按照参数名ASCII码从小到大排序（字典序）
			else
				return newDictionarys.OrderByDescending(o => o.Key).ToDictionary(o => o.Key, p => p.Value); //将集合内非空参数值的参数按照参数名ASCII码从大到小排序（字典降序）
		}

        /// <summary>
        /// 将参数字典转换成URL键值对的格式（即 key1=value1&amp;key2=value2…）
        /// </summary>
        /// <param name="dictionarys">参数（键值对）字典</param>
        /// <returns></returns>
        [Obsolete("已经永久迁移到：WeChatSignature.Generate")]
        public static string ToURLParameter(this Dictionary<string, string> dictionarys)
		{
			if (dictionarys == null || dictionarys.Count < 1) return string.Empty;
			StringBuilder UrlParameterString = new StringBuilder();
			foreach (KeyValuePair<string, string> item in dictionarys)
			{
				UrlParameterString.AppendFormat("{0}={1}&", item.Key, item.Value);
			}
			return UrlParameterString.ToString().Trim(new char[] { '&' });
		}
        #endregion

        /// <summary>
        /// 字典转换XML字符串
        /// </summary>
        /// <param name="dictionarys">字典</param>
        /// <param name="RootTag">根标签</param>
        /// <returns>XML字符串</returns>
        public static string ToXml(this Dictionary<string, string> dictionarys, string RootTag = "xml")
        {
            StringBuilder sb = new StringBuilder();
            foreach(KeyValuePair<string, string> pair in dictionarys)
            {
                if(string.IsNullOrWhiteSpace(pair.Key) || string.IsNullOrWhiteSpace(pair.Value)) continue;
                if(System.Text.RegularExpressions.Regex.IsMatch(pair.Value, "[<>&\'\"]+"))
                    sb.Append("<" + pair.Key + "><![CDATA[" + pair.Value + "]]></" + pair.Key + ">"); // 注：参数值用XML转义即可，CDATA标签用于说明数据不被XML解析器解析。
                else
                    sb.Append("<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">");
            }

            if(string.IsNullOrWhiteSpace(RootTag))
                return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + sb.ToString();
            else
                return $"<?xml version=\"1.0\" encoding=\"utf-8\"?><{RootTag}>{sb.ToString()}</{RootTag}>";
        }

	}
}
