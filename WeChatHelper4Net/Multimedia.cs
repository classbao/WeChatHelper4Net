using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// 多媒体文件
    /// </summary>
    public class Multimedia
    {
        #region 上传/下载多媒体文件
        /// <summary>
        /// 下载保存多媒体文件,返回多媒体保存路径
        /// </summary>
        /// <param name="ACCESS_TOKEN">ACCESS_TOKEN</param>
        /// <param name="MEDIA_ID">MEDIA_ID</param>
        /// <param name="physicalFolder">物理文件夹</param>
        /// <param name="fileName">保存文件名(不需要后缀名)</param>
        /// <param name="fileSize">文件大小</param>
        /// <returns>多媒体保存路径</returns>
        [Obsolete("由于微信接口变更，该方法已过时")]
        private static string DownloadMultimedia(string ACCESS_TOKEN, string MEDIA_ID, string physicalFolder,ref string fileName, out long fileSize)
        {
            //LogHelper.Save("DownloadMultimedia > " + "ACCESS_TOKEN=" + ACCESS_TOKEN + "，MEDIA_ID=" + MEDIA_ID + "，physicalFolder=" + physicalFolder + "，fileName=" + fileName, "DownloadMultimedia", LogType.Common, LogTime.day);
            fileSize = 0;

            string file = string.Empty;
            string content = string.Empty;
            string strpath = string.Empty;
            string savepath = string.Empty;
            string stUrl = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + ACCESS_TOKEN + "&media_id=" + MEDIA_ID;

            string suffixName = string.Empty;

            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(stUrl);

            req.Method = "GET";
            using(System.Net.WebResponse wr = req.GetResponse())
            {
                System.Net.HttpWebResponse myResponse = (System.Net.HttpWebResponse)req.GetResponse();
                strpath = myResponse.ResponseUri.ToString();
                fileSize = myResponse.ContentLength;
                string _filename = Common.Replace(myResponse.GetResponseHeader("Content-disposition"), ".*filename=\"", "").Trim(new Char[] { '"' });
                suffixName = System.IO.Path.GetExtension(_filename).ToLower();

                //LogHelper.Save("接收信息：" + "StatusCode=" + myResponse.StatusCode.ToString() + "，StatusDescription=" + myResponse.StatusDescription + "，ContentType=" + myResponse.ContentType + "，Content-disposition=" + myResponse.GetResponseHeader("Content-disposition") + "，filename=" + _filename + "，suffixName=" + suffixName, "DownloadMultimedia", LogType.Common, LogTime.day);
                physicalFolder = Common.Replace(physicalFolder, @"[\\\s]+$", "") + "\\";
                if(!System.IO.Directory.Exists(physicalFolder))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(physicalFolder);
                    }
                    catch(Exception Ex)
                    {
                        LogHelper.Save("DownloadMultimedia > 创建文件路径失败！physicalFolder=" + physicalFolder, "DownloadMultimedia", LogType.Common, LogTime.day);
                        throw Ex;
                    }
                }
                if(string.IsNullOrWhiteSpace(suffixName))
                {
                    switch(myResponse.ContentType)
                    {
                        case "image/jpeg": suffixName = ".jpg"; break;
                        case "application/x-jpg": suffixName = ".jpg"; break;
                        case "image/png": suffixName = ".png"; break;
                        case "application/x-png": suffixName = ".png"; break;
                        case "audio/amr": suffixName = ".amr"; break;
                        case "audio/mp3": suffixName = ".mp3"; break;
                        case "video/mp4": suffixName = ".mp4"; break;
                        default: suffixName = !string.IsNullOrWhiteSpace(suffixName) ? suffixName : ".jpg"; break;
                    }
                }
                fileName = !string.IsNullOrWhiteSpace(fileName) ? (fileName + suffixName)
                    : !string.IsNullOrWhiteSpace(_filename) ? _filename
                    : (DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random()).Next().ToString().Substring(0, 4) + suffixName);

                savepath = physicalFolder + fileName;

                System.Net.WebClient mywebclient = new System.Net.WebClient();
                //LogHelper.Save("下载到路径：" + savepath, "DownloadMultimedia", LogType.Common, LogTime.day);
                try
                {
                    mywebclient.DownloadFile(strpath, savepath);
                    file = savepath;
                }
                catch(Exception Ex)
                {
                    savepath = Ex.ToString();
                    throw Ex;
                }

            }
            return file;
        }
        /// <summary>
        /// 下载保存多媒体文件,返回多媒体保存路径
        /// </summary>
        /// <param name="MEDIA_ID">已上传微信服务器的多媒体ID（多个ID用英文半角逗号分隔）</param>
        /// <param name="physicalFolder">保存目标物理文件夹路径。例如：D:\\UploadFiles\\</param>
        /// <param name="fileName">保存文件名(不需要后缀名)</param>
        /// <param name="fileSize">文件大小</param>
        /// <param name="access_token">access_token</param>
        /// <returns>多媒体保存路径（多个ID用英文半角逗号分隔）</returns>
        [Obsolete("由于微信接口变更，该方法已过时")]
        public static string DownloadMultimedia(string MEDIA_ID, string physicalFolder, ref string fileName, out long fileSize, string access_token)
        {
            fileSize = 0;
            if(string.IsNullOrWhiteSpace(MEDIA_ID)) throw new Exception("必传参数MEDIA_ID不能为空！");
            if(string.IsNullOrWhiteSpace(physicalFolder)) throw new Exception("保存目标物理文件夹路径physicalFolder不能为空！");
            MEDIA_ID = Regex.Replace(MEDIA_ID, @"(^[,，\s]+)|([,，\s]+$)", "");
            if(Regex.IsMatch(MEDIA_ID, @"\s*(,|，)\s*"))
            {
                StringBuilder result = new StringBuilder();
                string[] mediaIds = Regex.Split(MEDIA_ID, @"\s*(,|，)\s*");
                //LogHelper.Save("DownloadMultimedia > MEDIA_ID=" + MEDIA_ID + "；mediaIds.Length=" + mediaIds.Length, "DownloadMultimedia", LogType.Common, LogTime.day);

                int num = 0;
                foreach(string mediaId in mediaIds)
                {
                    if(string.IsNullOrWhiteSpace(mediaId) || Regex.IsMatch(mediaId, @"\s*(,|，)\s*"))
                        continue;

                    num++;
                    string _fileName = string.Empty;
                    if(!string.IsNullOrWhiteSpace(fileName))
                    {
                        _fileName = fileName + num;
                    }

                    result.AppendFormat("{0},", DownloadMultimedia(access_token, mediaId, physicalFolder, ref _fileName, out fileSize));
                }
                return Regex.Replace(result.ToString(), @"(^[,，\s]+)|([,，\s]+$)", "").ToString();
            }
            else
                return DownloadMultimedia(access_token, MEDIA_ID, physicalFolder, ref fileName, out fileSize);
        }
        #endregion

        #region 获取素材
        /*
            string fileName = "";
            long fileSize;
            string contentType;
            var bytes = WeChatHelper4Net.Multimedia.GetForeverMultimediaStream(accessToken, mediaId, ref fileName, out fileSize, out contentType);
            ClassLib4Net.LogHelper.Save("mediaId=" + mediaId + "，bytes.Length=" + bytes.Length + "，fileName=" + fileName + "，fileSize=" + fileSize + "，contentType=" + contentType, nameof(QRCodeController), ClassLib4Net.LogType.Common, ClassLib4Net.LogTime.hour);
            System.IO.Stream _stream = new System.IO.MemoryStream(bytes);
            _stream.Seek(0, System.IO.SeekOrigin.Begin);
            ClassLib4Net.LogHelper.Save("mediaId=" + mediaId + "，_stream.Length=" + _stream.Length + "，fileName=" + fileName + "，fileSize=" + fileSize + "，contentType=" + contentType, nameof(QRCodeController), ClassLib4Net.LogType.Common, ClassLib4Net.LogTime.hour);
         */

        /// <summary>
        /// 获取永久素材(除了图文)
        /// </summary>
        /// <param name="ACCESS_TOKEN"></param>
        /// <param name="MEDIA_ID"></param>
        /// <param name="fileName"></param>
        /// <param name="fileSize"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static byte[] GetForeverMultimediaStream(string ACCESS_TOKEN, string MEDIA_ID, ref string fileName, out long fileSize, out string contentType)
        {
            //LogHelper.Save("GetForeverMultimediaStream > " + "ACCESS_TOKEN=" + ACCESS_TOKEN + "，MEDIA_ID=" + MEDIA_ID + "，fileName=" + fileName, nameof(Multimedia), LogType.Common, LogTime.day);
            fileSize = 0;
            contentType = "";

            string strpath = string.Empty;
            string stUrl = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=" + ACCESS_TOKEN;
            var jsonString = "{\"media_id\":\"" + MEDIA_ID + "\"}";
            byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

            string suffixName = string.Empty;

            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(stUrl);
            req.Method = "POST";
            req.Timeout = 5000;
            req.ContentLength = bytes.Length;
            req.GetRequestStream().Write(bytes, 0, bytes.Length);

            try
            {
                using(System.Net.WebResponse wr = req.GetResponse())
                {
                    System.Net.HttpWebResponse myResponse = (System.Net.HttpWebResponse)req.GetResponse();
                    strpath = myResponse.ResponseUri.ToString();
                    fileSize = myResponse.ContentLength;
                    contentType = myResponse.ContentType;
                    string _filename = Common.Replace(myResponse.GetResponseHeader("Content-disposition"), ".*filename=\"", "").Trim(new Char[] { '"' });
                    suffixName = System.IO.Path.GetExtension(_filename).ToLower();

                    //LogHelper.Save("接收信息：" + "StatusCode=" + myResponse.StatusCode.ToString() + "，StatusDescription=" + myResponse.StatusDescription + "，ContentType=" + myResponse.ContentType + "，Content-disposition=" + myResponse.GetResponseHeader("Content-disposition") + "，filename=" + _filename + "，suffixName=" + suffixName, nameof(Multimedia), LogType.Common, LogTime.day);
                    //接收信息：StatusCode=OK，StatusDescription=OK，ContentType=，Content-disposition=attachment; filename="8a125212e0a52c0c59d6cabcd2de18e8.jpg"，filename=8a125212e0a52c0c59d6cabcd2de18e8.jpg，suffixName=.jpg

                    if(string.IsNullOrWhiteSpace(suffixName))
                    {
                        switch(myResponse.ContentType)
                        {
                            case "image/jpeg": suffixName = ".jpg"; break;
                            case "application/x-jpg": suffixName = ".jpg"; break;
                            case "image/png": suffixName = ".png"; break;
                            case "application/x-png": suffixName = ".png"; break;
                            case "audio/amr": suffixName = ".amr"; break;
                            case "audio/mp3": suffixName = ".mp3"; break;
                            case "video/mp4": suffixName = ".mp4"; break;
                            default: suffixName = !string.IsNullOrWhiteSpace(suffixName) ? suffixName : ".jpg"; break;
                        }
                    }
                    fileName = !string.IsNullOrWhiteSpace(fileName) ? (fileName + suffixName)
                        : !string.IsNullOrWhiteSpace(_filename) ? _filename
                        : (MEDIA_ID + suffixName);


                    using(var rs = myResponse.GetResponseStream())
                    {
                        using(var ms = new MemoryStream())
                        {
                            rs.CopyTo(ms);
                            ms.Seek(0, SeekOrigin.Begin);
                            int buffsize = (int)ms.Length; //rs.Length 此流不支持查找,先转为MemoryStream
                            byte[] rsBytes = new byte[buffsize];
                            ms.Read(rsBytes, 0, buffsize);
                            return rsBytes;
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                //LogHelper.Save(ex, "ACCESS_TOKEN=" + ACCESS_TOKEN + "，MEDIA_ID=" + MEDIA_ID, nameof(Multimedia), LogTime.day);
                throw ex;
            }
        }

        #endregion
    }
}
