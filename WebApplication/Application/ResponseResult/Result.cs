namespace Application.ResponseResult;

public class Result(Error? error, bool isOk)
{
    public Error? Error { get; set; } = error;
    public bool IsOk { get; } = isOk;
    
    public static Result Success() => new Result(null,true);
    public static Result Failure(Error error) => new Result(error, false);
}
public class Result<T>(T? value, Error? error, bool isOk): Result(error, isOk)
{
    public T? Value { get; } = value;
    
    public static Result<T> Success(T value) => new Result<T>(value, null, true);
}