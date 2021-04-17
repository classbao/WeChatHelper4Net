using System;
using System.Runtime.Serialization;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models
{
	/// <summary>
	/// 创建二维码ticket请求参数模型
	/// </summary>
	public class TicketModel : RequestResultBaseModel
	{
		private string _ticket = string.Empty;
		private int _expire_seconds;

		/// <summary>
		/// 获取的二维码ticket，凭借此ticket可以在有效时间内换取二维码
		/// </summary>
		public string ticket { get { return _ticket; } set { _ticket = value; } }
		/// <summary>
		/// 二维码的有效时间，以秒为单位。
		/// </summary>
		public int expire_seconds { get { return _expire_seconds; } set { _expire_seconds = value; } }
	}

    #region 短key托管类似于短链API
    /// <summary>
    /// 短key托管类似于短链API
    /// </summary>
    public class ShortenGenModel : RequestResult_errmsg
    {
        /// <summary>
        /// 短key，15字节，base62编码(0-9/a-z/A-Z)
        /// </summary>
        [DataMember(IsRequired = false)]
        public string short_key { get; set; }
    }
    /// <summary>
    /// 短key托管类似于短链API
    /// </summary>
    public class ShortenFetchModel : RequestResult_errmsg
    {
        /// <summary>
        /// 长信息
        /// </summary>
        [DataMember(IsRequired = false)]
        public string long_data { get; set; }
        /// <summary>
        /// 创建的时间戳
        /// </summary>
        [DataMember(IsRequired = false)]
        public int create_time { get; set; }
        /// <summary>
        /// 剩余的过期秒数
        /// </summary>
        [DataMember(IsRequired = false)]
        public uint expire_seconds { get; set; }
    }

    #endregion

}