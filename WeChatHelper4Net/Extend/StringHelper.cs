using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.IO;

namespace WeChatHelper4Net.Extend
{
    /// <summary>
    /// 用于进行字符串处理的类
    /// </summary>
    public class StringHelper
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public StringHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region 获取字符串的实际字节长度的方法
        /// <summary>
        /// 获取字符串的实际字节长度的方法
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns>实际长度</returns>
        public static int GetRealLength(string source)
        {
            return Encoding.Default.GetByteCount(source);
        }
        #endregion

        #region 截取字符串的方法
        /// <summary>
        /// 按字节数截取字符串的方法
        /// </summary>
        /// <param name="source">要截取的字符串</param>
        /// <param name="NumberOfBytes">要截取的字节数</param>
        /// <param name="needEndDot">是否需要结尾的省略号</param>
        /// <returns>截取后的字符串</returns>
        public static string SubString(string source, int NumberOfBytes, bool needEndDot)
        {
            string temp = string.Empty;
            if(GetRealLength(source) <= NumberOfBytes)
            {
                return source;
            }
            else
            {
                int t = 0;
                char[] q = source.ToCharArray();
                for(int i = 0; i < q.Length && t < NumberOfBytes; i++)
                {
                    if((int)q[i] > 127)
                    {
                        if(t == (NumberOfBytes - 1))
                            break;
                        temp += q[i];
                        t += 2;
                    }
                    else
                    {
                        temp += q[i];
                        t++;
                    }
                }
                if(needEndDot)
                    temp += "...";
                return temp;
            }
        }
        /// <summary>
        /// 按字节数截取字符串的方法
        /// </summary>
        /// <param name="source">要截取的字符串（可空）</param>
        /// <param name="NumberOfBytes">要截取的字节数</param>
        /// <param name="suffix">结果字符串的后缀（超出部分显示为该后缀）</param>
        /// <returns></returns>
        public static string SubString(string source, int NumberOfBytes, string suffix = "...")
        {
            if(string.IsNullOrWhiteSpace(source) || NumberOfBytes < 1) return string.Empty;
            string temp = string.Empty;
            if(GetRealLength(source) <= NumberOfBytes)
            {
                return source;
            }
            else
            {
                int t = 0;
                char[] q = source.ToCharArray();
                for(int i = 0; i < q.Length && t < NumberOfBytes; i++)
                {
                    if((int)q[i] > 127)
                    {
                        if(t == (NumberOfBytes - 1))
                            break;
                        temp += q[i];
                        t += 2;
                    }
                    else
                    {
                        temp += q[i];
                        t++;
                    }
                }
                if(!string.IsNullOrWhiteSpace(suffix))
                    temp += suffix;
                return temp;
            }
        }

        /// <summary>
        /// 截取字符串的方法（按正常字符串Length计算）
        /// </summary>
        /// <param name="source">要截取的字符串（可空）</param>
        /// <param name="length">要截取的字数</param>
        /// <param name="suffix">结果字符串的后缀（超出部分显示为该后缀）</param>
        /// <returns></returns>
        public static string SubStringNatural(string source, int length, string suffix = "...")
        {
            if(string.IsNullOrWhiteSpace(source) || length < 1) return string.Empty;
            return source.Length > length ? (source.Substring(0, length) + (string.IsNullOrWhiteSpace(suffix) ? "" : suffix)) : source;
        }
        #endregion

        #region 移除小数点后末尾0的方法
        /// <summary>
        /// 移除小数点后末尾0的方法
        /// </summary>
        /// <param name="decimals">包含小数点的字符串</param>
        /// <returns></returns>
        public static string TrimEndZeroOfDecimals(string decimals)
        {
            while(decimals.EndsWith("0") && decimals.IndexOf(".") > 0)
            {
                decimals = decimals.TrimEnd('0');
            }
            if(decimals.EndsWith("."))
                decimals = decimals.TrimEnd('.'); //decimals.Substring(0, decimals.Length - 1);
            return decimals;
        }
        #endregion

        #region 截取规定小数点后位数的方法
        /// <summary>
        /// 截取规定小数点后位数的方法
        /// </summary>
        /// <param name="objDecimal">截取前的小数对象</param>
        /// <param name="length">要截取的小数位长度</param>
        /// <returns>截取后的小数字符串</returns>
        public static string SubSpecialLengthDecimal(object objDecimal, int length)
        {
            decimal strDecimal = Convert.ToDecimal(objDecimal);
            return strDecimal.ToString("f" + length);
        }
        #endregion

        #region 日期与时间
        /// <summary>
        /// 获取8位当前日期字符串的方法
        /// </summary>
        /// <returns>8位当前日期字符串</returns>
        public static string GetCurrentDateString()
        {
            DateTime now = DateTime.Now;
            return string.Concat(now.Year, GetSpecialNumericString(now.Month, 2), GetSpecialNumericString(now.Day, 2));
        }

        /// <summary>
        /// 获取6位当前日期字符串的方法
        /// </summary>
        /// <returns>6位当前日期字符串</returns>
        public static string GetSmallCurrentDateString()
        {
            string currentDateString = GetCurrentDateString();
            return currentDateString.Substring(2);
        }

        /// <summary>
        /// 将日期字符串转换成日期时间（例如：20091225091010）
        /// </summary>
        /// <param name="time">日期字符串，例如：20091225091010</param>
        /// <returns></returns>
        public static DateTime GetTime(string time)
        {
            if(!string.IsNullOrEmpty(time) && time.Length > 4)
            {
                StringBuilder t = new StringBuilder();
                if(time.Length >= 14)
                {
                    t.AppendFormat("{0}-{1}-{2} {3}:{4}:{5}"
                        , time.Substring(0, 4)
                        , time.Substring(4, 2)
                        , time.Substring(6, 2)
                        , time.Substring(8, 2)
                        , time.Substring(10, 2)
                        , time.Substring(12, 2)
                        );
                }
                else if(time.Length >= 12)
                {
                    t.AppendFormat("{0}-{1}-{2} {3}:{4}:00"
                        , time.Substring(0, 4)
                        , time.Substring(4, 2)
                        , time.Substring(6, 2)
                        , time.Substring(8, 2)
                        , time.Substring(10, 2)
                        );
                }
                else if(time.Length >= 10)
                {
                    t.AppendFormat("{0}-{1}-{2} {3}:00:00"
                        , time.Substring(0, 4)
                        , time.Substring(4, 2)
                        , time.Substring(6, 2)
                        , time.Substring(8, 2)
                        );
                }
                else if(time.Length >= 8)
                {
                    t.AppendFormat("{0}-{1}-{2}"
                        , time.Substring(0, 4)
                        , time.Substring(4, 2)
                        , time.Substring(6, 2)
                        );
                }
                else if(time.Length >= 6)
                {
                    t.AppendFormat("{0}-{1}"
                        , time.Substring(0, 4)
                        , time.Substring(4, 2)
                        );
                }

                return Convert.ToDateTime(t.ToString());
            }
            return new DateTime(1970, 1, 1);
        }

        /// <summary>
        /// 获取用户友好的日期格式
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>字符串</returns>
        public static string GetHumanFriendDate(DateTime dt, bool isLongTime = true)
        {
            TimeSpan ts = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1) - dt;
            TimeSpan totalSpan = DateTime.Now - dt;
            string dateView = "";
            switch(ts.Days)
            {
                case 0:
                    if(totalSpan.TotalMinutes <= 0)
                    {
                        dateView = "刚刚";
                    }
                    else if(totalSpan.TotalMinutes <= 1)
                    {
                        dateView = totalSpan.Seconds + "秒钟前";
                    }
                    else if(totalSpan.TotalHours <= 1)
                    {
                        dateView = totalSpan.Minutes + "分钟前";
                    }
                    else
                    {
                        dateView = totalSpan.Hours + "小时前";
                    }
                    break;
                case 1:
                    dateView = dt.ToString("昨天 HH:mm");
                    break;
                case 2:
                    dateView = dt.ToString("前天 HH:mm");
                    break;
                default:
                    if(!isLongTime)
                    {
                        dateView = dt.ToString("MM-dd");
                    }
                    else
                    {
                        dateView = dt.ToString("yyyy-MM-dd HH:mm");
                    }
                    break;
            }
            return dateView;
        }
        #endregion

        #region 获取指定长度数字字符串的方法，不足位数用0填充
        /// <summary>
        /// 获取指定长度数字字符串的方法，不足位数用0填充
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="length">指定的长度</param>
        /// <returns>指定长度数字字符串</returns>
        public static string GetSpecialNumericString(int number, int length)
        {
            return number.ToString("d" + length);
        }
        #endregion

        #region 移除数字字符串开始0的方法
        /// <summary>
        /// 移除数字字符串开始0的方法
        /// </summary>
        /// <param name="source">移除前的字符串</param>
        /// <returns>移除后的字符串</returns>
        public static string TrimStartZero(string source)
        {
            while(source.StartsWith("0"))
            {
                source = source.Substring(1);
            }
            return source;
        }
        #endregion

        #region 转换字符串编码的方法
        /// <summary>
        /// 转换字符串编码的方法
        /// </summary>
        /// <param name="dstEncoding">转换后的编码格式</param>
        /// <param name="s">要进行转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ConvertEncoding(Encoding dstEncoding, string s)
        {
            return ConvertEncoding(Encoding.Default, dstEncoding, s);
        }
        /// <summary>
        /// 转换字符串编码的方法
        /// </summary>
        /// <param name="srcEncoding">转换前的编码格式</param>
        /// <param name="dstEncoding">转换后的编码格式</param>
        /// <param name="s">要进行转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ConvertEncoding(Encoding srcEncoding, Encoding dstEncoding, string s)
        {
            byte[] bytes = Encoding.Default.GetBytes(s);
            bytes = Encoding.Convert(srcEncoding, dstEncoding, bytes);
            return Encoding.Default.GetString(bytes);
        }
        #endregion

        #region 将全角字符串转成半角字符串的方法
        /// <summary>
        /// 将全角字符串转成半角字符串的方法
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns>半角字符串</returns>
        public static string ConvertDbcToSbcString(string source)
        {
            StringBuilder sb = new StringBuilder();
            char[] c = source.ToCharArray();
            for(int i = 0; i < c.Length; i++)
            {
                sb.Append(ConvertDbcToSbcChar(c[i]));
            }
            return sb.ToString();
        }
        #endregion

        #region 将字符串数组转成逗号分割字符串的方法
        /// <summary>
        /// 将字符串数组转成逗号分割字符串的方法
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <returns>逗号分割字符串</returns>
        public static string ConvertStringArrayToStrings(string[] strArray)
        {
            if(strArray == null)
            {
                return "";
            }

            string result = "";
            for(int i = 0; i < strArray.Length; i++)
            {
                result += strArray[i] + ",";
            }
            return result.TrimEnd(new char[] { ',' });
        }
        #endregion

        #region 对字符串进行Url编码的方法
        /// <summary>
        /// 对字符串进行Url编码的方法
        /// </summary>
        /// <param name="s">要进行Url编码的字符串</param>
        /// <returns>Url编码后的字符串</returns>
        public static string UrlEncode(string s)
        {
            return UrlEncode(s, Encoding.Default);
        }
        /// <summary>
        /// 对字符串进行Url编码的方法
        /// </summary>
        /// <param name="s">要进行Url编码的字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>Url编码后的字符串</returns>
        public static string UrlEncode(string s, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = encoding.GetBytes(s);
            for(int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
            return (sb.ToString());
        }
        #endregion

        #region 验证字符串是否为空
        /// <summary>
        /// 验证字符串是否为空
        /// </summary>
        /// <param name="str">被判断的字符串</param>
        /// <returns>bool值</returns>
        public static bool IsEmptyString(string str)
        {
            if(str == null || str.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 过滤脚本攻击:sql注入,跨站脚本,flash嵌入

        /// <summary>
        /// 过滤字符串中注入SQL脚本的方法
        /// </summary>
        /// <param name="source">传入的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string SqlFilter(string source)
        {
            //半角括号替换为全角括号
            source = source.Replace("'", "''").Replace(";", "；").Replace("(", "（").Replace(")", "）");

            //去除执行SQL语句的命令关键字
            source = Regex.Replace(source, "select", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "insert", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "update", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "delete", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "drop", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "truncate", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "declare", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "xp_cmdshell", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "/add", "", RegexOptions.IgnoreCase);
            //Regex.Replace(source, "asc(", "", RegexOptions.IgnoreCase);
            //Regex.Replace(source, "mid(", "", RegexOptions.IgnoreCase);
            //Regex.Replace(source, "char(", "", RegexOptions.IgnoreCase);
            //Regex.Replace(source, "count(", "", RegexOptions.IgnoreCase);
            //fetch 
            //IS_SRVROLEMEMBER
            //Cast(

            source = Regex.Replace(source, "net user", "", RegexOptions.IgnoreCase);

            //去除执行存储过程的命令关键字 
            source = Regex.Replace(source, "exec", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "execute", "", RegexOptions.IgnoreCase);

            //去除系统存储过程或扩展存储过程关键字
            source = Regex.Replace(source, "xp_", "x p_", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "sp_", "s p_", RegexOptions.IgnoreCase);

            //防止16进制注入
            source = Regex.Replace(source, "0x", "0 x", RegexOptions.IgnoreCase);

            return source;
        }

        /// <summary>
        /// 过滤字符串中的注入跨站脚本(先进行UrlDecode再过滤脚本关键字)
        /// </summary>
        /// <param name="source">需要过滤的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string XSSFilter(string source)
        {
            if(source == "") return source;

            string result = HttpUtility.UrlDecode(source);

            string replaceEventStr = " onXXX =";
            string tmpStr = "";

            string patternGeneral = @"<[^<>]*>";                              //例如 <abcd>
            string patternEvent = @"([\s]|[:])+[o]{1}[n]{1}\w*\s*={1}";     // 空白字符或: on事件=
            string patternScriptBegin = @"\s*((javascript)|(vbscript))\s*[:]?";  // javascript或vbscript:
            string patternScriptEnd = @"<([\s/])*script.*>";                       // </script>   
            string patternTag = @"<([\s/])*\S.+>";                       // 例如</textarea>

            Regex regexGeneral = new Regex(patternGeneral, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex regexEvent = new Regex(patternEvent, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex regexScriptEnd = new Regex(patternScriptEnd, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regexScriptBegin = new Regex(patternScriptBegin, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regexTag = new Regex(patternTag, RegexOptions.Compiled | RegexOptions.IgnoreCase);


            Match matchGeneral, matchEvent, matchScriptEnd, matchScriptBegin, matchTag;

            //符合类似 <abcd> 条件的
            #region 符合类似 <abcd> 条件的
            //过滤字符串中的 </script>   
            for(matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchScriptEnd = regexScriptEnd.Match(tmpStr);
                if(matchScriptEnd.Success)
                {
                    tmpStr = regexScriptEnd.Replace(tmpStr, " ");
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            //过滤字符串中的脚本事件
            for(matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchEvent = regexEvent.Match(tmpStr);
                if(matchEvent.Success)
                {
                    tmpStr = regexEvent.Replace(tmpStr, replaceEventStr);
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            //过滤字符串中的 javascript或vbscript:
            for(matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchScriptBegin = regexScriptBegin.Match(tmpStr);
                if(matchScriptBegin.Success)
                {
                    tmpStr = regexScriptBegin.Replace(tmpStr, " ");
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            #endregion

            //过滤字符串中的事件 例如 onclick --> onXXX
            for(matchEvent = regexEvent.Match(result); matchEvent.Success; matchEvent = matchEvent.NextMatch())
            {
                tmpStr = matchEvent.Groups[0].Value;
                tmpStr = regexEvent.Replace(tmpStr, replaceEventStr);
                result = result.Replace(matchEvent.Groups[0].Value, tmpStr);
            }

            //过滤掉html标签，类似</textarea>
            for(matchTag = regexTag.Match(result); matchTag.Success; matchTag = matchTag.NextMatch())
            {
                tmpStr = matchTag.Groups[0].Value;
                tmpStr = regexTag.Replace(tmpStr, "");
                result = result.Replace(matchTag.Groups[0].Value, tmpStr);
            }


            return result;
        }

        /// <summary>
        /// 过滤字符串中注入Flash代码
        /// </summary>
        /// <param name="htmlCode">输入字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string FlashFilter(string htmlCode)
        {
            string pattern = @"\w*<OBJECT\s+.*(macromedia)[\s*|\S*]{1,1300}</OBJECT>";

            return Regex.Replace(htmlCode, pattern, "", RegexOptions.Multiline);
        }

        #endregion

        #region 移除Html标签的方法
        /// <summary>
        /// 移除html标记
        /// </summary>
        /// <param name="source">移除Html标签之前的字符串</param>
        /// <returns>移除Html标签之后的字符串</returns>
        public static string RemoveHtmlTag(string source)
        {
            return Regex.Replace(source, "<[^>]*>", "");
        }

        /// <summary>
        /// 清理URL字符串#及其后面部分 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CleanUrl(string url)
        {
            if(string.IsNullOrWhiteSpace(url)) return string.Empty;
            return Replace(url, @"(#.*|&[^=]*)$", ""); //清理url结尾的#……与以及多余的&
        }
        #endregion

        #region 读取指定URL的内容
        /// <summary>
        /// 读取指定URL的内容
        /// </summary>
        /// <param name="URL">指定URL</param>
        /// <param name="Content">该URL包含的内容</param>
        /// <returns>读取URL的状态</returns>
        public static string ReadHttp(string URL, ref string Content)
        {
            string status = "ERROR";
            HttpWebRequest Webreq = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse Webresp = null;
            StreamReader strm = null;
            try
            {
                Webresp = (HttpWebResponse)Webreq.GetResponse();
                status = Webresp.StatusCode.ToString();
                strm = new StreamReader(Webresp.GetResponseStream(), Encoding.GetEncoding("GB2312"));
                Content = strm.ReadToEnd();
            }
            catch
            {
            }
            finally
            {
                if(Webresp != null) Webresp.Close();
                if(strm != null) strm.Close();
            }
            return (status);
        }
        #endregion

        #region 把浮点类型格式化为百分比字符串
        /// <summary>
        /// 把浮点类型格式化为百分比字符串
        /// </summary>
        /// <param name="num">浮点类型数据</param>
        /// <returns>百分比字符串</returns>
        public static string FormatDoubleToPercent(double num)
        {
            return (num * 100).ToString("#0.00") + "%";
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 将全角字符转成半角字符的方法
        /// </summary>
        /// <param name="c">转换前的字符</param>
        /// <returns>半角字符</returns>
        private static char ConvertDbcToSbcChar(char c)
        {
            //得到c的编码
            byte[] bytes = Encoding.Unicode.GetBytes(c.ToString());

            int H = Convert.ToInt32(bytes[1]);
            int L = Convert.ToInt32(bytes[0]);

            //得到unicode编码
            int value = H * 256 + L;

            //是全角
            if(value >= 65281 && value <= 65374)
            {
                int halfvalue = value - 65248;//65248是全半角间的差值。
                byte halfL = Convert.ToByte(halfvalue);

                bytes[0] = halfL;
                bytes[1] = 0;
            }
            else if(value == 12288)
            {
                int halfvalue = 32;
                byte halfL = Convert.ToByte(halfvalue);

                bytes[0] = halfL;
                bytes[1] = 0;
            }
            else
            {
                return c;
            }

            //将bytes转换成字符
            string ret = Encoding.Unicode.GetString(bytes);

            return Convert.ToChar(ret);
        }
        #endregion

        /// <summary>
        /// 替换掉字符串右边若干长度字符串
        /// （用于修改微信头像地址尺寸）
        /// </summary>
        /// <param name="text">原字符串</param>
        /// <param name="lastLength">从右边开始的长度</param>
        /// <param name="newText">新的字符串末尾</param>
        /// <returns></returns>
        public static string ReplaceRightChar(string text, int lastLength, string newText)
        {
            if(string.IsNullOrEmpty(text)) return string.Empty;
            text.Remove(text.Length - lastLength);
            return string.Concat(text, newText);
        }

        /// <summary>
        /// 获取参数字符串的指定值（例如：QK_QKDetail_jid-00029_cname-中国_issueid-0002920140806）
        /// </summary>
        /// <param name="text">源文本</param>
        /// <param name="key">要获取的键</param>
        /// <param name="separatorItem">参数项之间的分割符（例如：_）</param>
        /// <param name="separatorValue">键值对之间的分隔符（例如：-）</param>
        /// <returns></returns>
        public static string GetValue(string text, string key, char separatorItem, char separatorValue)
        {
            if(!string.IsNullOrEmpty(text))
            {
                string[] arr = text.Split(new Char[] { separatorItem });
                if(arr.Length > 0)
                {
                    string[] arr2 = { };
                    foreach(string s in arr)
                    {
                        if(s.IndexOf(key) > -1)
                        {
                            arr2 = s.Split(new Char[] { separatorValue });
                            if(arr2.Length == 2)
                                return arr2[1];
                            else
                                return string.Empty;
                        }
                    }
                }
            }
            return string.Empty;
        }

        #region 正则表达式&字符串清理
        /// <summary>
        /// 正则表达式验证匹配（不区分大小写）
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 正则表达式替换字符串（不区分大小写）
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string Replace(string input, string pattern, string replacement)
        {
            if(string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern) || replacement == null) return string.Empty;
            return Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 正则表达式分割字符串（不区分大小写）
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string[] Split(string input, string pattern)
        {
            string[] arrayString = Regex.Split(input, pattern, RegexOptions.IgnoreCase);
            List<string> _arrayString = new List<string>();
            foreach(string s in arrayString)
            {
                if(!string.IsNullOrWhiteSpace(s) && !Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase))
                    _arrayString.Add(s);
                else
                    continue;
            }
            return _arrayString.ToArray();
        }

        #region 清理字符串
        /// <summary>
        /// 移除字符串中所有空格，制表符，回车，换行，分页符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpace(string str)
        {
            return Regex.Replace(str, @"\s", ""); //移除空格，制表符，回车，换行，分页符
        }
        /// <summary>
        /// 移除URL指定参数键及参数值
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="key">参数键(将移除该键值对)</param>
        /// <returns></returns>
        public static string RemoveUrlParam(string url, string key)
        {
            if(string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key)) return url;
            string _url = Replace(url, @"(?<=(^|\?|&))" + key + @"\=[^&#]*", "[@@@]"); // @"(?<=(^|\?|&))state\=[^&#]*"
            _url = _url.Replace("&[@@@]", "").Replace("[@@@]", "");
            return Replace(_url, @"\?+$", "");
        }
        /// <summary>
        /// 清除字符串中的换行，分段，分页，制表符等
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearString(string str)
        {
            if(string.IsNullOrEmpty(str)) return string.Empty;
            return str
                .Replace("^|", "")
                .Replace("^p", "")
                .Replace("^t", "")
                .Replace("^m", "")
                .Replace("\n", "") //换行 0x000A
                .Replace("\r", "") //回车 0x000D
                .Replace("\f", "") //换页 0x000C
                .Replace("\t", "") //水平制表符 0x0009
                .Replace("\v", "") //垂直制表符 0x000B
                .Trim();
        }

        /// <summary>
        /// 转义用于拼接JSON的字符串中特殊字符（转义处理双引号，正斜杠，反斜杠，换行符号等）
        /// </summary>
        /// <param name="text">JSON中数据文本</param>
        /// <returns>转义后的JSON中数据文本方便反序列化</returns>
        public static string EscapeToJson(string text)
        {
            if(string.IsNullOrWhiteSpace(text)) return string.Empty;
            StringBuilder sb = new StringBuilder();
            Char[] stringArray = text.ToCharArray();
            for(long i = 0; i < stringArray.LongLength; i++)
            {
                switch(stringArray[i])
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\v':
                        sb.Append("\\v");
                        break;
                    default:
                        sb.Append(stringArray[i]);
                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 移除字符串中所有单引号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSingleQuotes(string str)
        {
            if(string.IsNullOrEmpty(str)) return string.Empty;
            return str.Replace("'", "");
        }

        /// <summary>
        /// 清理字符串中的正则表达式符号 (\ ^ $ * + ? { } . ( ) : = ! [ ] | -)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ClearRegex(string text)
        {
            text = Replace(
                Replace(
                    Replace(
                        Replace(
                            Replace(
                                Replace(
                                    Replace(
                                        Replace(
                                            text
                                            , @"(\,)", "，")
                                        , @"(\.)", "。")
                                    , @"(\()", "（")
                                , @"(\))", "）")
                            , @"(\:)", "：")
                        , @"(\!)", "！")
                    , @"(\[)", "【")
                , @"(\])", "】");
            return Replace(text, @"(\\|\^|\$|\*|\+|\?|\{|\}|\,|\.|\(|\)|\:|\=|\!|\[|\]|\||\-)", "");
        }
        #endregion

        /// <summary>
        /// 移除特殊字符，保留Unicode字符，字母，数字，汉字，标点符号，分隔符，符号，空白等
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveSpecialChar(string text)
        {
            if(string.IsNullOrWhiteSpace(text)) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach(char c in text)
            {
                if((!char.IsLetter(c)) && (!char.IsNumber(c)) && !char.IsPunctuation(c) && (!char.IsSeparator(c)) && (!char.IsSymbol(c)) && (!char.IsWhiteSpace(c))) continue;
                sb.Append(c);
            }
            return sb.ToString();
        }
        #endregion

        #region 常用的格式匹配
        /// <summary>
        /// 是否匹配Guid格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMatchGuid(string input)
        {
            if(!string.IsNullOrWhiteSpace(input))
                return Regex.IsMatch(input, @"^[a-zA-Z0-9]{8}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{12}$", RegexOptions.IgnoreCase);
            else
                return false;
        }

        /// <summary>
        /// 是否匹配 http、https格式的URL（不包括中文及特殊字符，需要URL编码）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMatchHTTP(string input)
        {
            if(!string.IsNullOrWhiteSpace(input))
                return Regex.IsMatch(input, @"^(http|https)://([\w\-]+\.)+[\w-]+(:[0-9]{1,4})?(/[\w\-./?%&=#]*)?$", RegexOptions.IgnoreCase);
            else
                return false;
        }
        /// <summary>
        /// 是否匹配物理磁盘路径，如E:\\iisroot格式的URL（包括中文）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMatchDiskPath(string input)
        {
            if(!string.IsNullOrWhiteSpace(input))
                return Regex.IsMatch(input, @"^[a-zA-Z]:\\([0-9a-zA-Z\u4e00-\u9fa5\\]*)$", RegexOptions.IgnoreCase);
            else
                return false;
        }
        /// <summary>
        /// 严格匹配IP格式（不包括0.0.0.0，最大上限是255.255.255.255）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMatchIP(string input)
        {
            if(!string.IsNullOrWhiteSpace(input))
                return Regex.IsMatch(input, @"^((((1\d{2})|(2[0-4]\d)|(25[0-5]))|([1-9]\d)|([1-9]{1}))\.){3}(((1\d{2})|(2[0-4]\d)|(25[0-5]))|([1-9]\d)|([0-9]{1}))$", RegexOptions.IgnoreCase);
            else
                return false;
        }

        #endregion

    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }

}
