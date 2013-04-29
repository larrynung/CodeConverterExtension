using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LevelUp.CodeConverter
{
	public static class ConvertHelper
	{
		public static string ConvertToVB(string code)
		{
			var formFields = new NameValueCollection();
			formFields.Add("code", code);
			formFields.Add("Language", "C#");
			formFields.Add("DestinationLanguage", "VB");
			WebClient client = new WebClient();
			return Encoding.ASCII.GetString(client.UploadValues("http://www.carlosag.net/Tools/CodeTranslator/translate.ashx", "POST", formFields));
		}

		public static string ConvertToCSharp(string code)
		{
			var formFields = new NameValueCollection();
			formFields.Add("code", code);
			formFields.Add("Language", "VB");
			formFields.Add("DestinationLanguage", "C#");
			WebClient client = new WebClient();
			return Encoding.ASCII.GetString(client.UploadValues("http://www.carlosag.net/Tools/CodeTranslator/translate.ashx", "POST", formFields));
		}
	}
}
