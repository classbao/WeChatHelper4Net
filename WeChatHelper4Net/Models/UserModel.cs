using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models
{
    /// <summary>
    /// 用户基本信息模型
    /// </summary>
    [Serializable]
    [DataContract]
    public class UserModel : RequestResult_errmsg
    {
        private byte _subscribe;
        private string _openid = string.Empty;
        private string _nickname = string.Empty;
        private byte _sex;
        private string _language = string.Empty;
        private string _city = string.Empty;
        private string _province = string.Empty;
        private string _country = string.Empty;
        private string _headimgurl = string.Empty;
        private int _subscribe_time;
        private string _unionid = string.Empty;
        private string _remark = string.Empty;
        private int _groupid;
        private List<int> _tagid_list;
        private List<string> _privilege;

        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        [DataMember]
        public byte subscribe { get { return _subscribe; } set { _subscribe = value; } }
        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        [DataMember]
        public string openid { get { return _openid; } set { _openid = value; } }
        /// <summary>
        /// 用户的昵称
        /// </summary>
        [DataMember]
        public string nickname { get { return _nickname; } set { _nickname = value; } }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        [DataMember]
        public byte sex { get { return _sex; } set { _sex = value; } }
        /// <summary>
        /// 用户的语言，简体中文为zh_CN，zh_CN 简体，zh_TW 繁体，en 英语
        /// </summary>
        [DataMember]
        public string language { get { return _language; } set { _language = value; } }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        [DataMember]
        public string city { get { return _city; } set { _city = value; } }
        /// <summary>
        /// 用户所在省份
        /// </summary>
        [DataMember]
        public string province { get { return _province; } set { _province = value; } }
        /// <summary>
        /// 用户所在国家
        /// </summary>
        [DataMember]
        public string country { get { return _country; } set { _country = value; } }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        [DataMember]
        public string headimgurl { get { return _headimgurl; } set { _headimgurl = value; } }
        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        [DataMember]
        public int subscribe_time { get { return _subscribe_time; } set { _subscribe_time = value; } }
        /// <summary>
        /// 用户在微信开放平台唯一标识
        /// </summary>
        [DataMember]
        public string unionid { get { return _unionid; } set { _unionid = value; } }
        /// <summary>
        /// 公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
        /// </summary>
        [DataMember]
        public string remark { get { return _remark; } set { _remark = value; } }
        /// <summary>
        /// 用户所在的分组ID
        /// </summary>
        [DataMember]
        public int groupid { get { return _groupid; } set { _groupid = value; } }
        /// <summary>
        /// 用户被打上的标签ID列表
        /// </summary>
        [DataMember]
        public List<int> tagid_list { get { return _tagid_list; } set { _tagid_list = value; } }

        /// <summary>
        /// 用户特权信息，json数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        [DataMember]
        public List<string> privilege { get { return _privilege; } set { _privilege = value; } }

    }

    /// <summary>
    /// 关注者列表模型
    /// </summary>
    [Serializable]
    [DataContract]
    public class UserListModel : RequestResult_errmsg
    {
        private int _total;
        private int _count;
        private OpenIdList _data;
        private string _next_openid = string.Empty;

        /// <summary>
        /// 关注该公众账号的总用户数
        /// </summary>
        [DataMember]
        public int total { get { return _total; } set { _total = value; } }
        /// <summary>
        /// 拉取的OPENID个数，最大值为10000
        /// </summary>
        [DataMember]
        public int count { get { return _count; } set { _count = value; } }
        /// <summary>
        /// 列表数据，OPENID的列表
        /// </summary>
        [DataMember]
        public OpenIdList data { get { return _data; } set { _data = value; } }
        /// <summary>
        /// 拉取列表的后一个用户的OPENID
        /// </summary>
        [DataMember]
        public string next_openid { get { return _next_openid; } set { _next_openid = value; } }

    }

    /// <summary>
    /// 列表数据，OPENID的列表
    /// </summary>
    [Serializable]
    [DataContract]
    public class OpenIdList
    {
        private string[] _openid;

        /// <summary>
        /// 列表数据，OPENID的列表
        /// </summary>
        [DataMember]
        public string[] openid { get { return _openid; } set { _openid = value; } }
    }
}
