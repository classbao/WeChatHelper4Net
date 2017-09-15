using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace WeChatHelper4Net.Models.TemplateMessage
{
	/// <summary>
	/// 微信模板消息记录模型
	/// </summary>
	class templatemessage
	{
		public int id { get; set; }
		public int mid { get; set; }
		public string wxid { get; set; }
		public int msgid { get; set; }
		public string templateid { get; set; }
		public int errcode { get; set; }
		public string errmsg { get; set; }
		public string status { get; set; }
		public string eventname { get; set; }
		public DateTime createtime { get; set; }
		public string sendwxid { get; set; }
		public string first { get; set; }
		public string remark { get; set; }
		public string keyword1 { get; set; }
		public string keyword2 { get; set; }
		public string keyword3 { get; set; }
		public string keyword4 { get; set; }
		public string keyword5 { get; set; }
		public DateTime addtime { get; set; }
	}

	class templatemessageHelper
	{
		/// <summary>
		/// 获取微信模板消息发送情况
		/// </summary>
		/// <param name="msgid"></param>
		/// <param name="sendwxid"></param>
		/// <returns></returns>
		public static templatemessage GetModel(int msgid, string sendwxid)
		{
			if (msgid < 0 || string.IsNullOrWhiteSpace(sendwxid)) return null;
			templatemessage model = new templatemessage();

			return model;
		}

		/// <summary>
		/// 修改微信模板消息发送情况
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static int Update(templatemessage model)
		{
			if (model == null || model.id < 1) return 0;
            
			return 0;
		}

		/// <summary>
		/// 记录微信模板消息发送情况
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static int Insert(templatemessage model)
		{
			if (model == null || string.IsNullOrWhiteSpace(model.templateid) || string.IsNullOrWhiteSpace(model.wxid)) return 0;
            
			return 0;
		}
	}

}
