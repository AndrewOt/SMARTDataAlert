using System;
using System.Management;
using System.Net;
using System.Net.Mail;

namespace SMARTnotification
{
	class Program
	{
		static void Main(string[] args)
		{
			string status = check_SMART();
			if (!status.ToUpper().Equals("OK"))
			{
				sendMail(status);
			}
			Environment.Exit(0);
		}

		private static string check_SMART()
		{
			try
			{
				//declare where to pull the SMART info
				string computerName = Dns.GetHostName();
				ManagementScope scope = new ManagementScope("\\\\" + computerName + "\\ROOT\\cimv2");
				//create a query object to search scope
				ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_DiskDrive Where DeviceID=\"\\\\\\\\.\\\\PHYSICALDRIVE0\"");
				//collet the items from query
				ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
				ManagementObjectCollection queryCollection = searcher.Get();
				//loop through and grab the status varible from the collection
				string a = "";
				foreach (ManagementObject m in queryCollection)
				{
					a = (string)m["Status"];
				}
				return a;

			}
			catch (Exception)
			{
				return "The SMART check failed";

			}
		}

		private static void sendMail(string data)
		{
			string name = Dns.GetHostName();
			try
			{
				MailMessage mail = new MailMessage();
				string DisplayName = "Smart HDD Check";

				mail.From = new MailAddress("", DisplayName);
				mail.To.Add("");
				mail.Subject = "SMART report for: " + name;
				mail.Body = "The computer " + name + " has a status of: " + data + ". The date is " + DateTime.Now + ".";
				SmtpClient smtp = new SmtpClient("");
				smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtp.Send(mail);
			}
			catch (SmtpException)
			{	}
		}

	}
}
//SDG