using Azure;
using Azure.Core;
using NotissimusOffers.Models;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace NotissimusOffers.Services
{
	public class XmlHttpRequestService
	{
		private readonly IHttpClientFactory _clientFactory;

		public XmlHttpRequestService(IHttpClientFactory clientFactory)
		{
			_clientFactory = clientFactory;
		}
		public T? sendRequest<T>(string url)
		{
			var client = _clientFactory.CreateClient();
			var request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Headers.Add("Accept", "text/xml");
			request.Headers.Add("User-Agent", "XmlRequestService");
			var response = client.Send(request);

			response.EnsureSuccessStatusCode();

			if (response.Content == null) return default(T);

			T? content = parseXml<T>(response.Content.ReadAsStream());

			return content;
		}
		private T? parseXml<T>(Stream xmlStream)
		{
			var xmlSerializer = new XmlSerializer(typeof(T));
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.DtdProcessing = DtdProcessing.Parse;
			settings.MaxCharactersFromEntities = 1024;
			var xmlReader = XmlReader.Create(xmlStream, settings);

			var content = (T?)xmlSerializer.Deserialize(xmlReader);

			return content;
		}
	}
}
