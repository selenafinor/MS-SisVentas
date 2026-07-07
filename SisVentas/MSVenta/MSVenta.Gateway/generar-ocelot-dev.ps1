$mapaPuertos = @{
    "ms-seguridad"  = 5001
    "ms-venta"      = 5002
    "ms-inventario" = 5003
    "ms-compras"    = 5004
    "ms-reportes"   = 5005
}

$json = Get-Content "ocelot.json" -Raw | ConvertFrom-Json

foreach ($ruta in $json.Routes) {
    foreach ($destino in $ruta.DownstreamHostAndPorts) {
        $hostOriginal = $destino.Host
        if ($mapaPuertos.ContainsKey($hostOriginal)) {
            $destino.Host = "localhost"
            $destino.Port = $mapaPuertos[$hostOriginal]
        }
    }
}

$json.GlobalConfiguration.BaseUrl = "http://localhost:5000"

$json | ConvertTo-Json -Depth 20 | Set-Content "ocelot.Development.json" -Encoding UTF8

Write-Host "ocelot.Development.json generado correctamente."