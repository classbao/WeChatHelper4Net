/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2016-12-15
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// 全局变量/状态控制
    /// </summary>
    public sealed class GlobalFlag
    {
        #region Instance
        private static readonly GlobalFlag instance = new GlobalFlag();
        private GlobalFlag() { }
        /// <summary>
        /// 全局变量/状态实例（单例模式）
        /// </summary>
        internal static GlobalFlag Instance { get { return instance; } }
        #endregion

        /// <summary>
        /// 是否当前代码正在刷新AccessToken
        /// </summary>
        public bool wxAccessToken_IsBusy { get; set; }
        /// <summary>
        /// 是否当前代码正在刷新JSApiTicket
        /// </summary>
        public bool wxJSApiTicket_IsBusy { get; set; }
    }
}
