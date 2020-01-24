using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ImageTransform.Client.Models;
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

        public async Task<OperationResult<IEnumerable<string>>> GetFiltersAsync(TimeSpan? timeout = null)
        {
            var response = await _httpClient
                .GetAsync(ApiUris.GetRegisteredFilters, timeout.HasValue ? TimeOut(timeout.Value) : CancellationToken.None);
            return await PrepareResponse<IEnumerable<string>>(response);
        }

        public async Task<OperationResult<byte[]>> FiltrateImageAsync(FiltrateImageModel requestModel)
        {
            if (string.IsNullOrEmpty(requestModel.FilterName))
                throw new ArgumentNullException(nameof(requestModel.FilterName), "Filter name should not be null or empty.");
            if (requestModel.ImgData == null || requestModel.ImgData.Length == 0)
                throw new ArgumentNullException(nameof(requestModel.ImgData), "Image data should not be null.");

            var content = new ByteArrayContent(requestModel.ImgData);
            content.Headers.ContentLength = requestModel.ImgData.Length;
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("octet/stream");
            var response = await _httpClient.PostAsync(ApiUris.ProcessImage(requestModel.FilterName, requestModel.Area), content);
            var binary = await response.Content.ReadAsByteArrayAsync()
                .ConfigureAwait(false);
            
            return OperationResult<byte[]>.CreateOk(binary);
        }

        private async Task<OperationResult<T>> PrepareResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return OperationResult<T>.CreateFailed($"Failed with code {response.StatusCode}, reason is {response.ReasonPhrase}");
            try
            {
                var json = await response.Content.ReadAsStringAsync()
                    .ConfigureAwait(false);
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
