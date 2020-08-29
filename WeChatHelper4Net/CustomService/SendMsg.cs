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

        private static string SendMsgApiUrl(string access_token)
        {
            return Common.ApiUrl + string.Format("message/custom/send?access_token={0}", access_token);
        }
        private static RequestResultBaseModel SendToWeCart(string jsonString, string access_token)
        {
            string result = HttpRequestHelper.Request(SendMsgApiUrl(access_token), jsonString, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            if(Common.ReturnJSONisOK(result))
                return new RequestResultBaseModel() { errcode = 0, errmsg = "ok" };
            else
            {
                RequestResultBaseModel entity = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
                if(entity != null)
                {
                    entity.url = SendMsgApiUrl(access_token);
                    entity.date = jsonString;
                }
                return entity;
            }
        }


        #region 发送文本消息
        public static RequestResultBaseModel SendText(TextMsg msg, string access_token)
        {
            if(null == msg)
                throw new ArgumentException("参数错误", nameof(msg));
            if(string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.text || string.IsNullOrWhiteSpace(msg.text.content))
                throw new ArgumentException("content参数错误");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendText(string openId, string content, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("参数错误", nameof(content));

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"text\",\"text\":{\"content\":\"Hello World\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("Hello World", content);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendTextByKF(kfTextMsg msg, string access_token)
        {
            if(null == msg)
                throw new ArgumentException("参数错误", nameof(msg));
            if(string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.text || string.IsNullOrWhiteSpace(msg.text.content))
                throw new ArgumentException("content参数错误");
            if(null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendTextByKF(string openId, string content, string kfAccount, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("参数错误", nameof(content));
            if(string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("参数错误", nameof(kfAccount));

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"text\",\"text\":{\"content\":\"Hello World\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("Hello World", content).Replace("test1@kftest", kfAccount);
            return SendToWeCart(jsonString, access_token);
        }
        #endregion

        #region 发送图片消息
        public static RequestResultBaseModel SendImage(ImageMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.image || string.IsNullOrWhiteSpace(msg.image.media_id))
                throw new ArgumentException("image错误");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendImage(string openId, string mediaId, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("参数错误", nameof(mediaId));

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"image\",\"image\":{\"media_id\":\"MEDIA_ID\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID", mediaId);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendImageByKF(kfImageMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.image || string.IsNullOrWhiteSpace(msg.image.media_id))
                throw new ArgumentException("image错误");
            if(null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendImageByKF(string openId, string mediaId, string kfAccount, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("mediaId错误");
            if(string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"image\",\"image\":{\"media_id\":\"MEDIA_ID\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID", mediaId).Replace("test1@kftest", kfAccount);
            return SendToWeCart(jsonString, access_token);
        }
        #endregion

        #region 发送语音消息
        public static RequestResultBaseModel SendVoice(VoiceMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.voice || string.IsNullOrWhiteSpace(msg.voice.media_id))
                throw new ArgumentException("voice错误");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendVoice(string openId, string mediaId, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("mediaId错误");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"voice\",\"voice\":{\"media_id\":\"MEDIA_ID\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID", mediaId);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendVoiceByKF(kfVoiceMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.voice || string.IsNullOrWhiteSpace(msg.voice.media_id))
                throw new ArgumentException("voice错误");
            if(null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendVoiceByKF(string openId, string mediaId, string kfAccount, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("mediaId错误");
            if(string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"voice\",\"voice\":{\"media_id\":\"MEDIA_ID\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID", mediaId).Replace("test1@kftest", kfAccount);
            return SendToWeCart(jsonString, access_token);
        }
        #endregion

        #region 发送视频消息
        public static RequestResultBaseModel SendVideo(VideoMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.video || string.IsNullOrWhiteSpace(msg.video.title) || string.IsNullOrWhiteSpace(msg.video.media_id))
                throw new ArgumentException("video错误");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendVideo(string openId, string mediaId, string thumb_media_id, string title, string description, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("title错误");
            if(string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("mediaId错误");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"video\",\"video\":{\"media_id\":\"MEDIA_ID1\",\"thumb_media_id\":\"MEDIA_ID2\",\"title\":\"TITLE\",\"description\":\"DESCRIPTION\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID1", mediaId).Replace("MEDIA_ID2", thumb_media_id).Replace("TITLE", title).Replace("DESCRIPTION", description);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendVideoByKF(kfVideoMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.video || string.IsNullOrWhiteSpace(msg.video.media_id) || string.IsNullOrWhiteSpace(msg.video.title))
                throw new ArgumentException("video错误");
            if(null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendVideoByKF(string openId, string mediaId, string thumb_media_id, string title, string description, string kfAccount, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("title错误");
            if(string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentException("mediaId错误");
            if(string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"video\",\"video\":{\"media_id\":\"MEDIA_ID1\",\"thumb_media_id\":\"MEDIA_ID2\",\"title\":\"TITLE\",\"description\":\"DESCRIPTION\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MEDIA_ID1", mediaId).Replace("MEDIA_ID2", thumb_media_id).Replace("TITLE", title).Replace("DESCRIPTION", description).Replace("test1@kftest", kfAccount);
            return SendToWeCart(jsonString, access_token);
        }
        #endregion

        #region 发送音乐消息
        public static RequestResultBaseModel SendMusic(MusicMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.music || string.IsNullOrWhiteSpace(msg.music.title) || string.IsNullOrWhiteSpace(msg.music.musicurl))
                throw new ArgumentException("music错误");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendMusic(string openId, string title, string description, string musicurl, string hqmusicurl, string thumb_media_id, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("title错误");
            if(string.IsNullOrWhiteSpace(musicurl))
                throw new ArgumentException("musicurl错误");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"music\",\"music\":{\"title\":\"MUSIC_TITLE\",\"description\":\"MUSIC_DESCRIPTION\",\"musicurl\":\"MUSIC_URL\",\"hqmusicurl\":\"HQ_MUSIC_URL\",\"thumb_media_id\":\"THUMB_MEDIA_ID\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MUSIC_TITLE", title).Replace("MUSIC_DESCRIPTION", description).Replace("HQ_MUSIC_URL", hqmusicurl).Replace("MUSIC_URL", musicurl).Replace("THUMB_MEDIA_ID", thumb_media_id);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendMusicByKF(kfMusicMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.music || string.IsNullOrWhiteSpace(msg.music.title) || string.IsNullOrWhiteSpace(msg.music.musicurl))
                throw new ArgumentException("music错误");
            if(null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendMusicByKF(string openId, string title, string description, string musicurl, string hqmusicurl, string thumb_media_id, string kfAccount, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("title错误");
            if(string.IsNullOrWhiteSpace(musicurl))
                throw new ArgumentException("musicurl错误");
            if(string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"music\",\"music\":{\"title\":\"MUSIC_TITLE\",\"description\":\"MUSIC_DESCRIPTION\",\"musicurl\":\"MUSIC_URL\",\"hqmusicurl\":\"HQ_MUSIC_URL\",\"thumb_media_id\":\"THUMB_MEDIA_ID\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("MUSIC_TITLE", title).Replace("MUSIC_DESCRIPTION", description).Replace("HQ_MUSIC_URL", hqmusicurl).Replace("MUSIC_URL", musicurl).Replace("THUMB_MEDIA_ID", thumb_media_id).Replace("test1@kftest", kfAccount);
            return SendToWeCart(jsonString, access_token);
        }
        #endregion

        #region 发送图文消息
        public static RequestResultBaseModel SendNews(NewsMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.news || null == msg.news.articles || 0 > msg.news.articles.Count || 8 < msg.news.articles.Count)
                throw new ArgumentException("图文消息条数限制在8条以内");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }

        public static RequestResultBaseModel SendNewsByKF(kfNewsMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.news || null == msg.news.articles || 1 > msg.news.articles.Count || 8 < msg.news.articles.Count)
                throw new ArgumentException("图文消息条数限制在8条以内");
            if(null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }

        public static RequestResultBaseModel SendmpNews(mpNewsMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.mpnews || string.IsNullOrWhiteSpace(msg.mpnews.media_id))
                throw new ArgumentException("图文消息条数限制在8条以内");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }

        public static RequestResultBaseModel SendmpNewsByKF(kfmpNewsMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.mpnews || string.IsNullOrWhiteSpace(msg.mpnews.media_id))
                throw new ArgumentException("图文消息条数限制在8条以内");
            if(null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }

        #endregion

        #region 发送卡券
        public static RequestResultBaseModel SendwxCard(wxCardMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.wxcard || string.IsNullOrWhiteSpace(msg.wxcard.card_id))
                throw new ArgumentException("card_id错误");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendwxCard(string openId, string card_id, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(card_id))
                throw new ArgumentException("card_id错误");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"wxcard\",\"wxcard\":{\"card_id\":\"CARD_ID\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("CARD_ID", card_id);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendwxCardByKF(kfwxCardMsg msg, string access_token)
        {
            if(null == msg || string.IsNullOrWhiteSpace(msg.touser))
                throw new ArgumentException("参数错误", nameof(msg.touser));
            if(null == msg.wxcard || string.IsNullOrWhiteSpace(msg.wxcard.card_id))
                throw new ArgumentException("card_id错误");
            if(null == msg.customservice || string.IsNullOrWhiteSpace(msg.customservice.kf_account))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = JsonHelper.Serialize(msg);
            return SendToWeCart(jsonString, access_token);
        }
        public static RequestResultBaseModel SendwxCardByKF(string openId, string card_id, string kfAccount, string access_token)
        {
            if(string.IsNullOrWhiteSpace(openId))
                throw new ArgumentException("参数错误", nameof(openId));
            if(string.IsNullOrWhiteSpace(card_id))
                throw new ArgumentException("card_id错误");
            if(string.IsNullOrWhiteSpace(kfAccount))
                throw new ArgumentException("customservice参数不能为空");

            string jsonString = "{\"touser\":\"OPENID\",\"msgtype\":\"wxcard\",\"wxcard\":{\"card_id\":\"CARD_ID\"},\"customservice\":{\"kf_account\":\"test1@kftest\"}}";
            jsonString = jsonString.Replace("OPENID", openId).Replace("CARD_ID", card_id).Replace("test1@kftest", kfAccount);
            return SendToWeCart(jsonString, access_token);
        }
        #endregion

    }
}
