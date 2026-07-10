using System.ComponentModel.DataAnnotations;

namespace Application.DTO.User;

/// <summary>
/// 管理员重置密码请求
/// </summary>
public class ResetPasswordRequest
{
    /// <summary>
    /// 新密码
    /// </summary>
    [Required(ErrorMessage = "新密码不能为空")]
    public string NewPassword { get; set; } = string.Empty;
}
