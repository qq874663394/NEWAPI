namespace Application.DTO.User;

/// <summary>
/// 更新用户基本信息请求（不允许修改 Name 和 Password）
/// </summary>
public class UpdateUserRequest
{
    /// <summary>用户 Code</summary>
    public Guid Code { get; set; }

    /// <summary>用户全名</summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>APO 账号</summary>
    public string? Apo { get; set; }

    /// <summary>邮箱</summary>
    public string? Email { get; set; }

    /// <summary>手机号</summary>
    public string? Phone { get; set; }

    /// <summary>座机</summary>
    public string? Tel { get; set; }

    /// <summary>生日</summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>性别</summary>
    public int? Sex { get; set; }

    /// <summary>证件类型</summary>
    public int? DocumentType { get; set; }

    /// <summary>证件号码</summary>
    public string? DocumentNumber { get; set; }

    /// <summary>是否活跃</summary>
    public bool IsActive { get; set; }
}
