using System;
namespace WOT_Models
{
	public static class WOTCSModel
	{
		public static List<WOTCTName> ConnectionStrings { get; set; }
		public static string GetConnectionString(string DBKEY)
		{
			return ConnectionStrings.Where(x => x.ContextName == DBKEY).Select(x => x.ConnectString).FirstOrDefault();
		}
	}
	public class WOTCTName
	{
		public string ContextName { get; set; }
        public string ConnectString { get; set; }
    }
}

