using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Concurrent;
using System.Runtime.Serialization;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// Xml助手
    /// </summary>
    public static class XmlHelper
    {
        #region XML序列化
        /// <summary>
        /// 将对象序列化成xml字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象实例</param>
        /// <returns>xml字符串</returns>
        public static string Serialize<T>(T o)
        {
            if(o == null) return string.Empty;
            string result = string.Empty;
            try
            {
                using(MemoryStream memoryStream = new MemoryStream())
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", ""); //Add an empty namespace and empty value
                    XmlWriterSettings writerSet = new XmlWriterSettings()
                    {
                        CloseOutput = false,
                        Encoding = System.Text.Encoding.UTF8,
                        OmitXmlDeclaration = true, //忽略XML声明 <?xml version="1.0" encoding="utf-8"?>
                        Indent = false
                    };
                    XmlWriter writer = XmlWriter.Create(memoryStream, writerSet);
                    XmlSerializer formatter = new XmlSerializer(typeof(T));
                    formatter.Serialize(writer, o, ns);
                    writer.Close();

                    memoryStream.Position = 0;
                    StreamReader reader = new StreamReader(memoryStream);
                    result = reader.ReadToEnd();
                    memoryStream.Close();
                }
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
            return result;
        }

        /// <summary>
        /// 将对象序列化成xml字符串
        /// </summary>
        /// <param name="o">对象实例</param>
        /// <returns>xml字符串</returns>
        public static string Serialize(object o)
        {
            if(o == null || o == System.DBNull.Value) return string.Empty;
            string result = string.Empty;
            using(MemoryStream memoryStream = new MemoryStream())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", ""); //Add an empty namespace and empty value
                XmlWriterSettings writerSet = new XmlWriterSettings()
                {
                    CloseOutput = false,
                    Encoding = System.Text.Encoding.UTF8,
                    OmitXmlDeclaration = true, //忽略XML声明 <?xml version="1.0" encoding="utf-8"?>
                    Indent = false
                };
                XmlWriter writer = XmlWriter.Create(memoryStream, writerSet);
                XmlSerializer formatter = new XmlSerializer(o.GetType());
                formatter.Serialize(writer, o, ns);
                writer.Close();

                memoryStream.Position = 0;
                StreamReader reader = new StreamReader(memoryStream);
                result = reader.ReadToEnd();
                memoryStream.Close();
            }
            return result;
        }

        /// <summary>
        /// 将对象序列化到指定xml文件
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="o">对象实例</param>
        /// <param name="filePath">完整xml文件路径</param>
        public static void Serialize<T>(T o, string filePath)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", ""); //Add an empty namespace and empty value
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                StreamWriter sw = new StreamWriter(filePath, false);
                formatter.Serialize(sw, o, ns);
                sw.Flush();
                sw.Close();
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion

        #region XML反序列化
        /// <summary>
        /// 将xml字符串反序列化成对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xmlText">xml字符串文本</param>
        /// <returns>对象</returns>
        public static T DeSerialize<T>(string xmlText)
        {
            try
            {
                TextReader textReader = new StringReader(xmlText);
                XmlSerializer formatter = new XmlSerializer(typeof(T), string.Empty);
                T o = (T)formatter.Deserialize(textReader);
                textReader.Close();
                textReader.Dispose();
                return o;
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// 将指定的xml文件反序列化成对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="filePath">完整xml文件路径</param>
        /// <returns>对象</returns>
        public static T DeSerializeByPath<T>(string filePath)
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                StreamReader sr = new StreamReader(filePath);
                T o = (T)formatter.Deserialize(sr);
                sr.Close();
                return o;
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion

        

        #region 高性能XML序列化方案，专门处理Dictionary<string, string>并支持CDATA标记。

        /// <summary>
        /// 高性能序列化Dictionary到XML（自动处理CDATA）
        /// </summary>
        public static string Serialize(IDictionary<string, string> dictionary, string RootElementName)
        {
            if(dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            var settings = new XmlWriterSettings
            {
                Indent = false,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Document
            };

            using(var stringWriter = new StringWriterWithCapacity(EstimateCapacity(dictionary)))
            using(var writer = XmlWriter.Create(stringWriter, settings))
            {
                writer.WriteStartElement(RootElementName);

                foreach(var kvp in dictionary)
                {
                    //writer.WriteStartElement("Item");
                    //writer.WriteAttributeString("Key", kvp.Key);
                    writer.WriteStartElement(kvp.Key);

                    if(NeedCData(kvp.Value))
                    {
                        writer.WriteCData(kvp.Value);
                    }
                    else
                    {
                        writer.WriteString(kvp.Value ?? string.Empty);
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Flush();

                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// 高性能反序列化XML到Dictionary
        /// </summary>
        public static Dictionary<string, string> Deserialize(string xml, string rootElementName)
        {
            if(string.IsNullOrWhiteSpace(xml)) throw new ArgumentNullException(nameof(xml));

            var dictionary = new Dictionary<string, string>();
            bool rootElementFound = false;

            using(var stringReader = new StringReader(xml))
            using(var reader = XmlReader.Create(stringReader, new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
                DtdProcessing = DtdProcessing.Prohibit
            }))
            {
                while(reader.Read())
                {
                    // 检查根元素
                    if(reader.NodeType == XmlNodeType.Element && reader.Name == rootElementName)
                    {
                        rootElementFound = true;
                        continue;
                    }

                    // 只有找到根元素后才处理子元素
                    if(rootElementFound && ((reader.NodeType == XmlNodeType.Element && !string.IsNullOrWhiteSpace(reader.Name)) || reader.Name == "err_code"))
                    {
                        //string key = reader.GetAttribute("Key");
                        string key = reader.Name;

                        string value = string.Empty;

                        // 移动到内容节点
                        if(reader.Read())
                        {
                            switch(reader.NodeType)
                            {
                                case XmlNodeType.CDATA:
                                    value = reader.Value;
                                    break;
                                case XmlNodeType.Text:
                                    value = reader.Value;
                                    break;
                                case XmlNodeType.EndElement:
                                    // 空元素情况
                                    value = string.Empty;
                                    break;
                                default:
                                    // 意外节点类型
                                    break;
                            }

                            // 移动到结束标签
                            reader.Read();
                        }

                        if(key != null && key != rootElementName)
                        {
                            dictionary[key] = value;
                        }
                    }

                    // 如果遇到根元素的结束标签，停止处理
                    if(reader.NodeType == XmlNodeType.EndElement && reader.Name == rootElementName)
                    {
                        break;
                    }

                }
            }

            return dictionary;
        }

        private static bool NeedCData(string value)
        {
            if(string.IsNullOrEmpty(value)) return false;

            // 检查是否包含需要CDATA的特殊字符
            foreach(char c in value)
            {
                /*
                if(string.IsNullOrWhiteSpace(pair.Key) || string.IsNullOrWhiteSpace(pair.Value)) continue;
                if(System.Text.RegularExpressions.Regex.IsMatch(pair.Value, "[<>&\'\"]+"))
                    sb.Append("<" + pair.Key + "><![CDATA[" + pair.Value + "]]></" + pair.Key + ">"); // 注：参数值用XML转义即可，CDATA标签用于说明数据不被XML解析器解析。
                else
                    sb.Append("<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">");
                */

                if(c == '<' || c == '>' || c == '&' || c == '\'' || c == '\"')
                {
                    return true;
                }
            }
            return false;
        }

        private static int EstimateCapacity(IDictionary<string, string> dict)
        {
            // 预估初始容量减少内存分配
            int capacity = 100; // 基础XML结构长度
            foreach(var kvp in dict)
            {
                capacity += 50; // 每个元素的标签开销
                capacity += kvp.Key?.Length ?? 0;
                capacity += kvp.Value?.Length ?? 0;
            }
            return capacity;
        }

        // 高性能StringWriter，预设容量
        private sealed class StringWriterWithCapacity : StringWriter
        {
            public StringWriterWithCapacity(int capacity) : base(new StringBuilder(capacity)) { }
        }

        #endregion

        #region HttpRequest接收XML

        /// <summary>
        /// 接收request.InputStream，并转化为字符串
        /// </summary>
        /// <param name="request">System.Web.HttpRequestBase</param>
        /// <param name="charset">编码格式</param>
        /// <returns>XML</returns>
        public static string receiveRequestInputStream(System.Web.HttpRequestBase request, System.Text.Encoding charset)
        {
            try
            {
                using(var reader = new StreamReader(request.InputStream)) // 注意：InputStream是只读，且只能读1次。读过了就销毁了。
                {
                    string xml = reader.ReadToEnd();
                    return xml;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 接收request.InputStream，并转化为XML，调用示例：System.Xml.XmlDocument xml = WeChatHelper4Net.XmlHelper.GetXML(Request, Request.ContentEncoding)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static System.Xml.XmlDocument receiveRequestXML(System.Web.HttpRequestBase request, System.Text.Encoding charset)
        {
            try
            {
                using(var reader = new StreamReader(request.InputStream)) // 注意：InputStream是只读，且只能读1次。读过了就销毁了。
                {
                    string inputstr = reader.ReadToEnd();
                    System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
                    xml.LoadXml(inputstr);
                    return xml;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 接收request.InputStream，并转化为实体（DataContractSerializer），调用示例：System.Xml.XmlDocument xml = WeChatHelper4Net.XmlHelper.GetXML(Request, Request.ContentEncoding)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static T receiveEntityByRequestXML<T>(System.Web.HttpRequestBase request, System.Text.Encoding charset)
        {
            try
            {
                using(var reader = new StreamReader(request.InputStream)) // 注意：InputStream是只读，且只能读1次。读过了就销毁了。
                {
                    string xml = reader.ReadToEnd();
                    return DeSerialize<T>(xml);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }


}
