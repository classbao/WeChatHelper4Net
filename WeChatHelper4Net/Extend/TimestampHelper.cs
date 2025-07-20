/**
* 命名空间: WeChatHelper4Net.Extend
*
* 功 能： N/A
* 类 名： TimestampHelper
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/13 13:09:30 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2025 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;

namespace WeChatHelper4Net.Extend
{
    /// <summary>
    /// 时间戳
    /// </summary>
    public static class TimestampHelper
    {
        #region BeijingEpoch（性能优化）
        /// <summary>
        /// UTC基准时间：1970-01-01 08:00:00（北京时间）。性能优化建议：将基准时间设为静态变量高频调用场景。
        /// </summary>
        private static readonly DateTime BeijingEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(8);
        /// <summary>
        /// 生成基于北京时间基准时间（+8）的当前时刻的时间戳（秒）
        /// </summary>
        /// <returns></returns>
        public static long ToBeijingTimestamp()
        {
            return (long)(DateTime.UtcNow - BeijingEpoch).TotalSeconds;  // 使用 long 避免 2038 问题
        }
        /// <summary>
        /// 生成基于北京时间基准时间（+8）的目标时刻的时间戳（秒）
        /// </summary>
        /// <param name="targetTime"></param>
        /// <returns></returns>
        public static long ToBeijingTimestamp(DateTime targetTime)
        {
            return (long)(targetTime - BeijingEpoch).TotalSeconds;  // 使用 long 避免 2038 问题
        }
        /// <summary>
        /// 将时间戳（秒）还原为北京时间基准时间（+8）
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime FromBeijingTimestamp(long timestamp)
        {
            return BeijingEpoch.AddSeconds(timestamp);
        }
        #endregion

        #region 基于北京时间的时间戳转换（性能优化）
        private static readonly DateTime baseTimeBeijing = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 时间戳转换为标准时间格式（基准时间为"1970-1-1 08:00:00"）
        /// </summary>
        /// <param name="TimeStamp">时间戳</param>
        /// <returns>标准时间格式</returns>
        public static DateTime ConvertTime(long TimeStamp)
        {
            return baseTimeBeijing.AddSeconds(TimeStamp);
        }
        /// <summary>
        /// 时间戳转换为标准时间格式（基准时间为"1970-1-1 08:00:00"）
        /// </summary>
        /// <param name="TimeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertTime(string TimeStamp)
        {
            return ConvertTime(Convert.ToInt64(TimeStamp));
        }
        /// <summary>
        /// 标准时间格式转换为时间戳（基准时间为"1970-1-1 08:00:00"）
        /// </summary>
        /// <param name="time">标准时间格式</param>
        /// <returns>时间戳</returns>
        public static long ConvertTime(DateTime time)
        {
            TimeSpan ts = time - baseTimeBeijing;
            return (long)ts.TotalSeconds;
        }
        #endregion



        #region 不同时区通用版本

        /// <summary>
        /// 生成基于自定义基准时间的当前时刻的时间戳（秒）
        /// </summary>
        /// <param name="baseTime">自定义基准时间</param>
        /// <param name="useUtc">是否使用UTC时间计算</param>
        /// <returns>long类型时间戳，规避2038问题</returns>
        public static long ToTimestamp(DateTime baseTime, bool useUtc = false)
        {
            DateTime currentTime = useUtc ? DateTime.UtcNow : DateTime.Now;
            TimeSpan timeSpan = currentTime - baseTime;
            return (long)timeSpan.TotalSeconds;  // 使用 long 避免 2038 问题
        }
        /// <summary>
        /// 生成基于自定义基准时间的目标时刻的时间戳（秒）
        /// </summary>
        /// <param name="baseTime"></param>
        /// <param name="targetTime"></param>
        /// <returns></returns>
        public static long ToTimestamp(DateTime baseTime, DateTime targetTime)
        {
            TimeSpan timeSpan = targetTime - baseTime;
            return (long)timeSpan.TotalSeconds;  // 使用 long 避免 2038 问题
        }

        /// <summary>
        /// 从自定义时间戳还原时间
        /// </summary>
        /// <param name="timestamp">时间戳（秒）</param>
        /// <param name="baseTime">自定义基准时间</param>
        /// <returns>还原后的DateTime</returns>
        public static DateTime FromTimestamp(long timestamp, DateTime baseTime)
        {
            return baseTime.AddSeconds(timestamp);
        }

        /// <summary>
        /// 生成基于时区偏移的Unix时间戳（秒）当前时刻的
        /// </summary>
        /// <param name="timeZoneOffset">基于UTC基准时区偏移（小时）</param>
        /// <returns>long类型时间戳</returns>
        public static long ToTimeZoneAdjustedTimestamp(int timeZoneOffset = 0)
        {
            DateTime adjustedEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(timeZoneOffset);
            return ToTimestamp(adjustedEpoch, true);
        }
        /// <summary>
        /// 生成基于时区偏移的Unix时间戳（秒） 目标时刻的
        /// </summary>
        /// <param name="targetTime">目标时刻</param>
        /// <param name="timeZoneOffset">基于UTC基准时区偏移（小时）</param>
        /// <returns></returns>
        public static long ToTimeZoneAdjustedTimestamp(DateTime targetTime, int timeZoneOffset = 0)
        {
            DateTime adjustedEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(timeZoneOffset);
            return ToTimestamp(adjustedEpoch, targetTime);
        }

        /// <summary>
        /// 还原基于时区偏移的Unix时间戳（秒）
        /// </summary>
        /// <param name="timestamp">时间戳（秒）</param>
        /// <param name="timeZoneOffset">基于UTC基准时区偏移（小时）</param>
        /// <returns></returns>
        public static DateTime FromTimeZoneAdjustedTimestamp(long timestamp, int timeZoneOffset = 0)
        {
            DateTime adjustedEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(timeZoneOffset);
            return FromTimestamp(timestamp, adjustedEpoch);
        }



        #endregion

        /* 测试用例：
            var t1 = TimestampHelper.ConvertTime(1752389251); // 2025-07-13 14:47:31
            var t2 = TimestampHelper.ConvertTime(Convert.ToDateTime("2025-07-13 14:47:31"));

            var t3 = TimestampHelper.FromBeijingTimestamp(1752389251);
            var t4 = TimestampHelper.ToBeijingTimestamp(Convert.ToDateTime("2025-07-13 14:47:31"));

            var t5 = TimestampHelper.FromTimestamp(1752389251, Convert.ToDateTime("1970-01-01 08:00:00"));
            var t6 = TimestampHelper.ToTimestamp(Convert.ToDateTime("1970-01-01 08:00:00"), Convert.ToDateTime("2025-07-13 14:47:31"));

            var t7 = TimestampHelper.FromTimeZoneAdjustedTimestamp(1752389251, 8);
            var t8 = TimestampHelper.ToTimeZoneAdjustedTimestamp(Convert.ToDateTime("2025-07-13 14:47:31"), 8);
        */

    }
}
