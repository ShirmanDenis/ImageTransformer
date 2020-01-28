using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageTransform.Client.Models;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Vostok.Clusterclient.Core;
using Vostok.Clusterclient.Core.Model;
using Vostok.Logging.Abstractions;

namespace ImageTransform.Client
{
    internal class ImageTransformClient : IImageTransformClient
    {
        private readonly ILog _log;
        private readonly TimeSpan _defaultTimeOut = TimeSpan.FromSeconds(10);
        private readonly IClusterClient _clusterClient;

        public ImageTransformClient(ClusterClientSetup setup, ILog log)
        {
            _clusterClient = new ClusterClient(log, setup);
            _log = log;
        }

        public async Task<OperationResult<IEnumerable<string>>> GetFiltersAsync(TimeSpan? timeout = null)
        {
            var request = Request.Get(ApiUris.GetRegisteredFilters);

            var clusterResult = await _clusterClient.SendAsync(request, timeout ?? _defaultTimeOut)
                .ConfigureAwait(false);

            return await PrepareResponse<IEnumerable<string>>(clusterResult);
        }

        public async Task<OperationResult<byte[]>> FiltrateImageAsync(FiltrateRequest requestModel)
        {
            if (string.IsNullOrEmpty(requestModel.FilterName))
                throw new ArgumentNullException(nameof(requestModel.FilterName), "Filter name should not be null or empty.");
            if (requestModel.ImgData == null || requestModel.ImgData.Length == 0)
                throw new ArgumentNullException(nameof(requestModel.ImgData), "Image data should not be null.");

            var request = Request.Post(ApiUris.ProcessImage(requestModel.FilterName, requestModel.Area))
                .WithContent(requestModel.ImgData)
                .WithContentTypeHeader("octet/stream");

            var clusterResult = await _clusterClient.SendAsync(request)
                .ConfigureAwait(false);

            return await PrepareResponse<byte[]>(clusterResult);
        }

        [ItemCanBeNull]
        private async Task<OperationResult<T>> PrepareResponse<T>(ClusterResult clusterResult)
        {
            if (clusterResult.Status != ClusterResultStatus.Success)
                return OperationResult<T>.CreateFailed($"Failed with code {clusterResult.Response.Code}, reason is {clusterResult.Status.ToString()}");
            try
            {
                var response = clusterResult.Response;
                if (!response.HasContent)
                    return OperationResult<T>.CreateOk(default);
                string json;
                if (response.HasStream)
                {
                    using (var streamReader = new StreamReader(response.Stream))
                    {
                        json = await streamReader.ReadToEndAsync()
                            .ConfigureAwait(false);
                    }
                    
                }
                else
                    json = response.Content.ToString();

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
    }
}
