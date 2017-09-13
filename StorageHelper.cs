using System;
using System.IO;
using System.Collections.Generic;

namespace wpscanauto{
	public class StorageHelper{
		public enum RepoName{
			SHELL_TEMPLATE = 1,
			TWEET_CONTENTS_BLACKLIST = 2,
			TWITTER_FOLLOWERS = 3,
			TWITTER_TAGLIST = 4,
			TWITTER_PASTE_DUMPERS = 5,
			EMAIL_OFFENCES = 6,
			GITHUB_OFFENCES = 7,
			GITHUB_BLACKLIST = 8,
			WORDPRESS_SCANNER_DATA =9
		}

		private static Dictionary<RepoName,string> RepositoryPaths = new Dictionary<RepoName, string>(){
			{RepoName.SHELL_TEMPLATE,"shell.cs"},
			{RepoName.TWEET_CONTENTS_BLACKLIST,"blackList.txt"},
			{RepoName.TWITTER_FOLLOWERS,"followedList.txt"},
			{RepoName.TWITTER_PASTE_DUMPERS,"downloadFollowList.txt"},
			{RepoName.TWITTER_TAGLIST,"streamTagList.txt"},
			{RepoName.EMAIL_OFFENCES,"offencelist.txt"},
			{RepoName.GITHUB_OFFENCES, "github-offences.txt"},
			{RepoName.GITHUB_BLACKLIST,"github-blacklist.txt"},
			{RepoName.WORDPRESS_SCANNER_DATA,"wordpress-urls-and-users.txt"}
		};

		public StorageHelper(){
		}

		#region READ TEXT files
		public static string ReadToEnd(string filename){
			using (StreamReader reader = new StreamReader (filename)) {
				return reader.ReadToEnd ();
			}
		}

		public static string ReadToEnd(RepoName repoName){
			return ReadToEnd (RepositoryPaths [repoName]);
		}

		public static List<string> ReadLines(RepoName repoName){
			List<string> rows = new List<string> ();
			using (StreamReader reader = new StreamReader (RepositoryPaths [repoName])) {
				string row = reader.ReadLine ();
				while (row != null && row != "") {
					rows.Add (row.Trim());
					row = reader.ReadLine ();
				}				
			}
			return rows;
		}
		#endregion

		#region WRITE TEXT files
		public static void WriteLines(RepoName repoName, List<string> lines){
			using (StreamWriter writer = new StreamWriter (RepositoryPaths[repoName])) {
				foreach (string line in lines) {
					writer.WriteLine (line.Trim());
				}
			}
		}
		#endregion

		#region READ BINARY files
		public static byte[] ReadBinaryData(string filename)
		{
			const int CHUNK_SIZE = 1024;
			List<byte> bytes = new List<byte>();

			using (FileStream fs = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
			{
				using (System.IO.BinaryReader br = new System.IO.BinaryReader(fs))
				{
					byte[] chunk;

					chunk = br.ReadBytes(CHUNK_SIZE);
					while (chunk.Length > 0)
					{
						bytes.AddRange(chunk);
						chunk = br.ReadBytes(CHUNK_SIZE);
					}
				}
			}

			return bytes.ToArray();
		}
		#endregion
	}
}
