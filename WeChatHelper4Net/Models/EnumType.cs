using System;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models
{
    public enum MenuType
    {
        click,
        view
    }

    public enum MsgType
    {
        text,
        image,
        voice,
        video,
        music,
        news
    }

    public enum scope
    {
        snsapi_base,
        snsapi_userinfo,
        snsapi_login,
    }

    public enum TicketType
    {
        QR_SCENE,
        QR_LIMIT_SCENE
    }

    public class UrlParameter
    {
        public object key { get; set; }
        public object value { get; set; }
    }

}