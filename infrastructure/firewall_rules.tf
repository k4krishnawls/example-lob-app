
resource "azurerm_mssql_firewall_rule" "example" {
  name             = "Eli Home"
  server_id        = azurerm_mssql_server.db.id
  start_ip_address = "99.3.71.225"
  end_ip_address   = "99.3.71.225"
}