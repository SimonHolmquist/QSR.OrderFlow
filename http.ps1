$base = "https://localhost:7283"
$body = @{
  customerId = [guid]::NewGuid()
  items = @(
    @{ productId = [guid]::NewGuid(); qty = 2; unitPrice = 125.50 }
    @{ productId = [guid]::NewGuid(); qty = 1; unitPrice = 49.99 }
  )
} | ConvertTo-Json

$res = Invoke-RestMethod -Method POST -Uri "$base/orders" -ContentType 'application/json' -Body $body
$orderId = $res.orderId
"OrderId: $orderId"

Invoke-RestMethod -Method POST -Uri "http://localhost:7071/api/simulate-payment/$orderId" | Out-Null
"OK"

Start-Sleep -Seconds 3
Invoke-RestMethod -Method GET -Uri "$base/orders/$orderId" | Format-List
