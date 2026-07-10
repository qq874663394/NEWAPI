namespace Application.DTO.Log;

/// <summary>
/// 日志 DTO
/// </summary>
public class LogDto
{
    public Guid Code { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? MenuName { get; set; }
    public string? ModuleName { get; set; }
    public string? ButtonName { get; set; }
    public string? Content { get; set; }
    public string? Result { get; set; }
    public string? Url { get; set; }
    public string? Ip { get; set; }
    public string? WorkStationName { get; set; }
    public string? Method { get; set; }
    public string? Params { get; set; }
    public DateTime CreateTime { get; set; }
}
