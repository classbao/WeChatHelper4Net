using System;
using WeChatHelper4Net.Models;
using WeChatHelper4Net.Models.CustomService;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2016-12-12
 */
namespace WeChatHelper4Net.CustomService
{
    /// <summary>
    /// 客服接口-发消息
    /// </summary>
    public class SendMsg
    {
        private SendMsg() { }

        private static string SendMsgApiUrl(DateTime now)
        {
            return Common.ApiUrl + string.Format("message/custom/send?access_token={0}", AccessToken.GetAccessToken(now));
        }

        

        #region 发送文本消息
        public static RequestResultBaseModel SendText(TextMsg msg, DateTime now)
        {
            if (null == msg)
                throw new ArgumentException("参数错误", nameof(msg));
            if (string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if (null == msg.text || string.IsNullOrWhiteSpace(msg.text.content))
                throw new ArgumentException("content参数错误");

            string jsonString = JsonHelper.Serialize(msg);
            string result = HttpRequestHelper.Request(SendMsgApiUrl(now), jsonString, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            if (Common.ReturnJSONisOK(result))
                return new RequestResultBaseModel() { errcode = 0, errmsg = "ok" };
            else
            {
                RequestResultBaseModel entity = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
                if (entity != null)
                {
                    entity.url = SendMsgApiUrl(now);
                    entity.date = jsonString;
                }
                return entity;
            }
        }
        public static RequestResultBaseModel SendText(string openId, string content, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("参数错误", nameof(content));

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"text\",\"text\":{\"content\":\"Hello World\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("Hello World", content);

            string result = HttpRequestHelper.Request(SendMsgApiUrl(now), jsonString, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            if (Common.ReturnJSONisOK(result))
                return new RequestResultBaseModel() { errcode = 0, errmsg = "ok" };
            else
            {
                RequestResultBaseModel entity = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
                if (entity != null)
                {
                    entity.url = SendMsgApiUrl(now);
                    entity.date = jsonString;
                }
                return entity;
            }
        }
        public static RequestResultBaseModel SendTextByKF(kfTextMsg msg, DateTime now)
        {
            if (null == msg)
                throw new ArgumentException("参数错误", nameof(msg));
            if (string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if (null == msg.text || string.IsNullOrWhiteSpace(msg.text.content))
                throw new ArgumentException("content参数错误");
            if (null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            string result = HttpRequestHelper.Request(SendMsgApiUrl(now), jsonString, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            if (Common.ReturnJSONisOK(result))
                return new RequestResultBaseModel() { errcode = 0, errmsg = "ok" };
            else
            {
                RequestResultBaseModel entity = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
                if (entity != null)
                {
                    entity.url = SendMsgApiUrl(now);
                    entity.date = jsonString;
                }
                return entity;
            }
        }
        public static RequestResultBaseModel SendTextByKF(string openId, string content, string kfAccount, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("参数错误", nameof(content));
            if (string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("参数错误", nameof(kfAccount));

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"text\",\"text\":{\"content\":\"Hello World\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("Hello World", content).Replace("test1@kftest", kfAccount);
            string result = HttpRequestHelper.Request(SendMsgApiUrl(now), jsonString, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            if (Common.ReturnJSONisOK(result))
                return new RequestResultBaseModel() { errcode = 0, errmsg = "ok" };
            else
            {
                RequestResultBaseModel entity = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
                if (entity != null)
                {
                    entity.url = SendMsgApiUrl(now);
                    entity.date = jsonString;
                }
                return entity;
            }
        }
        #endregion

        #region 发送图片消息
        public static string SendImage(ImageMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.image || string.IsNullOrWhiteSpace(msg.image.media_id))
                throw new ArgumentException("image错误");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }
        public static string SendImage(string openId, string mediaId, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("mediaId错误");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"image\",\"image\":{\"media_id\":\"MEDIA_ID\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID", mediaId);
            return jsonString;
        }
        public static string SendImageByKF(kfImageMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.image || string.IsNullOrWhiteSpace(msg.image.media_id))
                throw new ArgumentException("image错误");
            if (null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }
        public static string SendImageByKF(string openId, string mediaId, string kfAccount, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("mediaId错误");
            if (string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"image\",\"image\":{\"media_id\":\"MEDIA_ID\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID", mediaId).Replace("test1@kftest", kfAccount);
            return jsonString;
        }
        #endregion

        #region 发送语音消息
        public static string SendVoice(VoiceMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.voice || string.IsNullOrWhiteSpace(msg.voice.media_id))
                throw new ArgumentException("voice错误");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }
        public static string SendVoice(string openId, string mediaId, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("mediaId错误");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"voice\",\"voice\":{\"media_id\":\"MEDIA_ID\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID", mediaId);
            return jsonString;
        }
        public static string SendVoiceByKF(kfVoiceMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.voice || string.IsNullOrWhiteSpace(msg.voice.media_id))
                throw new ArgumentException("voice错误");
            if (null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }
        public static string SendVoiceByKF(string openId, string mediaId, string kfAccount, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("mediaId错误");
            if (string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"voice\",\"voice\":{\"media_id\":\"MEDIA_ID\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID", mediaId).Replace("test1@kftest", kfAccount);
            return jsonString;
        }
        #endregion

        #region 发送视频消息
        public static string SendVideo(VideoMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.video || string.IsNullOrWhiteSpace(msg.video.title) || string.IsNullOrWhiteSpace(msg.video.media_id))
                throw new ArgumentException("video错误");

            if (null != msg && !string.IsNullOrWhiteSpace(msg.touser))
            {
                string jsonString = JsonHelper.Serialize(msg);
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    return jsonString;
                }
            }
            return string.Empty;
        }
        public static string SendVideo(string openId, string mediaId, string thumb_media_id, string title, string description, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("title错误");
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("mediaId错误");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"video\",\"video\":{\"media_id\":\"MEDIA_ID1\",\"thumb_media_id\":\"MEDIA_ID2\",\"title\":\"TITLE\",\"description\":\"DESCRIPTION\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID1", mediaId).Replace("MEDIA_ID2", thumb_media_id).Replace("TITLE", title).Replace("DESCRIPTION", description);
            return jsonString;
        }
        public static string SendVideoByKF(kfVideoMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.video || string.IsNullOrWhiteSpace(msg.video.media_id) || string.IsNullOrWhiteSpace(msg.video.title))
                throw new ArgumentException("video错误");
            if (null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }
        public static string SendVideoByKF(string openId, string mediaId, string thumb_media_id, string title, string description, string kfAccount, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("title错误");
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("mediaId错误");
            if (string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"video\",\"video\":{\"media_id\":\"MEDIA_ID1\",\"thumb_media_id\":\"MEDIA_ID2\",\"title\":\"TITLE\",\"description\":\"DESCRIPTION\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID1", mediaId).Replace("MEDIA_ID2", thumb_media_id).Replace("TITLE", title).Replace("DESCRIPTION", description).Replace("test1@kftest", kfAccount);
            return jsonString;
        }
        #endregion

        #region 发送音乐消息
        public static string SendMusic(MusicMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.music || string.IsNullOrWhiteSpace(msg.music.title) || string.IsNullOrWhiteSpace(msg.music.musicurl))
                throw new ArgumentException("music错误");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }
        public static string SendMusic(string openId, string title, string description, string musicurl, string hqmusicurl, string thumb_media_id, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("title错误");
            if (string.IsNullOrWhiteSpace(musicurl))
                throw new ArgumentException("musicurl错误");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"music\",\"music\":{\"title\":\"MUSIC_TITLE\",\"description\":\"MUSIC_DESCRIPTION\",\"musicurl\":\"MUSIC_URL\",\"hqmusicurl\":\"HQ_MUSIC_URL\",\"thumb_media_id\":\"THUMB_MEDIA_ID\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MUSIC_TITLE", title).Replace("MUSIC_DESCRIPTION", description).Replace("HQ_MUSIC_URL", hqmusicurl).Replace("MUSIC_URL", musicurl).Replace("THUMB_MEDIA_ID", thumb_media_id);
            return jsonString;
        }
        public static string SendMusicByKF(kfMusicMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.music || string.IsNullOrWhiteSpace(msg.music.title) || string.IsNullOrWhiteSpace(msg.music.musicurl))
                throw new ArgumentException("music错误");
            if (null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }
        public static string SendMusicByKF(string openId, string title, string description, string musicurl, string hqmusicurl, string thumb_media_id, string kfAccount, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("title错误");
            if (string.IsNullOrWhiteSpace(musicurl))
                throw new ArgumentException("musicurl错误");
            if (string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"music\",\"music\":{\"title\":\"MUSIC_TITLE\",\"description\":\"MUSIC_DESCRIPTION\",\"musicurl\":\"MUSIC_URL\",\"hqmusicurl\":\"HQ_MUSIC_URL\",\"thumb_media_id\":\"THUMB_MEDIA_ID\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MUSIC_TITLE", title).Replace("MUSIC_DESCRIPTION", description).Replace("HQ_MUSIC_URL", hqmusicurl).Replace("MUSIC_URL", musicurl).Replace("THUMB_MEDIA_ID", thumb_media_id).Replace("test1@kftest", kfAccount);
            return jsonString;
        }
        #endregion

        #region 发送图文消息
        public static string SendNews(NewsMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.news || null == msg.news.articles || 0 > msg.news.articles.Count || 8 < msg.news.articles.Count)
                throw new ArgumentException("图文消息条数限制在8条以内");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }

        public static string SendNewsByKF(kfNewsMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.news || null == msg.news.articles || 1 > msg.news.articles.Count || 8 < msg.news.articles.Count)
                throw new ArgumentException("图文消息条数限制在8条以内");
            if (null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }

        public static string SendmpNews(mpNewsMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.mpnews || string.IsNullOrWhiteSpace(msg.mpnews.media_id))
                throw new ArgumentException("图文消息条数限制在8条以内");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }

        public static string SendmpNewsByKF(kfmpNewsMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.mpnews || string.IsNullOrWhiteSpace(msg.mpnews.media_id))
                throw new ArgumentException("图文消息条数限制在8条以内");
            if (null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }

        #endregion

        #region 发送卡券
        public static string SendwxCard(wxCardMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.wxcard || string.IsNullOrWhiteSpace(msg.wxcard.card_id))
                throw new ArgumentException("card_id错误");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }
        public static string SendwxCard(string openId, string card_id, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(card_id))
                throw new ArgumentException("card_id错误");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"wxcard\",\"wxcard\":{\"card_id\":\"CARD_ID\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("CARD_ID", card_id);
            return jsonString;
        }
        public static string SendwxCardByKF(kfwxCardMsg msg, DateTime now)
        {
            if (null == msg || string.IsNullOrWhiteSpace(msg.touser))
                return string.Empty;
            if (null == msg.wxcard || string.IsNullOrWhiteSpace(msg.wxcard.card_id))
                throw new ArgumentException("card_id错误");
            if (null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }
            return string.Empty;
        }
        public static string SendwxCardByKF(string openId, string card_id, string kfAccount, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(openId))
                return string.Empty;
            if (string.IsNullOrWhiteSpace(card_id))
                throw new ArgumentException("card_id错误");
            if (string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"wxcard\",\"wxcard\":{\"card_id\":\"CARD_ID\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("CARD_ID", card_id).Replace("test1@kftest", kfAccount);
            return jsonString;
        }
        #endregion

    }
}
