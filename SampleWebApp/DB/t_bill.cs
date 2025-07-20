using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleWebApp.DB
{
    public class t_bill
    {
        public string bill_source_channel { get; set; }
        public string bill_pay_channel { get; set; }
        public string bill_third_notify_url { get; set; }


        #region 统一下单
        public string bill_ID { get; set; }


        public string bill_appid { get; set; }
        public string bill_mch_id { get; set; }
        public string bill_fee_type { get; set; }
        public int bill_total_fee { get; set; }
        public string bill_time_start { get; set; }
        public string bill_time_expire { get; set; }
        public string bill_goods_tag { get; set; }
        public string bill_notify_url { get; set; }
        public string bill_trade_type { get; set; }
        public string bill_product_id { get; set; }
        public string bill_openid { get; set; }
        public string bill_attach { get; set; }
        public string bill_body { get; set; }
        public string bill_device_info { get; set; }
        public string bill_receipt { get; set; }
        public string bill_profit_sharing { get; set; }
        public string bill_scene_info { get; set; }
        public string bill_spbill_create_ip { get; set; }

        #endregion


        #region 统一下单结果（同步）

        public string bill_return_code { get; set; }
        public string bill_result_code { get; set; }
        public string bill_err_code { get; set; }
        public string bill_prepay_id { get; set; }
        public string bill_code_url { get; set; }

        #endregion


        #region 支付结果通知（异步）

        public int bill_notify_total_fee { get; set; }
        public string bill_notify_fee_type { get; set; }
        public string bill_notify_time_end { get; set; }
        public string bill_notify_transaction_id { get; set; }

        #endregion
        

        public long bill_ctime { get; set; }
        public long bill_notifytime { get; set; }
        public long bill_bill_mtime { get; set; }
    }

}