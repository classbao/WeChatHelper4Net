using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace SampleWebApp.DB
{

    public static class DapperHelper
    {
        #region 私有字段和初始化

        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["MyBillConnStr"]?.ConnectionString;

        static DapperHelper()
        {
            // 配置Dapper默认设置
            SqlMapper.Settings.CommandTimeout = 30; // 默认命令超时30秒
            //SqlMapper.Settings.Buffered = true; // 缓冲查询结果
        }

        #endregion

        #region 连接管理

        /// <summary>
        /// 获取数据库连接对象（SELECT查询不需要打开状态哦！）
        /// </summary>
        /// <param name="needOpen">打开状态，默认打开</param>
        /// <returns></returns>
        private static IDbConnection GetConnection(bool needOpen = true)
        {
            if(string.IsNullOrEmpty(_connectionString))
                throw new ArgumentNullException("Connection string is not configured");

            var connection = new MySqlConnection(_connectionString);
            if(needOpen)
            {
                connection.Open();
            }

            return connection;
        }

        #endregion

        #region 基本CRUD操作

        /// <summary>
        /// 执行SQL语句并返回受影响的行数（适用于：增、删、改，等变更类的命令！）
        /// </summary>
        public static int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if(transaction != null)
                return transaction.Connection.Execute(sql, param, transaction, commandTimeout);

            using(var conn = GetConnection())
            {
                return conn.Execute(sql, param, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// 查询单个值(第一行第一列)：Execute parameterized SQL that selects a single value. 返回值：The first cell returned, as T.
        /// 常用于表主键是自增长类型的数据插入同时执行SELECT LAST_INSERT_ID(); 或者SELECT @@IDENTITY; 返回自增长主键值。但是，对于 VARCHAR 或非自增主键，函数会返回 0。也就是说，非自增主键的表 INSERT INTO 请用Execute方法返回受影响行数！！！
        /// </summary>
        public static T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if(transaction != null)
                return transaction.Connection.ExecuteScalar<T>(sql, param, transaction, commandTimeout);

            using(var conn = GetConnection())
            {
                return conn.ExecuteScalar<T>(sql, param, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// 查询返回第一条实体，获取"最近/默认"记录，不关心数量
        /// </summary>
        public static T QueryFirstOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if(transaction != null)
                return transaction.Connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout);

            using(var conn = GetConnection(false))
            {
                return conn.QueryFirstOrDefault<T>(sql, param, commandTimeout: commandTimeout);
            }
        }
        /// <summary>
        /// 查询单个实体，严格确保只有0或1条记录
        /// </summary>
        public static T QuerySingleOrDefault<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            if(transaction != null)
                return transaction.Connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout);

            using(var conn = GetConnection(false))
            {
                return conn.QuerySingleOrDefault<T>(sql, param, commandTimeout: commandTimeout);
            }
        }

        /// <summary>
        /// 查询实体列表
        /// </summary>
        public static IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            if(transaction != null)
                return transaction.Connection.Query<T>(sql, param, transaction, buffered, commandTimeout);

            using(var conn = GetConnection(false))
            {
                return conn.Query<T>(sql, param, commandTimeout: commandTimeout);
            }
        }

        #endregion

        #region 事务操作

        /// <summary>
        /// 执行事务操作（已实现提交、回滚逻辑）
        /// </summary>
        public static bool ExecuteTransaction(Action<IDbTransaction> action, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using(var conn = GetConnection())
            {
                using(var transaction = conn.BeginTransaction(isolationLevel))
                {
                    try
                    {
                        action(transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region 分页查询
        public static ResultBase<PagedResult<T>> QueryPaged<T>(string sql, string orderBy, int start, int length, object param = null, int? commandTimeout = null)
        {
            if(start < 0) throw new ArgumentException("start must be greater than or equal to 0");
            if(length < 1) throw new ArgumentException("length must be greater than 0");

            var countSql = $@"SELECT COUNT(*) FROM ({sql}) AS FilteredTotalRows;";
            var pagedSql = $@"{sql} {orderBy} LIMIT {start}, {length};";

            using(var conn = GetConnection(false))
            {
                var totalCount = conn.ExecuteScalar<int>(countSql, param, commandTimeout: commandTimeout);
                var items = conn.Query<T>(pagedSql, param, commandTimeout: commandTimeout);

                return new ResultBase<PagedResult<T>>()
                {
                    data = new PagedResult<T>(start, length, items, totalCount, totalCount) { }
                };
            }
        }

        /// <summary>
        /// 分页查询，适用于 ROW_NUMBER() 结合 PageIndex，PageSize 分页结果
        /// </summary>
        public static PagedResultVersatile<T> QueryPagedVersatile<T>(string sql, string orderBy, int pageIndex, int pageSize, object param = null, int? commandTimeout = null)
        {
            if(pageIndex < 1) throw new ArgumentException("Page index must be greater than 0");
            if(pageSize < 1) throw new ArgumentException("Page size must be greater than 0");

            var countSql = $"SELECT COUNT(1) FROM ({sql}) AS TotalCount";
            var pagedSql = $@"
            WITH Data AS (
                SELECT *, ROW_NUMBER() OVER (ORDER BY {orderBy}) AS RowNum
                FROM ({sql}) AS Query
            )
            SELECT * FROM Data
            WHERE RowNum BETWEEN {(pageIndex - 1) * pageSize + 1} AND {pageIndex * pageSize}";

            using(var conn = GetConnection(false))
            {
                var totalCount = conn.ExecuteScalar<int>(countSql, param, commandTimeout: commandTimeout);
                var items = conn.Query<T>(pagedSql, param, commandTimeout: commandTimeout);

                return new PagedResultVersatile<T>
                {
                    List = items,
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            }
        }

        #endregion
    }

    #region PagedResult
    /// <summary>
    ///     服务器处理状态枚举
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 失败
        /// </summary>
        FAIL = 100,
        /// <summary>
        /// 登录状态异常
        /// </summary>
        LoginEx = 10010,
        /// <summary>
        /// 当前处在锁定时间
        /// </summary>
        TimeLock = 10011,

        /// <summary>
        /// 成功
        /// </summary>
        SUCCESS = 200,
        /// <summary>
        /// 验证码验证成功
        /// </summary>
        ValidateCodeSuccessful = 20001,


        /// <summary>
        /// 未完成
        /// </summary>
        InComplete = 201,
        /// <summary>
        /// 已经完成
        /// </summary>
        Completed = 202,

        /// <summary>
        /// 错误
        /// </summary>
        ERROR = 500,
        /// <summary>
        /// 服务器端处理错误
        /// </summary>
        ServerError = 501,

        /// <summary>
        /// 授权或参数错误
        /// </summary>
        AuthOrParams = 400,
        /// <summary>
        /// 未授权的请求
        /// </summary>
        Unauthorized = 401,
        /// <summary>
        /// Token 过期
        /// </summary>
        TokenExpire = 411,
        /// <summary>
        /// 参数无效
        /// </summary>
        InvalidParameter = 420,
        /// <summary>
        /// 用户名或者密码错误
        /// </summary>
        NameOrPasswordError = 42001,
        /// <summary>
        /// 密码错误
        /// </summary>
        UserPwd = 42002,
        /// <summary>
        /// 验证码错误
        /// </summary>
        ValidateCodeFailed = 42003,
        /// <summary>
        /// 用户名是空
        /// </summary>
        UserNameEmpty = 42004,

        /// <summary>
        /// 不能正确匹配
        /// </summary>
        NotMatch = 421,

        /// <summary>
        /// 用户废弃
        /// </summary>
        UserDiscard = 43001,
        /// <summary>
        /// 用户禁用
        /// </summary>
        UserDisable = 43002,


        /// <summary>
        /// 未确认的
        /// </summary>
        UnConfirmed = 480,
        /// <summary>
        /// 当前值或者模型已经在数据库中存在
        /// </summary>
        Existed = 490,
        /// <summary>
        /// 当前值或者模型在数据库中不存在
        /// </summary>
        NotExisted = 491,

    }


    /// <summary>
    /// 数据结果最基本模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultBase<T>
    {
        /// <summary>
        /// 结果状态码
        /// </summary>
        public ResultCode code { get; set; }
        /// <summary>
        /// 结果：SUCCESS/FAIL
        /// </summary>
        public string msg { get; set; }
        public T data { get; set; }
        public string remark { get; set; }

        public ResultBase(ResultCode _code = ResultCode.SUCCESS, string _msg = "SUCCESS", T _data = default(T))
        {
            code = _code;
            msg = _msg;
            data = _data;
        }
    }

    /// <summary>
    /// 适用于 LIMIT (PageStart，PageLength) 分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T>
    {
        public int start { get; set; }
        public int length { get; set; }
        public IEnumerable<T> list { get; set; }
        public int totalRows { get; set; }
        public int filteredTotalRows { get; set; }

        public PagedResult(int _start = 0, int _length = 1, IEnumerable<T> _list = default(IEnumerable<T>), int _totalRows = 0, int _filteredTotalRows = 0
            )
        {
            start = _start;
            length = _length;
            list = _list;
            totalRows = _totalRows;
            filteredTotalRows = _filteredTotalRows;
        }
    }



    /// <summary>
    /// 适用于 ROW_NUMBER() 结合 PageIndex，PageSize 分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResultVersatile<T>
    {
        public IEnumerable<T> List { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }

    #endregion


}

/*
 * 使用示例：
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

        public static ResultBase<PagedResult<t_bill>> List(int page = 1, int pageSize = 10)
        {
            // 基础查询SQL
            var sql = "SELECT * FROM t_bill WHERE bill_ctime > 1";

            // 排序字段
            var orderBy = "bill_ctime DESC";

            // 执行分页查询
            var result = DapperHelper.QueryPaged<t_bill>(sql, orderBy, page, pageSize);
            return result;
        }

        public static string Add(t_bill bill)
        {
            // 基础插入SQL
            var sql = "INSERT INTO t_bill(bill_ID,bill_source_channel,bill_pay_channel,bill_third_notify_url,bill_appid,bill_mch_id,bill_fee_type,bill_total_fee,bill_time_start,bill_time_expire,bill_goods_tag,bill_notify_url,bill_trade_type,bill_product_id,bill_openid,bill_attach,bill_body,bill_device_info,bill_receipt,bill_profit_sharing,bill_scene_info,bill_spbill_create_ip,bill_ctime) VALUES(@bill_ID,@bill_source_channel,@bill_pay_channel,@bill_third_notify_url,@bill_appid,@bill_mch_id,@bill_fee_type,@bill_total_fee,@bill_time_start,@bill_time_expire,@bill_goods_tag,@bill_notify_url,@bill_trade_type,@bill_product_id,@bill_openid,@bill_attach,@bill_body,@bill_device_info,@bill_receipt,@bill_profit_sharing,@bill_scene_info,@bill_spbill_create_ip,@bill_ctime); SELECT LAST_INSERT_ID();"; //  SELECT @@IDENTITY;

            // 执行分页查询
            string result = DapperHelper.ExecuteScalar<string>(sql, bill);
            return result;
        }

        [Obsolete("统一下单成功后更新数据库")]
        public static int Update(string bill_ID, string bill_return_code, string bill_result_code, string bill_err_code, string bill_prepay_id, string bill_code_url)
        {
            long bill_bill_mtime = WeChatHelper4Net.Extend.TimestampHelper.ConvertTime(DateTime.Now);

            // 基础更新SQL
            var sql = "UPDATE t_bill SET bill_return_code=@bill_return_code,bill_result_code=@bill_result_code,bill_err_code=@bill_err_code,bill_prepay_id=@bill_prepay_id,bill_code_url=@bill_code_url,bill_bill_mtime=@bill_bill_mtime WHERE bill_ID = @bill_ID;";
            int result = DapperHelper.Execute(sql, new
            {
                bill_ID = bill_ID,
                bill_return_code = bill_return_code,
                bill_result_code = bill_result_code,
                bill_err_code = bill_err_code,
                bill_prepay_id = bill_prepay_id,
                bill_code_url = bill_code_url,
                bill_bill_mtime = bill_bill_mtime
            });

            return result;
        }

        /// <summary>
        /// 接收到“支付结果通知”更新数据库状态
        /// </summary>
        /// <param name="bill_ID"></param>
        /// <param name="bill_notify_total_fee"></param>
        /// <param name="bill_notify_fee_type"></param>
        /// <param name="bill_notify_time_end"></param>
        /// <param name="bill_notify_transaction_id"></param>
        /// <param name="bill_notifytime"></param>
        /// <returns></returns>
        public static int NotifyCompleteBill(string bill_ID, int bill_notify_total_fee, string bill_notify_fee_type, string bill_notify_time_end, string bill_notify_transaction_id, long bill_notifytime)
        {
            long bill_bill_mtime = WeChatHelper4Net.Extend.TimestampHelper.ConvertTime(DateTime.Now);

            // 基础更新SQL
            var sql = "UPDATE t_bill SET bill_notify_total_fee=@bill_notify_total_fee,bill_notify_fee_type=@bill_notify_fee_type,bill_notify_time_end=@bill_notify_time_end,bill_notify_transaction_id=@bill_notify_transaction_id WHERE bill_ID = @bill_ID;";
            int result = DapperHelper.Execute(sql, new
            {
                bill_ID = bill_ID,
                bill_notify_total_fee = bill_notify_total_fee,
                bill_notify_fee_type = bill_notify_fee_type,
                bill_notify_time_end = bill_notify_time_end,
                bill_notify_transaction_id = bill_notify_transaction_id,
                bill_notifytime = bill_notifytime,
                bill_bill_mtime = bill_bill_mtime
            });

            return result;
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
                DapperHelper.Execute("DELETE FROM t_bill WHERE bill_ID = @bill_ID;",
                    new { bill_ID = bill_ID }, transaction);

                // 删除用户
                //DapperHelper.Execute("DELETE FROM Users WHERE Id = @Id",
                //    new { bill_ID = bill_ID }, transaction);
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
                DapperHelper.Execute("DELETE FROM UserAddresses WHERE UserId IN @UserIds",
                    new { UserIds = ids }, transaction);

                // 删除多个用户
                DapperHelper.Execute("DELETE FROM Users WHERE Id IN @Ids",
                    new { Ids = ids }, transaction);
            });

            if(success)
                return success;
            else
                return false;

        }




    }

 */
