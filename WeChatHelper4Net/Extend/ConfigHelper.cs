using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace WeChatHelper4Net.Extend
{
    /// <summary>
    /// 用于读取配置文件信息的类
    /// </summary>
    public class ConfigHelper
    {
        #region 读取配置信息的方法
        /// <summary>
        /// 读取配置信息的方法
        /// </summary>
        /// <param name="key">配置信息键</param>
        /// <returns>配置信息值</returns>
        public static string GetAppSetting(string key)
        {
            return GetAppSetting(key, true);
        }
        /// <summary>
        /// 读取配置信息的方法
        /// </summary>
        /// <param name="key">配置信息键</param>
        /// <param name="throwException">配置文件里没有该配置时是否抛出异常</param>
        /// <returns>配置信息值</returns>
        public static string GetAppSetting(string key, bool throwException)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch
            {
                if(throwException)
                    throw new Exception("没有在配置文件里找到名为'" + key + "'的配置信息。");
                else
                    return "";
            }
        }

        /// <summary>
        /// 读取配置信息的方法
        /// </summary>
        /// <param name="key">配置信息键</param>
        /// <returns>Integer类型的配置信息值</returns>
        public static int GetAppSettingInteger(string key)
        {
            return GetAppSettingInteger(key, true);
        }
        /// <summary>
        /// 读取配置信息的方法
        /// </summary>
        /// <param name="key">配置信息键</param>
        /// <param name="throwException">配置文件里没有该配置时是否抛出异常</param>
        /// <returns>Integer类型的配置信息值</returns>
        public static int GetAppSettingInteger(string key, bool throwException)
        {
            try
            {
                string appSettingValue = ConfigurationManager.AppSettings[key];
                return Convert.ToInt32(appSettingValue ?? "");
            }
            catch
            {
                if(throwException)
                    throw new Exception("没有在配置文件里找到名为'" + key + "'的配置信息。");
                else
                    return 0;
            }
        }

        /// <summary>
        /// 读取配置信息的方法
        /// </summary>
        /// <param name="key">配置信息键</param>
        /// <returns>Boolean类型的配置信息值</returns>
        public static bool GetAppSettingBoolean(string key)
        {
            return GetAppSettingBoolean(key, true);
        }
        /// <summary>
        /// 读取配置信息的方法
        /// </summary>
        /// <param name="key">配置信息键</param>
        /// <param name="throwException">配置文件里没有该配置时是否抛出异常</param>
        /// <returns>Boolean类型的配置信息值</returns>
        public static bool GetAppSettingBoolean(string key, bool throwException)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToLower().Equals("true");
            }
            catch
            {
                if(throwException)
                    throw new Exception("没有在配置文件里找到名为'" + key + "'的配置信息。");
                else
                    return false;
            }
        }
        #endregion

        #region 读取连接字符串信息的方法
        /// <summary>
        /// 读取连接字符串信息的方法
        /// </summary>
        /// <param name="key">连接字符串信息键</param>
        /// <returns>连接字符串信息值</returns>
        public static string GetConnectionString(string key)
        {
            try { return ConfigurationManager.ConnectionStrings[key].ConnectionString; }
            catch { throw new Exception("没有在配置文件里找到名为'" + key + "'的连接字符串信息。"); }
        }
        #endregion


        /// <summary>
        /// 加载config
        /// </summary>
        /// <param name="configFilePath"></param>
        public static void LoadConfig(string configFilePath)
        {
            try
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = configFilePath
                };
                Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                foreach(KeyValueConfigurationElement element in config.AppSettings.Settings)
                {
                    ConfigurationManager.AppSettings[element.Key] = element.Value;
                }
                foreach(KeyValueConfigurationElement element in config.ConnectionStrings.ConnectionStrings)
                {
                    ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings(element.Key, element.Value));
                }
            }
            catch(Exception exception)
            {
                throw new Exception("config文件不存在或配置错误." + exception.Message);
            }
        }

    }
}
