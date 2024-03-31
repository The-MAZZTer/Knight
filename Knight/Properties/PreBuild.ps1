$ErrorActionPreference = "Stop"

Push-Location $PSScriptRoot

(Get-Date).Ticks | Set-Content "BuildDate.txt"

#[string] $date = (Get-Date).ToString("yyyy.MM.dd.HHmm")

#((Get-Content "AssemblyInfo.cs") `
#	-replace "\[assembly: AssemblyVersion\(`".*?`"\)\]", ("[assembly: AssemblyVersion(`"" + $date + "`")]") `
#	-replace "\[assembly: AssemblyFileVersion\(`".*?`"\)\]", ("[assembly: AssemblyFileVersion(`"" + $date + "`")]") `
#) | Set-Content "AssemblyInfo.cs"

Pop-Location