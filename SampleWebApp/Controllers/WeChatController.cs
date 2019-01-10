using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeChatHelper4Net;
using WeChatHelper4Net.CustomService;
using WeChatHelper4Net.Extend;
using WeChatHelper4Net.Models;
using WeChatHelper4Net.Models.Menu;
using WeChatHelper4Net.Models.ReceivedMsg;

namespace SampleWebApp.Controllers
{
    /// <summary>
    /// 只用来用来与微信消息对接的控制器
    /// 开源社区：https://github.com/classbao/WeChatHelper4Net
    /// </summary>
    public class WeChatController : Controller
    {
        // GET: WeChat

        /// <summary>
        /// 自定义Token
        /// </summary>
        private readonly string Token = "Token123ByClassbao"; // ConfigurationManager.AppSettings["WeChatToken"];

        /// <summary>
        /// 接收自定义通知的管理员OpenID
        /// </summary>
        private static string administratorOpenID = ConfigHelper.GetAppSetting("administratorOpenID");
        private static string paymentNoticeOpenID = ConfigHelper.GetAppSetting("paymentNoticeOpenID");

        private static string showRequestResult(RequestResultBaseModel result)
        {
            if(result != null)
            {
                StringBuilder str = new StringBuilder();
                str.Append("result.msg_id=<span style=\"color:#45b97c;\">" + result.msg_id + "</span>，");
                str.Append("result.errcode=<span style=\"color:#45b97c;\">" + result.errcode + "</span>，");
                str.Append("result.errmsg=<span style=\"color:#45b97c;\">" + result.errmsg + "</span><br />");
                str.Append("result.url=<span style=\"color:#45b97c;\">" + result.url + "</span><br />");
                str.Append("result.date=<span style=\"color:#45b97c;\">" + result.date + "</span>");
                return str.ToString();
            }
            return string.Empty;
        }

        private static string AutoWelcomeMessage
        {
            get
            {
                StringBuilder _WelcomeMessage = new StringBuilder();
                _WelcomeMessage.Append("空调好用，但是价格高、费用贵啊，最关键的是没有！/发呆");
                _WelcomeMessage.Append("凉席睡一会就成铁板烧！电扇不能常吹，也不凉快！晚上燥热，睡不好觉，对身体健康造成了极大的影响，比如代谢变慢、容易衰老、皮肤暗淡、对心脏不好、脾气暴躁，关键是第二天上课会困，影响走上人生巅峰 /抓狂");
                _WelcomeMessage.Append("\n想迎娶白富美，征服高富帅！/傲慢");
                _WelcomeMessage.Append("\n炎炎夏日，让我一起抗日吧！/得意");
                _WelcomeMessage.Append("\n回复：");
                _WelcomeMessage.Append("\n【介绍】产品介绍");
                _WelcomeMessage.Append("\n【抽奖】抽惊喜大礼");
                _WelcomeMessage.Append("\n【价格】价格详情");
                _WelcomeMessage.Append("\n【代理】加入我们");
                _WelcomeMessage.Append("\n【体验】体验凉飕飕的感觉");

                //_WelcomeMessage.Append("欢迎来到壹学者 /微笑\n\r");
                //_WelcomeMessage.Append("壹学者是老师、同学、学术爱好者必备的学习和科研工具。\n\r");
                //_WelcomeMessage.Append("百万论文|千部图书|科研工具|学术社区\n\r");
                //_WelcomeMessage.Append("<a href='" + WeiXinAPI.Common.WeiXinDomainName + "'>点击“进入壹学者”体验</a>\n\r");
                //_WelcomeMessage.Append("(安卓系统手机，可点击右上角将“壹学者”添加到手机桌面)\n\r");

                //_WelcomeMessage.Append("欢迎加入壹学者！/微笑\n\r");
                //_WelcomeMessage.Append("君子壹教  凝子壹学\n\r");
                //_WelcomeMessage.Append("百万论文|千部图书|科研工具|学术社区\n\r");
                //_WelcomeMessage.Append("<a href='" + WeiXinAPI.Common.WeiXinDomainName + "'>点击“进入壹学者”体验</a>\n\r");
                //_WelcomeMessage.Append("(安卓系统手机，可点击右上角将“壹学者”添加到手机桌面)");
                //_WelcomeMessage.Append("\n\r<a href='" + WeiXinAPI.Common.WeiXinDomainName + "/Activity/CashCouponShop'>点击参与活动，免费领取智能手环</a>");
                return _WelcomeMessage.ToString();
            }
        }

        /// <summary>
        /// 响应消息推送，接收微信服务器推送来的请求或消息或事件或结果
        /// </summary>
        /// <returns></returns>
        //[HttpGet, HttpPost]
        public ActionResult Index()
        {
            if("GET" == Request.HttpMethod)
            {
                //微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.classbao.com/WeChat
                LogHelper.Save(nameof(Index) + "> 当前AbsoluteUri=" + Request.Url.AbsoluteUri + "，ContentEncoding=" + Request.ContentEncoding.ToString() + "，ContentType=" + Request.ContentType + "，RequestType=" + Request.RequestType + "，HttpMethod=" + Request.HttpMethod + "，UserHostAddress=" + Request.UserHostAddress + "，UserHostName=" + Request.UserHostName, nameof(WeChatController), LogType.Common, LogTime.day);
                #region 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url
                var signature = Request["signature"];
                var timestamp = Request["timestamp"];
                var nonce = Request["nonce"];
                var echostr = Request["echostr"];

                var result = ValidationToken.Validation(signature, timestamp, nonce, echostr, Token);
                if(!string.IsNullOrWhiteSpace(result))
                {
                    return Content(result);
                }
                else
                {
                    return Content("如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
                }
                #endregion
            }
            else if("POST" == Request.HttpMethod)
            {
                string receivedPostString = ReceivingMessage.StreamToString(Request.InputStream); //System.Web.HttpContext.Current.Request.InputStream;
                //log.Info("Index，receivedPostString=<code><pre>" + receivedPostString + "</pre></code>");
                if(!string.IsNullOrWhiteSpace(receivedPostString))
                {
                    string MsgType = ReceivingMessage.GetMsgType(receivedPostString);
                    //log.Info("Index，MsgType=" + MsgType);
                    switch(MsgType)
                    {
                        #region 接收普通消息
                        case "text":
                            #region MyRegion
                            try
                            {
                                var msg = XmlHelper.DeSerialize<ReceivedTextModel>(receivedPostString);
                                //log.Info("Index，MsgId=" + msg.MsgId + "，MsgType=" + msg.MsgType + "，FromUserName=" + msg.FromUserName + "，ToUserName=" + msg.ToUserName + "，CreateTime=" + msg.CreateTime + "，Content=" + msg.Content);
                                Task.Factory.StartNew(() =>
                                {
                                    AsyncManager.OutstandingOperations.Increment();
                                    //Models.wxCustomHandler.TextMessage(msg);
                                    AsyncManager.OutstandingOperations.Decrement();
                                });
                            }
                            catch(Exception Ex)
                            {
                                //log.Error(string.Format("Index > MsgType={0}", MsgType, receivedPostString), Ex);
                            }
                            #endregion
                            break;
                        case "image":
                            #region MyRegion
                            try
                            {
                                var msg = XmlHelper.DeSerialize<ReceivedImageModel>(receivedPostString);
                                Task.Factory.StartNew(() =>
                                {
                                    AsyncManager.OutstandingOperations.Increment();
                                    /*
                                    var result = SendMessage.NewsToUser(msg.FromUserName, new List<NewsModel>() {
                                        new NewsModel() { title = "已经收到图片消息", description = msg.PicUrl, picurl = msg.PicUrl, url = Common.WeChatDomainName + Url.Action("receivePic","Activity",new{PicUrl=HttpUtility.UrlPathEncode(msg.PicUrl)}) }
                                    }, Models.TokenOrTicket.GetAccessToken().access_token);
                                    */

                                    // 图片活动·开始
                                    /*
                                    int id = ActivityController.GetActivityscore(msg, new Onexz.Model.APIModel() { custom = "YMCS2015" });
                                    if (id <= 0)
                                    {
                                        //1，下载图片保存到本地服务器
                                        Onexz.Model.APIModel saveImageApi = DownloadMultimediaFiles(msg.MediaId, msg.MsgType, "OfficialActivity");
                                        if (saveImageApi != null && saveImageApi.errno == 0 && !string.IsNullOrWhiteSpace(saveImageApi.data.ToString()))
                                        {
                                            //2，图片记录到活动表
                                            saveImageApi.custom = "YMCS2015"; //“壹抹春色”手机摄影大赛
                                            id = ActivityController.activityscoreRecordFile(msg, saveImageApi);
                                        }
                                        if (id < 1)
                                        {
                                            SendMessage.TextToUser(msg.FromUserName, "图片文件接收失败，请稍后再试。", Models.TokenOrTicket.GetAccessToken().access_token);
                                        }
                                        else
                                        {
                                            SendMessage.TextToUser(administratorOpenID, Common.WeChatName + "收到一条图片消息，" + Common.ConvertTime(msg.CreateTime) + "\r\nMediaId=" + msg.MediaId + "\r\nPicUrl=" + msg.PicUrl, Models.TokenOrTicket.GetAccessToken().access_token);
                                        }
                                        // 图片活动·结束 
                                        log.Debug("回复完成>> OpenID=" + msg.FromUserName + "    CreateTime=" + msg.CreateTime + "    PicUrl=" + msg.PicUrl + "    回复类型=发送图文消息<br />");
                                    }
                                    if (id > 0)
                                    {
                                        //该图片已经记录
                                        var result = SendMessage.NewsToUser(msg.FromUserName, new List<NewsModel>() {
                                            new NewsModel() { title = "我的摄影作品入选“壹抹春色”手机摄影大赛，美极啦！", description = "非常感谢参与2015年“壹抹春色”随手拍校园摄影大赛，你的作品美极了！", picurl = msg.PicUrl, url = Common.WeChatDomainName + Url.Action("YMCS2015","Activity",new{id=id}) }
                                        }, Models.TokenOrTicket.GetAccessToken().access_token);
                                    }
                                    */

                                    SendMsg.SendText(administratorOpenID, Common.ConvertTime(msg.CreateTime) + Common.WeChatName + "收到一条图片消息" + "\r\nMediaId=" + msg.MediaId + "\r\nPicUrl=" + msg.PicUrl, Models.WeChatTokenOrTicket.GetAccessToken().access_token);
                                    AsyncManager.OutstandingOperations.Decrement();
                                });
                            }
                            catch(Exception Ex)
                            {
                                //log.Error(string.Format("Index > MsgType={0}", MsgType, receivedPostString), Ex);
                            }
                            #endregion
                            break;
                        case "voice":
                            #region MyRegion
                            try
                            {
                                //var msg = WeiXinAPI.XmlHelper.DeSerialize<ReceivedVoiceModel>(receivedPostString);
                                Task.Factory.StartNew(() =>
                                {
                                    AsyncManager.OutstandingOperations.Increment();
                                    SendMsg.SendText(administratorOpenID, DateTime.Now + Common.WeChatName + "收到一条语音消息", Models.WeChatTokenOrTicket.GetAccessToken().access_token);
                                    AsyncManager.OutstandingOperations.Decrement();
                                });
                            }
                            catch(Exception Ex)
                            {
                                //log.Error(string.Format("Index > MsgType={0}", MsgType, receivedPostString), Ex);
                            }
                            #endregion
                            break;
                        case "video":
                            #region MyRegion
                            try
                            {
                                //var msg = WeiXinAPI.XmlHelper.DeSerialize<ReceivedVideoModel>(receivedPostString);
                                Task.Factory.StartNew(() =>
                                {
                                    AsyncManager.OutstandingOperations.Increment();
                                    SendMsg.SendText(administratorOpenID, DateTime.Now + Common.WeChatName + "收到一条视频消息", Models.WeChatTokenOrTicket.GetAccessToken().access_token);
                                    AsyncManager.OutstandingOperations.Decrement();
                                });
                            }
                            catch(Exception Ex)
                            {
                                //log.Error(string.Format("Index > MsgType={0}", MsgType, receivedPostString), Ex);
                            }
                            #endregion
                            break;
                        case "shortvideo":
                            #region MyRegion
                            try
                            {
                                //var msg = WeiXinAPI.XmlHelper.DeSerialize<ReceivedVideoModel>(receivedPostString);
                                Task.Factory.StartNew(() =>
                                {
                                    AsyncManager.OutstandingOperations.Increment();
                                    SendMsg.SendText(administratorOpenID, DateTime.Now + Common.WeChatName + "收到一条小视频消息", Models.WeChatTokenOrTicket.GetAccessToken().access_token);
                                    AsyncManager.OutstandingOperations.Decrement();
                                });
                            }
                            catch(Exception Ex)
                            {
                                //log.Error(string.Format("Index > MsgType={0}", MsgType, receivedPostString), Ex);
                            }
                            #endregion
                            break;
                        case "location":
                            #region MyRegion
                            try
                            {
                                /*
                                var msg = XmlHelper.DeSerialize<ReceivedLocationModel>(receivedPostString);
                                Task.Factory.StartNew(() =>
                                {
                                    AsyncManager.OutstandingOperations.Increment();
                                    UserLocationModel location = new UserLocationModel()
                                    {
                                        FromUserName = msg.FromUserName,
                                        Latitude = msg.Location_X,
                                        Longitude = msg.Location_Y,
                                        Precision = Convert.ToDecimal(msg.Scale),
                                        Location_X = msg.Location_X,
                                        Location_Y = msg.Location_Y,
                                        Scale = msg.Scale,
                                        Label = msg.Label,
                                        CreateTime = msg.CreateTime
                                    };
                                    CacheHelper.SetCache("1xuezhe_location_" + msg.FromUserName, location, 10);

                                    UserLocationModel locationCache = (UserLocationModel)CacheHelper.GetCache("1xuezhe_location_" + msg.FromUserName);
                                    if (locationCache != null)
                                    {
                                        StringBuilder _msg = new StringBuilder();
                                        _msg.AppendLine("您的当前地理位置信息：location");
                                        _msg.AppendLine("纬度：" + locationCache.Location_X);
                                        _msg.AppendLine("经度：" + locationCache.Location_Y);
                                        _msg.AppendLine("地图缩放大小：" + locationCache.Scale);
                                        _msg.AppendLine("地理位置信息：" + locationCache.Label);
                                        _msg.AppendLine("cachekey：" + "1xuezhe_location_" + msg.FromUserName);
                                        SendMessage.TextToUser(locationCache.FromUserName, _msg.ToString(), Models.TokenOrTicket.GetAccessToken().access_token);
                                    }
                                    //LogHelper.Save("ToUserName=" + msg.ToUserName + "    FromUserName=" + msg.FromUserName + "    Location_X=" + msg.Location_X + "    Location_Y=" + msg.Location_Y + "    Scale=" + msg.Scale + "    Label=" + msg.Label + "    CreateTime=" + msg.CreateTime, "地理位置消息", LogType.Report, LogTime.day);
                                    
                                    AsyncManager.OutstandingOperations.Decrement();
                                });
                                */
                            }
                            catch(Exception Ex)
                            {
                                //log.Error(string.Format("Index > MsgType={0}", MsgType, receivedPostString), Ex);
                            }
                            #endregion
                            break;
                        case "link":
                            #region MyRegion
                            try
                            {
                                //var msg = WeiXinAPI.XmlHelper.DeSerialize<ReceivedLinkModel>(receivedPostString);
                                Task.Factory.StartNew(() =>
                                {
                                    AsyncManager.OutstandingOperations.Increment();
                                    SendMsg.SendText(administratorOpenID, DateTime.Now + Common.WeChatName + "收到一条链接消息", Models.WeChatTokenOrTicket.GetAccessToken().access_token);
                                    AsyncManager.OutstandingOperations.Decrement();
                                });
                            }
                            catch(Exception Ex)
                            {
                                //log.Error(string.Format("Index > MsgType={0}", MsgType, receivedPostString), Ex);
                            }
                            #endregion
                            break;
                        #endregion
                        #region 接收事件推送
                        case "event":
                            string Event = ReceivingMessage.GetEvent(receivedPostString);
                            switch(Event)
                            {
                                case "subscribe":
                                    #region MyRegion
                                    if(!ReceivingMessage.ContainQrscene(receivedPostString))
                                    {
                                        try
                                        {
                                            var msg = XmlHelper.DeSerialize<ReceivedSubscribeModel>(receivedPostString);
                                            //log.Debug("Index >MsgType=" + msg.MsgType + "，Event=" + msg.Event + "，ToUserName=" + msg.ToUserName + "，FromUserName=" + msg.FromUserName + "，CreateTime=" + msg.CreateTime);
                                            Task.Factory.StartNew(() =>
                                            {
                                                AsyncManager.OutstandingOperations.Increment();
                                                //new Models.wxUserService().handleSubscribe(msg.FromUserName);
                                                AsyncManager.OutstandingOperations.Decrement();

                                                AsyncManager.OutstandingOperations.Increment();
                                                SendMsg.SendText(administratorOpenID, Common.ConvertTime(msg.CreateTime) + Common.WeChatName + "收到一个用户关注：\r\n" + msg.FromUserName, Models.WeChatTokenOrTicket.GetAccessToken().access_token);
                                                AsyncManager.OutstandingOperations.Decrement();
                                            });

                                            return Content(PassiveReply.ReplyText(new WeChatHelper4Net.Models.PassiveReply.ReplyTextModel()
                                            {
                                                ToUserName = msg.FromUserName,
                                                CreateTime = Common.ConvertTime(DateTime.Now),
                                                Content = AutoWelcomeMessage
                                            }));
                                        }
                                        catch(Exception Ex)
                                        {
                                            //log.Error(string.Format("Index > MsgType={0} > Event={1} > receivedPostString={2}", MsgType, Event, receivedPostString), Ex);
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            var msg = XmlHelper.DeSerialize<ReceivedScanModel>(receivedPostString);
                                            //log.Debug("Index >MsgType=" + msg.MsgType + "，Event=" + msg.Event + "，EventKey=" + msg.EventKey + "，Ticket=" + msg.Ticket + "，ToUserName=" + msg.ToUserName + "，FromUserName=" + msg.FromUserName + "，CreateTime=" + msg.CreateTime);
                                            string qrscene = msg.EventKey.Replace("qrscene_", "").Trim();
                                            Task.Factory.StartNew(() =>
                                            {
                                                AsyncManager.OutstandingOperations.Increment();
                                                //new Models.wxUserService().handleSubscribe(msg.FromUserName, qrscene);
                                                AsyncManager.OutstandingOperations.Decrement();

                                                AsyncManager.OutstandingOperations.Increment();
                                                SendMsg.SendText(administratorOpenID, Common.ConvertTime(msg.CreateTime) + Common.WeChatName + "收到一个扫码关注：\r\n" + msg.FromUserName, Models.WeChatTokenOrTicket.GetAccessToken().access_token);
                                                AsyncManager.OutstandingOperations.Decrement();
                                            });

                                            return Content(PassiveReply.ReplyText(new WeChatHelper4Net.Models.PassiveReply.ReplyTextModel()
                                            {
                                                ToUserName = msg.FromUserName,
                                                CreateTime = Common.ConvertTime(DateTime.Now),
                                                Content = AutoWelcomeMessage
                                            }));
                                        }
                                        catch(Exception Ex)
                                        {
                                            //log.Error(string.Format("Index > MsgType={0} > Event={1} > receivedPostString={2}", MsgType, Event, receivedPostString), Ex);
                                        }
                                    }
                                    #endregion
                                    break;
                                case "unsubscribe":
                                    #region MyRegion
                                    try
                                    {
                                        var msg = XmlHelper.DeSerialize<ReceivedSubscribeModel>(receivedPostString);
                                        //log.Debug("Index >MsgType=" + msg.MsgType + "，Event=" + msg.Event + "，ToUserName=" + msg.ToUserName + "，FromUserName=" + msg.FromUserName + "，CreateTime=" + msg.CreateTime);
                                        Task.Factory.StartNew(() =>
                                        {
                                            AsyncManager.OutstandingOperations.Increment();
                                            //new Models.wxUserService().handleUnsubscribe(msg.FromUserName);
                                            AsyncManager.OutstandingOperations.Decrement();
                                        });

                                        return Content(PassiveReply.ReplyText(new WeChatHelper4Net.Models.PassiveReply.ReplyTextModel()
                                        {
                                            ToUserName = msg.FromUserName,
                                            CreateTime = Common.ConvertTime(DateTime.Now),
                                            Content = "是什么原因让您决定取消订阅（关注）" + Common.WeChatName + "？"
                                        }));
                                    }
                                    catch(Exception Ex)
                                    {
                                        //log.Error(string.Format("Index > MsgType={0} > Event={1} > receivedPostString={2}", MsgType, Event, receivedPostString), Ex);
                                    }
                                    #endregion
                                    break;
                                case "SCAN":
                                    #region MyRegion
                                    //try
                                    //{
                                    //    var msg = XmlHelper.DeSerialize<ReceivedScanModel>(receivedPostString);
                                    //    int scene_id = Convert.ToInt32(msg.EventKey);
                                    //    string ticket = msg.Ticket;
                                    //    Task.Factory.StartNew(() =>
                                    //    {
                                    //        AsyncManager.OutstandingOperations.Increment();
                                    //        //...
                                    //        AsyncManager.OutstandingOperations.Decrement();
                                    //        //return;
                                    //    });

                                    //    SendMessage.TextToUser(msg.FromUserName, "欢迎您扫描我们的二维码", Models.TokenOrTicket.GetAccessToken().access_token);
                                    //}
                                    //catch(Exception Ex)
                                    //{
                                    //    //log.Error(string.Format("Index > MsgType={0} > Event={1} > receivedPostString={2}", MsgType, Event, receivedPostString), Ex);
                                    //}
                                    #endregion
                                    break;
                                case "LOCATION":
                                    #region MyRegion
                                    /*
                                    try
                                    {
                                        var msg = XmlHelper.DeSerialize<ReceivedLOCATIONModel>(receivedPostString);
                                        UserLocationModel location = new UserLocationModel()
                                        {
                                            FromUserName = msg.FromUserName,
                                            Latitude = msg.Latitude,
                                            Longitude = msg.Longitude,
                                            Precision = msg.Precision,
                                            Location_X = msg.Latitude,
                                            Location_Y = msg.Longitude,
                                            Scale = Convert.ToInt32(msg.Precision),
                                            Label = string.Empty,
                                            CreateTime = msg.CreateTime
                                        };
                                        CacheHelper.SetCache("1xuezhe_location_" + msg.FromUserName, location, 10);
                                        
                                        UserLocationModel LOCATIONCache = (UserLocationModel)CacheHelper.GetCache("1xuezhe_location_" + msg.FromUserName);
                                        if(LOCATIONCache != null)
                                        {
                                            StringBuilder _msg = new StringBuilder();
                                            _msg.AppendLine("您的当前地理位置信息：LOCATION");
                                            _msg.AppendLine("纬度：" + LOCATIONCache.Latitude);
                                            _msg.AppendLine("经度：" + LOCATIONCache.Longitude);
                                            _msg.AppendLine("精度：" + LOCATIONCache.Precision);
                                            _msg.AppendLine("Scale：" + LOCATIONCache.Scale);
                                            _msg.AppendLine("FromUser：" + LOCATIONCache.FromUserName);
                                            _msg.AppendLine("cachekey：" + "1xuezhe_location_" + msg.FromUserName);
                                            SendMessage.TextToUser(LOCATIONCache.FromUserName, _msg.ToString(), Models.TokenOrTicket.GetAccessToken().access_token);
                                        }
                                        LogHelper.Save("ToUserName=" + msg.ToUserName + "    FromUserName=" + msg.FromUserName + "    Latitude=" + msg.Latitude + "    Longitude=" + msg.Longitude + "    Precision=" + msg.Precision + "    CreateTime=" + msg.CreateTime, "上报地理位置事件", LogType.Report, LogTime.day);
                                        
                                    }
                                    catch(Exception Ex)
                                    {
                                        //log.Error(string.Format("Index > MsgType={0} > Event={1} > receivedPostString={2}", MsgType, Event, receivedPostString), Ex);
                                    }
                                    */
                                    #endregion
                                    break;
                                case "CLICK":
                                    #region MyRegion
                                    try
                                    {
                                        var msg = XmlHelper.DeSerialize<ReceivedClickModel>(receivedPostString);
                                        //log.Debug("Index >MsgType=" + msg.MsgType + "，Event=" + msg.Event + "，EventKey=" + msg.EventKey + "，ToUserName=" + msg.ToUserName + "，FromUserName=" + msg.FromUserName + "，CreateTime=" + msg.CreateTime);
                                        Task.Factory.StartNew(() =>
                                        {
                                            AsyncManager.OutstandingOperations.Increment();
                                            //Models.wxCustomHandler.ClickEventHandler(msg);
                                            AsyncManager.OutstandingOperations.Decrement();

                                            //AsyncManager.OutstandingOperations.Increment();
                                            SendMsg.SendText(msg.FromUserName, "您点击了菜单，EventKey=" + msg.EventKey, Models.WeChatTokenOrTicket.GetAccessToken().access_token);
                                            //AsyncManager.OutstandingOperations.Decrement();
                                        });
                                    }
                                    catch(Exception Ex)
                                    {
                                        //log.Error(string.Format("Index > MsgType={0} > Event={1} > receivedPostString={2}", MsgType, Event, receivedPostString), Ex);
                                    }
                                    #endregion
                                    break;
                                case "VIEW":
                                    #region MyRegion
                                    //try
                                    //{
                                    //    var msg = XmlHelper.DeSerialize<ReceivedViewModel>(receivedPostString);
                                    //}
                                    //catch(Exception Ex)
                                    //{
                                    //    //log.Error(string.Format("Index > MsgType={0} > Event={1} > receivedPostString={2}", MsgType, Event, receivedPostString), Ex);
                                    //}
                                    #endregion
                                    break;

                                case "MASSSENDJOBFINISH":
                                    #region MyRegion
                                    try
                                    {
                                        var msg = XmlHelper.DeSerialize<ReceivedMassSensJobFinishModel>(receivedPostString);
                                        //log.Debug("Index >MsgType=" + msg.MsgType + "，Event=" + msg.Event + "，ToUserName=" + msg.ToUserName + "，FromUserName=" + msg.FromUserName + "，CreateTime=" + msg.CreateTime);
                                        Task.Factory.StartNew(() =>
                                        {
                                            AsyncManager.OutstandingOperations.Increment();
                                            SendMsg.SendText(administratorOpenID, Common.ConvertTime(msg.CreateTime) + Common.WeChatName + "收到事件推送群发结果：\r\n群发的结果：" + msg.Status + "\r\n粉丝数：" + msg.TotalCount + "\r\n准备发送的粉丝数：" + msg.FilterCount + "\r\n发送成功的粉丝数：" + msg.SentCount + "\r\n发送失败的粉丝数：" + msg.ErrorCount, Models.WeChatTokenOrTicket.GetAccessToken().access_token);
                                            AsyncManager.OutstandingOperations.Decrement();
                                        });
                                    }
                                    catch(Exception Ex)
                                    {
                                        //log.Error(string.Format("Index > MsgType={0} > Event={1} > receivedPostString={2}", MsgType, Event, receivedPostString), Ex);
                                    }
                                    #endregion
                                    break;

                                case "TEMPLATESENDJOBFINISH":
                                    #region MyRegion
                                    /*
                                    try
                                    {
                                        var msg = XmlHelper.DeSerialize<ReceivedTemplateSensJobFinishModel>(receivedPostString);
                                        //templatemessage templatemessage = BLLRepository.TemplatemessageBLL.GetModel((int)msg.MsgID, msg.ToUserName);
                                        if(msg.Status == "success") //送达成功
                                        {
                                            //LogHelper.Save("送达成功    Status=" + msg.Status + "    MsgID=" + msg.MsgID + "    FromUserName=" + msg.FromUserName + "    ToUserName=" + msg.ToUserName + "    MsgType=" + msg.MsgType + "    Event=" + msg.Event + "    CreateTime=" + msg.CreateTime, "模版消息发送任务完成后事件推送", WeiXinAPI.LogType.Report, WeiXinAPI.LogTime.day);
                                            if(templatemessage != null && templatemessage.id > 0)
                                            {
                                                templatemessage.status = msg.Status;
                                            }
                                        }
                                        else if(msg.Status == "failed:user block") //送达由于用户拒收（用户设置拒绝接收公众号消息）而失败
                                        {
                                            if(templatemessage != null && templatemessage.id > 0)
                                            {
                                                templatemessage.status = msg.Status;
                                            }

                                            SendMessage.TextToUser(administratorOpenID, "模版消息被用户拒收了。" + "\r\nMsgID=" + msg.MsgID + "\r\nFromUserName=" + msg.FromUserName + "\r\nCreateTime=" + Common.ConvertTime(msg.CreateTime), Models.TokenOrTicket.GetAccessToken().access_token);
                                            LogHelper.Save("送达由于用户拒收    Status=" + msg.Status + "    MsgID=" + msg.MsgID + "    FromUserName=" + msg.FromUserName + "    ToUserName=" + msg.ToUserName + "    MsgType=" + msg.MsgType + "    Event=" + msg.Event + "    CreateTime=" + msg.CreateTime, "模版消息发送任务完成后事件推送", LogType.Report, LogTime.day);
                                        }
                                        else if(msg.Status == "failed: system failed") //送达由于其他原因失败
                                        {
                                            if(templatemessage != null && templatemessage.id > 0)
                                            {
                                                templatemessage.status = msg.Status;
                                            }

                                            LogHelper.Save("送达由于其他原因失败    Status=" + msg.Status + "    MsgID=" + msg.MsgID +"    FromUserName=" + msg.FromUserName + "    ToUserName=" +msg.ToUserName + "    MsgType=" + msg.MsgType + "    Event=" +msg.Event + "    CreateTime=" + msg.CreateTime, "模版消息发送任务完成后事件推送",LogType.Report, LogTime.day);
                                        }
                                        else
                                        {
                                            if(templatemessage != null && templatemessage.id > 0)
                                            {
                                                templatemessage.status = msg.Status;
                                            }

                                            LogHelper.Save("送达由于未知原因失败    Status=" + msg.Status + "    MsgID=" + msg.MsgID +"    FromUserName=" + msg.FromUserName + "    ToUserName=" +msg.ToUserName + "    MsgType=" + msg.MsgType + "    Event=" +msg.Event + "    CreateTime=" + msg.CreateTime, "模版消息发送任务完成后事件推送",LogType.Report, LogTime.day);
                                        }

                                        if(templatemessage != null && templatemessage.id > 0)
                                        {
                                            templatemessage.eventname = msg.Event;
                                            templatemessage.createtime = Common.ConvertTime(msg.CreateTime);

                                            BLLRepository.TemplatemessageBLL.Update(templatemessage);
                                        }
                                    }
                                    catch(Exception Ex)
                                    {
                                        //log.Error(string.Format("Index > MsgType={0} > Event={1} > receivedPostString={2}", MsgType, Event, receivedPostString), Ex);
                                    }
                                    */
                                    #endregion
                                    break;

                                default:
                                    //log.Warn(string.Format("Index > MsgType={0} > Event={1} > receivedPostString={2}", MsgType, Event, receivedPostString), new Exception("未识别的微信推送事件类型"));
                                    break;
                            }
                            break;
                        #endregion

                        default:
                            //log.Warn("Index > MsgType=" + MsgType + " receivedPostString=" + receivedPostString, new Exception("未识别的微信推送消息类型"));
                            break;
                    }
                }
                else
                {
                    //log.Error("Index > receivedPostString=" + receivedPostString, new Exception("微信推送消息receivedPostString异常"));
                }
            }
            else
            {
                LogHelper.Save(new HttpException("微信推送消息HttpMethod异常"), nameof(Index) + "> 当前AbsoluteUri=" + Request.Url.AbsoluteUri + "，HttpMethod=" + Request.HttpMethod + "，UserHostAddress=" + Request.UserHostAddress + "，UserHostName=" + Request.UserHostName, nameof(WeChatController), LogTime.day);
            }

            return Content(PassiveReply.ReplyEmpty());
        }
        
        /// <summary>
        /// 维权通知
        /// </summary>
        /// <returns></returns>
        public ActionResult MaintainLegalRights()
        {
            //log.Warn("MaintainLegalRights> 维权通知 当前AbsoluteUri=" + Request.Url.AbsoluteUri + "，ContentEncoding=" + Request.ContentEncoding.ToString() + "，ContentType=" + Request.ContentType + "，RequestType=" + Request.RequestType + "，HttpMethod=" + Request.HttpMethod + "，UserHostAddress=" + Request.UserHostAddress + "，UserHostName=" + Request.UserHostName);
            SendMsg.SendText(administratorOpenID, DateTime.Now.ToString("yyyy年MM月 dd日HH时mm分") + "\r\n收到一条维权通知。", Models.WeChatTokenOrTicket.GetAccessToken().access_token);
            return Content("");
        }

        /// <summary>
        /// 告警通知
        /// </summary>
        /// <returns></returns>
        public ActionResult WarningNotice()
        {
            //log.Warn("WarningNotice> 告警通知 当前AbsoluteUri=" + Request.Url.AbsoluteUri + "，ContentEncoding=" + Request.ContentEncoding.ToString() + "，ContentType=" + Request.ContentType + "，RequestType=" + Request.RequestType + "，HttpMethod=" + Request.HttpMethod + "，UserHostAddress=" + Request.UserHostAddress + "，UserHostName=" + Request.UserHostName);
            SendMsg.SendText(administratorOpenID, DateTime.Now.ToString("yyyy年MM月 dd日HH时mm分") + "\r\n收到一条告警通知。", Models.WeChatTokenOrTicket.GetAccessToken().access_token);
            return Content("");
        }

        /// <summary>
        /// http://weixin.classbao.com/WeChat/GetAccessToken
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccessToken()
        {
            var AccessToken = Models.WeChatTokenOrTicket.GetAccessToken();
            return Json(AccessToken);
        }
        /// <summary>
        /// http://weixin.classbao.com/WeChat/GetJSConfig
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJSConfig()
        {
            var jsConfig = Models.WeChatTokenOrTicket.GetJSConfig(Request.Url.AbsoluteUri);
            /*
                jsConfig.appId
                jsConfig.timestamp
                jsConfig.nonceStr
                jsConfig.signature
            */

            return Content(JsonHelper.Serialize(Models.WeChatTokenOrTicket.GetJSConfig(Request.Url.AbsoluteUri)));
        }


        /// <summary>
        /// 创建自定义菜单：http://weixin.classbao.com/WeChat/CreateMenu
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMenu()
        {
            //var click = new WeChatHelper4Net.Models.Menu.Click("xiong", "aaa").ToJson();

            //var subbutton = new WeChatHelper4Net.Models.Menu.SubButton("二级菜单", new List<WeChatHelper4Net.Models.Menu.Base.SingleButton>()
            //{
            //    new Click("xiong", "aaa"),
            //    new View("xue", "bbb"),
            //});
            //string subbuttonJson = subbutton.ToJson();
            
            var button = new Button(new List<WeChatHelper4Net.Models.Menu.Base.BaseButton>()
            {
                new Click("今日歌曲", "V1001_TODAY_MUSIC"),
                new SubButton("菜单", new List<WeChatHelper4Net.Models.Menu.Base.SingleButton>()
                {
                    new View("搜索", "http://www.soso.com/"),
                    new MiniProgram() {  name="wxa",
                        url="http://mp.weixin.qq.com",
                        appid="wx286b93c14bbf93aa",
                        pagepath="pages/lunar/index" },
                    new Click("赞一下我们", "V1001_GOOD"),
                    new Pic_weixin("相册","rselfmenu_1_2",new List<WeChatHelper4Net.Models.Menu.Base.SingleButton>()
                        {
                            new Click("相册111", "xc111"),
                            new View("相册222", "xc222"),
                        })
                })
            });

            string access_token = Models.WeChatTokenOrTicket.GetAccessToken().access_token;
            if(Menu.CreateMenu(button, access_token))
            {
                return Content("创建自定义菜单 成功。");
            }
            else
            {
                return Content("创建自定义菜单 失败！");
            }
        }
        /// <summary>
        /// 查询当前使用的自定义菜单的结构：http://weixin.classbao.com/WeChat/GetMenu
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenu()
        {
            string access_token = Models.WeChatTokenOrTicket.GetAccessToken().access_token;
            string menuJson = Menu.GetMenu(access_token);
            return Content(menuJson);
        }
        /// <summary>
        /// 删除当前使用的自定义菜单：http://weixin.classbao.com/WeChat/DeleteMenu
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteMenu()
        {
            string access_token = Models.WeChatTokenOrTicket.GetAccessToken().access_token;
            string result = Menu.DeleteMenu(access_token);
            return Content(result);
        }
        /// <summary>
        /// 获取自定义菜单配置：http://weixin.classbao.com/WeChat/GetCurrentSelfMenu
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCurrentSelfMenu()
        {
            string access_token = Models.WeChatTokenOrTicket.GetAccessToken().access_token;
            string menuJson = Menu.GetCurrentSelfMenu(access_token);
            return Content(menuJson);
        }


        #region 多媒体文件
        /// <summary>
        /// 获取经纬度
        /// </summary>
        /// <param name="wxid"></param>
        /// <returns></returns>
        public static WeChatHelper4Net.Models.UserLocationModel GetLocation(string wxid)
        {
            WeChatHelper4Net.Models.UserLocationModel model = new WeChatHelper4Net.Models.UserLocationModel();
            try
            {
                model = (WeChatHelper4Net.Models.UserLocationModel)WeChatHelper4Net.CacheHelper.GetCache("1xuezhe_location_" + wxid);
                if(model != null && !string.IsNullOrWhiteSpace(model.FromUserName))
                {
                    if(model.Location_X > 0m)
                    {
                        model.Latitude = model.Location_X;
                        model.Longitude = model.Location_Y;
                        model.Precision = model.Scale;
                    }
                }
            }
            catch(Exception Ex)
            {
                //log.Error("WeiXin >GetLocation  wxid=" + wxid + " msg=" + Ex.Message, Ex);
            }

            return model;
        }
        /*
        /// <summary>
        /// 下载多媒体文件
        /// 公众号可调用本接口来获取多媒体文件。请注意，视频文件不支持下载，调用该接口需http协议。（多个ID用英文半角逗号分隔）
        /// </summary>
        /// <param name="mediaid">已上传微信服务器的多媒体ID（多个ID用英文半角逗号分隔）</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb，主要用于视频与音乐格式的缩略图）</param>
        /// <param name="folder">文件在指定根目录下的存放文件夹路径（comment：评论/回复/讨论区；notes：笔记；topics：话题（争鸣/求助）；OfficialActivity：官方活动相关；默认Multimedia）</param>
        /// <returns>多媒体保存路径（多个ID用英文半角逗号分隔）</returns>
        public Model.APIModel DownloadMultimediaFiles(string mediaid, string type, string folder = "Multimedia")
        {
            log.Info("WeiXin > DownloadMultimediaFiles > mediaid=" + mediaid + "；type=" + type + "；folder=" + folder + "； 当前URL=" + Request.Url + " AbsoluteUri=" + Request.Url.AbsoluteUri);
            var api = new Model.APIModel() { };
            if(string.IsNullOrWhiteSpace(mediaid) || string.IsNullOrWhiteSpace(type))
            {
                api.status = 50001;
                api.message = "参数错误！";
            }
            else
            {
                DateTime now = DateTime.Now;
                switch(folder)
                {
                    case "comment": folder = "comment"; break; //评论&回复&讨论区
                    case "notes": folder = "notes"; break; //笔记
                    case "topics": folder = "topics"; break; //话题（争鸣/求助）
                    case "OfficialActivity": folder = "OfficialActivity"; break; //官方活动相关
                    default: folder = "Multimedia"; break; //默认的多媒体下载文件夹（例如："Multimedia"或者"Multimedia\\image"等格式。）
                }
                string physicalFolder = Common.FilterWord.Replace(Lib.ConfigHelper.GetAppSetting("UploadFileRootFolder"), @"[\\\s]+$", "") + "\\" + Common.FilterWord.Replace(folder, @"[\\\s]+$", "");
                if(type == "image")
                    physicalFolder += "\\image";
                else if(type == "thumb")
                    physicalFolder += "\\thumb";
                else if(type == "voice")
                    physicalFolder += "\\voice";
                else if(type == "video")
                    physicalFolder += "\\video";
                else
                    physicalFolder += "\\other";
                physicalFolder += "\\" + now.ToString("yyyyMM");

                string savepath = "";
                try
                {
                    long fileSize;
                    savepath = WeChatHelper4Net.Multimedia.DownloadMultimedia(mediaid, physicalFolder, "", out fileSize, Models.TokenOrTicket.GetAccessToken().access_token);
                }
                catch(Exception Ex)
                {
                    log.Error("WeiXin > DownloadMultimediaFiles > mediaid=" + mediaid + "；type=" + type + "；folder=" + folder + "； 当前URL=" + Request.Url + " AbsoluteUri=" + Request.Url.AbsoluteUri, Ex);
                }

                if(!string.IsNullOrWhiteSpace(savepath))
                {
                    api.status = 0;
                    string[] savepathList = savepath.Split(new Char[] { ',' });
                    if(savepathList != null && savepathList.Length > 0)
                    {
                        StringBuilder resultpath = new StringBuilder();
                        foreach(string path in savepathList)
                        {
                            if(string.IsNullOrWhiteSpace(path) || Common.FilterWord.IsMatch(path, @"\s*(,|，)\s*"))
                                continue;

                            bool flag_small = false;
                            string suffixName = System.IO.Path.GetExtension(path).ToLower();
                            if("|.jpg|.jpeg|.png|.gif|.bmp|.tiff|".IndexOf(suffixName) > 0)
                            {
                                string path_small = path.Replace(suffixName, "_small" + suffixName);
                                flag_small = Lib.ImageHelper.MakeSmallImage(path, path_small, 240, Lib.ImageHelper.LimitSideMode.Auto, 75);
                                if(flag_small)
                                {
                                    resultpath.AppendFormat("{0},",
                                                                    Common.FilterWord.Replace(
                                                                        Common.FilterWord.Replace(
                                                                        path_small
                                                                        , @".*UploadFiles"
                                                                        , "/UploadFiles")
                                                                    , @"\\"
                                                                    , "/"
                                                                    )
                                                                );
                                }
                            }

                            if(!flag_small)
                            {
                                resultpath.AppendFormat("{0},",
                                                                Common.FilterWord.Replace(
                                                                    Common.FilterWord.Replace(
                                                                    path
                                                                    , @".*UploadFiles"
                                                                    , "/UploadFiles")
                                                                , @"\\"
                                                                , "/"
                                                                )
                                                            );
                            }
                        }
                        api.Data = resultpath.ToString().TrimEnd(new Char[] { ',' });
                    }
                    api.message = mediaid;
                }
                else
                {
                    api.status = 50002;
                    api.message = "下载失败！";
                }
            }
            return api;
        }

        /// <summary>
        /// 下载多媒体文件
        /// 公众号可调用本接口来获取多媒体文件。请注意，视频文件不支持下载，调用该接口需http协议。（多个ID用英文半角逗号分隔）
        /// </summary>
        /// <param name="mediaid">媒体文件上传微信后，获取的mediaid标识</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb，主要用于视频与音乐格式的缩略图）</param>
        /// <param name="folder">文件在指定根目录下的存放文件夹路径（comment：评论&回复&讨论区；notes：笔记；topics：话题（争鸣/求助）；OfficialActivity：官方活动相关；默认Multimedia）</param>
        /// <returns>多媒体保存路径（多个ID用英文半角逗号分隔）</returns>
        [HttpGet]
        public ActionResult asyncDownloadMultimediaFiles(string mediaid, string type, string folder)
        {
            //log.Info("WeiXin > asyncDownloadMultimediaFiles 当前URL=" + Request.Url + " AbsoluteUri=" + Request.Url.AbsoluteUri);
            var api = DownloadMultimediaFiles(mediaid, type, folder);
            return Json(api, "text/html", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }
        */
        #endregion

    }
}