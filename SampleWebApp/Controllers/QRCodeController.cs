using SampleWebApp.Models;
using System;
using System.Drawing;
using System.Web.Mvc;

namespace SampleWebApp.Controllers
{
    /// <summary>
    /// 二维码
    /// </summary>
    public class QRCodeController : Controller
    {
        // GET: QRCode
        public ActionResult Index()
        {
            /*
             * 微信颜色标准
（绿色：#09BB07）
             * 支付宝颜色标准：
主蓝色：#1677FF
辅助蓝：#1890FF
             * 云闪付颜色标准：
主红色：#FF3333
标准红：#FF0033


             */
            return View();
        }

        #region 高效生成QRCode二维码

        private readonly QRCodeService _qrService = new QRCodeService();

        [HttpGet]
        public ActionResult Generate(
            string content,
            int size = 300,
            int margin = 1,
            string foreground = "000000",
            string background = "FFFFFF",
            string errorCorrection = "M")
        {
            try
            {
                var options = new QRCodeOptions
                {
                    Size = Math.Min(Math.Max(size, 100), 1000), // 限制大小在100-1000之间
                    Margin = Math.Min(Math.Max(margin, 0), 10),
                    ForegroundColor = ColorTranslator.FromHtml("#" + foreground),
                    BackgroundColor = ColorTranslator.FromHtml("#" + background),
                    ErrorCorrectionLevel = errorCorrection
                };

                var imageBytes = _qrService.GenerateQRCode(content, options);
                return File(imageBytes, "image/png", $"QrCode{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.png");
            }
            catch
            {
                return HttpNotFound();
            }
        }
        /*
         * 调用示例：
<!-- 简单二维码 -->
<img src="@Url.Action("Generate", "QRCode", new { 
    content = "https://example.com", 
    size = 250, 
    foreground = "FF5722", 
    background = "F5F5F5" 
})" alt="QR Code" />
         */

        [HttpPost]
        public ActionResult GenerateWithIcon(
            string content,
            int size = 300,
            int margin = 1,
            string foreground = "000000",
            string background = "FFFFFF",
            string errorCorrection = "M",
            int iconSize = 20,
            int iconBorder = 2)
        {
            try
            {
                var options = new QRCodeOptions
                {
                    Size = Math.Min(Math.Max(size, 100), 1000),
                    Margin = Math.Min(Math.Max(margin, 0), 10),
                    ForegroundColor = ColorTranslator.FromHtml("#" + foreground),
                    BackgroundColor = ColorTranslator.FromHtml("#" + background),
                    ErrorCorrectionLevel = errorCorrection,
                    IconSizePercent = Math.Min(Math.Max(iconSize, 10), 40), // 限制图标大小在10%-40%之间
                    IconBorderWidth = Math.Min(Math.Max(iconBorder, 0), 5)
                };

                // 从请求中获取上传的图标
                if(Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    using(var stream = Request.Files[0].InputStream)
                    {
                        options.CenterIcon = new Bitmap(stream);
                    }
                }

                var imageBytes = _qrService.GenerateQRCode(content, options);
                return File(imageBytes, "image/png");
            }
            catch
            {
                return HttpNotFound();
            }
        }
        /*
         * 调用示例：
<!-- 带图标的二维码 (需要表单上传) -->
@using (Html.BeginForm("GenerateWithIcon", "QRCode", FormMethod.Post, new { enctype = "multipart/form-data" }))
{
    <input type="hidden" name="content" value="https://example.com" />
    <input type="hidden" name="size" value="300" />
    <input type="file" name="icon" accept="image/*" />
    <button type="submit">生成带图标的二维码</button>
}
         */



        /*
         * 高级使用（控制器中直接调用）
var options = new QRCodeOptions
{
    Size = 400,
    ForegroundColor = Color.DarkBlue,
    BackgroundColor = Color.LightGray,
    ErrorCorrectionLevel = ErrorCorrectionLevel.H,
    CenterIcon = new Bitmap(Server.MapPath("~/Content/logo.png")),
    IconSizePercent = 25,
    IconBorderWidth = 3
};

var qrService = new QRCodeService();
var qrCode = qrService.GenerateQRCode("VIP User: 10001", options);
         */
        #endregion



    }
}