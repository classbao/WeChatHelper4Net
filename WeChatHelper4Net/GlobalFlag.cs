/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2016-12-15
 */
namespace WeChatHelper4Net
{
    public sealed class GlobalFlag
    {
        #region Instance
        private static readonly GlobalFlag instance = new GlobalFlag();
        private GlobalFlag() { }
        internal static GlobalFlag Instance { get { return instance; } }
        #endregion

        public bool wxAccessToken_IsBusy { get; set; }
        public bool wxJSApiTicket_IsBusy { get; set; }
    }
}
