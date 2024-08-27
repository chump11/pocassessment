

namespace POCAssessment.Common;

public class Result<T>
{
    public Result(bool success, Error error, T? data)
    {
        if (success && error != Error.None ||
            !success && error == Error.None)
        {
            throw new ArgumentException("Failed result must have an error");
        }

        IsSuccess = success;
        Error = error;
        Data = data;
    }

    public bool IsSuccess { get; }

    public bool HasErrors => !IsSuccess;
    public Error Error { get; }
    public T? Data { get; }

    public static Result<T> SuccessResult(T data) => new(true, Error.None, data);
    public static Result<T> FailedResult(Error error) => new(false, error, default);
}
