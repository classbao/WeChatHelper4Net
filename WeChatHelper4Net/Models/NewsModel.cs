using System;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models
{
	/// <summary>
	/// 图文消息模型
	/// </summary>
	public class NewsModel
	{
		private string _title = string.Empty;
		private string _description = string.Empty;
		private string _url = string.Empty;
		private string _picurl = string.Empty;

		/// <summary>
		/// 标题
		/// </summary>
		public string title { get { return _title; } set { _title = value; } }
		/// <summary>
		/// 描述
		/// </summary>
		public string description { get { return _description; } set { _description = value; } }
		/// <summary>
		/// 点击后跳转的链接
		/// </summary>
		public string url { get { return _url; } set { _url = value; } }
		/// <summary>
		/// 图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图640*320（或者宽高比例是3/1），小图80*80
		/// </summary>
		public string picurl { get { return _picurl; } set { _picurl = value; } }
	}

	/// <summary>
	/// 上传图文消息素材模型
	/// </summary>
	public class ArticleModel
	{
		private string _thumb_media_id = string.Empty;
		private string _author = string.Empty;
		private string _title = string.Empty;
		private string _content_source_url = string.Empty;
		private string _content = string.Empty;
		private string _digest = string.Empty;
		private byte _show_cover_pic;

		/// <summary>
		/// 图文消息缩略图的media_id，可以在基础支持-上传多媒体文件接口中获得
		/// </summary>
		public string thumb_media_id { get { return _thumb_media_id; } set { _thumb_media_id = value; } }
		/// <summary>
		/// 图文消息的作者
		/// </summary>
		public string author { get { return _author; } set { _author = value; } }
		/// <summary>
		/// 图文消息的标题
		/// </summary>
		public string title { get { return _title; } set { _title = value; } }
		/// <summary>
		/// 在图文消息页面点击“阅读原文”后的页面
		/// </summary>
		public string content_source_url { get { return _content_source_url; } set { _content_source_url = value; } }
		/// <summary>
		/// 图文消息页面的内容，支持HTML标签
		/// </summary>
		public string content { get { return _content; } set { _content = value; } }
		/// <summary>
		/// 图文消息的描述
		/// </summary>
		public string digest { get { return _digest; } set { _digest = value; } }
		/// <summary>
		/// 是否显示封面，1为显示，0为不显示
		/// </summary>
		public byte show_cover_pic { get { return _show_cover_pic; } set { _show_cover_pic = value; } }
	}

	/// <summary>
	/// 上传图文消息素材接口返回数据模型
	/// </summary>
	public class ArticleResultModel : RequestResultBaseModel
	{
		private string _type = string.Empty;
		private string _media_id = string.Empty;
		private int _created_at;

		/// <summary>
		/// 媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb），次数为news，即图文消息
		/// </summary>
		public string type { get { return _type; } set { _type = value; } }
		/// <summary>
		/// 媒体文件/图文消息上传后获取的唯一标识
		/// </summary>
		public string media_id { get { return _media_id; } set { _media_id = value; } }
		/// <summary>
		/// 媒体文件上传时间
		/// </summary>
		public int created_at { get { return _created_at; } set { _created_at = value; } }
	}
}
