using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace wpscanauto
{
	public class Config
	{
		private static Config instance = null;
		private static String locker = "1";
		private Config()
		{
			FromAddress = ConfigurationManager.AppSettings ["notification.FromAddress"];
			SMTPServer = ConfigurationManager.AppSettings ["notification.SMTPServer"];
			Debug = ConfigurationManager.AppSettings ["debug"] == "1" ? true : false;
		}

		public static Config getInstance()
		{
			if (instance == null)
			{
				lock (locker)
				{
					instance = new Config();
				}
			}
			return instance;
		}

		public string FromAddress { get;set; }
		public string SMTPServer { get;set; }
		public bool Debug{ get; set; }
    }
}
