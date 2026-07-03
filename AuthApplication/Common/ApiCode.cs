namespace AuthApplication.Common;

/// <summary>
/// API返回状态码
/// </summary>
public static class ApiCode
{
    /// <summary>
    /// 成功
    /// </summary>
    public const int Success = 200;

    /// <summary>
    /// 失败
    /// </summary>
    public const int Fail = 500;

    /// <summary>
    /// 参数错误
    /// </summary>
    public const int BadRequest = 400;

    /// <summary>
    /// 未登录
    /// </summary>
    public const int Unauthorized = 401;

    /// <summary>
    /// 无权限
    /// </summary>
    public const int Forbidden = 403;

    /// <summary>
    /// 数据不存在
    /// </summary>
    public const int NotFound = 404;

    /// <summary>
    /// Token过期
    /// </summary>
    public const int TokenExpired = 1001;

    /// <summary>
    /// 登录失败
    /// </summary>
    public const int LoginFailed = 1002;

    /// <summary>
    /// 用户锁定
    /// </summary>
    public const int UserLocked = 1003;
}