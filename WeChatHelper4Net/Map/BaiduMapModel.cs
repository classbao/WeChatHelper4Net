using System;
using System.Collections.Generic;

/*
 * 地图服务
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Map
{
	public class BaiduMapModel
	{
	}

	/// <summary>
	/// 百度地图API返回结果基模型
	/// </summary>
	public class BaiduMapResultBaseModel
	{
		/// <summary>
		/// 本次API访问状态，如果成功返回0，如果失败返回其他数字。
		/// </summary>
		public int status { get; set; }
		/// <summary>
		/// 对API访问状态值的英文说明，如果成功返回"ok"，并返回结果字段，如果失败返回错误说明。
		/// </summary>
		public string message { get; set; }
		/// <summary>
		/// 检索总数，用户请求中设置了page_num字段才会出现total字段。当检索总数值大于760时，多次刷新同一请求得到total的值可能稍有不同，属于正常情况。 
		/// </summary>
		public int total { get; set; }
	}

	public class BaiduMapPlaceSearchResultModel : BaiduMapResultBaseModel
	{
		public List<BaiduMapPlaceSearchResultItemModel> results { get; set; }
	}

	public class BaiduMapPlaceSearchResultItemModel
	{
		/// <summary>
		/// poi名称 
		/// </summary>
		public string name { get; set; }
		/// <summary>
		/// poi经纬度坐标 
		/// </summary>
		public BaiduMapPoint location { get; set; }
		/// <summary>
		/// poi地址信息 
		/// </summary>
		public string address { get; set; }
		/// <summary>
		/// poi电话信息 
		/// </summary>
		public string telephone { get; set; }
		/// <summary>
		/// poi的唯一标示 
		/// </summary>
		public string uid { get; set; }
		/// <summary>
		/// poi的扩展信息，不同的poi类型，显示的detail_info字段不同。 
		/// </summary>
		public Object detail_info { get; set; }
	}

	public class BaiduMapGeocoderResultModel
	{
		/// <summary>
		/// 返回结果状态值， 成功返回0，其他值请查看下方返回码状态表。
		/// </summary>
		public int status { get; set; }
		public BaiduMapGeocoderResultItemModel result { get; set; }
	}
	public class BaiduMapGeocoderResultItemModel
	{
		/// <summary>
		/// 经纬度坐标
		/// </summary>
		public BaiduMapPoint location { get; set; }
		/// <summary>
		/// 位置的附加信息，是否精确查找。1为精确查找，0为不精确。
		/// </summary>
		public int precise { get; set; }
		/// <summary>
		/// 可信度
		/// </summary>
		public int confidence { get; set; }
		/// <summary>
		/// 地址类型
		/// </summary>
		public string level { get; set; }
	}

	public class BaiduMapGeocoderReverseResultModel
	{
		/// <summary>
		/// 返回结果状态值， 成功返回0，其他值请查看下方返回码状态表。
		/// </summary>
		public int status { get; set; }
		public BaiduMapGeocoderReverseResultItemModel result { get; set; }
	}
    public class BaiduMapGeocoderReverseResultItemModel
    {
        /// <summary>
        /// 经纬度坐标
        /// </summary>
        public BaiduMapPoint location { get; set; }
        /// <summary>
        /// 结构化地址信息
        /// </summary>
        public string formatted_address { get; set; }
        /// <summary>
        /// 所在商圈信息，如 "人民大学,中关村,苏州街" 
        /// </summary>
        public string business { get; set; }

        public addressComponent addressComponent { get; set; }
        public List<pois> pois { get; set; }
        public List<object> roads { get; set; }
        public List<pois> poiRegions { get; set; }

        /// <summary>
        /// 当前位置结合POI的语义化结果描述。 
        /// </summary>
        public string sematic_description { get; set; }

        public int cityCode { get; set; }
    }

	public class addressComponent
	{
		/// <summary>
		/// 国家
		/// </summary>
		public string country { get; set; }
		/// <summary>
		/// 省名
		/// </summary>
		public string province { get; set; }
		/// <summary>
		/// 城市名
		/// </summary>
		public string city { get; set; }
		/// <summary>
		/// 区县名
		/// </summary>
		public string district { get; set; }
		/// <summary>
		/// 街道名
		/// </summary>
		public string street { get; set; }
		/// <summary>
		/// 街道门牌号
		/// </summary>
		public string street_number { get; set; }
		/// <summary>
		/// 国家code  
		/// </summary>
		public string country_code { get; set; }
		/// <summary>
		/// 和当前坐标点的方向，当有门牌号的时候返回数据
		/// </summary>
		public string direction { get; set; }
		/// <summary>
		/// 和当前坐标点的距离，当有门牌号的时候返回数据 
		/// </summary>
		public string distance { get; set; }
	}
    public class pois
    {
        /// <summary>
        /// 地址信息
        /// </summary>
        public string addr { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public string cp { get; set; }
        /// <summary>
        /// 和当前坐标点的方向
        /// </summary>
        public string direction { get; set; }
        /// <summary>
        /// 离坐标点距离
        /// </summary>
        public string distance { get; set; }
        /// <summary>
        /// poi名称 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// poi类型，如’ 办公大厦,商务大厦’
        /// </summary>
        public string poiType { get; set; }
        /// <summary>
        /// poi坐标{x,y} 
        /// </summary>
        public poisPoint point { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string tel { get; set; }
        /// <summary>
        /// poi唯一标识
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string zip { get; set; }
        public pois parent_poi { get; set; }

        public string direction_desc { get; set; }
        public string tag { get; set; }

    }
    public class poisPoint
    {
        /// <summary>
        /// 纬度值 
        /// </summary>
        public float y { get; set; }
        /// <summary>
        /// 经度值 
        /// </summary>
        public float x { get; set; }

        #region 构造函数
        public poisPoint() { }
        public poisPoint(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion
    }

    /// <summary>
    /// 百度地图经纬度坐标 
    /// </summary>
    public class BaiduMapPoint
    {
        /// <summary>
        /// 纬度值 
        /// </summary>
        public float lat { get; set; }
        /// <summary>
        /// 经度值 
        /// </summary>
        public float lng { get; set; }


        #region 构造函数
        /// <summary>
        /// 百度地图经纬度坐标
        /// </summary>
        public BaiduMapPoint() { }
        /// <summary>
        /// 百度地图经纬度坐标
        /// </summary>
        /// <param name="lng">经度值</param>
        /// <param name="lat">纬度值</param>
        public BaiduMapPoint(float lng, float lat)
        {
            this.lng = lng;
            this.lat = lat;
        } 
        #endregion

    }

	/// <summary>
	/// 百度地图逆地理编码服务
	/// 坐标的类型，目前支持的坐标类型包括：bd09ll（百度经纬度坐标）、gcj02ll（国测局经纬度坐标）、wgs84ll（ GPS经纬度）
	/// </summary>
	public enum coordtype
	{
		/// <summary>
		/// bd09ll（百度经纬度坐标）
		/// </summary>
		bd09ll,
        /// <summary>
        /// gcj02ll（国测局经纬度坐标，又叫火星坐标系统，是加密插件）GCJ-02是由中国国家测绘局（G表示Guojia国家，C表示Cehui测绘，J表示Ju局）制订的地理信息系统的坐标系统。它是一种对经纬度数据的加密算法，即加入随机的偏差。国内出版的各种地图系统（包括电子形式），必须至少采用GCJ-02对地理位置进行首次加密。
		/// </summary>
		gcj02ll,
        /// <summary>
        /// wgs84ll（ GPS经纬度）WGS84:World Geodetic System 1984，是为GPS全球定位系统使用而建立的坐标系统。
        /// </summary>
        wgs84ll,
	}

}
