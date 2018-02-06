using System;
using System.Collections.Generic;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2018-02-05
 */
namespace WeChatHelper4Net.Models.Menu.Base
{
    public interface IBase
    {
        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过60个字节
        /// </summary>
        string name { get; set; }

        /// <summary>
        /// 菜单转换成Json字符串
        /// </summary>
        /// <returns></returns>
        string ToJson();
    }
    public interface ISingleButton : IBase
    {
        /// <summary>
        /// 菜单类型，构造函数内部将会初始化赋值
        /// </summary>
        string type { get; set; }
    }

    public interface ISubButton : IBase
    {
        /// <summary>
        /// 子菜单组合
        /// </summary>
        IList<SingleButton> sub_button { get; set; }
    }
}
