﻿[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$source,
  [Parameter(Mandatory=$True,Position=2)]
  [string]$destinationDirectory,
  [Parameter(Mandatory=$true,Position=3)]
  [string]$coreAssemblyPath,
  [Parameter(Mandatory=$false,Position=4)]
  [string]$signingCertThumbprint
)

$command = "Command Excuted: " + $MyInvocation.Line
Write-Output $command

Write-Host "Source: " $source
Write-Host "Destination Directory: " $destinationDirectory
Write-Host "Signing Cert Thumbprint: " $signingCertThumbprint
Write-Host "Core Assembly Path: " $coreAssemblyPath

try
{
  $addType = Add-Type -Path $coreAssemblyPath -PassThru -ErrorAction Stop
}
catch
{
    ForEach($ex in $_.Exception.LoaderExceptions)
    {
        Write-Error $ex.Message
    }
}

if($addType){
  if($signingCertThumbprint){
      $resultPack = [Microsoft.Templates.Core.Packaging.TemplatePackage]::PackAndSign($source, $signingCertThumbprint, "text/plain")
  }
  else{
      $resultPack = [Microsoft.Templates.Core.Packaging.TemplatePackage]::Pack($source, "text/plain")
  }
  
  if($resultPack){
    if (!(Test-Path -path $destinationDirectory)) 
    {
        Write-Host "Creating destination directory" $destinationDirectory
        New-Item $destinationDirectory -Type Directory -Force
    }
    $destinationFileName = "Templates.mstx"
    $destinationPath = Join-Path $destinationDirectory $destinationFileName

    Write-Host "Copying" $resultPack "to" $destinationPath
    Copy-Item -Path $resultPack -Destination $destinationPath -Force
  }
  else{
    throw "Source not packed and signed properly!"
  }
}
else{
  throw "Core Assembly not found. Can't continue.!"
}
Write-Host 'Finished!'