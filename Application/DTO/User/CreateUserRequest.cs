using System.ComponentModel.DataAnnotations;

namespace Application.DTO.User;

/// <summary>
/// 创建用户请求
/// </summary>
public class CreateUserRequest
{
    /// <summary>登录账号（必填，唯一）</summary>
    [Required(ErrorMessage = "登录账号不能为空")]
    public string Name { get; set; } = string.Empty;

    /// <summary>用户全名（必填）</summary>
    [Required(ErrorMessage = "用户全名不能为空")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>登录密码（必填）</summary>
    [Required(ErrorMessage = "密码不能为空")]
    public string Password { get; set; } = string.Empty;

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

    /// <summary>初始岗位分配列表（角色 + 组织），可选</summary>
    public List<AssignRoleOrgRequest>? OrgRoles { get; set; }
}
