using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleWebApp.Models
{
    /*
     * 该Model对应您项目数据库表及字段，请先执行如下SQL脚本（详情请看 t_wechattokenorticket.sql）。
insert into t_wechattokenorticket(appid,type) values('您的appid', 'AccessToken');
insert into t_wechattokenorticket(appid,type) values('您的appid', 'JSApiTicket');
select * from t_wechattokenorticket;
     */
    /// <summary>
    /// 存储微信AccessToken，JSTicket的模型，该模型对应您项目数据库表及字段（详情请看 t_wechattokenorticket.sql），可以实现select，update操作。
    /// 正在运行该系统前，请自行执行脚本添加2行数据（确保有且仅有2行，appid，type必须完全匹配）。
    /// 一般情况下该模型可迁移到您项目的Model层。
    /// </summary>
    public class TbWeChatTokenOrTicketModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 您的appid
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// AccessToken值或JSApiTicket值
        /// </summary>
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string expires_time { get; set; }
        public int errcode { get; set; }
        public string errmsg { get; set; }
        /// <summary>
        /// AccessToken/JSApiTicket
        /// </summary>
        public string type { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}