using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Services.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MinioService
    {
        private const string DefaultContentType = "text/markdown";
        private const string BucketName = "md-bucket";
        private const string Endpoint = "minio:9000";

        private readonly MinioSetting _minioSettings;
        private readonly IMinioClient _minioClient;

        public MinioService(IOptions<MinioSetting> options)
        {
            _minioSettings = options.Value;
            Console.WriteLine("Minio: " + _minioSettings.AccessKey + "" + _minioSettings.SecretKey);
            _minioClient = new MinioClient()
                .WithEndpoint(Endpoint)
                .WithCredentials(_minioSettings.AccessKey, _minioSettings.SecretKey)
                .Build();
        }

        private async Task EnsureBucketExistsAsync()
        {
            if (!await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(BucketName)))
            {
                await _minioClient.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(BucketName)
                );
            }
            
        }

        public async Task<string?> UploadMarkdownTextAsync(string markdownContent, string objectName, CancellationToken cancellationToken)
        {
            var fileBytes = Encoding.UTF8.GetBytes(markdownContent);
            return await UploadFileAsync(fileBytes, objectName, DefaultContentType, cancellationToken);
        }

        public async Task<string?> UploadFileAsync(byte[] fileBytes, string objectName, string contentType = DefaultContentType, CancellationToken cancellationToken = default)
        {
            try
            {
                await EnsureBucketExistsAsync();

                using var stream = new MemoryStream(fileBytes);
                await _minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(objectName)
                    .WithStreamData(stream)
                    .WithObjectSize(fileBytes.Length)
                    .WithContentType(contentType), cancellationToken);

                var objectUrl = $"{Endpoint}/{BucketName}/{objectName}";
                return objectUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteFileAsync(string objectName, CancellationToken cancellationToken = default)
        {
            try
            {
                await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(objectName), cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении файла {objectName}: {ex.Message}");
                return false;
            }
        }

        public async Task<string?> GetMarkdownTextAsync(string objectName, CancellationToken cancellationToken = default)
        {
            try
            {
                var fileBytes = await GetFileAsync(objectName, cancellationToken);
                return fileBytes != null ? Encoding.UTF8.GetString(fileBytes) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении файла: {ex.Message}");
                return null;
            }
        }

        public async Task<byte[]> GetFileAsync(string objectName, CancellationToken cancellationToken = default)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await _minioClient.GetObjectAsync(new GetObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(objectName)
                    .WithCallbackStream(async stream =>
                    {
                        await stream.CopyToAsync(memoryStream, cancellationToken);
                    }), cancellationToken);

                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении файла {objectName}: {ex.Message}");
                return null;
            }
        }
    }
}
