using System;
using System.Collections.Generic;
using System.Text;
using WeChatHelper4Net.Models;
using WeChatHelper4Net.Models.Menu;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
    /// <summary>
    /// 自定义菜单
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// 查询当前使用的自定义菜单的结构
        /// </summary>
        /// <returns></returns>
        public static string GetMenu(string access_token)
        {
            string url = Common.ApiUrl + string.Format("menu/get?access_token={0}", access_token);
            return HttpRequestHelper.Request(url);
        }

        /// <summary>
        /// 删除当前使用的自定义菜单
        /// </summary>
        /// <returns></returns>
        public static string DeleteMenu(string access_token)
        {
            string url = Common.ApiUrl + string.Format("menu/delete?access_token={0}", access_token);
            return HttpRequestHelper.Request(url);
        }

        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <param name="data">创建自定义菜单JSON结构字符串</param>
        /// <returns></returns>
        public static bool CreateMenu(string data, string access_token)
        {
            if(string.IsNullOrEmpty(data)) return false;
            try
            {
                string url = Common.ApiUrl + string.Format("menu/create?access_token={0}", access_token);

                string result = HttpRequestHelper.Request(url, data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
                if(Common.ReturnJSONisOK(result))
                    return true;
                else
                {
                    RequestResultBaseModel model = JsonHelper.DeSerialize<RequestResultBaseModel>(result);
                    if(model != null && !string.IsNullOrEmpty(model.errmsg) && model.errcode == 0)
                    {
                        return true;
                    }
                    else
                    {
                        LogHelper.Save("创建自定义菜单失败！    url=" + url + "    data=" + data + "    result=" + result, "CreateMenu", LogType.Error, LogTime.day);
                        return false;
                    }
                }
            }
            catch(Exception Ex)
            {
                LogHelper.Save(Ex);
                throw Ex;
            }
        }
        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <param name="button">自定义菜单对象</param>
        /// <returns></returns>
        public static bool CreateMenu(Button button, string access_token)
        {
            return CreateMenu(button.ToJson(), access_token);
        }

        /// <summary>
        /// 获取自定义菜单配置
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string GetCurrentSelfMenu(string access_token)
        {
            string url = Common.ApiUrl + string.Format("get_current_selfmenu_info?access_token={0}", access_token);
            return HttpRequestHelper.Request(url);
        }

    }
}
