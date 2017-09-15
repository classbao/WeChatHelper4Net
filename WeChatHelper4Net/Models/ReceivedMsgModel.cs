using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2016-12-10
 */
namespace WeChatHelper4Net.Models.ReceivedMsg
{
    /// <summary>
    /// 调用API接收事件推送返回结果基础模型
    /// </summary>
    [Serializable]
    //[DataContract(Name = "xml")]
    //[XmlRoot("xml")]
    public class ReceivedMsgModel
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(ToUserName))]
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(FromUserName))]
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(CreateTime))]
        public int CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(MsgType))]
        public string MsgType { get; set; }
    }

    #region 接收普通消息
    [Serializable]
    //[DataContract(Name = "MsgId")]
    //[XmlRoot("MsgId")]
    public class MsgIdMember : ReceivedMsgModel
    {
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(MsgId))]
        public long MsgId { get; set; }
    }

    [Serializable]
    //[DataContract(Name = "MediaId")]
    //[XmlRoot("MediaId")]
    public class MediaIdMember : MsgIdMember
    {
        /// <summary>
        /// 媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(MediaId))]
        public string MediaId { get; set; }
    }

    /// <summary>
    /// 文本消息
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedTextModel : MsgIdMember
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Content))]
        public string Content { get; set; }
    }

    /// <summary>
    /// 图片消息
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedImageModel : MediaIdMember
    {
        /// <summary>
        /// 图片链接（由系统生成）
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(PicUrl))]
        public string PicUrl { get; set; }
    }

    /// <summary>
    /// 语音消息
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedVoiceModel : MediaIdMember
    {
        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Format))]
        public string Format { get; set; }

        /// <summary>
        /// 语音识别结果，UTF8编码
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Recognition))]
        public string Recognition { get; set; }
    }

    /// <summary>
    /// 视频消息，小视频消息
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedVideoModel : MediaIdMember
    {
        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(ThumbMediaId))]
        public string ThumbMediaId { get; set; }
    }

    /// <summary>
    /// 地理位置消息
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedLocationModel : MsgIdMember
    {
        /// <summary>
        /// 地理位置维度
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Location_X))]
        public decimal Location_X { get; set; }

        /// <summary>
        /// 地理位置经度
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Location_Y))]
        public decimal Location_Y { get; set; }

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Scale))]
        public int Scale { get; set; }

        /// <summary>
        /// 地理位置信息
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Label))]
        public string Label { get; set; }
    }

    /// <summary>
    /// 链接消息
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedLinkModel : MsgIdMember
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Title))]
        public string Title { get; set; }

        /// <summary>
        /// 消息描述
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Description))]
        public string Description { get; set; }

        /// <summary>
        /// 消息链接
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Url))]
        public string Url { get; set; }
    }


    /*
    string postTextStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1348831860</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[this is a test]]></Content><MsgId>1234567890123456</MsgId></xml>";
    var resultText = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedTextModel>(postTextStr);

    string postImageStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1348831860</CreateTime><MsgType><![CDATA[image]]></MsgType><PicUrl><![CDATA[this is a url]]></PicUrl><MediaId><![CDATA[media_id]]></MediaId><MsgId>1234567890123456</MsgId></xml>";
    var resultImage = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedImageModel>(postImageStr);

    string postVoiceStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1357290913</CreateTime><MsgType><![CDATA[voice]]></MsgType><MediaId><![CDATA[media_id]]></MediaId><Format><![CDATA[Format]]></Format><MsgId>1234567890123456</MsgId></xml>";
    var resultVoice = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedVoiceModel>(postVoiceStr);

    string postVideoStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1357290913</CreateTime><MsgType><![CDATA[video]]></MsgType><MediaId><![CDATA[media_id]]></MediaId><ThumbMediaId><![CDATA[thumb_media_id]]></ThumbMediaId><MsgId>1234567890123456</MsgId></xml>";
    var resultVideo = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedVideoModel>(postVideoStr);

    string postshortvideoStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1357290913</CreateTime><MsgType><![CDATA[shortvideo]]></MsgType><MediaId><![CDATA[media_id]]></MediaId><ThumbMediaId><![CDATA[thumb_media_id]]></ThumbMediaId><MsgId>1234567890123456</MsgId></xml>";
    var resultshortvideo = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedVideoModel>(postshortvideoStr);

    string postLocationStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1351776360</CreateTime><MsgType><![CDATA[location]]></MsgType><Location_X>23.134521</Location_X><Location_Y>113.358803</Location_Y><Scale>20</Scale><Label><![CDATA[位置信息]]></Label><MsgId>1234567890123456</MsgId></xml>";
    var resultLocation = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedLocationModel>(postLocationStr);

    string postLinkStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>1351776360</CreateTime><MsgType><![CDATA[link]]></MsgType><Title><![CDATA[公众平台官网链接]]></Title><Description><![CDATA[公众平台官网链接]]></Description><Url><![CDATA[url]]></Url><MsgId>1234567890123456</MsgId></xml>";
    var resultLink = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedLinkModel>(postLinkStr);
    */

    #endregion

    #region 接收事件推送
    [Serializable]
    //[DataContract(Name = "Event")]
    //[XmlRoot("Event")]
    public class EventMember : ReceivedMsgModel
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Event))]
        public string Event { get; set; }
    }

    [Serializable]
    //[DataContract(Name = "EventKey")]
    //[XmlRoot("EventKey")]
    public class EventKeyMember : EventMember
    {
        /// <summary>
        /// 事件KEY值
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(EventKey))]
        public string EventKey { get; set; }
    }


    /// <summary>
    /// 关注/取消关注事件（subscribe(订阅)、unsubscribe(取消订阅)）
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedSubscribeModel : EventMember
    {
    }

    /// <summary>
    /// 扫描带参数二维码事件
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedScanModel : EventKeyMember
    {
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Ticket))]
        public string Ticket { get; set; }
    }

    /// <summary>
    /// 上报地理位置事件
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedLOCATIONModel : EventMember
    {
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Latitude))]
        public decimal Latitude { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Longitude))]
        public decimal Longitude { get; set; }
        /// <summary>
        /// 地理位置精度
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Precision))]
        public decimal Precision { get; set; }
    }

    /// <summary>
    /// 自定义菜单事件（点击菜单拉取消息时的事件推送）
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedClickModel : EventKeyMember
    {
    }

    /// <summary>
    /// 自定义菜单事件（点击菜单跳转链接时的事件推送）
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedViewModel : EventKeyMember
    {
    }

    /*
    string postSubscribeStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[FromUser]]></FromUserName><CreateTime>123456789</CreateTime><MsgType><![CDATA[event]]></MsgType><Event><![CDATA[subscribe]]></Event></xml>";
    var resultSubscribe = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedSubscribeModel>(postSubscribeStr);

    string postqrsceneStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[FromUser]]></FromUserName><CreateTime>123456789</CreateTime><MsgType><![CDATA[event]]></MsgType><Event><![CDATA[subscribe]]></Event><EventKey><![CDATA[qrscene_123123]]></EventKey><Ticket><![CDATA[TICKET]]></Ticket></xml>";
    var resultqrscene = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedScanModel>(postqrsceneStr);

    string postScanStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[FromUser]]></FromUserName><CreateTime>123456789</CreateTime><MsgType><![CDATA[event]]></MsgType><Event><![CDATA[SCAN]]></Event><EventKey><![CDATA[SCENE_VALUE]]></EventKey><Ticket><![CDATA[TICKET]]></Ticket></xml>";
    var resultScan = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedScanModel>(postScanStr);

    string postLOCATIONStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[fromUser]]></FromUserName><CreateTime>123456789</CreateTime><MsgType><![CDATA[event]]></MsgType><Event><![CDATA[LOCATION]]></Event><Latitude>23.137466</Latitude><Longitude>113.352425</Longitude><Precision>119.385040</Precision></xml>";
    var resultLOCATION = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedLOCATIONModel>(postLOCATIONStr);

    string postClickStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[FromUser]]></FromUserName><CreateTime>123456789</CreateTime><MsgType><![CDATA[event]]></MsgType><Event><![CDATA[CLICK]]></Event><EventKey><![CDATA[EVENTKEY]]></EventKey></xml>";
    var resultClick = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedClickModel>(postClickStr);

    string postViewStr = "<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[FromUser]]></FromUserName><CreateTime>123456789</CreateTime><MsgType><![CDATA[event]]></MsgType><Event><![CDATA[VIEW]]></Event><EventKey><![CDATA[www.qq.com]]></EventKey></xml>";
    var resultView = WeChatHelper4Net.XmlHelper.DeSerialize<WeChatHelper4Net.Models.ReceivedMsg.ReceivedViewModel>(postViewStr);
    */

    #endregion

    #region 事件推送群发结果
    /// <summary>
    /// 事件推送群发结果
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedMassSensJobFinishModel : EventMember
    {
        /// <summary>
        /// 群发的消息ID
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(MsgID))]
        public long MsgID { get; set; }

        /// <summary>
        /// 群发的结果
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Status))]
        public string Status { get; set; }

        /// <summary>
        /// tag_id下粉丝数；或者openid_list中的粉丝数
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(TotalCount))]
        public int TotalCount { get; set; }

        /// <summary>
        /// 过滤（过滤是指特定地区、性别的过滤、用户设置拒收的过滤，用户接收已超4条的过滤）后，准备发送的粉丝数，原则上，FilterCount = SentCount + ErrorCount
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(FilterCount))]
        public int FilterCount { get; set; }

        /// <summary>
        /// 发送成功的粉丝数
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(SentCount))]
        public int SentCount { get; set; }

        /// <summary>
        /// 发送失败的粉丝数
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(ErrorCount))]
        public int ErrorCount { get; set; }
    }
    #endregion

    #region 模版消息发送结果
    /// <summary>
    /// 模版消息发送结果
    /// </summary>
    [Serializable]
    [DataContract(Name = "xml")]
    [XmlRoot("xml")]
    public class ReceivedTemplateSensJobFinishModel : EventMember
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(MsgID))]
        public long MsgID { get; set; }

        /// <summary>
        /// 发送结果
        /// </summary>
        [DataMember(IsRequired = true)]
        [XmlElement(ElementName = nameof(Status))]
        public string Status { get; set; }
    }
    #endregion
}
