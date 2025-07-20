/**
* 命名空间: WeChatHelper4Net.Pay.notify
*
* 功 能： N/A
* 类 名： notifyModel
*
* Ver 变更日期 负责人 当前系统用户名 CLR版本 变更内容
* ───────────────────────────────────
* V0.01 2025/7/13 15:32:34 熊仔其人 xxh 4.0.30319.42000 初版
*
* Copyright (c) 2025 熊仔其人 Corporation. All rights reserved.
*┌─────────────────────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．   │
*│　版权所有：熊仔其人 　　　　　　　　　　　　　　　　　　　　　　　 │
*└─────────────────────────────────────────────────┘
*/

using System;


/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Pay.notify
{
    /// <summary>
    /// 微信返回支付通知模型
    /// </summary>
    public class notifyModel
    {
        public string appid { get; set; }
        public string attach { get; set; }
        public string bank_type { get; set; }
        public string cash_fee { get; set; }
        public string device_info { get; set; }
        public string fee_type { get; set; }
        public string is_subscribe { get; set; }
        public string mch_id { get; set; }
        public string nonce_str { get; set; }
        public string openid { get; set; }
        public string out_trade_no { get; set; }
        public string result_code { get; set; }
        public string return_code { get; set; }
        public string return_msg { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }

        public string sign { get; set; }
        public string time_end { get; set; }
        public int total_fee { get; set; }
        public string trade_type { get; set; }
        public string transaction_id { get; set; }

        //<xml>
        //	<appid>
        //		<!--[CDATA[wx7d00660ce462f8f9]]-->
        //	</appid>
        //	<attach>
        //		<!--[CDATA[附加数据]]-->
        //	</attach>
        //	<bank_type>
        //		<!--[CDATA[CMB_CREDIT]]-->
        //	</bank_type>
        //	<cash_fee>
        //		<!--[CDATA[1]]-->
        //	</cash_fee>
        //	<device_info>
        //		<!--[CDATA[WEB]]-->
        //	</device_info>
        //	<fee_type>
        //		<!--[CDATA[CNY]]-->
        //	</fee_type>
        //	<is_subscribe>
        //		<!--[CDATA[Y]]-->
        //	</is_subscribe>
        //	<mch_id>
        //		<!--[CDATA[1219538201]]-->
        //	</mch_id>
        //	<nonce_str>
        //		<!--[CDATA[E9RDuEsnysVFJ1qY]]-->
        //	</nonce_str>
        //	<openid>
        //		<!--[CDATA[o==NRtx58MS4JX9ilO_BV-VjBAGU]]-->
        //	</openid>
        //	<out_trade_no>
        //		<!--[CDATA[201504221636523252887]]-->
        //	</out_trade_no>
        //	<result_code>
        //		<!--[CDATA[SUCCESS]]-->
        //	</result_code>
        //	<return_code>
        //		<!--[CDATA[SUCCESS]]-->
        //	</return_code>
        //	<sign>
        //		<!--[CDATA[5BA340B56B781E4CE7710EC8B4988DF0]]-->
        //	</sign>
        //	<time_end>
        //		<!--[CDATA[20150422163657]]-->
        //	</time_end>
        //	<total_fee>1</total_fee>
        //	<trade_type>
        //		<!--[CDATA[JSAPI]]-->
        //	</trade_type>
        //	<transaction_id>
        //		<!--[CDATA[1009720334201504220081889158]]-->
        //	</transaction_id>
        //</xml>
    }
}
