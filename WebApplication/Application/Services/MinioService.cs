using System.Text;
using Application.Abstract.Services;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Application.Services;

public class MinioService : IMinioService
{
    private readonly IMinioClient _client;
    private readonly MinioOptions _options;
    private readonly string _bucketName;
    public MinioService(IOptions<MinioOptions> minioOptions)
    {
        _options = minioOptions.Value;
        _client = new MinioClient()
            .WithEndpoint(_options.EndPoint)
            .WithCredentials(_options.AccessKey, _options.SecretKey)
            .WithSSL(false)
            .Build();
        _bucketName = _options.BucketName;
    }

    public async Task<bool> UploadFileAsync(string fileName, Stream fileStream)
    {
        try
        {
            await EnsureBucketExistsAsync();
            fileStream.Position = 0;
            await _client.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType("text/plain"));
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        
    }

    public async Task<bool> UpdateFileContentAsync(string fileName, string content)
    {
        var obj = await _client.StatObjectAsync(
            new StatObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName));

        if (obj is null)
        {
            return false;
        }
        
        using var memoryStream = new MemoryStream();
        await memoryStream.WriteAsync(Encoding.UTF8.GetBytes(content));
        memoryStream.Seek(0, SeekOrigin.Begin);
        await _client.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithStreamData(memoryStream)
                .WithObjectSize(memoryStream.Length)
                .WithContentType("text/plain"));
        return true;
    }

    public async Task<string?> DownloadFileContentAsync(string fileName)
    {
        var obj = await _client.StatObjectAsync(
            new StatObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName));

        if (obj is null)
        {
            return null;
        }
        
        using var memoryStream = new MemoryStream();
        await _client.GetObjectAsync(
            new GetObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(fileName)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream)));
        
        memoryStream.Seek(0, SeekOrigin.Begin);
        
        using var reader = new StreamReader(memoryStream);
        return await reader.ReadToEndAsync();
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        var obj = await _client.StatObjectAsync(
            new StatObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName));

        if (obj is null)
        {
            return false;
        }
        
        await _client.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName));
        return true;
    }

    public async Task<byte[]> GetDownloadFileAsync(string fileName)
    {
        var obj = await _client.StatObjectAsync(
            new StatObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName));

        if (obj is null)
        {
            return new byte[0];
        }
        
        using MemoryStream memoryStream = new MemoryStream();
        await _client.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream)));
        
        return memoryStream.ToArray();
    }

    private async Task EnsureBucketExistsAsync()
    {
        bool isFound = await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
        if (!isFound)
        {
            await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
        }
    }
}