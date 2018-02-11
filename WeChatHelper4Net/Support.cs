using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using WeChatHelper4Net.Models;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
	/// <summary>
	/// 推广支持
	/// </summary>
	public class Support
	{
		/// <summary>
		/// 创建二维码ticket
		/// </summary>
		/// <param name="type">二维码类型，QR_SCENE为临时,QR_LIMIT_SCENE为永久</param>
		/// <param name="expire_seconds">该二维码有效时间，以秒为单位。 最大不超过604800（即7天）。</param>
		/// <param name="scene_id">场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）</param>
		/// <param name="scene_str">场景值ID（字符串形式的ID），字符串类型，长度限制为1到64，仅永久二维码支持此字段</param>
		/// <returns>ticket模型</returns>
		private static TicketModel getTicket(TicketType type, int expire_seconds, int scene_id, string access_token, string scene_str = "")
		{
			if (TicketType.QR_SCENE == type && expire_seconds < 1)
				expire_seconds = 604800;

			string url = Common.ApiUrl + string.Format("qrcode/create?access_token={0}", access_token);
			string data = string.Empty;
			TicketModel ticket = new TicketModel();
			if (type == TicketType.QR_LIMIT_SCENE)
			{
				//永久
				if (!string.IsNullOrWhiteSpace(scene_str))
				{
					data = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"" + scene_str + "\"}}}";
				}
				else if (1 <= scene_id && scene_id <= 100000)
				{
					data = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + scene_id + "}}}";
				}
				else
				{
					throw new Exception("创建二维码ticket出错！scene_id：场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）");
				}
			}
			else
			{
				//临时
				if (1 <= expire_seconds && expire_seconds <= 2592000)
				{
					data = "{\"expire_seconds\": " + expire_seconds + ", \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + scene_id + "}}}";
				}
				else
				{
					throw new Exception("创建二维码ticket出错！expire_seconds：该二维码有效时间，以秒为单位。 最大不超过604800");
				}
			}

			try
			{
				string result = HttpRequestHelper.Request(url, data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
				ticket = JsonHelper.DeSerialize<TicketModel>(result);
				return ticket;
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
		}

		/// <summary>
		/// 生成带参数的二维码，本接口无须登录态即可调用。
		/// ticket正确情况下，http 返回码是200，是一张图片，可以直接展示或者下载。
		/// 该方法内部已经对TICKET进行UrlEncode处理。
		/// </summary>
		/// <param name="ticket">二维码ticket</param>
		/// <returns>ticket正确情况下，http 返回码是200，是一张图片，可以直接展示或者下载</returns>
		private static string getQrCode(string ticket)
		{
			if (!string.IsNullOrEmpty(ticket))
				return string.Format("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}", HttpUtility.UrlEncode(ticket));
			return string.Empty;
		}

        /// <summary>
        /// 生成带参数的二维码（临时二维码），本接口无须登录态即可调用。正确情况下，http 返回码是200，是一张图片，可以直接展示或者下载。
        /// </summary>
        /// <param name="expire_seconds">该二维码有效时间，以秒为单位。 最大不超过604800（即7天）。</param>
        /// <param name="scene_id">场景值ID，临时二维码时为32位非0整型</param>
        /// <param name="access_token">access_token</param>
        /// <returns>正确情况下，http 返回码是200，是一张图片，可以直接展示或者下载</returns>
        public static string GetQrCode(int expire_seconds, int scene_id, string access_token)
		{
			TicketModel model = getTicket(TicketType.QR_SCENE, expire_seconds, scene_id, access_token);
			if (null != model)
				return getQrCode(model.ticket);
			return string.Empty;
		}
        /// <summary>
        /// 生成带参数的二维码（永久二维码），本接口无须登录态即可调用。正确情况下，http 返回码是200，是一张图片，可以直接展示或者下载。
        /// </summary>
        /// <param name="scene_id">场景值ID，永久二维码时最大值为100000（目前参数只支持1--100000）</param>
        /// <param name="access_token">access_token</param>
        /// <returns>正确情况下，http 返回码是200，是一张图片，可以直接展示或者下载</returns>
        public static string GetQrCode(int scene_id, string access_token)
		{
			TicketModel model = getTicket(TicketType.QR_LIMIT_SCENE, 0, scene_id, access_token);
			if (null != model)
				return getQrCode(model.ticket);
			return string.Empty;
		}
        /// <summary>
        /// 生成带参数的二维码（永久二维码），本接口无须登录态即可调用。正确情况下，http 返回码是200，是一张图片，可以直接展示或者下载。
        /// </summary>
        /// <param name="scene_str">场景值ID（字符串形式的ID），字符串类型，长度限制为1到64，仅永久二维码支持此字段</param>
        /// <param name="access_token">access_token</param>
        /// <returns>正确情况下，http 返回码是200，是一张图片，可以直接展示或者下载</returns>
        public static string GetQrCode(string scene_str, string access_token)
		{
			TicketModel model = getTicket(TicketType.QR_LIMIT_SCENE, 0, 0, access_token, scene_str);
			if (null != model)
				return getQrCode(model.ticket);
			return string.Empty;
		}

	}
}