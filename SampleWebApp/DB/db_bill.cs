using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleWebApp.DB
{
    public static class db_bill
    {
        public static t_bill GetById(string bill_ID)
        {
            // 基础查询SQL
            var sql = "SELECT * FROM t_bill WHERE bill_ID=@bill_ID ORDER BY bill_ctime DESC;";

            // 执行查询
            var result = DapperHelper.QueryFirstOrDefault<t_bill>(sql, new { bill_ID = bill_ID });
            return result;
        }

        public static ResultBase<PagedResult<t_bill>> List(int start = 0, int length = 10)
        {
            // 基础查询SQL
            var sql = "SELECT * FROM t_bill WHERE bill_ctime > 1";

            // 排序字段
            var orderBy = " ORDER BY bill_notifytime DESC, bill_bill_mtime DESC, bill_ctime DESC";

            // 执行分页查询
            var result = DapperHelper.QueryPaged<t_bill>(sql, orderBy, start, length);
            return result;
        }

        public static int Add(t_bill bill)
        {
            bill.bill_bill_mtime = WeChatHelper4Net.Extend.TimestampHelper.ConvertTime(DateTime.Now);
            // 基础插入SQL
            var sql = "INSERT INTO t_bill(bill_ID,bill_source_channel,bill_pay_channel,bill_third_notify_url,bill_appid,bill_mch_id,bill_fee_type,bill_total_fee,bill_time_start,bill_time_expire,bill_goods_tag,bill_notify_url,bill_trade_type,bill_product_id,bill_openid,bill_attach,bill_body,bill_device_info,bill_receipt,bill_profit_sharing,bill_scene_info,bill_spbill_create_ip,bill_prepay_id,bill_code_url,bill_ctime,bill_bill_mtime) VALUES(@bill_ID,@bill_source_channel,@bill_pay_channel,@bill_third_notify_url,@bill_appid,@bill_mch_id,@bill_fee_type,@bill_total_fee,@bill_time_start,@bill_time_expire,@bill_goods_tag,@bill_notify_url,@bill_trade_type,@bill_product_id,@bill_openid,@bill_attach,@bill_body,@bill_device_info,@bill_receipt,@bill_profit_sharing,@bill_scene_info,@bill_spbill_create_ip,@bill_prepay_id,@bill_code_url,@bill_ctime,@bill_bill_mtime);"; // SELECT LAST_INSERT_ID();  SELECT @@IDENTITY;

            // 执行分页查询
            int rowsAffected = DapperHelper.Execute(sql, bill);
            return rowsAffected;
        }

        [Obsolete("统一下单成功后更新数据库")]
        public static int Update(string bill_ID, string bill_return_code, string bill_result_code, string bill_err_code, string bill_prepay_id, string bill_code_url)
        {
            long bill_bill_mtime = WeChatHelper4Net.Extend.TimestampHelper.ConvertTime(DateTime.Now);

            // 基础更新SQL
            var sql = "UPDATE t_bill SET bill_return_code=@bill_return_code,bill_result_code=@bill_result_code,bill_err_code=@bill_err_code,bill_prepay_id=@bill_prepay_id,bill_code_url=@bill_code_url,bill_bill_mtime=@bill_bill_mtime WHERE bill_ID = @bill_ID;";
            int rowsAffected = DapperHelper.Execute(sql, new
            {
                bill_ID = bill_ID,
                bill_return_code = bill_return_code,
                bill_result_code = bill_result_code,
                bill_err_code = bill_err_code,
                bill_prepay_id = bill_prepay_id,
                bill_code_url = bill_code_url,
                bill_bill_mtime = bill_bill_mtime
            });

            return rowsAffected;
        }

        /// <summary>
        /// 接收到“支付结果通知”更新数据库状态
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="bill_ID"></param>
        /// <returns></returns>
        public static int NotifyCompleteBill(t_bill bill)
        {
            bill.bill_bill_mtime = WeChatHelper4Net.Extend.TimestampHelper.ConvertTime(DateTime.Now);

            // 基础更新SQL
            var sql = "UPDATE t_bill SET bill_return_code=@bill_return_code,bill_result_code=@bill_result_code,bill_err_code=@bill_err_code,bill_notify_total_fee=@bill_notify_total_fee,bill_notify_fee_type=@bill_notify_fee_type,bill_notify_time_end=@bill_notify_time_end,bill_notify_transaction_id=@bill_notify_transaction_id,bill_notifytime=@bill_notifytime,bill_bill_mtime=@bill_bill_mtime WHERE bill_ID = @bill_ID;";
            int rowsAffected = DapperHelper.Execute(sql, bill);

            return rowsAffected;
        }

        /// <summary>
        /// 删除数据(多表事务)
        /// </summary>
        /// <param name="bill_ID"></param>
        /// <returns></returns>
        public static bool Delete(string bill_ID)
        {
            bool success = DapperHelper.ExecuteTransaction(transaction =>
            {
                // 删除用户地址
                int rowsAffected = DapperHelper.Execute("DELETE FROM t_bill WHERE bill_ID = @bill_ID;", new { bill_ID = bill_ID }, transaction);

                // 删除用户
                //DapperHelper.Execute("DELETE FROM Users WHERE Id = @Id", new { bill_ID = bill_ID }, transaction);
            });

            if(success)
                return success;
            else
                return false;

        }

        /// <summary>
        /// /// 删除多行数据(多表多行事务)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool Delete(List<int> ids)
        {
            if(ids == null || ids.Count == 0)
            {
                return false;
            }

            bool success = DapperHelper.ExecuteTransaction(transaction =>
            {
                // 删除多个用户的地址
                int rowsAffected1 = DapperHelper.Execute("DELETE FROM UserAddresses WHERE UserId IN @UserIds", new { UserIds = ids }, transaction);

                // 删除多个用户
                int rowsAffected2 = DapperHelper.Execute("DELETE FROM Users WHERE Id IN @Ids", new { Ids = ids }, transaction);
            });

            if(success)
                return success;
            else
                return false;

        }




    }
}