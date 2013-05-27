using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelUp.CodeConverter
{
	public class DeveloperfusionConverter
	{
		class BatchResponse
		{
			public string status { get; set; }
			public string batchId { get; set; }
			public string redirectUrl { get; set; }
		}

		class ConvertResponse
		{
			public string Message { get; set; }
			public string Code { get; set; }
			public string Status { get; set; }
		}

		#region Const
		const string WEB_URL = "http://www.developerfusion.com";
		const string REQUEST_URL_PATTERN = "http://www.developerfusion.com/tools/convert/{0}/";
		const string RESPONSE_URL_PATTERN = "http://resp.developerfusion.com/get.aspx?id={0}";
		#endregion

		#region Static Var
		private static DeveloperfusionConverter _defaulte;
		#endregion

		#region Public Static Property
		public static DeveloperfusionConverter Default
		{
			get
			{
				return _defaulte ?? (_defaulte = new DeveloperfusionConverter());
			}
		}
		#endregion


		#region Event
		public event EventHandler<ProgressChangedEventArgs> ProgressChanged;
		#endregion


		#region Private Method
		private string GetSourceCode(string url, NameValueCollection parameters)
		{
			WebClient client = new WebClient();
			return Encoding.ASCII.GetString(client.UploadValues(url, "POST", parameters));
		}

		private string GetSourceCode(string url)
		{
			return GetSourceCode(url, new NameValueCollection());
		}

		private string ConvertCode(string arguements, string code)
		{
			try
			{
				OnProgressChanged(new ProgressChangedEventArgs(0, "Code convert started"));

				var parameters = new NameValueCollection();
				parameters.Add("code", code);

				var response = GetSourceCode(string.Format(REQUEST_URL_PATTERN, arguements), parameters);

				var batchResponse = JsonConvert.DeserializeObject<BatchResponse>(response);

				OnProgressChanged(new ProgressChangedEventArgs(30, string.Format("Request uri: {0}", WEB_URL + batchResponse.redirectUrl)));

				response = GetSourceCode(string.Format(RESPONSE_URL_PATTERN, batchResponse.batchId)).Trim('(', ')', '[', ']');

				var convertResponse = JsonConvert.DeserializeObject<ConvertResponse>(response);

				if (!string.IsNullOrEmpty(convertResponse.Message))
				{
					OnProgressChanged(new ProgressChangedEventArgs(95, string.Format("Error: {0}", convertResponse.Message)));
					return null;
				}

				OnProgressChanged(new ProgressChangedEventArgs(95, "Fetch converted code"));

				return convertResponse.Code;
			}
			finally 
			{
				OnProgressChanged(new ProgressChangedEventArgs(100, "Code convert finished"));
			}
		}
		#endregion


		#region Protected Method
		protected void OnProgressChanged(ProgressChangedEventArgs e)
		{
			if (ProgressChanged == null)
				return;

			ProgressChanged(this, e);
		}
		#endregion


		#region Public Method
		public string ConvertToVB(string code)
		{
			return ConvertCode("csharp-to-vb", code);
		}

		public string ConvertToVBFromClipboard()
		{
			return ConvertToVB(Clipboard.GetText());
		}

		public string ConvertToCSharp(string code)
		{
			return ConvertCode("vb-to-csharp", code);
		}


		public string ConvertToCSharpFromClipboard()
		{
			return ConvertToCSharp(Clipboard.GetText());
		}
		#endregion
	}
}