/**
* 命名空间: WeChatHelper4Net.Map
*
* 功 能： N/A
* 类 名： BaiduWeatherModel
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2020/6/5 22:33:48 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2020 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatHelper4Net.Map
{
    public class BaiduWeatherModel
    {

    }

    /// <summary>
    /// 百度天气接口返回结果
    /// </summary>
    public class BaiduWeatherResultModel : BaiduMapResultBaseModel
    {
        public BaiduWeatherResultItemModel result { get; set; }
    }

    public class BaiduWeatherResultItemModel
    {
        /// <summary>
        /// 位置（行政区划） 
        /// </summary>
        public BaiduWeatherLocationItem location { get; set; }
        /// <summary>
        /// 现在天气 
        /// </summary>
        public BaiduWeatherNowItem now { get; set; }
        /// <summary>
        /// 预测天气 
        /// </summary>
        public BaiduWeatherForecastsItem[] forecasts { get; set; }
    }

    public class BaiduWeatherLocationItem
    {
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class BaiduWeatherNowItem
    {
        /// <summary>
        /// 天气现象（晴）
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 温度（℃）
        /// </summary>
        public string temp { get; set; }
        /// <summary>
        /// 体感温度（℃）
        /// </summary>
        public string feels_like { get; set; }
        /// <summary>
        /// 相对湿度(%)
        /// </summary>
        public string rh { get; set; }
        /// <summary>
        /// 风力等级
        /// </summary>
        public string wind_class { get; set; }
        /// <summary>
        /// 风向描述
        /// </summary>
        public string wind_dir { get; set; }
        /// <summary>
        /// 数据更新时间，北京时间
        /// </summary>
        public string uptime { get; set; }
    }

    public class BaiduWeatherForecastsItem
    {
        /// <summary>
        /// 白天天气现象
        /// </summary>
        public string text_day { get; set; }
        /// <summary>
        /// 晚上天气现象
        /// </summary>
        public string text_night { get; set; }
        /// <summary>
        /// 最高温度(℃)
        /// </summary>
        public string high { get; set; }
        /// <summary>
        /// 最低温度(℃)
        /// </summary>
        public string low { get; set; }
        /// <summary>
        /// 白天风力
        /// </summary>
        public string wc_day { get; set; }
        /// <summary>
        /// 白天风向
        /// </summary>
        public string wd_day { get; set; }
        /// <summary>
        /// 晚上风力
        /// </summary>
        public string wc_night { get; set; }
        /// <summary>
        /// 晚上风向
        /// </summary>
        public string wd_night { get; set; }
        /// <summary>
        /// 日期，北京时区
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 星期，北京时区
        /// </summary>
        public string week { get; set; }
    }

}
