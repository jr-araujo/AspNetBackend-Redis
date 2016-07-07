# Backend that was built using AspNet and Redis as Repository

Instructions to configure your PublishProfiles file:

1. Change the temp output to a path with a shorter path name.

2. Open your target .pubxml in .\Properties\PublishProfiles from a text editor.

3. Change or add an element called PublishOutputPathNoTrailingSlash under PropertyGroup and set the value to your desired path:

```XML
<Project ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <PublishOutputPathNoTrailingSlash>C:\Temp\Publish</PublishOutputPathNoTrailingSlash>
  </PropertyGroup>
</Project>
```
