using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SeedApp.Common.Exception;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using ModernHttpClient;
using Newtonsoft.Json;
using Polly;

namespace SeedApp.Data.Services
{
    public class MemberPlusApiService : IMemberPlusApiService
    {
        private static int _innvocationId;

        private readonly IMemberPlusAppConfig _mmpAppConfig;
        private readonly ISecurityManager _securityManager;
        private readonly ILogger _logger;
        private readonly IConnectivityHelper _connectivityHelper;
        private readonly IAppAnalyticsProvider _analyticsProvider;

        public MemberPlusApiService(IMemberPlusAppConfig mmpAppConfig, ISecurityManager securityManager, ILogger logger, IConnectivityHelper connectivityHelper, IAppAnalyticsProvider analyticsProvider)
        {
            _mmpAppConfig = mmpAppConfig;
            _securityManager = securityManager;
            _logger = logger;
            _connectivityHelper = connectivityHelper;
            _analyticsProvider = analyticsProvider;
        }

        public async Task<string> PostRequest(string api, object data)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                AssignAccessToken(client);
                var url = new Uri(_mmpAppConfig.ServerDataUrl + api);
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
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                AssignAccessToken(client);
                var url = new Uri(_mmpAppConfig.ServerDataUrl + api);
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

        public async Task<bool> PostRequest(object data, string api)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                AssignAccessToken(client);
                var url = new Uri(_mmpAppConfig.ServerDataUrl + api);
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(data),
                        Encoding.UTF8,
                        "application/json")
                };

                return await ExecuteRequest<bool>(api, data, url, client, request);
            }
        }

        public async Task<T> PostFileRequest<T>(byte[] fileBytes, string filename, string api)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            using (var content = new MultipartFormDataContent())
            {
                AssignAccessToken(client);
                var url = new Uri(_mmpAppConfig.ServerDataUrl + api);

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
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                AssignAccessToken(client);
                var url = new Uri(_mmpAppConfig.ServerDataUrl + api);
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
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                AssignAccessToken(client);
                var url = new Uri(_mmpAppConfig.ServerDataUrl + api);
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                return await ExecuteRequest<string>(api, null, url, client, request);
            }
        }

        public async Task<string> PostRequest(string api)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                AssignAccessToken(client);
                var url = new Uri(_mmpAppConfig.ServerDataUrl + api);
                var request = new HttpRequestMessage(HttpMethod.Post, url);

                return await ExecuteRequest<string>(api, null, url, client, request);
            }
        }

        public async Task<string> DeleteRequest(string api)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                AssignAccessToken(client);
                var url = new Uri(_mmpAppConfig.ServerDataUrl + api);
                var request = new HttpRequestMessage(HttpMethod.Delete, url);

                return await ExecuteRequest<string>(api, null, url, client, request);
            }
        }

        private void AssignAccessToken(HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _securityManager.CurrentAccessToken);
        }

        private async Task<T> ExecuteRequest<T>(string api, object data, Uri url, HttpClient client, HttpRequestMessage request)
        {
            if (!_connectivityHelper.IsConnected)
            {
                throw new MemberPlusApiException(MmpApiErrorCodes.ConnectivityLost, "No connectivity found when about to make an api request");
            }

            int id = Interlocked.Increment(ref _innvocationId);
            _logger.Verbose($"InvocationId<{id}> Request to {api}", new { verb = request.Method.ToString(), url, data },
                new[] { LoggerConstants.ApiRequest });

            HttpResponseMessage response;

            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                response = await Policy.Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                                       .WaitAndRetryAsync
                                       (
                                           retryCount: 5,
                                           sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                           onRetry: (ex, time) =>
                                           {
                                               Debug.WriteLine($"Something went wrong: {ex.Message}, retrying...");
                                           }
                                       )
                                       .ExecuteAsync(async () => await client.SendAsync(request));
                watch.Stop();
                _analyticsProvider.ReportApiTiming(request.Method + " " + api, watch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                await Task.Delay(5000);
                throw MemberPlusApiException.ProcessApiException(ex, id, api, request.Method.ToString(), url.ToString(), data);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new MemberPlusApiException(MmpApiErrorCodes.Unauthorized, "Your session has been expired. Please login again.");
            }

            if (!response.IsSuccessStatusCode)
            {
                string responseContent = string.Empty;

                try
                {
                    if (response.Content != null)
                        responseContent = await response.Content.ReadAsStringAsync();
                }
                catch
                {
                    // Swallowing the exeception since we already know api failed
                }

                _logger.Error($"InvocationId<{id}> Reponse from {api}",
                    new { response.StatusCode, response.ReasonPhrase, ResponseContent = responseContent },
                    new[] { LoggerConstants.ApiRequest });

                throw new MemberPlusApiException(MmpApiErrorCodes.GenericError, response.ReasonPhrase);
            }

            Type typeParameterType = typeof(T);
            if (typeParameterType == typeof(string))
            {
                string json = await response.Content.ReadAsStringAsync();
                _logger.Verbose($"InvocationId<{id}> Reponse from {api}", new { verb = request.Method.ToString(), url, json },
                    new[] { LoggerConstants.ApiRequest });
                return (T)Convert.ChangeType(json, typeof(T));
            }
            else if (typeParameterType == typeof(bool))
            {
                return (T)Convert.ChangeType(response.IsSuccessStatusCode, typeof(T));
            }
            else
            {
                _logger.Verbose($"InvocationId<{id}> Reponse from {api}", new { verb = request.Method.ToString(), url },
                    new[] { LoggerConstants.ApiRequest });
                return (T)Convert.ChangeType(await response.Content.ReadAsByteArrayAsync(), typeof(T));
            }
        }
    }
}