using Application;
using Application.Interfaces.MinioS3;
using Application.Interfaces.Services;
using Application.Utils;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.MinioS3;

public class MinioStorage : IMinioStorage
{
    private readonly IMinioClient _minioClient; 
    private readonly MinioOptions _options;
    
    public MinioStorage(IOptions<MinioOptions> options)
    {
        _options = options.Value;
        _minioClient = new MinioClient()
            .WithEndpoint(_options.Endpoint)
            .WithCredentials(_options.AccessKey, _options.SecretKey)
            .Build();
        EnsureBucketExists().GetAwaiter().GetResult();
    }
    
    private async Task EnsureBucketExists()
    {
        var bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_options.BucketName));
        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_options.BucketName));
        }
    }
    
    public async Task<Result> UploadFile(string fileName, byte[] fileBytes)
    {
        try
        {
            using var memoryStream = new MemoryStream(fileBytes);
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(fileName)
                .WithStreamData(memoryStream)
                .WithObjectSize(fileBytes.Length)
                .WithContentType("text/plain"));
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> DeleteFile(string fileName)
    {
        try
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(fileName));
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result<byte[]>> DownloadFile(string fileName)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(fileName)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream)));

            return Result<Byte[]>.Success(memoryStream.ToArray());
        }
        catch (Exception ex)
        {
            return Result<byte[]>.Failure(ex.Message);
        }
    }
}