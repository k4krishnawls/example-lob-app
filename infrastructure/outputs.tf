output "resource_group_id" {
  value = azurerm_resource_group.rg.id
}

output "resource_group_name" {
  value = azurerm_resource_group.rg.name
}

output "asp_id" {
  value = azurerm_app_service_plan.asp.id
}

output "app_id" {
  value = azurerm_app_service_plan.asp.id
}

output "app_name" {
  value = azurerm_app_service_plan.asp.name
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
