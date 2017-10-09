using System;
using System.Collections.Generic;
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
        /// <param name="fileName">保存文件名</param>
        /// <param name="fileSize">文件大小</param>
        /// <returns>多媒体保存路径</returns>
        private static string DownloadMultimedia(string ACCESS_TOKEN, string MEDIA_ID, string physicalFolder, string fileName, out long fileSize)
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
        /// <param name="fileName">保存文件名</param>
        /// <param name="fileSize">文件大小</param>
        /// <param name="now">当前时间对象</param>
        /// <returns>多媒体保存路径（多个ID用英文半角逗号分隔）</returns>
        public static string DownloadMultimedia(string MEDIA_ID, string physicalFolder, string fileName, out long fileSize, DateTime now)
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

                    result.AppendFormat("{0},", DownloadMultimedia(AccessToken.GetAccessToken(now), mediaId, physicalFolder, _fileName, out fileSize));
                }
                return Regex.Replace(result.ToString(), @"(^[,，\s]+)|([,，\s]+$)", "").ToString();
            }
            else
                return DownloadMultimedia(AccessToken.GetAccessToken(now), MEDIA_ID, physicalFolder, fileName, out fileSize);
        }
        #endregion
    }
}
