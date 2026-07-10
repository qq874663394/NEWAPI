using System.ComponentModel.DataAnnotations;

namespace Application.DTO.User;

/// <summary>
/// 修改密码请求
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>用户 Code</summary>
    [Required]
    public Guid UserCode { get; set; }

    /// <summary>旧密码</summary>
    [Required(ErrorMessage = "旧密码不能为空")]
    public string OldPassword { get; set; } = string.Empty;

    /// <summary>新密码</summary>
    [Required(ErrorMessage = "新密码不能为空")]
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>确认新密码</summary>
    [Required(ErrorMessage = "确认密码不能为空")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
