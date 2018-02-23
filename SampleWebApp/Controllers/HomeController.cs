using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleWebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 允许微信分享的页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AllowWeChatToShare()
        {
            return View();
        }
        /// <summary>
        /// 不允许微信分享的页面
        /// </summary>
        /// <returns></returns>
        public ActionResult NotAllowWeChatToShare()
        {
            return View();
        }

    }
}