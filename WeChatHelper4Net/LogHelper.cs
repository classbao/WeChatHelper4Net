using System;
using System.IO;
using System.Web;
using System.Text;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 报告
        /// </summary>
        Report,
        /// <summary>
        /// 普通日志
        /// </summary>
        Common,
        /// <summary>
        /// 错误
        /// </summary>
        Error
    }
    /// <summary>
    /// 日志文件名时间格式
    /// </summary>
    public enum LogTime
    {
        /// <summary>
        /// 天
        /// </summary>
        day,
        /// <summary>
        /// 时
        /// </summary>
        hour,
        /// <summary>
        /// 分
        /// </summary>
        minute
    }

    /// <summary>
    /// 程序日志
    /// </summary>
    public class LogHelper
	{
		/// <summary>
		/// 日志文件位置
		/// 作者：熊学浩
		/// 日期：2014-5-27
		/// </summary>
		/// <param name="prefix">日志文件名前缀</param>
		/// <param name="logType">日志类型</param>
		/// <param name="logTime">日志文件名时间格式</param>
		/// <returns></returns>
		private static string LogPath(string prefix, LogType logType, LogTime logTime)
		{
			try
			{
				string fileName = string.IsNullOrEmpty(prefix) ? "" : prefix;
				if (logTime == LogTime.hour)
				{
					fileName += DateTime.Now.ToString("yyyyMMddHH");
				}
				if (logTime == LogTime.day)
				{
					fileName += DateTime.Now.ToString("yyyyMMdd");
				}
				if (logTime == LogTime.minute)
				{
					fileName += DateTime.Now.ToString("yyyyMMddHHmm");
				}
				string subPath = string.Format("log/{0}/{1}_{2}.txt", DateTime.Now.ToString("yyyyMM"), logType.ToString(), fileName);
				string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subPath);
				if (HttpContext.Current != null)
				{
					path = Path.Combine(HttpContext.Current.Server.MapPath("~/"), subPath);
				}
				return path;
			}
			catch (Exception)
			{
				return "";
			}
		}

		/// <summary>
		/// 生成错误信息
		/// </summary>
		/// <param name="Ex"></param>
		/// <param name="sb"></param>
		private static void SetSB(Exception Ex, ref StringBuilder sb)
		{
			if (sb == null) sb = new StringBuilder();
			sb.AppendLine();
			sb.AppendLine("========================================================================");
			sb.AppendLine("异常发生时间           : " + DateTime.Now);
			if (HttpContext.Current != null)
			{
				HttpRequest request = HttpContext.Current.Request;
				sb.AppendLine("客户端IP地址           : " + request.UserHostAddress);
				sb.AppendLine("客户端浏览器版本       : " + request.Browser.Type);
				sb.AppendLine("客户端上次请求的地址   : " + HttpContext.Current.Server.UrlDecode((request.UrlReferrer != null) ? request.UrlReferrer.ToString() : ""));
				sb.AppendLine("客户端本次请求的地址   : " + HttpContext.Current.Server.UrlDecode((request.Url != null) ? request.Url.ToString() : ""));
			}
			sb.AppendLine("当前异常的消息         : " + Ex.Message);
			sb.AppendLine("引发当前异常的类名     : " + Ex.TargetSite.DeclaringType.FullName);
			sb.AppendLine("引发当前异常的方法名   : " + Ex.TargetSite.Name);
			sb.AppendLine("堆栈信息               : " + Ex.StackTrace);
			sb.AppendLine("========================================================================");
			sb.AppendLine();
		}

		/// <summary>
		/// 记录日志到文件
		/// 作者：熊学浩
		/// 日期：2014-5-27
		/// </summary>
		/// <param name="msg">日志内容</param>
		/// <param name="path">日志路径</param>
		private static void Save(string msg, string path)
		{
			try
			{
				if (!Directory.Exists(Path.GetDirectoryName(path)))
					Directory.CreateDirectory(Path.GetDirectoryName(path));
				using (StreamWriter sw = new StreamWriter(path, true))
				{
					sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >");
					sw.WriteLine(msg);
					sw.WriteLine();
					sw.Close();
					sw.Dispose();
				}
			}
			catch (Exception)
			{
			}
		}

		/// <summary>
		/// 记录日志到文件
		/// 作者：熊学浩
		/// 日期：2014-5-27
		/// </summary>
		/// <param name="msg">日志内容</param>
		/// <param name="prefix">日志文件名前缀</param>
		/// <param name="logType">日志类型</param>
		/// <param name="logTime">日志文件名时间格式</param>       
		/// <returns>日志路径</returns>
		public static void Save(string msg, string prefix, LogType logType, LogTime logTime)
		{
			Save(msg, LogPath(prefix, logType, logTime));
		}

		/// <summary>
		/// 记录异常日志到文件
		/// 调用此方法必须支持HttpContext和HttpRequest对象才行
		/// 作者：熊学浩
		/// 日期：2014-5-27
		/// </summary>
		/// <param name="Ex">Exception</param>
		public static void Save(Exception Ex)
		{
			StringBuilder sb = new StringBuilder();
			SetSB(Ex, ref sb);
			Save(sb.ToString(), LogPath("Exception", LogType.Error, LogTime.day));
		}
		/// <summary>
		/// 记录异常日志到文件
		/// 调用此方法必须支持HttpContext和HttpRequest对象才行
		/// 作者：熊学浩
		/// 日期：2015-8-13
		/// </summary>
		/// <param name="Ex">Exception</param>
		/// <param name="msg">自定义消息</param>
		public static void Save(Exception Ex, string msg)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine();
			sb.AppendLine(msg);

			SetSB(Ex, ref sb);

			Save(sb.ToString(), LogPath("Exception", LogType.Error, LogTime.day));
		}
		/// <summary>
		/// 程序异常监控日志
		/// 作者：熊学浩
		/// 日期：2014-5-27
		/// </summary>
		/// <param name="Ex">Exception</param>
		/// <param name="prefix">日志文件名前缀</param>
		/// <param name="logTime">日志文件名时间格式</param>
		public static void Save(Exception Ex, string prefix, LogTime logTime)
		{
			StringBuilder sb = new StringBuilder();
			SetSB(Ex, ref sb);
			Save(sb.ToString(), LogPath(prefix, LogType.Error, logTime));
		}
		/// <summary>
		/// 程序异常监控日志
		/// 作者：熊学浩
		/// 日期：2014-5-27
		/// </summary>
		/// <param name="Ex">Exception</param>
		/// <param name="msg">自定义消息</param>
		/// <param name="prefix">日志文件名前缀</param>
		/// <param name="logTime">日志文件名时间格式</param>
		public static void Save(Exception Ex, string msg, string prefix, LogTime logTime)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine();
			sb.AppendLine(msg);

			SetSB(Ex, ref sb);

			Save(sb.ToString(), LogPath(prefix, LogType.Error, logTime));
		}

	}
}
