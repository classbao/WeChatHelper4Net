﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeChatHelper4Net;
using WeChatHelper4Net.Models.Menu;

namespace SampleWebApp.Controllers
{
    /// <summary>
    /// 只用来用来与微信消息对接的控制器
    /// </summary>
    public class WeChatController : Controller
    {
        // GET: WeChat

        /// <summary>
        /// 自定义Token
        /// </summary>
        private readonly string Token = "Token123ByClassbao"; // ConfigurationManager.AppSettings["WeChatToken"];

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.classbao.com/WeChat
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            LogHelper.Save(nameof(Index) + "> 当前AbsoluteUri=" + Request.Url.AbsoluteUri + "，ContentEncoding=" + Request.ContentEncoding.ToString() + "，ContentType=" + Request.ContentType + "，RequestType=" + Request.RequestType + "，HttpMethod=" + Request.HttpMethod + "，UserHostAddress=" + Request.UserHostAddress + "，UserHostName=" + Request.UserHostName, nameof(WeChatController), LogType.Common, LogTime.day);
            var signature = Request["signature"];
            var timestamp = Request["timestamp"];
            var nonce = Request["nonce"];
            var echostr = Request["echostr"];

            var result = ValidationToken.Validation(signature, timestamp, nonce, echostr, Token);
            if(!string.IsNullOrWhiteSpace(result))
            {
                return Content(result);
            }
            else
            {
                return Content("如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }


        /// <summary>
        /// 创建自定义菜单：http://weixin.classbao.com/WeChat/CreateMenu
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMenu()
        {
            //var click = new WeChatHelper4Net.Models.Menu.Click("xiong", "aaa").ToJson();

            //var subbutton = new WeChatHelper4Net.Models.Menu.SubButton("二级菜单", new List<WeChatHelper4Net.Models.Menu.Base.SingleButton>()
            //{
            //    new Click("xiong", "aaa"),
            //    new View("xue", "bbb"),
            //});
            //string subbuttonJson = subbutton.ToJson();

            var button = new Button(new List<WeChatHelper4Net.Models.Menu.Base.BaseButton>()
            {
                new Click("今日歌曲", "V1001_TODAY_MUSIC"),
                new SubButton("菜单", new List<WeChatHelper4Net.Models.Menu.Base.SingleButton>()
                {
                    new View("搜索", "http://www.soso.com/"),
                    new MiniProgram() {  name="wxa",
                        url="http://mp.weixin.qq.com",
                        appid="wx286b93c14bbf93aa",
                        pagepath="pages/lunar/index" },
                    new Click("赞一下我们", "V1001_GOOD"),
                    new Pic_weixin("相册","rselfmenu_1_2",new List<WeChatHelper4Net.Models.Menu.Base.SingleButton>()
                        {
                            new Click("相册111", "xc111"),
                            new View("相册222", "xc222"),
                        })
                })
            });
            string buttonJson = button.ToJson();

            if(Menu.CreateMenu(button, DateTime.Now))
            {
                return Content("创建自定义菜单 成功。");
            }
            else
            {
                return Content("创建自定义菜单 失败！");
            }
        }


    }
}