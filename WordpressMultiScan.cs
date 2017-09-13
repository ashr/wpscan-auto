using System;
using System.Linq;
using System.Net.Mail;
using System.Diagnostics;
using System.Collections.Generic;

namespace wpscanauto
{
	public class WordpressMultiScan
	{
		private List<WordPressScan> scanRequests = null;

		public WordpressMultiScan ()
		{
			scanRequests = new List<WordPressScan> ();
		}

		public void Scan(){
			scanRequests = loadScanRequests ();

			if (scanRequests == null || scanRequests.Count == 0) {
				Console.WriteLine ("No scan requests found, aborting");
				return;
			}

			try{
				scanRequests.ForEach(x => scanSite(x));

			}
			catch(Exception e){
				Console.WriteLine (e.Message);
			}
		}

		private void scanSite(WordPressScan scan){
			try{
				Console.WriteLine("Scanning " + scan.SiteUrl);

				ProcessStartInfo psi = new ProcessStartInfo("/bin/bash", "runwpscan.sh " + scan.SiteUrl);

				psi.RedirectStandardOutput = true;
				psi.RedirectStandardError = true;
				psi.UseShellExecute = false;
				psi.CreateNoWindow = true;

				Process p = new Process();
				p.StartInfo = psi;
				p.Start();
				p.WaitForExit();

				string output = p.StandardOutput.ReadToEnd();

				Console.Write(output);

				if (Config.getInstance().Debug){
					Console.WriteLine("DEBUG MODE ENABLED::No emails will be sent");
					scan.Receivers.ForEach(x => Console.WriteLine("\tReceiver:" + x));
					return;
				}

				MailMessage mm = new MailMessage();
				mm.From = new MailAddress(Config.getInstance().FromAddress);
				scan.Receivers.ForEach(x => mm.To.Add(x));

				mm.Subject = "Automated Wordpress Scan for " + scan.SiteUrl;
				mm.IsBodyHtml = false;
				mm.Body = output;

				SmtpClient sc = new SmtpClient(Config.getInstance().SMTPServer);
				sc.Send(mm);
			}
			catch(Exception e){
				Console.WriteLine (e.Message);
			}
		}

		private List<WordPressScan> loadScanRequests(){
			List<WordPressScan> scanRequests = new List<WordPressScan> ();
			List<string> scanRequestData = StorageHelper.ReadLines (StorageHelper.RepoName.WORDPRESS_SCANNER_DATA);

			foreach (string line in scanRequestData) {
				string trimmedLine = line.Trim ();
				string[] data = line.Split ('|');
				if (data != null && data.Length == 2) {

					string[] emails = data [1].Split (';');

					if (emails != null && emails.Length > 0) {
						WordPressScan scan = new WordPressScan ();
						scan.SiteUrl = data [0];

						foreach(string email in emails){
							scan.Receivers.Add(email);
						}

						scanRequests.Add (scan);
					}
				}
			}

			return scanRequests;
		}
	}
}

