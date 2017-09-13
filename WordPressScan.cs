using System;
using System.Collections.Generic;

namespace wpscanauto
{
	public class WordPressScan
	{
		public WordPressScan ()
		{
			Receivers = new List<string> ();
		}

		public string SiteUrl{ get; set;}
		public List<string> Receivers{ get; set;}
	}
}

