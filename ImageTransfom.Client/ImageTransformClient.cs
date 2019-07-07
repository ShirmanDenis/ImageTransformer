using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Vostok.Logging.Abstractions;

namespace ImageTransform.Client
{
    internal class ImageTransformClient : IImageTransformClient
    {
        private readonly ILog _log;
        private readonly TimeSpan _defaultTimeOut = TimeSpan.FromSeconds(10);
        private readonly HttpClient _httpClient = new HttpClient();

        public ImageTransformClient(Uri uri, ILog log)
        {
            _httpClient.BaseAddress = uri;
            _httpClient.Timeout = _defaultTimeOut;
            _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            _log = log;
        }

        public async Task<OperationResult<IEnumerable<string>>> GetRegisteredFilters(TimeSpan? timeout = null)
        {
            var response = await _httpClient
                .GetAsync(ApiUris.GetRegisteredFilters, timeout.HasValue ? TimeOut(timeout.Value) : CancellationToken.None);
            return await PrepareResponse<IEnumerable<string>>(response);
        }

        public async Task<OperationResult<byte[]>> FiltrateImage(byte[] imageBytes, string filter, Rectangle rect, params object[] @params)
        {
            var content = new ByteArrayContent(imageBytes);
            content.Headers.ContentLength = imageBytes.Length;
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("octet/stream");
            var response = await _httpClient.PostAsync(ApiUris.ProcessImage(filter, rect), content);
            return await PrepareResponse<byte[]>(response);
        }

        private async Task<OperationResult<T>> PrepareResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return OperationResult<T>.CreateFailed($"Failed with code {response.StatusCode}, reason is {response.ReasonPhrase}");
            try
            {
                var json = await response.Content.ReadAsStringAsync();
                return !TryDeserialize<T>(json, out var model, out var error) ? 
                    OperationResult<T>.CreateFailed(error) : 
                    OperationResult<T>.CreateOk(model);
            }
            catch (Exception e)
            {
                return OperationResult<T>.CreateFailed($"Error in reading response body: {e.Message}");
            }
        }

        private bool TrySerialize<T>(T model, out string json, out string errorMsg)
        {
            errorMsg = string.Empty;
            json = string.Empty;
            try
            {
                json = JsonConvert.SerializeObject(model);
                return true;
            }
            catch (Exception e)
            {
                errorMsg = $"Serialization error: {e.Message}";
                _log.Error(errorMsg);
                return false;
            }
        }

        private bool TryDeserialize<T>(string json, out T model, out string errorMsg)
        {
            model = default;
            errorMsg = string.Empty;
            try
            {
                model = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            catch (Exception e)
            {
                errorMsg = $"Deserialization error: {e.Message}";
                _log.Error(errorMsg);
                return false;
            }
        }

        private static CancellationToken TimeOut(TimeSpan timeout)
        {
            var cts = new CancellationTokenSource(timeout);
            return cts.Token;
        }
    }
}
