#region 描述信息
/**
* 文件名：PositionUtil
* 类   名：PositionUtil
* 命名空间：WeChatHelper4Net.Map
* 当前系统用户名：XIONGXUEHAO
* 当前用户所在的域：KINGSOFT
* 当前机器名称：BZD14582-PC
* 注册的组织名：Microsoft
* 时间：2019/1/17 10:08:54
* CLR：4.0.30319.42000 
* GUID: 52d6258e-daf4-4a44-9d8e-1801f64623d6 
* 当前系统时间：2019
* Copyright (c) 2019 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．    │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　   │
*└─────────────────────────────────────────────────┘
* * Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2019/1/17 10:08:54 熊仔其人 XIONGXUEHAO 4.0.30319.42000 初版
**/
#endregion

using System;

namespace WeChatHelper4Net.Map
{
/**
* 各地图API坐标系统比较与转换;
* WGS84坐标系：即地球坐标系，国际上通用的坐标系。设备一般包含GPS芯片或者北斗芯片获取的经纬度为WGS84地理坐标系,
* 谷歌地图采用的是WGS84地理坐标系（中国范围除外）;
* GCJ02坐标系：即火星坐标系，是由中国国家测绘局制订的地理信息系统的坐标系统。由WGS84坐标系经加密后的坐标系。
* 谷歌中国地图和搜搜中国地图采用的是GCJ02地理坐标系; BD09坐标系：即百度坐标系，GCJ02坐标系经加密后的坐标系;
* 搜狗坐标系、图吧坐标系等，估计也是在GCJ02基础上加密而成的。
*/

    /// <summary>
    /// 经纬度转换组件
    /// </summary>
    public class LatLonUtil
    {
        /// <summary>
        /// 圆周率π的近似值
        /// </summary>
        public static double pi = 3.1415926535897932384626;
        /// <summary>
        /// 椭球体长半轴
        /// </summary>
        public static double ELLIPSOID_PARAM_A = 6378245.0;

        /// <summary>
        /// 百度坐标系圆周率π的近似值
        /// </summary>
        private static double bd_pi = 3.14159265358979324 * 3000.0 / 180.0;

        /**
     * Krasovsky 1940 (北京54)椭球长半轴第一偏心率平方
     * 计算方式：
     * 长半轴：
     * a = 6378245.0
     * 扁率：
     * 1/f = 298.3（变量相关计算为：(a-b)/a）
     * 短半轴：
     * b = 6356863.0188 (变量相关计算方法为：b = a * (1 - f))
     * 第一偏心率平方:
     * e2 = (a^2 - b^2) / a^2;
     */
        /// <summary>
        /// 椭球长半轴第一偏心率平方
        /// </summary>
        public static double ELLIPSOID_PARAM_E2 = 0.00669342162296594323;

        /**
         * 84 to 火星坐标系 (GCJ-02) World Geodetic System ==> Mars Geodetic System
         * 
         * @param lat
         * @param lon
         * @return
         */
        /// <summary>
        /// GPS84 to 火星坐标系 (GCJ-02) World Geodetic System ==> Mars Geodetic System
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lon">经度</param>
        /// <returns></returns>
        public static LatLon gps84_To_Gcj02(double lat, double lon)
        {
            if(outOfChina(lat, lon))
            {
                return null;
            }
            double dLat = transformLat(lon - 105.0, lat - 35.0);
            double dLon = transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ELLIPSOID_PARAM_E2 * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((ELLIPSOID_PARAM_A * (1 - ELLIPSOID_PARAM_E2)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (ELLIPSOID_PARAM_A / sqrtMagic * Math.Cos(radLat) * pi);
            double mgLat = lat + dLat;
            double mgLon = lon + dLon;
            return new LatLon(mgLat, mgLon);
        }

        /**
         * * 火星坐标系 (GCJ-02) to 84 
         * * @param lon 
         * * @param lat
         * * @return
         * */
        /// <summary>
        /// 火星坐标系 (GCJ-02) to GPS84
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static LatLon gcj_To_Gps84(double lat, double lon)
        {
            LatLon gps = transform(lat, lon);
            double lontitude = lon * 2 - gps.getWgLon();
            double latitude = lat * 2 - gps.getWgLat();
            return new LatLon(latitude, lontitude);
        }

        /**
         * 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法 将 GCJ-02 坐标转换成 BD-09 坐标
         * 
         * @param gg_lat
         * @param gg_lon
         */
        /// <summary>
        /// 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法 将 GCJ-02 坐标转换成 BD-09 坐标
        /// </summary>
        /// <param name="gg_lat"></param>
        /// <param name="gg_lon"></param>
        /// <returns></returns>
        public static LatLon gcj02_To_Bd09(double gg_lat, double gg_lon)
        {
            double x = gg_lon, y = gg_lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * bd_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * bd_pi);
            double bd_lon = z * Math.Cos(theta) + 0.0065;
            double bd_lat = z * Math.Sin(theta) + 0.006;
            return new LatLon(bd_lat, bd_lon);
        }

        /**
         * * 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法
         * * 将 BD-09 坐标转换成GCJ-02 坐标 
         * * @param bd_lat 
         * * @param bd_lon 
         * * @return
         */
        /// <summary>
        /// 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法 * * 将 BD-09 坐标转换成GCJ-02 坐标
        /// </summary>
        /// <param name="bd_lat"></param>
        /// <param name="bd_lon"></param>
        /// <returns></returns>
        public static LatLon bd09_To_Gcj02(double bd_lat, double bd_lon)
        {
            double x = bd_lon - 0.0065, y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * bd_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * bd_pi);
            double gg_lon = z * Math.Cos(theta);
            double gg_lat = z * Math.Sin(theta);
            return new LatLon(gg_lat, gg_lon);
        }

        /**
         * (BD-09)-->84
         * @param bd_lat
         * @param bd_lon
         * @return
         */
        public static LatLon bd09_To_Gps84(double bd_lat, double bd_lon)
        {

            LatLon gcj02 = LatLonUtil.bd09_To_Gcj02(bd_lat, bd_lon);
            LatLon map84 = LatLonUtil.gcj_To_Gps84(gcj02.getWgLat(),
                    gcj02.getWgLon());
            return map84;

        }

        /// <summary>
        /// 是否在中国区域内（矩形区域）
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static Boolean outOfChina(double lat, double lon)
        {
            if(lon < 72.004 || lon > 137.8347)
                return true;
            if(lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }

        public static LatLon transform(double lat, double lon)
        {
            if(outOfChina(lat, lon))
            {
                return new LatLon(lat, lon);
            }
            double dLat = transformLat(lon - 105.0, lat - 35.0);
            double dLon = transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ELLIPSOID_PARAM_E2 * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((ELLIPSOID_PARAM_A * (1 - ELLIPSOID_PARAM_E2)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (ELLIPSOID_PARAM_A / sqrtMagic * Math.Cos(radLat) * pi);
            double mgLat = lat + dLat;
            double mgLon = lon + dLon;
            return new LatLon(mgLat, mgLon);
        }

        public static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y
                    + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        public static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1
                    * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0
                    * pi)) * 2.0 / 3.0;
            return ret;
        }

        //public static void main(String[] args)
        //{

        //    // 北斗芯片获取的经纬度为WGS84地理坐标 31.426896,119.496145
        //    LatLon gps = new LatLon(31.426896, 119.496145);
        //    Console.WriteLine("gps :" + gps);
        //    LatLon gcj = gps84_To_Gcj02(gps.getWgLat(), gps.getWgLon());
        //    Console.WriteLine("gcj :" + gcj);
        //    LatLon star = gcj_To_Gps84(gcj.getWgLat(), gcj.getWgLon());
        //    Console.WriteLine("star:" + star);
        //    LatLon bd = gcj02_To_Bd09(gcj.getWgLat(), gcj.getWgLon());
        //    Console.WriteLine("bd  :" + bd);
        //    LatLon gcj2 = bd09_To_Gcj02(bd.getWgLat(), bd.getWgLon());
        //    Console.WriteLine("gcj :" + gcj2);
        //}
    }

    /// <summary>
    /// 经纬度坐标模型
    /// </summary>
    public class LatLon
    {
        private double wgLat;
        private double wgLon;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wgLat">纬度</param>
        /// <param name="wgLon">经度</param>
        public LatLon(double wgLat, double wgLon)
        {
            setWgLat(wgLat);
            setWgLon(wgLon);
        }
        /// <summary>
        /// 获取纬度
        /// </summary>
        /// <returns></returns>
        public double getWgLat()
        {
            return wgLat;
        }
        /// <summary>
        /// 设置纬度
        /// </summary>
        /// <param name="wgLat"></param>
        public void setWgLat(double wgLat)
        {
            this.wgLat = wgLat;
        }
        /// <summary>
        /// 获取经度
        /// </summary>
        /// <returns></returns>
        public double getWgLon()
        {
            return wgLon;
        }
        /// <summary>
        /// 设置经度
        /// </summary>
        /// <param name="wgLon"></param>
        public void setWgLon(double wgLon)
        {
            this.wgLon = wgLon;
        }
        /// <summary>
        /// 转换字符串格式“纬度,经度”
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            return wgLat + "," + wgLon;
        }
    }

}