namespace AuthApplication.Common;

/// <summary>
/// API统一返回结果
/// </summary>
public class ApiResult
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 返回消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 返回数据
    /// </summary>
    public object? Data { get; set; }

    #region Static

    public static ApiResult Ok()
    {
        return new ApiResult
        {
            Code = ApiCode.Success,
            Success = true,
            Message = "Success"
        };
    }

    public static ApiResult Ok(object? data)
    {
        return new ApiResult
        {
            Code = ApiCode.Success,
            Success = true,
            Message = "Success",
            Data = data
        };
    }

    public static ApiResult Ok(string message, object? data = null)
    {
        return new ApiResult
        {
            Code = ApiCode.Success,
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResult Fail(string message)
    {
        return new ApiResult
        {
            Code = ApiCode.Fail,
            Success = false,
            Message = message
        };
    }

    public static ApiResult Fail(int code, string message)
    {
        return new ApiResult
        {
            Code = code,
            Success = false,
            Message = message
        };
    }

    #endregion
}

/// <summary>
/// API统一返回结果
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResult<T> : ApiResult
{
    /// <summary>
    /// 返回数据
    /// </summary>
    public new T? Data
    {
        get => (T?)base.Data;
        set => base.Data = value;
    }

    public static ApiResult<T> Ok(T? data)
    {
        return new ApiResult<T>
        {
            Code = ApiCode.Success,
            Success = true,
            Message = "Success",
            Data = data
        };
    }

    public new static ApiResult<T> Fail(string message)
    {
        return new ApiResult<T>
        {
            Code = ApiCode.Fail,
            Success = false,
            Message = message
        };
    }
}