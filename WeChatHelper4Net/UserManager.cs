using System;
using System.Collections.Generic;
using System.Text;
using WeChatHelper4Net.Extend;
using WeChatHelper4Net.Models;

/*
 * 微信公众账号API
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net
{
	/// <summary>
	/// 用户管理
	/// </summary>
	public class UserManager
	{
		#region 分组管理接口
		/// <summary>
		/// 查询所有分组
		/// </summary>
		/// <returns>GroupsModel</returns>
		public static GroupsModel GetGroup(string access_token)
		{
			try
			{
				string url = Common.ApiUrl + string.Format("groups/get?access_token={0}", access_token);
				string result = HttpRequestHelper.Request(url, HttpRequestHelper.Method.GET);
				if (!string.IsNullOrEmpty(result))
				{
					return JsonHelper.DeSerialize<GroupsModel>(result);
				}
			}
			catch (Exception Ex)
			{
				LogHelper.Save(Ex, "GetGroup", LogTime.day);
				throw Ex;
			}
			return new GroupsModel();
		}

		/// <summary>
		/// 查询用户所在分组
		/// </summary>
		/// <param name="openid">用户的OpenID</param>
		/// <returns>GroupModel</returns>
		public static GroupModel GetGroupID(string openid, string access_token)
		{
			if (string.IsNullOrEmpty(openid)) return new GroupModel();
			try
			{
				string url = Common.ApiUrl + string.Format("groups/getid?access_token={0}", access_token);
				string data = "{\"openid\":\"OpenId\"}";
				data = data.Replace("OpenId", openid);
				string result = HttpRequestHelper.Request(url, data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
				if (!string.IsNullOrEmpty(result))
				{
					return JsonHelper.DeSerialize<WeChatHelper4Net.Models.GroupModel>(result);
				}
			}
			catch (Exception Ex)
			{
				LogHelper.Save(Ex, "GetGroupID", LogTime.day);
				throw Ex;
			}
			return new GroupModel();
		}

		/// <summary>
		/// 修改分组名
		/// </summary>
		/// <param name="groupId">分组id，由微信分配</param>
		/// <param name="name">分组名字（30个字符以内）</param>
		/// <returns>RequestResultBaseModel</returns>
		public static RequestResultBaseModel UpdateGroupName(int groupId, string name, string access_token)
		{
			if (groupId == null || groupId < 1 || string.IsNullOrEmpty(name)) return new RequestResultBaseModel();
			try
			{
				string url = Common.ApiUrl + string.Format("groups/update?access_token={0}", access_token);
				string data = "{\"group\":{\"id\":GroupID,\"name\":\"GroupName\"}}";
				data = data.Replace("GroupID", groupId.ToString())
					.Replace("GroupName", name);
				string result = HttpRequestHelper.Request(url, data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
				if (!string.IsNullOrEmpty(result))
				{
					return JsonHelper.DeSerialize<WeChatHelper4Net.Models.RequestResultBaseModel>(result);
				}
			}
			catch (Exception Ex)
			{
				LogHelper.Save(Ex, "UpdateGroupName", LogTime.day);
				throw Ex;
			}
			return new RequestResultBaseModel();
		}

		/// <summary>
		/// 移动用户分组
		/// </summary>
		/// <param name="openid">用户的OpenID</param>
		/// <param name="groupId">分组id，由微信分配</param>
		/// <returns>RequestResultBaseModel</returns>
		public static RequestResultBaseModel MoveUserToGroup(string openid, int groupId, string access_token)
		{
			if (groupId == null || groupId < 1 || string.IsNullOrEmpty(openid)) return new RequestResultBaseModel();
			try
			{
				string url = Common.ApiUrl + string.Format("groups/members/update?access_token={0}", access_token);
				string data = "{\"openid\":\"OpenId\",\"to_groupid\":GroupID}";
				data = data.Replace("GroupID", groupId.ToString())
					.Replace("OpenId", openid);
				string result = HttpRequestHelper.Request(url, data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
				if (!string.IsNullOrEmpty(result))
				{
					return JsonHelper.DeSerialize<WeChatHelper4Net.Models.RequestResultBaseModel>(result);
				}
			}
			catch (Exception Ex)
			{
				LogHelper.Save(Ex, "MoveUserToGroup", LogTime.day);
				throw Ex;
			}
			return new RequestResultBaseModel();
		}

		/// <summary>
		/// 创建分组
		/// </summary>
		/// <param name="name">分组名字（30个字符以内）</param>
		/// <returns>JSON数据包</returns>
		public static GroupSubModel CreateGroup(string name, string access_token)
		{
			if (string.IsNullOrEmpty(name)) return new GroupSubModel();
			try
			{
				string url = Common.ApiUrl + string.Format("groups/create?access_token={0}", access_token);
				string data = "{\"group\":{\"name\":\"GroupName\"}}";
				data = data.Replace("GroupName", name);
				string result = HttpRequestHelper.Request(url, data, HttpRequestHelper.Method.POST, System.Text.Encoding.UTF8);
				if (!string.IsNullOrEmpty(result))
				{
					return JsonHelper.DeSerialize<WeChatHelper4Net.Models.GroupSubModel>(result);
				}
			}
			catch (Exception Ex)
			{
				LogHelper.Save(Ex, "CreateGroup", LogTime.day);
				throw Ex;
			}
			return new GroupSubModel();
		}
		#endregion

		#region 获取用户基本信息
		/// <summary>
		/// 获取用户基本信息，返回JSON数据包
		/// </summary>
		/// <param name="openid">普通用户的标识，对当前公众号唯一</param>
		/// <returns>UserModel</returns>
		public static UserModel GetUserInfo(string openid, string access_token)
		{
			if (string.IsNullOrEmpty(openid)) return new UserModel();
			string result = string.Empty;
			try
			{
				string url = Common.ApiUrl + string.Format("user/info?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);
				result = HttpRequestHelper.Request(url, HttpRequestHelper.Method.GET);
				if (!string.IsNullOrEmpty(result))
				{
					return JsonHelper.DeSerialize<UserModel>(StringHelper.RemoveSpecialChar(result));
				}
			}
			catch (Exception Ex)
			{
				LogHelper.Save("【Message】：" + Ex.Message + "          【StackTrace】：" + Ex.StackTrace + "\n\r" + result, "GetUserInfo", LogType.Error, LogTime.day);
				throw Ex;
			}
			return new UserModel();
		}
		#endregion

		#region 获取关注者列表
		/// <summary>
		/// 获取关注者列表，返回JSON数据包，公众号可通过本接口来获取帐号的关注者列表，关注者列表由一串OpenID（加密后的微信号，每个用户对每个公众号的OpenID是唯一的）组成。一次拉取调用最多拉取10000个关注者的OpenID，可以通过多次拉取的方式来满足需求。
		/// </summary>
		/// <param name="next_openid">第一个拉取的OPENID，不填默认从头开始拉取</param>
		/// <returns>UserListModel</returns>
		public static UserListModel GetUsers(string next_openid, string access_token)
		{
			try
			{
				string url = Common.ApiUrl + string.Format("user/get?access_token={0}&next_openid={1}", access_token, string.IsNullOrEmpty(next_openid) ? string.Empty : next_openid);
				string result = HttpRequestHelper.Request(url, HttpRequestHelper.Method.GET);
				if (!string.IsNullOrEmpty(result))
				{
					return JsonHelper.DeSerialize<WeChatHelper4Net.Models.UserListModel>(result);
				}
			}
			catch (Exception Ex)
			{
				LogHelper.Save(Ex, "GetUsers", LogTime.day);
				throw Ex;
			}
			return new UserListModel();
		}
		#endregion
	}
}