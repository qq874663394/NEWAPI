<#
.SYNOPSIS
    生成当前目录的树形结构，排除常见的 .NET / GitHub 等临时文件和文件夹。
.DESCRIPTION
    递归列出所有子文件夹（可选文件），跳过指定名称的文件夹和指定扩展名的文件。
#>

# ----------------------------- 配置排除列表 -----------------------------
# 需要排除的文件夹名称（不区分大小写）
$excludedFolders = @(
    "bin", "obj", ".vs", ".git", "node_modules", "packages", "TestResults",
    "CoverageResults", "Release", "Debug", ".idea", ".vscode", "logs", "Logs",
    "_ReSharper.Caches", "_ReSharper.*", "App_Data", "Backup"
)

# 需要排除的文件扩展名（不含点号，不区分大小写）
$excludedExtensions = @(
    "suo", "user", "dll", "exe", "pdb", "cache", "log", "tmp", "nupkg"
)

# 是否同时显示文件（$true 显示文件，$false 只显示文件夹）
$showFiles = $true
# -----------------------------------------------------------------------

# 设置控制台输出编码为 UTF-8（解决中文乱码）
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$OutputEncoding = [System.Text.Encoding]::UTF8

function Show-Tree {
    param(
        [string]$Path = ".",
        [string]$Prefix = "",
        [bool]$IsLast = $true,
        [string]$RootFullPath = (Resolve-Path $Path).Path
    )

    $items = Get-ChildItem $Path -Force | Where-Object {
        $shouldExclude = $false
        # 排除文件夹
        if ($_.PSIsContainer) {
            foreach ($ex in $excludedFolders) {
                if ($_.Name -like $ex) { $shouldExclude = $true; break }
            }
        }
        # 排除文件
        elseif (-not $_.PSIsContainer -and -not $showFiles) {
            $shouldExclude = $true
        }
        elseif (-not $_.PSIsContainer -and $showFiles) {
            $ext = $_.Extension.TrimStart('.')
            foreach ($ex in $excludedExtensions) {
                if ($ext -eq $ex) { $shouldExclude = $true; break }
            }
        }
        -not $shouldExclude
    }

    $count = $items.Count
    for ($i = 0; $i -lt $count; $i++) {
        $item = $items[$i]
        $isLastItem = ($i -eq $count - 1)
        $currentPrefix = if ($isLastItem) { "└──" } else { "├──" }

        # 输出当前项
        Write-Host "$Prefix$currentPrefix $($item.Name)"

        # 递归处理子文件夹
        if ($item.PSIsContainer) {
            $newPrefix = if ($isLastItem) { "    " } else { "│   " }
            Show-Tree -Path $item.FullName -Prefix ($Prefix + $newPrefix) -IsLast $isLastItem -RootFullPath $RootFullPath
        }
    }
}

# 开始生成树
Write-Host "Directory tree (excluded: $($excludedFolders -join ', '))" -ForegroundColor Cyan
Show-Tree