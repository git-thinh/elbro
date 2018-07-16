﻿using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
//using System.Web.Script.Serialization;

namespace SimpleBrowser
{
	public static class ObjectExtensions
	{
		public static bool EqualsAny(this object source, params object[] comparisons)
		{
			return comparisons.Any(o => Equals(source, o));
		}

		public static NameValueCollection ToNameValueCollection(this object o)
		{
			var nvc = new NameValueCollection();
			foreach(var p in o.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance))
				nvc.Add(p.Name, (p.GetValue(o, null) ?? "").ToString());
			return nvc;
		}

		public static PropertyInfo[] GetSettableProperties(this object o)
		{
			return o.GetType()
				.GetProperties(BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.GetSetMethod() != null)
				.ToArray();
		}

		public static string ToQueryString(this object o)
		{
			return o.ToNameValueCollection().ToQueryString();
		}

		public static string ToJson(this object obj)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
		}

		public static T DuckTypeAs<T>(this object o)
		{ 
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(o));
		}
	}
}