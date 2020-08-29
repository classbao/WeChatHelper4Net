using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WeChatHelper4Net.Extend;
using WeChatHelper4Net.Models.Multimedia;

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
        /// <param name="ACCESS_TOKEN">访问令牌</param>
        /// <param name="MEDIA_ID">MEDIA_ID</param>
        /// <param name="physicalFolder">物理文件夹</param>
        /// <param name="fileName">保存文件名(不需要后缀名)</param>
        /// <param name="fileSize">文件大小</param>
        /// <returns>多媒体保存路径</returns>
        [Obsolete("由于微信接口变更，该方法已过时")]
        private static string DownloadMultimedia(string ACCESS_TOKEN, string MEDIA_ID, string physicalFolder, ref string fileName, out long fileSize)
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
        /// <param name="access_token">访问令牌</param>
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
         c# 将byte数组保存成图片
        将byte数组保存成图片：

        方式一：System.IO.File.WriteAllBytes(@"c:\test.jpg", bytes);

        方式二：MemoryStream ms=new MemoryStream(Byte[] b);  把那个byte[]数组传进去,然后
                   FileStream fs=new FileStream(路径 例如:"E:\image\1.jpg");
        　　　　ms.writeto(fs);
        　　　　ms.close();
        　　　　fs.close();

        方法三：

               //得到图片地址
               var stringFilePath = context.Server.MapPath(string.Format("~/Image/{0}{1}.jpg", imageName, id));
               //声明一个FileStream用来将图片暂时放入流中
               FileStream stream = new FileStream(stringFilePath, FileMode.Open);
               using (stream)
               {
                   //透过GetImageFromStream方法将图片放入byte数组中
                   byte[] imageBytes = this.GetImageFromStream(stream,context);
                   //上下文确定写到客户短时的文件类型
                   context.Response.ContentType = "image/jpeg";
                   //上下文将imageBytes中的数据写到前段
                   context.Response.BinaryWrite(imageBytes);
                   stream.Close();
                }
        */


        /// <summary>
        /// 获取临时素材(除了图文，视频，语音)
        /// 公众号可以使用本接口获取临时素材（即下载临时的多媒体文件）。请注意，视频文件不支持https下载，调用该接口需http协议。本接口即为原“下载多媒体文件”接口。
        /// </summary>
        /// <param name="ACCESS_TOKEN">访问令牌</param>
        /// <param name="MEDIA_ID"></param>
        /// <param name="fileName"></param>
        /// <param name="fileSize"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static byte[] GetTempMultimediaStream(string ACCESS_TOKEN, string MEDIA_ID, ref string fileName, out long fileSize, out string contentType)
        {
            //LogHelper.Save("GetTempMultimediaStream > " + "ACCESS_TOKEN=" + ACCESS_TOKEN + "，MEDIA_ID=" + MEDIA_ID + "，fileName=" + fileName, nameof(Multimedia), LogType.Common, LogTime.day);
            fileSize = 0;
            contentType = "";

            string strpath = string.Empty;
            string stUrl = Common.ApiUrl + string.Format("media/get?access_token={0}&media_id={1}", ACCESS_TOKEN, MEDIA_ID);

            string suffixName = string.Empty;

            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(stUrl);
            req.Method = "GET";
            req.Timeout = 30000;
            req.KeepAlive = true;

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
                    //接收信息：StatusCode=OK，StatusDescription=OK，ContentType=image/jpeg，Content-disposition=attachment; filename="-zu3wS2bJFqRKMonF0CrbEBvnCtKCS7NPyHAvdUIh1rowBDclir9sPP5s1QSBHGT.jpg"，filename=-zu3wS2bJFqRKMonF0CrbEBvnCtKCS7NPyHAvdUIh1rowBDclir9sPP5s1QSBHGT.jpg，suffixName=.jpg

                    if(string.IsNullOrWhiteSpace(suffixName) && !string.IsNullOrWhiteSpace(contentType))
                        suffixName = GetSuffixNameByContentType(contentType);
                    else if(!string.IsNullOrWhiteSpace(suffixName) && string.IsNullOrWhiteSpace(contentType))
                        contentType = GetContentTypeBySuffixName(suffixName);
                    if(string.IsNullOrWhiteSpace(suffixName) && string.IsNullOrWhiteSpace(contentType))
                    {
                        if("CropImage" == _filename)
                        {
                            suffixName = ".jpg";
                            contentType = GetContentTypeBySuffixName(suffixName);
                            fileName = "CropImage";
                        }
                        else if(!string.IsNullOrWhiteSpace(fileName))
                        {
                            suffixName = System.IO.Path.GetExtension(fileName).ToLower();
                            contentType = GetContentTypeBySuffixName(suffixName);
                            fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        }
                    }

                    fileName = !string.IsNullOrWhiteSpace(fileName) ? (fileName + suffixName)
                        : !string.IsNullOrWhiteSpace(_filename) ? _filename
                        : (MEDIA_ID + suffixName);

                    //将输出流转换字节数组并返回
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
        /// <summary>
        /// 获取永久素材(临时)
        /// </summary>
        /// <param name="ACCESS_TOKEN">访问令牌</param>
        /// <param name="MEDIA_ID"></param>
        /// <returns></returns>
        public static TempVideo GetTempVideo(string ACCESS_TOKEN, string MEDIA_ID)
        {
            if(string.IsNullOrEmpty(MEDIA_ID)) return new TempVideo();
            try
            {
                string url = Common.ApiUrl + string.Format("media/get?access_token={0}&media_id={1}", ACCESS_TOKEN, MEDIA_ID);
                var jsonString = "";
                string result = HttpRequestHelper.Request(url, jsonString, HttpRequestHelper.Method.GET, System.Text.Encoding.UTF8);
                //LogHelper.Save("GetTempVideo > " + "ACCESS_TOKEN=" + ACCESS_TOKEN + "，MEDIA_ID=" + MEDIA_ID + "，result=" + result, nameof(Multimedia), LogType.Common, LogTime.day);
                if(!string.IsNullOrEmpty(result))
                {
                    return JsonHelper.DeSerialize<TempVideo>(StringHelper.RemoveSpecialChar(result));
                }
            }
            catch(Exception Ex)
            {
                LogHelper.Save(Ex);
                throw Ex;
            }
            return new TempVideo();
        }
        /// <summary>
        /// 高清语音素材获取接口(临时)
        /// 公众号可以使用本接口获取从JSSDK的uploadVoice接口上传的临时语音素材，格式为speex，16K采样率。该音频比上文的临时素材获取接口（格式为amr，8K采样率）更加清晰，适合用作语音识别等对音质要求较高的业务。
        /// </summary>
        /// <param name="ACCESS_TOKEN">访问令牌</param>
        /// <param name="MEDIA_ID"></param>
        /// <param name="fileName"></param>
        /// <param name="fileSize"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static byte[] GetTempVoiceMultimediaStream(string ACCESS_TOKEN, string MEDIA_ID, ref string fileName, out long fileSize, out string contentType)
        {
            //LogHelper.Save("GetTempVoiceMultimediaStream > " + "ACCESS_TOKEN=" + ACCESS_TOKEN + "，MEDIA_ID=" + MEDIA_ID + "，fileName=" + fileName, nameof(Multimedia), LogType.Common, LogTime.day);
            fileSize = 0;
            contentType = "";

            string strpath = string.Empty;
            string stUrl = Common.ApiUrl + string.Format("media/get/jssdk?access_token={0}&media_id={1}", ACCESS_TOKEN, MEDIA_ID);

            string suffixName = string.Empty;

            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(stUrl);
            req.Method = "GET";
            req.Timeout = 30000;
            req.KeepAlive = true;

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

                    if(string.IsNullOrWhiteSpace(suffixName) && !string.IsNullOrWhiteSpace(contentType))
                        suffixName = GetSuffixNameByContentType(contentType);
                    else if(!string.IsNullOrWhiteSpace(suffixName) && string.IsNullOrWhiteSpace(contentType))
                        contentType = GetContentTypeBySuffixName(suffixName);
                    if(string.IsNullOrWhiteSpace(suffixName) && string.IsNullOrWhiteSpace(contentType))
                    {
                        if("CropImage" == _filename)
                        {
                            suffixName = ".jpg";
                            contentType = GetContentTypeBySuffixName(suffixName);
                            fileName = "CropImage";
                        }
                        else if(!string.IsNullOrWhiteSpace(fileName))
                        {
                            suffixName = System.IO.Path.GetExtension(fileName).ToLower();
                            contentType = GetContentTypeBySuffixName(suffixName);
                            fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        }
                    }

                    fileName = !string.IsNullOrWhiteSpace(fileName) ? (fileName + suffixName)
                        : !string.IsNullOrWhiteSpace(_filename) ? _filename
                        : (MEDIA_ID + suffixName);

                    //将输出流转换字节数组并返回
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
        /// 获取永久素材(除了图文，视频)
        /// 公众号的素材库保存总数量有上限：图文消息素材、图片素材上限为5000，其他类型为1000。
        /// 素材的格式大小等要求与公众平台官网一致：
        /// 图片（image）: 2M，支持bmp/png/jpeg/jpg/gif格式
        /// 语音（voice）：2M，播放长度不超过60s，mp3/wma/wav/amr格式
        /// 视频（video）：10MB，支持MP4格式
        /// 缩略图（thumb）：64KB，支持JPG格式
        /// </summary>
        /// <param name="ACCESS_TOKEN">访问令牌</param>
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
            string stUrl = Common.ApiUrl + "material/get_material?access_token=" + ACCESS_TOKEN;
            var jsonString = "{\"media_id\":\"" + MEDIA_ID + "\"}";
            byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

            string suffixName = string.Empty;

            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(stUrl);
            req.Method = "POST";
            req.Timeout = 30000;
            req.KeepAlive = true;
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

                    if(string.IsNullOrWhiteSpace(suffixName) && !string.IsNullOrWhiteSpace(contentType))
                        suffixName = GetSuffixNameByContentType(contentType);
                    else if(!string.IsNullOrWhiteSpace(suffixName) && string.IsNullOrWhiteSpace(contentType))
                        contentType = GetContentTypeBySuffixName(suffixName);
                    if(string.IsNullOrWhiteSpace(suffixName) && string.IsNullOrWhiteSpace(contentType))
                    {
                        if("CropImage" == _filename)
                        {
                            suffixName = ".jpg";
                            contentType = GetContentTypeBySuffixName(suffixName);
                            fileName = "CropImage";
                        }
                        else if(!string.IsNullOrWhiteSpace(fileName))
                        {
                            suffixName = System.IO.Path.GetExtension(fileName).ToLower();
                            contentType = GetContentTypeBySuffixName(suffixName);
                            fileName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        }
                    }

                    fileName = !string.IsNullOrWhiteSpace(fileName) ? (fileName + suffixName)
                        : !string.IsNullOrWhiteSpace(_filename) ? _filename
                        : (MEDIA_ID + suffixName);

                    //将输出流转换字节数组并返回
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

        /// <summary>
        /// 获取永久素材(视频)
        /// </summary>
        /// <param name="ACCESS_TOKEN">访问令牌</param>
        /// <param name="MEDIA_ID"></param>
        /// <returns></returns>
        public static ForeverVideo GetForeverVideo(string ACCESS_TOKEN, string MEDIA_ID)
        {
            if(string.IsNullOrEmpty(MEDIA_ID)) return new ForeverVideo();
            try
            {
                string url = Common.ApiUrl + string.Format("material/get_material?access_token={0}", ACCESS_TOKEN);
                var jsonString = "{\"media_id\":\"" + MEDIA_ID + "\"}";
                string result = HttpRequestHelper.Request(url, jsonString, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
                //LogHelper.Save("GetForeverMultimediaVideoStream > " + "ACCESS_TOKEN=" + ACCESS_TOKEN + "，MEDIA_ID=" + MEDIA_ID + "，result=" + result, nameof(Multimedia), LogType.Common, LogTime.day);
                /*
                 GetForeverMultimediaVideoStream > ACCESS_TOKEN=13_AS8GWsxovYZjCQtLN9CGDrsNCFJeWnEI9hSqX-b99agBMqH75oiZkRsS5b2GWs4AekEj2eKnw4cQcxhn-lafEGHT2Mk7CpwuznIFG05QfFML1Qn7L0uhsKzS7TGF6IxZ4RNb-dxBL3TMb8B4JXHiAFAXLV，MEDIA_ID=8DxY1tadrdbK7TQGFDwTzMNUaJUA79uPtoDPY_Qks4o，result={"title":"第一期-关闭分页预览","description":"","down_url":"http:\/\/mp.weixin.qq.com\/mp\/mp\/video?__biz=MzIxMzc4MTMzMA==&mid=100001728&sn=b9e6258f2abb37028af88bad81976879&vid=o1345teloaj&idx=1&vidsn=5efab83eb152c43d3cfff638d39cc909&fromid=1#rd"}
                 */
                if(!string.IsNullOrEmpty(result))
                {
                    return JsonHelper.DeSerialize<ForeverVideo>(StringHelper.RemoveSpecialChar(result));
                }
            }
            catch(Exception Ex)
            {
                LogHelper.Save(Ex);
                throw Ex;
            }
            return new ForeverVideo();
        }
        /// <summary>
        /// 获取永久素材(图文)
        /// 待完成
        /// </summary>
        /// <param name="ACCESS_TOKEN">访问令牌</param>
        /// <param name="MEDIA_ID"></param>
        public static void GetForeverNews(string ACCESS_TOKEN, string MEDIA_ID)
        {
        }
        #endregion

        #region 多媒体文件类型与后缀名转换
        /// <summary>
        /// 根据后缀名获取ContentType。示例：.png返回：image/png
        /// </summary>
        /// <param name="suffixName"></param>
        /// <returns></returns>
        public static string GetContentTypeBySuffixName(string suffixName)
        {
            if(string.IsNullOrWhiteSpace(suffixName)) return "";
            switch(suffixName.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg"; //"application/x-jpg"
                case ".png":
                    return "image/png"; //"application/x-png"
                case ".bmp": return "application/x-bmp";
                case ".gif": return "image/gif";

                case ".amr": return "audio/amr";
                case ".wma": return "audio/x-ms-wma";
                case ".wav": return "audio/wav";
                case ".mp3": return "audio/mp3";
                case ".mp4": return "video/mp4"; //"video/mpeg4"

                default: return "";
            }
        }
        /// <summary>
        /// 根据ContentType获取后缀名。示例：image/png返回：.png
        /// </summary>
        /// <param name="ContentType">示例：</param>
        /// <returns></returns>
        public static string GetSuffixNameByContentType(string ContentType)
        {
            switch(ContentType)
            {
                case "image/jpeg":
                case "application/x-jpg":
                    return ".jpg";
                case "image/png":
                case "application/x-png":
                    return ".png";
                case "application/x-bmp": return ".bmp";
                //case "image/jpeg": suffixName = ".jpeg"; break;
                case "image/gif": return ".gif";

                case "audio/amr": return ".amr";
                case "audio/x-ms-wma": return ".wma";
                case "audio/wav": return ".wav";
                case "audio/mp3": return ".mp3";
                case "video/mp4":
                case "video/mpeg4":
                    return ".mp4";

                default: return "";
            }
        }
        #endregion

    }
}
