packages\NuGet.CommandLine.3.3.0\tools\nuget pack Jal.Persistence\Jal.Persistence.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.Persistence.Nuget

packages\NuGet.CommandLine.3.3.0\tools\nuget pack Jal.Persistence.Installer\Jal.Persistence.Installer.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.Persistence.Nuget

packages\NuGet.CommandLine.3.3.0\tools\nuget pack Jal.Persistence.Logger\Jal.Persistence.Logger.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.Persistence.Nuget

packages\NuGet.CommandLine.3.3.0\tools\nuget pack Jal.Persistence.Logger.Installer\Jal.Persistence.Logger.Installer.csproj -Properties "Configuration=Release;Platform=AnyCPU;OutputPath=bin\Release" -Build -IncludeReferencedProjects -OutputDirectory Jal.Persistence.Nuget

pause;