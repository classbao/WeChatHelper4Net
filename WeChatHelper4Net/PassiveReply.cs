using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Text;
using WeChatHelper4Net.Models;
using WeChatHelper4Net.Models.PassiveReply;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2016-12-08
 */
namespace WeChatHelper4Net
{
    /// <summary>
	/// 被动回复用户消息
	/// </summary>
	public class PassiveReply
    {
        private PassiveReply() { }

        public static string ReplySuccess() { return "success"; }
        public static string ReplyError() { return "error"; }
        public static string ReplyEmpty() { return ""; }



        public static string ReplyText(ReplyTextModel entity)
        {
            if (null != entity && !string.IsNullOrWhiteSpace(entity.ToUserName))
            {
                string xmlString = XmlHelper.Serialize(entity);
                if (!string.IsNullOrWhiteSpace(xmlString))
                {
                    return xmlString;
                }
            }
            return ReplyEmpty();
        }

        public static string ReplyImage(ReplyImageModel entity)
        {
            if (null != entity && !string.IsNullOrWhiteSpace(entity.ToUserName) && null != entity.Image)
            {
                string xmlString = string.Empty;
                //var s1 = XmlHelper.Serialize(entity);
                #region xmlString
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                System.Xml.XmlNode rootNode;
                System.Xml.XmlElement ele;
                System.Xml.XmlCDataSection cdata;

                #region xml
                rootNode = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, "xml", null);

                ele = xmlDoc.CreateElement(nameof(entity.ToUserName));
                cdata = xmlDoc.CreateCDataSection(entity.ToUserName);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.FromUserName));
                cdata = xmlDoc.CreateCDataSection(entity.FromUserName);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.CreateTime));
                ele.InnerXml = entity.CreateTime.ToString();
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.MsgType));
                cdata = xmlDoc.CreateCDataSection(entity.MsgType);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);
                #endregion

                System.Xml.XmlNode nodeImage;
                nodeImage = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, nameof(entity.Image), null);
                if (null != entity.Image)
                {
                    ele = xmlDoc.CreateElement(nameof(entity.Image.MediaId));
                    cdata = xmlDoc.CreateCDataSection(entity.Image.MediaId);
                    ele.AppendChild(cdata);
                    nodeImage.AppendChild(ele);
                }

                rootNode.AppendChild(nodeImage);
                xmlDoc.AppendChild(rootNode);

                xmlString = xmlDoc.OuterXml;
                #endregion

                if (!string.IsNullOrWhiteSpace(xmlString))
                {
                    return xmlString;
                }
            }
            return ReplyEmpty();
        }

        public static string ReplyVoice(ReplyVoiceModel entity)
        {
            if (null != entity && !string.IsNullOrWhiteSpace(entity.ToUserName) && null != entity.Voice)
            {
                string xmlString = string.Empty;
                //var s1 = XmlHelper.Serialize(entity);
                #region xmlString
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                System.Xml.XmlNode rootNode;
                System.Xml.XmlElement ele;
                System.Xml.XmlCDataSection cdata;

                #region xml
                rootNode = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, "xml", null);

                ele = xmlDoc.CreateElement(nameof(entity.ToUserName));
                cdata = xmlDoc.CreateCDataSection(entity.ToUserName);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.FromUserName));
                cdata = xmlDoc.CreateCDataSection(entity.FromUserName);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.CreateTime));
                ele.InnerXml = entity.CreateTime.ToString();
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.MsgType));
                cdata = xmlDoc.CreateCDataSection(entity.MsgType);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);
                #endregion

                System.Xml.XmlNode nodeVoice;
                nodeVoice = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, nameof(entity.Voice), null);
                if (null != entity.Voice)
                {
                    ele = xmlDoc.CreateElement(nameof(entity.Voice.MediaId));
                    cdata = xmlDoc.CreateCDataSection(entity.Voice.MediaId);
                    ele.AppendChild(cdata);
                    nodeVoice.AppendChild(ele);
                }

                rootNode.AppendChild(nodeVoice);
                xmlDoc.AppendChild(rootNode);

                xmlString = xmlDoc.OuterXml;
                #endregion

                if (!string.IsNullOrWhiteSpace(xmlString))
                {
                    return xmlString;
                }
            }
            return ReplyEmpty();
        }

        public static string ReplyVideo(ReplyVideoModel entity)
        {
            if (null != entity && !string.IsNullOrWhiteSpace(entity.ToUserName) && null != entity.Video)
            {
                string xmlString = string.Empty;
                //var s1 = XmlHelper.Serialize(entity);
                #region xmlString
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                System.Xml.XmlNode rootNode;
                System.Xml.XmlElement ele;
                System.Xml.XmlCDataSection cdata;

                #region xml
                rootNode = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, "xml", null);

                ele = xmlDoc.CreateElement(nameof(entity.ToUserName));
                cdata = xmlDoc.CreateCDataSection(entity.ToUserName);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.FromUserName));
                cdata = xmlDoc.CreateCDataSection(entity.FromUserName);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.CreateTime));
                ele.InnerXml = entity.CreateTime.ToString();
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.MsgType));
                cdata = xmlDoc.CreateCDataSection(entity.MsgType);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);
                #endregion

                System.Xml.XmlNode nodeVideo;
                nodeVideo = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, nameof(entity.Video), null);
                if (null != entity.Video)
                {
                    ele = xmlDoc.CreateElement(nameof(entity.Video.MediaId));
                    cdata = xmlDoc.CreateCDataSection(entity.Video.MediaId);
                    ele.AppendChild(cdata);
                    nodeVideo.AppendChild(ele);

                    ele = xmlDoc.CreateElement(nameof(entity.Video.Title));
                    cdata = xmlDoc.CreateCDataSection(entity.Video.Title);
                    ele.AppendChild(cdata);
                    nodeVideo.AppendChild(ele);

                    ele = xmlDoc.CreateElement(nameof(entity.Video.Description));
                    cdata = xmlDoc.CreateCDataSection(entity.Video.Description);
                    ele.AppendChild(cdata);
                    nodeVideo.AppendChild(ele);
                }

                rootNode.AppendChild(nodeVideo);
                xmlDoc.AppendChild(rootNode);

                xmlString = xmlDoc.OuterXml;
                #endregion

                if (!string.IsNullOrWhiteSpace(xmlString))
                {
                    return xmlString;
                }
            }
            return ReplyEmpty();
        }

        public static string ReplyMusic(ReplyMusicModel entity)
        {
            if (null != entity && !string.IsNullOrWhiteSpace(entity.ToUserName) && null != entity.Music)
            {
                string xmlString = string.Empty;
                //var s1 = XmlHelper.Serialize(entity);
                #region xmlString
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                System.Xml.XmlNode rootNode;
                System.Xml.XmlElement ele;
                System.Xml.XmlCDataSection cdata;

                #region xml
                rootNode = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, "xml", null);

                ele = xmlDoc.CreateElement(nameof(entity.ToUserName));
                cdata = xmlDoc.CreateCDataSection(entity.ToUserName);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.FromUserName));
                cdata = xmlDoc.CreateCDataSection(entity.FromUserName);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.CreateTime));
                ele.InnerXml = entity.CreateTime.ToString();
                rootNode.AppendChild(ele);

                ele = xmlDoc.CreateElement(nameof(entity.MsgType));
                cdata = xmlDoc.CreateCDataSection(entity.MsgType);
                ele.AppendChild(cdata);
                rootNode.AppendChild(ele);
                #endregion

                System.Xml.XmlNode nodeMusic;
                nodeMusic = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, nameof(entity.Music), null);
                if (null != entity.Music)
                {
                    ele = xmlDoc.CreateElement(nameof(entity.Music.Title));
                    cdata = xmlDoc.CreateCDataSection(entity.Music.Title);
                    ele.AppendChild(cdata);
                    nodeMusic.AppendChild(ele);

                    ele = xmlDoc.CreateElement(nameof(entity.Music.Description));
                    cdata = xmlDoc.CreateCDataSection(entity.Music.Description);
                    ele.AppendChild(cdata);
                    nodeMusic.AppendChild(ele);

                    ele = xmlDoc.CreateElement(nameof(entity.Music.MusicUrl));
                    cdata = xmlDoc.CreateCDataSection(entity.Music.MusicUrl);
                    ele.AppendChild(cdata);
                    nodeMusic.AppendChild(ele);

                    ele = xmlDoc.CreateElement(nameof(entity.Music.HQMusicUrl));
                    cdata = xmlDoc.CreateCDataSection(entity.Music.HQMusicUrl);
                    ele.AppendChild(cdata);
                    nodeMusic.AppendChild(ele);

                    ele = xmlDoc.CreateElement(nameof(entity.Music.ThumbMediaId));
                    cdata = xmlDoc.CreateCDataSection(entity.Music.ThumbMediaId);
                    ele.AppendChild(cdata);
                    nodeMusic.AppendChild(ele);
                }

                rootNode.AppendChild(nodeMusic);
                xmlDoc.AppendChild(rootNode);

                xmlString = xmlDoc.OuterXml;
                #endregion

                if (!string.IsNullOrWhiteSpace(xmlString))
                {
                    return xmlString;
                }
            }
            return ReplyEmpty();
        }

        public static string ReplyNews(ReplyNewsModel entity)
        {
            if (null != entity && !string.IsNullOrWhiteSpace(entity.ToUserName) && null != entity.Articles && entity.Articles.list.Count > 0)
            {
                if (entity.Articles.list.Count <= 10)
                {
                    string xmlString = string.Empty;
                    //var s1 = XmlHelper.Serialize(entity);
                    #region xmlString
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    System.Xml.XmlNode rootNode;
                    System.Xml.XmlElement ele;
                    System.Xml.XmlCDataSection cdata;

                    #region xml
                    rootNode = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, "xml", null);

                    ele = xmlDoc.CreateElement(nameof(entity.ToUserName));
                    cdata = xmlDoc.CreateCDataSection(entity.ToUserName);
                    ele.AppendChild(cdata);
                    rootNode.AppendChild(ele);

                    ele = xmlDoc.CreateElement(nameof(entity.FromUserName));
                    cdata = xmlDoc.CreateCDataSection(entity.FromUserName);
                    ele.AppendChild(cdata);
                    rootNode.AppendChild(ele);

                    ele = xmlDoc.CreateElement(nameof(entity.CreateTime));
                    ele.InnerXml = entity.CreateTime.ToString();
                    rootNode.AppendChild(ele);

                    ele = xmlDoc.CreateElement(nameof(entity.MsgType));
                    cdata = xmlDoc.CreateCDataSection(entity.MsgType);
                    ele.AppendChild(cdata);
                    rootNode.AppendChild(ele);
                    #endregion

                    ele = xmlDoc.CreateElement(nameof(entity.ArticleCount));
                    ele.InnerXml = entity.ArticleCount.ToString();
                    rootNode.AppendChild(ele);

                    System.Xml.XmlNode nodeArticles, nodeItem;
                    nodeArticles = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, nameof(entity.Articles), null);
                    if (entity.ArticleCount > 0)
                    {
                        foreach (var i in entity.Articles.list)
                        {
                            nodeItem = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, "item", null);

                            ele = xmlDoc.CreateElement(nameof(i.Title));
                            cdata = xmlDoc.CreateCDataSection(i.Title);
                            ele.AppendChild(cdata);
                            nodeItem.AppendChild(ele);

                            ele = xmlDoc.CreateElement(nameof(i.Description));
                            cdata = xmlDoc.CreateCDataSection(i.Description);
                            ele.AppendChild(cdata);
                            nodeItem.AppendChild(ele);

                            ele = xmlDoc.CreateElement(nameof(i.PicUrl));
                            cdata = xmlDoc.CreateCDataSection(i.PicUrl);
                            ele.AppendChild(cdata);
                            nodeItem.AppendChild(ele);

                            ele = xmlDoc.CreateElement(nameof(i.Url));
                            cdata = xmlDoc.CreateCDataSection(i.Url);
                            ele.AppendChild(cdata);
                            nodeItem.AppendChild(ele);

                            nodeArticles.AppendChild(nodeItem);
                        }
                    }

                    rootNode.AppendChild(nodeArticles);
                    xmlDoc.AppendChild(rootNode);

                    xmlString = xmlDoc.OuterXml;
                    #endregion

                    if (!string.IsNullOrWhiteSpace(xmlString))
                    {
                        return xmlString;
                    }

                }
                else
                    throw new Exception("如果图文数超过10，则将会无响应。详情情参考微信官方文档");
            }
            return ReplyEmpty();
        }

    }
}
