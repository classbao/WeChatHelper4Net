using System;
using System.Collections.Generic;

/*
 * 微信公众账号API，模型
 * 作者：熊学浩
 * 时间：2014-5-25
 */
namespace WeChatHelper4Net.Models
{
	/// <summary>
	/// 用户分组模型
	/// </summary>
	public class GroupModel : RequestResultBaseModel
	{
		private int _id;
		private string _name;
		private int _count;

		private int _groupid;

		/// <summary>
		/// 分组id，由微信分配
		/// </summary>
		public int id { get { return _id; } set { _id = value; } }
		/// <summary>
		/// 分组名字，UTF8编码
		/// </summary>
		public string name { get { return _name; } set { _name = value; } }
		/// <summary>
		/// 分组内用户数量
		/// </summary>
		public int count { get { return _count; } set { _count = value; } }

		/// <summary>
		/// 用户所属的groupid
		/// </summary>
		public int groupid { get { return _groupid; } set { _groupid = value; } }
	}

	/// <summary>
	/// 分组
	/// </summary>
	public class GroupSubModel : RequestResultBaseModel
	{
		private GroupModel _group;

		/// <summary>
		/// 分组
		/// </summary>
		public GroupModel group { get { return _group; } set { _group = value; } }
	}

	/// <summary>
	/// 查询所有分组
	/// </summary>
	public class GroupsModel : RequestResultBaseModel
	{
		private List<GroupModel> _groups = new List<GroupModel>();

		/// <summary>
		/// 查询所有分组
		/// </summary>
		public List<GroupModel> groups { get { return _groups; } set { _groups = value; } }
	}
}
