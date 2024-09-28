namespace BetterSync.Common.Utilities;

public class Result<T, E>
{
    public T? Value { get; }
    public E? Error { get; }
    public bool IsSuccess { get; }

    protected Result(T value)
    {
        Value = value;
        IsSuccess = true;
    }

    protected Result(E error)
    {
        Error = error;
        IsSuccess = false;
    }

    public static Result<T, E> Ok(T value) => new Result<T, E>(value);
    public static Result<T, E> Err(E error) => new Result<T, E>(error);
}
