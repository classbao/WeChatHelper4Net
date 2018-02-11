using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Text;
using WeChatHelper4Net.Models;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// 向关注用户，订阅用户，群等发送消息
    /// 注意：无论在公众平台网站上，还是使用接口群发，用户每月只能接收4条群发消息，多于4条的群发将对该用户发送失败。
    /// </summary>
    [Obsolete("客服消息迁移到" + nameof(CustomService))]
    public class SendMessage
    {
        #region 发送客服消息
        /// <summary>
        /// 发送客服消息接口，http请求方式: POST
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        private static string urlToUser(string access_token)
        {
            return Common.ApiUrl + string.Format("message/custom/send?access_token={0}", access_token);
        }
        /// <summary>
        /// 发送客服消息接口，http请求方式: POST
        /// </summary>
        //private static string urlToUser = Common.ApiUrl + string.Format("message/custom/send?access_token={0}", access_token);

        /// <summary>
        /// 发送文本消息。
        /// 开发者在一段时间内（目前修改为48小时）可以调用客服消息接口，通过POST一个JSON数据包来发送消息给普通用户，在48小时内不限制发送次数。此接口主要用于客服等有人工消息处理环节的功能，方便开发者为用户提供更加优质的服务。
        /// </summary>
        /// <param name="openid">普通用户openid</param>
        /// <param name="textContent">文本消息内容</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel TextToUser(string openid, string textContent, string access_token)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(textContent)) return new RequestResultBaseModel();
            string data = "{\"touser\": \"OpenId\", \"msgtype\": \"text\", \"text\": {\"content\": \"TextContent\"}}";
            data = data.Replace("OpenId", openid)
                .Replace("TextContent", textContent);
            string result = HttpRequestHelper.Request(urlToUser(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToUser(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 发送图片消息。
        /// 开发者在一段时间内（目前修改为48小时）可以调用客服消息接口，通过POST一个JSON数据包来发送消息给普通用户，在48小时内不限制发送次数。此接口主要用于客服等有人工消息处理环节的功能，方便开发者为用户提供更加优质的服务。
        /// </summary>
        /// <param name="openid">普通用户openid</param>
        /// <param name="MediaId">发送的图片的媒体ID</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel ImageToUser(string openid, string MediaId, string access_token)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(MediaId)) return new RequestResultBaseModel();
            string data = "{\"touser\": \"OpenId\", \"msgtype\": \"image\", \"image\": {\"media_id\": \"MediaId\"}}";
            data = data.Replace("OpenId", openid)
                .Replace("MediaId", MediaId);
            string result = HttpRequestHelper.Request(urlToUser(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToUser(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 发送语音消息。
        /// 开发者在一段时间内（目前修改为48小时）可以调用客服消息接口，通过POST一个JSON数据包来发送消息给普通用户，在48小时内不限制发送次数。此接口主要用于客服等有人工消息处理环节的功能，方便开发者为用户提供更加优质的服务。
        /// </summary>
        /// <param name="openid">普通用户openid</param>
        /// <param name="MediaId">发送的图片的媒体ID</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel VoiceToUser(string openid, string MediaId, string access_token)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(MediaId)) return new RequestResultBaseModel();
            string data = "{\"touser\": \"OpenId\", \"msgtype\": \"voice\", \"voice\": {\"media_id\": \"MediaId\"}}";
            data = data.Replace("OpenId", openid)
                .Replace("MediaId", MediaId);
            string result = HttpRequestHelper.Request(urlToUser(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToUser(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 发送视频消息。
        /// 开发者在一段时间内（目前修改为48小时）可以调用客服消息接口，通过POST一个JSON数据包来发送消息给普通用户，在48小时内不限制发送次数。此接口主要用于客服等有人工消息处理环节的功能，方便开发者为用户提供更加优质的服务。
        /// </summary>
        /// <param name="openid">普通用户openid</param>
        /// <param name="MediaId">发送的图片的媒体ID</param>
        /// <param name="title">视频消息的标题</param>
        /// <param name="description">视频消息的描述</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel VideoToUser(string openid, string MediaId, string title, string description, string access_token)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(MediaId)) return new RequestResultBaseModel();
            string data = "{\"touser\": \"OpenId\", \"msgtype\": \"video\", \"video\": {\"media_id\": \"MediaId\", \"title\": \"Title\", \"description\": \"Description\"}}";
            data = data.Replace("OpenId", openid)
                .Replace("MediaId", MediaId)
                .Replace("Title", title)
                .Replace("Description", description);
            string result = HttpRequestHelper.Request(urlToUser(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToUser(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 发送音乐消息。
        /// 开发者在一段时间内（目前修改为48小时）可以调用客服消息接口，通过POST一个JSON数据包来发送消息给普通用户，在48小时内不限制发送次数。此接口主要用于客服等有人工消息处理环节的功能，方便开发者为用户提供更加优质的服务。
        /// </summary>
        /// <param name="openid">普通用户openid</param>
        /// <param name="title">音乐标题</param>
        /// <param name="description">音乐描述</param>
        /// <param name="musicurl">音乐链接</param>
        /// <param name="hqmusicurl">高品质音乐链接，wifi环境优先使用该链接播放音乐</param>
        /// <param name="thumb_media_id">缩略图的媒体ID</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel MusicToUser(string openid, string title, string description, string musicurl, string hqmusicurl, string thumb_media_id, string access_token)
        {
            if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(musicurl) || string.IsNullOrEmpty(hqmusicurl) || string.IsNullOrEmpty(thumb_media_id)) return new RequestResultBaseModel();
            string data = "{\"touser\":\"OpenId\", \"msgtype\":\"music\", \"music\": {\"title\":\"Title\", \"description\":\"Description\", \"musicurl\":\"MUSIC_URL\", \"hqmusicurl\":\"HQ_MUSIC_URL\", \"thumb_media_id\":\"THUMB_MEDIA_ID\"}}";
            data = data.Replace("OpenId", openid)
                .Replace("Title", title)
                .Replace("Description", description)
                .Replace("MUSIC_URL", musicurl)
                .Replace("HQ_MUSIC_URL", hqmusicurl)
                .Replace("THUMB_MEDIA_ID", thumb_media_id);
            string result = HttpRequestHelper.Request(urlToUser(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToUser(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 发送图文消息。（图文消息条数限制在10条以内，注意，如果图文数超过10，则将会无响应。第一条图文的图片大小建议为640*320，其他图文的图片大小建议为80*80。）
        /// 开发者在一段时间内（目前修改为48小时）可以调用客服消息接口，通过POST一个JSON数据包来发送消息给普通用户，在48小时内不限制发送次数。此接口主要用于客服等有人工消息处理环节的功能，方便开发者为用户提供更加优质的服务。
        /// </summary>
        /// <param name="openid">普通用户openid</param>
        /// <param name="news">图文消息条数限制在10条以内，注意，如果图文数超过10，则将会无响应</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel NewsToUser(string openid, List<NewsModel> news, string access_token)
        {
            if (string.IsNullOrEmpty(openid) || news.Count < 1) return new RequestResultBaseModel();
            if (news.Count > 10) throw new Exception("图文消息条数限制在10条以内，注意，如果图文数超过10，则将会无响应。");
            string data = "{ \"touser\":\"OPENID\", \"msgtype\":\"news\", \"news\":{ \"articles\": NewsModel}}";
            data = data.Replace("OPENID", openid)
                .Replace("NewsModel", JsonHelper.Serialize<List<NewsModel>>(news));
            string result = HttpRequestHelper.Request(urlToUser(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToUser(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        #endregion

        #region 高级群发接口

        #region 根据分组进行群发
        /// <summary>
        /// 根据分组进行群发接口，http请求方式: POST
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        private static string urlToGroup(string access_token)
        {
            return Common.ApiUrl + string.Format("message/mass/sendall?access_token={0}", access_token);
        }
        /// <summary>
        /// 根据分组进行群发接口，http请求方式: POST
        /// </summary>
        //private static string urlToGroup = Common.ApiUrl + string.Format("message/mass/sendall?access_token={0}", access_token);

        /// <summary>
        /// 分组进行群发（图文消息）
        /// </summary>
        /// <param name="groupId">群发到的分组的group_id</param>
        /// <param name="mediaId">用于群发的消息的media_id</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel NewsToGroup(int groupId, string mediaId, string access_token)
        {
            if (groupId < 1 || string.IsNullOrEmpty(mediaId)) return new RequestResultBaseModel();
            string data = "{\"filter\":{ \"group_id\":\"GroupId\" }, \"mpnews\":{\"media_id\":\"MediaId\" }, \"msgtype\":\"mpnews\"}";
            data = data.Replace("GroupId", groupId.ToString())
                .Replace("MediaId", mediaId);
            string result = HttpRequestHelper.Request(urlToGroup(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToGroup(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 分组进行群发（文本消息）
        /// </summary>
        /// <param name="groupId">群发到的分组的group_id</param>
        /// <param name="textContent">文本</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel TextToGroup(int groupId, string textContent, string access_token)
        {
            if (groupId < 1 || string.IsNullOrEmpty(textContent)) return new RequestResultBaseModel();
            string data = "{\"filter\":{ \"group_id\":\"GroupId\" }, \"text\":{ \"content\":\"TextContent\" }, \"msgtype\":\"text\"}";
            data = data.Replace("GroupId", groupId.ToString())
                .Replace("TextContent", textContent);
            string result = HttpRequestHelper.Request(urlToGroup(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToGroup(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 分组进行群发（语音消息）
        /// </summary>
        /// <param name="groupId">群发到的分组的group_id</param>
        /// <param name="mediaId">用于群发的消息的media_id</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel VoiceToGroup(int groupId, string mediaId, string access_token)
        {
            if (groupId < 1 || string.IsNullOrEmpty(mediaId)) return new RequestResultBaseModel();
            string data = "{\"filter\":{ \"group_id\":\"GroupId\" }, \"voice\":{\"media_id\":\"MediaId\" }, \"msgtype\":\"voice\"}";
            data = data.Replace("GroupId", groupId.ToString())
                .Replace("MediaId", mediaId);
            string result = HttpRequestHelper.Request(urlToGroup(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToGroup(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 分组进行群发（图片消息）
        /// </summary>
        /// <param name="groupId">群发到的分组的group_id</param>
        /// <param name="mediaId">用于群发的消息的media_id</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel ImageToGroup(int groupId, string mediaId, string access_token)
        {
            if (groupId < 1 || string.IsNullOrEmpty(mediaId)) return new RequestResultBaseModel();
            string data = "{\"filter\":{ \"group_id\":\"GroupId\" }, \"image\":{\"media_id\":\"MediaId\" }, \"msgtype\":\"image\"}";
            data = data.Replace("GroupId", groupId.ToString())
                .Replace("MediaId", mediaId);
            string result = HttpRequestHelper.Request(urlToGroup(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToGroup(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        #endregion

        #region 发送消息-高级群发-上传图文消息素材
        /// <summary>
        /// 上传图文消息素材，一个图文消息支持1到10条图文
        /// </summary>
        /// <param name="articles">图文集合（支持1到10条图文）</param>
        /// <returns>ArticleResultModel</returns>
        public static ArticleResultModel UploadNews(List<ArticleModel> articles, string access_token)
        {
            if (articles.Count < 1) return new ArticleResultModel();
            if (articles.Count > 10) throw new Exception("一个图文消息支持1到10条图文。");
            string url = Common.ApiUrl + string.Format("media/uploadnews?access_token={0}", access_token);
            string data = "{ \"articles\": ArticleModel}";
            data = data.Replace("ArticleModel", JsonHelper.Serialize<List<ArticleModel>>(articles));
            string result = HttpRequestHelper.Request(url, data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            ArticleResultModel model = JsonHelper.DeSerialize<ArticleResultModel>(result);

            return model != null ? model : new ArticleResultModel();
        }
        #endregion

        #region 根据OpenID列表群发
        /// <summary>
        /// 根据OpenID列表群发消息接口，http请求方式: POST
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        private static string urlToUserList(string access_token)
        {
            return Common.ApiUrl + string.Format("message/mass/send?access_token={0}", access_token);
        }
        /// <summary>
        /// 根据OpenID列表群发消息接口，http请求方式: POST
        /// </summary>
        //private static string urlToUserList = Common.ApiUrl + string.Format("message/mass/send?access_token={0}", access_token);

        /// <summary>
        /// 根据OpenID列表群发（文本消息）
        /// </summary>
        /// <param name="openid">OpenID列表</param>
        /// <param name="textContent">文本消息内容</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel TextToUser(string[] openid, string textContent, string access_token)
        {
            if (openid == null || openid.Length < 1 || string.IsNullOrEmpty(textContent)) return new RequestResultBaseModel();
            string data = "{\"touser\": OpenIdList, \"text\":{ \"content\": \"TextContent\"}, \"msgtype\":\"text\"}";
            data = data.Replace("OpenIdList", JsonHelper.Serialize<string[]>(openid))
                .Replace("TextContent", textContent);
            string result = HttpRequestHelper.Request(urlToUserList(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToUserList(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 根据OpenID列表群发（图文消息）
        /// </summary>
        /// <param name="openid">OpenID列表</param>
        /// <param name="mediaid">图文消息的media_id</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel NewsToUser(string[] openid, string mediaid, string access_token)
        {
            if (openid == null || openid.Length < 1 || string.IsNullOrEmpty(mediaid)) return new RequestResultBaseModel();
            string data = "{\"touser\": OpenIdList, \"mpnews\":{ \"media_id\": \"MediaId\"}, \"msgtype\":\"mpnews\"}";
            data = data.Replace("OpenIdList", JsonHelper.Serialize<string[]>(openid))
                .Replace("MediaId", mediaid);
            string result = HttpRequestHelper.Request(urlToUserList(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToUserList(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 根据OpenID列表群发（语音消息）
        /// </summary>
        /// <param name="openid">OpenID列表</param>
        /// <param name="mediaid">语音消息的media_id</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel VoiceToUser(string[] openid, string mediaid, string access_token)
        {
            if (openid == null || openid.Length < 1 || string.IsNullOrEmpty(mediaid)) return new RequestResultBaseModel();
            string data = "{\"touser\": OpenIdList, \"voice\":{ \"media_id\": \"MediaId\"}, \"msgtype\":\"voice\"}";
            data = data.Replace("OpenIdList", JsonHelper.Serialize<string[]>(openid))
                .Replace("MediaId", mediaid);
            string result = HttpRequestHelper.Request(urlToUserList(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToUserList(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 根据OpenID列表群发（图片消息）
        /// </summary>
        /// <param name="openid">OpenID列表</param>
        /// <param name="mediaid">图片消息的media_id</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel ImageToUser(string[] openid, string mediaid, string access_token)
        {
            if (openid == null || openid.Length < 1 || string.IsNullOrEmpty(mediaid)) return new RequestResultBaseModel();
            string data = "{\"touser\": OpenIdList, \"image\":{ \"media_id\": \"MediaId\"}, \"msgtype\":\"image\"}";
            data = data.Replace("OpenIdList", JsonHelper.Serialize<string[]>(openid))
                .Replace("MediaId", mediaid);
            string result = HttpRequestHelper.Request(urlToUserList(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToUserList(access_token);
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        #endregion

        #endregion

        #region 删除群发
        /// <summary>
        /// 删除群发
        /// </summary>
        /// <param name="msgid">发送出去的消息ID</param>
        /// <returns>RequestResultBaseModel</returns>
        public static RequestResultBaseModel Delete(long msgid, string access_token)
        {
            string url = Common.ApiUrl + string.Format("message/mass/delete?access_token={0}", access_token);
            if (msgid < 0) return new RequestResultBaseModel();
            string data = "{\"msgid\": MsgId}";
            data = data.Replace("MsgId", msgid.ToString());
            string result = HttpRequestHelper.Request(url, data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = url;
                model.date = data;
            }
            return model != null ? model : new RequestResultBaseModel();
        }
        #endregion

        #region 模板消息
        /// <summary>
        /// 发送模版消息接口，http请求方式: POST
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        private static string urlToTemplateMessage(string access_token)
        {
            return Common.ApiUrl + string.Format("message/template/send?access_token={0}", access_token);
        }

        /// <summary>
        /// 模版消息【待办工作提醒】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.WorkRemind msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "vEgkxw9Iva-u8UxpCcOq50dL63VD_t50rsFqv2yP2mI";

            string data = JsonHelper.Serialize<Models.TemplateMessage.WorkRemind>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.work.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >WorkRemind  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【订单支付成功通知】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.PaymentSuccess msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "tf_LJ-gkGlV_PYPoLwsFwevCPyo7sZIB14GzJYhN-Xw";

            string data = JsonHelper.Serialize<Models.TemplateMessage.PaymentSuccess>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.keyword1.value,
                        keyword2 = msg.data.keyword2.value,
                        keyword3 = msg.data.keyword3.value,
                        keyword4 = msg.data.keyword4.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >PaymentSuccess  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【客户留言通知】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.CustomerMessageNotice msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "EeZccH5irngrlVdKYEdkCkHgnK_DAGPVTfz2b6cHAkg";

            string data = JsonHelper.Serialize<Models.TemplateMessage.CustomerMessageNotice>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.keyword1.value,
                        keyword2 = msg.data.keyword2.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >CustomerMessageNotice  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【论文收录结果通知】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.PapersIncludedNotice msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "PIXDLN1jdUE9S-uerKEcnjtdL2pAWu71dwDniuEsNa4";

            string data = JsonHelper.Serialize<Models.TemplateMessage.PapersIncludedNotice>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.paperName.value,
                        keyword2 = msg.data.name.value,
                        keyword3 = msg.data.result.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >PapersIncludedNotice  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【会议提醒】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.MeetingReminder msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "viPo0HRQLj_aKzzbievJtgQBV7d6A7tbvx_Q4Pcd4sM";

            string data = JsonHelper.Serialize<Models.TemplateMessage.MeetingReminder>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.Topic.value,
                        keyword2 = msg.data.Time.value,
                        keyword3 = msg.data.Address.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >MeetingReminder  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【会议提醒】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.Track msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "oogGbiEMJcfo0btT58VoqecYQjhpeBQNujnHneDrSJU";

            string data = JsonHelper.Serialize<Models.TemplateMessage.Track>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.keyword1.value,
                        keyword2 = msg.data.keyword2.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >Track  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【最新学术活动通知】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.DynamicNotice msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "3x7HeSvKNDsYJk0N_cOUqiW0SYX03jRUBA6876ixX9c";

            string data = JsonHelper.Serialize<Models.TemplateMessage.DynamicNotice>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.keyword1.value,
                        keyword2 = msg.data.keyword2.value,
                        keyword3 = msg.data.keyword3.value,
                        keyword4 = msg.data.keyword4.value,
                        keyword5 = msg.data.keyword5.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >DynamicNotice  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【活动报名成功通知】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.ActivitiesJoinNotice msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "5uzQfY8xUMj7G4OJtmQlL57Y1NZ8UovDCPrUUWQUZf0";

            string data = JsonHelper.Serialize<Models.TemplateMessage.ActivitiesJoinNotice>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.keyword1.value,
                        keyword2 = msg.data.keyword2.value,
                        keyword3 = msg.data.keyword3.value,
                        keyword4 = msg.data.keyword4.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >ActivitiesJoinNotice  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【最新学术交流意见反馈通知】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.ExchangeFeedbackNotice msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "7WW4mePITAAuJc1ZYhPjj929SXiktWowGbzGrPL2dxg";

            string data = JsonHelper.Serialize<Models.TemplateMessage.ExchangeFeedbackNotice>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.keyword1.value,
                        keyword2 = msg.data.keyword2.value,
                        keyword3 = msg.data.keyword3.value,
                        keyword4 = msg.data.keyword4.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >ExchangeFeedbackNotice  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【教授名单更新通知】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.ProfessorUpdateNotice msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "Cv0k1xb4IzZSXO__TG9rkhMXaJTZXOgWpRbAi5pTSHg";

            string data = JsonHelper.Serialize<Models.TemplateMessage.ProfessorUpdateNotice>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.keyword1.value,
                        keyword2 = msg.data.keyword2.value,
                        keyword3 = msg.data.keyword3.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >ProfessorUpdateNotice  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【新活动参与者提醒】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.ActivityParticipantsReminder msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "Ryxw4D7rqZHSoPJDeX7Kja_ZqNMYJokDkCHA3GMXM_M";

            string data = JsonHelper.Serialize<Models.TemplateMessage.ActivityParticipantsReminder>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.keyword1.value,
                        keyword2 = msg.data.keyword2.value,
                        keyword3 = msg.data.keyword3.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >ActivityParticipantsReminder  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【待处理事项通知】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.WorkNotice msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "_S_YOC9ctE0gc4PTkTd6t8QVX_SWlqQNxDV8RIsjhTg";

            string data = JsonHelper.Serialize<Models.TemplateMessage.WorkNotice>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.keyword1.value,
                        keyword2 = msg.data.keyword2.value,
                        keyword3 = msg.data.keyword3.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >WorkNotice  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        /// <summary>
        /// 模版消息【开奖结果通知】
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static RequestResultBaseModel Template(Models.TemplateMessage.LotteryResultsNotice msg, string access_token)
        {
            if (msg == null || string.IsNullOrEmpty(msg.touser)) return new RequestResultBaseModel();
            msg.template_id = !string.IsNullOrEmpty(msg.template_id) ? msg.template_id : "y1kZQW3w4JML4MkpPq2RjK0pzbxWLIGSFWqMaeq1GPY";

            string data = JsonHelper.Serialize<Models.TemplateMessage.LotteryResultsNotice>(msg);
            string result = HttpRequestHelper.Request(urlToTemplateMessage(access_token), data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
            RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
            if (model != null)
            {
                model.url = urlToTemplateMessage(access_token);
                model.date = data;

                #region 记录模版消息
                try
                {
                    WeChatHelper4Net.Models.TemplateMessage.templatemessageHelper.Insert(new Models.TemplateMessage.templatemessage()
                    {
                        wxid = msg.touser,
                        templateid = msg.template_id,
                        msgid = (int)model.msgid,
                        errcode = model.errcode,
                        errmsg = model.errmsg,
                        sendwxid = Common.WeChatId,

                        first = msg.data.first.value,
                        remark = msg.data.remark.value,
                        keyword1 = msg.data.program.value,
                        keyword2 = msg.data.result.value,
                    });
                }
                catch (Exception Ex)
                {
                    LogHelper.Save(Ex, "WeChatHelper4Net.SendMessage > Template() >LotteryResultsNotice  记录模版消息错误！", "templatemessageHelper-Insert", LogTime.hour);
                }
                #endregion
            }
            return model != null ? model : new RequestResultBaseModel();
        }

        #endregion

    }
}
