namespace Domain.Exceptions;

/// <summary>
/// 业务异常
/// </summary>
public class BusinessException : Exception
{
    /// <summary>
    /// 错误码
    /// </summary>
    public int Code { get; }

    public BusinessException(string message)
        : base(message)
    {
        Code = 500;
    }

    public BusinessException(int code, string message)
        : base(message)
    {
        Code = code;
    }
}