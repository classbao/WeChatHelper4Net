/**
* 命名空间: WeChatHelper4Net.Extend
*
* 功 能： N/A
* 类 名： QRCodeService
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/14 21:07:43 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2025 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace SampleWebApp.Models
{
    /// <summary>
    /// 高效生成QRCode二维码服务
    /// </summary>
    public class QRCodeService
    {
        // 使用静态变量提高性能
        private static readonly Lazy<BarcodeWriter> _barcodeWriter = new Lazy<BarcodeWriter>(() =>
            new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Renderer = new BitmapRenderer()
            });

        /// <summary>
        /// 生成自定义二维码
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="options">生成选项</param>
        /// <returns>PNG格式的字节数组</returns>
        public byte[] GenerateQRCode(string content, QRCodeOptions options)
        {
            if(string.IsNullOrEmpty(content))
                throw new ArgumentException("内容不能为空");

            // 设置基本参数
            var writer = _barcodeWriter.Value;
            writer.Options = new QrCodeEncodingOptions
            {
                Width = options.Size,
                Height = options.Size,
                Margin = options.Margin,
                ErrorCorrection = GetErrorCorrectionLevel(options.ErrorCorrectionLevel),
                CharacterSet = "UTF-8"
            };

            // 生成基础二维码
            using(var bitmap = writer.Write(content))
            {
                // 应用颜色变换
                if(options.ForegroundColor != Color.Black || options.BackgroundColor != Color.White)
                {
                    ApplyColor(bitmap, options.ForegroundColor, options.BackgroundColor);
                }

                // 添加中心图标
                if(options.CenterIcon != null)
                {
                    AddCenterIcon(bitmap, options.CenterIcon, options.IconSizePercent, options.IconBorderWidth);
                }

                // 转换为PNG
                using(var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
        }

        private ErrorCorrectionLevel GetErrorCorrectionLevel(ErrorCorrectionLevel level)
        {
            return level ?? ErrorCorrectionLevel.M;
        }
        private ErrorCorrectionLevel GetErrorCorrectionLevel(string level = "M")
        {
            //纠错级别：L(7 %) / M(15 %) / Q(25 %) / H(30 %)
            switch(level)
            {
                case "H": return ErrorCorrectionLevel.H;
                case "Q": return ErrorCorrectionLevel.Q;
                case "L": return ErrorCorrectionLevel.L;
                default:
                case "M": return ErrorCorrectionLevel.M;
            }
        }

        /// <summary>
        /// 应用颜色变换
        /// </summary>
        private void ApplyColor(Bitmap bitmap, Color foreground, Color background)
        {
            // 替代不安全代码的安全版本
            for(int y = 0; y < bitmap.Height; y++)
            {
                for(int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);

                    // 近似黑色判断
                    if(pixel.R < 50 && pixel.G < 50 && pixel.B < 50)
                    {
                        bitmap.SetPixel(x, y, foreground);
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, background);
                    }
                }
            }
        }

        /// <summary>
        /// 添加中心图标
        /// </summary>
        private void AddCenterIcon(Bitmap qrCode, Bitmap icon, int iconSizePercent = 20, int borderWidth = 2)
        {
            // 计算图标大小
            int iconWidth = qrCode.Width * iconSizePercent / 100;
            int iconHeight = qrCode.Height * iconSizePercent / 100;

            // 保持图标比例
            if(icon.Width > icon.Height)
            {
                iconHeight = iconHeight * icon.Height / icon.Width;
            }
            else
            {
                iconWidth = iconWidth * icon.Width / icon.Height;
            }

            // 调整图标大小
            using(var resizedIcon = new Bitmap(icon, new Size(iconWidth, iconHeight)))
            {
                // 计算位置
                int x = (qrCode.Width - iconWidth) / 2;
                int y = (qrCode.Height - iconHeight) / 2;

                using(var g = Graphics.FromImage(qrCode))
                {
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    // 添加边框
                    if(borderWidth > 0)
                    {
                        using(var brush = new SolidBrush(Color.White))
                        {
                            g.FillRectangle(brush,
                                x - borderWidth,
                                y - borderWidth,
                                iconWidth + borderWidth * 2,
                                iconHeight + borderWidth * 2);
                        }
                    }

                    // 绘制图标
                    g.DrawImage(resizedIcon, x, y, iconWidth, iconHeight);
                }
            }
        }
    }

    /// <summary>
    /// 二维码生成选项
    /// </summary>
    public class QRCodeOptions
    {
        /// <summary>
        /// 二维码图片的宽度和高度（正方形）建议100-1000像素（默认值300）
        /// </summary>
        public int Size { get; set; } = 300;
        /// <summary>
        /// 二维码边距（空白区域）取值范围0-10，推荐1-4
        /// </summary>
        public int Margin { get; set; } = 1;
        /// <summary>
        /// 二维码前景色（条码颜色）
        /// </summary>
        public Color ForegroundColor { get; set; } = Color.Black;
        /// <summary>
        /// 二维码背景色
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.White;
        /// <summary>
        /// 纠错级别：L(7%)/M(15%)/Q(25%)/H(30%)
        /// </summary>
        public string ErrorCorrectionLevel { get; set; } = "M";
        //public ErrorCorrectionLevel ErrorCorrectionLevel { get; set; }

        /// <summary>
        /// 中心图标（可选）任意Bitmap对象，示例：(Bitmap)Image.FromFile("logo.png") 或者 new Bitmap(Server.MapPath("~/Content/logo.png"))
        /// </summary>
        public Bitmap CenterIcon { get; set; }
        /// <summary>
        /// 图标占二维码大小的百分比，取值范围10-40（建议值）（默认值20）
        /// </summary>
        public int IconSizePercent { get; set; } = 20;
        /// <summary>
        /// 图标周围的白色边框宽度，取值范围0-5像素（默认值2）
        /// </summary>
        public int IconBorderWidth { get; set; } = 2;
    }


}