output "resource_group_id" {
  value = azurerm_resource_group.rg.id
}

output "resource_group_name" {
  value = azurerm_resource_group.rg.name
}

output "resource_group_location" {
  value = azurerm_resource_group.rg.location
}

output "asp_id" {
  value = azurerm_app_service_plan.asp.id
}

output "app_id" {
  value = azurerm_app_service.app.id
}

output "app_name" {
  value = azurerm_app_service.app.name
}

output "app_staging_url" {
  value = azurerm_app_service_slot.app-staging.default_site_hostname
}

output "db_server_id" {
  value = azurerm_mssql_server.db.id
}

output "db_server_name" {
  value = azurerm_mssql_server.db.name
}

output "db_database_name" {
  value = azurerm_mssql_database.db.name
}

