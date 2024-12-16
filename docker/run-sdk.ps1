param(
    [Parameter(ValueFromRemainingArguments=$true)]
    [string[]]$CommandArgs
)

$command = "$($CommandArgs -join ' ')"
Write-Host "Executing: $command"
docker compose run --rm sdk sh -c "$command"
