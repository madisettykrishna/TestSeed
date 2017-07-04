using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SeedApp.Common.Exceptions;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Data.Interfaces;

namespace SeedApp.Data.Services
{
	public class SeedAppApiServices : ISeedAppApiServices
	{
		private static int _innvocationId;
		private readonly IAppConfig _appConfig;
		private readonly ILogger _logger;
		private readonly IConnectivityHelper _connectivityHelper;

		public SeedAppApiServices(IAppConfig appConfig, ILogger logger, IConnectivityHelper connectivityHelper)
		{
			_appConfig = appConfig;
			_logger = logger;
			_connectivityHelper = connectivityHelper;
		}

		public async Task<string> PostRequest(string api, object data)
		{
			using (var client = new HttpClient())
			{
				var url = new Uri(_appConfig.ApiBaseUrl + api);
				var request = new HttpRequestMessage(HttpMethod.Post, url)
				{
					Content = new StringContent(JsonConvert.SerializeObject(data),
						Encoding.UTF8,
						"application/json")
				};

				return await ExecuteRequest<string>(api, data, url, client, request);
			}
		}

		public async Task<T> PostRequest<T>(string api, object data)
		{
			using (var client = new HttpClient())
			{
				var url = new Uri(_appConfig.ApiBaseUrl + api);
				var request = new HttpRequestMessage(HttpMethod.Post, url)
				{
					Content = new StringContent(JsonConvert.SerializeObject(data),
						Encoding.UTF8,
						"application/json")
				};

				string json = await ExecuteRequest<string>(api, data, url, client, request);
				return JsonConvert.DeserializeObject<T>(json,
															new JsonSerializerSettings()
															{
																MissingMemberHandling = MissingMemberHandling.Ignore,
																NullValueHandling = NullValueHandling.Ignore
															});
			}
		}

		public async Task<T> PostImageFileRequest<T>(byte[] fileBytes, string filename, string api)
		{
			using (var client = new HttpClient())
			using (var content = new MultipartFormDataContent())
			{
				var url = new Uri(_appConfig.ApiBaseUrl + api);

				var byteContent = new ByteArrayContent(fileBytes);
				byteContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

				// Add first file content 
				content.Add(byteContent, "image", filename);

				var request = new HttpRequestMessage(HttpMethod.Post, url)
				{
					Content = content
				};

				string json = await ExecuteRequest<string>(api, null, url, client, request);
				return JsonConvert.DeserializeObject<T>(json,
											new JsonSerializerSettings()
											{
												MissingMemberHandling = MissingMemberHandling.Ignore,
												NullValueHandling = NullValueHandling.Ignore
											});
			}
		}

		public async Task<T> GetRequest<T>(string api)
		{
			using (var client = new HttpClient())
			{
				var url = new Uri(_appConfig.ApiBaseUrl + api);
				var request = new HttpRequestMessage(HttpMethod.Get, url);

				Type typeParameterType = typeof(T);
				if (typeParameterType != typeof(byte[]))
				{
					string json = await ExecuteRequest<string>(api, null, url, client, request);
					return JsonConvert.DeserializeObject<T>(json);
				}
				else
				{
					return await ExecuteRequest<T>(api, null, url, client, request);
				}
			}
		}

		public async Task<string> GetRequest(string api)
		{
			using (var client = new HttpClient())
			{
				var url = new Uri(_appConfig.ApiBaseUrl + api);
				var request = new HttpRequestMessage(HttpMethod.Get, url);

				return await ExecuteRequest<string>(api, null, url, client, request);
			}
		}

		public async Task<string> DeleteRequest(string api)
		{
			using (var client = new HttpClient())
			{
				var url = new Uri(_appConfig.ApiBaseUrl + api);
				var request = new HttpRequestMessage(HttpMethod.Delete, url);

				return await ExecuteRequest<string>(api, null, url, client, request);
			}
		}

		/// <summary>
		/// This method will add access token in http client, if any api call need access token then this method need to be called, in that method.
		/// </summary>
		/// <param name="client">Client.</param>
		/// <param name="accessToken">Access token.</param>
		private void AssignAccessToken(HttpClient client, string accessToken)
		{
			if (string.IsNullOrEmpty(accessToken))
				return;

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		}

		private async Task<T> ExecuteRequest<T>(string api, object data, Uri url, HttpClient client, HttpRequestMessage request)
		{
			if (!_connectivityHelper.IsConnected)
				throw new SeedAppApiExceptions(ApiErrorCodes.ConnectivityLost, "No connectivity found when about to make an api request");

			int id = Interlocked.Increment(ref _innvocationId);
			_logger.Verbose($"InvocationId<{id}> Request to {api}", new { verb = request.Method.ToString(), url, data },
				new[] { LoggerConstants.ApiOperation });

			HttpResponseMessage response;

			try
			{
				response = await client.SendAsync(request);
			}
			catch (Exception ex)
			{
				throw SeedAppApiExceptions.ProcessApiException(ex, id, api, request.Method.ToString(), url.ToString(), data);
			}

			if (!response.IsSuccessStatusCode)
			{
				string responseContent = "";

				try
				{
					if (response.Content != null)
						responseContent = await response.Content.ReadAsStringAsync();
				}
				catch
				{
				}

				_logger.Error($"InvocationId<{id}> Reponse from {api}",
					new { response.StatusCode, response.ReasonPhrase, ResponseContent = responseContent },
							  new[] { LoggerConstants.ApiOperation });

				throw new SeedAppApiExceptions(ApiErrorCodes.GenericError, response.ReasonPhrase) { ResponseContent = responseContent };
			}

			Type typeParameterType = typeof(T);
			if (typeParameterType == typeof(string))
			{
				string json = await response.Content.ReadAsStringAsync();
				_logger.Verbose($"InvocationId<{id}> Reponse from {api}", new { verb = request.Method.ToString(), url, json },
								new[] { LoggerConstants.ApiOperation });
				return (T)Convert.ChangeType(json, typeof(T));
			}
			else
			{
				_logger.Verbose($"InvocationId<{id}> Reponse from {api}", new { verb = request.Method.ToString(), url },
								new[] { LoggerConstants.ApiOperation });
				return (T)Convert.ChangeType(await response.Content.ReadAsByteArrayAsync(), typeof(T));
			}
		}
	}
}
