using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

/*
 * 地图服务
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Map
{
	/// <summary>
	/// 百度地图
	/// </summary>
	public class BaiduMap
	{
		/// <summary>
		/// 区域搜索POI服务
		/// </summary>
		/// <param name="ak"></param>
		/// <param name="query"></param>
		/// <param name="tag"></param>
		/// <param name="pi"></param>
		/// <param name="ps"></param>
		/// <param name="region"></param>
		public static BaiduMapPlaceSearchResultModel PlaceSearch(string ak, string query, string tag = "", int pi = 0, int ps = 20, string region = "全国")
		{
			string url = string.Format("http://api.map.baidu.com/place/v2/search?output=json&scope=1&page_size={0}&page_num={1}&ak={2}&query={3}&tag={4}&region={5}"
				, ps
				, pi
				, ak
				, HttpUtility.UrlEncode(query, Encoding.UTF8)
				, HttpUtility.UrlEncode(tag, Encoding.UTF8)
				, HttpUtility.UrlEncode(region, Encoding.UTF8)
				);
			string POI = HttpGet(url);

			if (!string.IsNullOrWhiteSpace(POI))
				return JsonHelper.DeSerialize<BaiduMapPlaceSearchResultModel>(POI);

			return null;
		}

		/// <summary>
		/// 地理编码：即地址解析，由详细到街道的结构化地址得到百度经纬度信息，且支持名胜古迹、标志性建筑名称直接解析返回百度经纬度。例如：“北京市海淀区中关村南大街27号”地址解析的结果是“lng:116.31985,lat:39.959836”，“百度大厦”地址解析的结果是“lng:116.30815,lat:40.056885” 
		/// </summary>
		/// <param name="ak"></param>
		/// <param name="address"></param>
		/// <param name="city"></param>
		/// <returns></returns>
		public static BaiduMapGeocoderResultModel Geocoder(string ak, string address, string city = "")
		{
			string url = string.Format("http://api.map.baidu.com/geocoder/v2/?output=json&ak={0}&address={1}&city={2}"
				, ak
				, HttpUtility.UrlEncode(address, Encoding.UTF8)
				, HttpUtility.UrlEncode(city, Encoding.UTF8)
				);
			string geocoder = HttpGet(url);
			if (!string.IsNullOrWhiteSpace(geocoder))
				return JsonHelper.DeSerialize<BaiduMapGeocoderResultModel>(geocoder);
			return null;
		}

        /// <summary>
        /// 逆地理编码，即逆地址解析，由百度经纬度信息得到结构化地址信息。例如：“lat:31.325152,lng:120.558957”逆地址解析的结果是“江苏省苏州市虎丘区塔园路318号”。 
        /// </summary>
        /// <param name="ak"></param>
        /// <param name="point"></param>
        /// <param name="coordtype">坐标的类型，目前支持的坐标类型包括：bd09ll（百度经纬度坐标）、gcj02ll（国测局经纬度坐标）、wgs84ll（ GPS经纬度）</param>
        /// <param name="pois">是否显示指定位置周边的poi，0为不显示，1为显示。当值为1时，显示周边100米内的poi</param>
        /// <returns></returns>
        public static BaiduMapGeocoderReverseResultModel GeocoderReverse(string ak, BaiduMapPoint point, coordtype coordtype = coordtype.bd09ll, int pois = 0)
		{
			string _point = string.Format("{0},{1}", point.lat, point.lng);
			string url = string.Format("http://api.map.baidu.com/geocoder/v2/?output=json&ak={0}&coordtype={1}&location={2}&pois={3}"
				, ak
				, coordtype.ToString()
				, _point
                , pois
				);
			string geocoder = HttpGet(url);
			if (!string.IsNullOrWhiteSpace(geocoder))
				return JsonHelper.DeSerialize<BaiduMapGeocoderReverseResultModel>(geocoder);
			return null;
		}

		/// <summary>
		/// HTTP get请求
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		private static string HttpGet(string url)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
			request.Method = "GET";
			request.ContentType = "text/html;charset=UTF-8";
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			var myResponseStream = response.GetResponseStream();
			try
			{
				if (myResponseStream == null)
					return "";
				var myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
				var retString = myStreamReader.ReadToEnd();
				myStreamReader.Close();
				//  myResponseStream.Close();
				return retString;
			}
			catch (Exception er)
			{
				throw er;
			}
			finally
			{
				if (myResponseStream != null)
					myResponseStream.Close();
			}
		}
	}
}
