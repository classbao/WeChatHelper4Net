using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeChatHelper4Net.Map;

namespace SampleWebApp.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        //public ActionResult Index()
        //{
        //    return View();
        //}

        /// <summary>
        /// 测试百度天气接口-基础服务
        /// </summary>
        /// <returns></returns>
        public ActionResult BaiduWeather1()
        {
            BaiduWeatherResultModel result = BaiduWeather.BasicServices("NDUHAGtRMpnC4W0yujTqIRIs", "411523");

            return Json(result);
        }

    }
}