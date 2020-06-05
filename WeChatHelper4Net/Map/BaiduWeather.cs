/**
* 命名空间: WeChatHelper4Net.Map
*
* 功 能： N/A
* 类 名： BaiduWeather
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2020/6/5 22:22:33 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2020 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;
using System.Text;

/*
 * 百度地图服务
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Map
{
    /// <summary>
    /// 百度天气
    /// </summary>
    public class BaiduWeather
    {
        /// <summary>
        /// 百度天气接口-基础服务
        /// </summary>
        /// <param name="ak">开发者密钥，可在API控制台申请获得</param>
        /// <param name="district_id">区县的行政区划编码</param>
        /// <param name="data_type">请求数据类型。数据类型有：now/fc/index/alert/fc_hour/all，控制返回内容</param>
        /// <param name="output">返回格式，目前支持json/xml</param>
        /// <returns></returns>
        public static BaiduWeatherResultModel BasicServices(string ak, string district_id, string data_type = "all", string output = "json")
        {
            string url = string.Format($"http://api.map.baidu.com/weather/v1/?district_id={district_id}&data_type={data_type}&output={output}&ak={ak}"
                , ak
                , district_id
                , data_type
                , output
                );
            string POI = BaiduMap.HttpGet(url);
            if(!string.IsNullOrWhiteSpace(POI))
                return JsonHelper.DeSerialize<BaiduWeatherResultModel>(POI);
            return null;
        }
    }
}
