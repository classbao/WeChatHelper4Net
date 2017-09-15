using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WeChatHelper4Net.Models.Menu
{
    public class MenuModel
    {
        /// <summary>
		/// 二级菜单，{0}菜单名称，{1}二级菜单json
		/// 注意：参数设置完成后，请将“aaa”，“bbb”分别替换为“{”，“}”
		/// </summary>
		public static string SubMenu { get { return "aaa\"name\":\"{0}\",\"sub_button\":[{1}]bbb"; } }

        /// <summary>
        /// 一级菜单json，{0}一级菜单json
        /// 注意：参数设置完成后，请将“aaa”，“bbb”分别替换为“{”，“}”
        /// </summary>
        public static string Menu { get { return "aaa\"button\":[{0}]bbb"; } }
    }


}
