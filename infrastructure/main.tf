# Configure the Azure provider
terraform {
  backend "azurerm" {
    resource_group_name  = "terraform-rg"
    storage_account_name = "lobterraformstatestor"
    container_name       = "stateprod"
    key                  = "dev.terraform.tfstate"
  }
  
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = "~> 2.65"
    }
  }

  required_version = ">= 0.14.9"
}

provider "azurerm" {
  features {}
}

# Resource Group
resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

# Virtual Network and subnet
resource "azurerm_virtual_network" "vnet" {
  name                = "tf-vnet"
  address_space       = ["10.0.0.0/16"]
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_subnet" "vnet" {
  name                 = "tf-subnet"
  resource_group_name  = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.1.0/24"]
  service_endpoints    = ["Microsoft.Sql"]

  delegation {
    name = "vnet-delegation"

    service_delegation {
      name    = "Microsoft.Web/serverFarms"
      actions = ["Microsoft.Network/virtualNetworks/subnets/action"]
    }
  }
}

# SQL Database
resource "azurerm_mssql_server" "db" {
  name                         = "tf-dbsrv"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = var.location
  version                      = "12.0"
  administrator_login          = var.sql-login
  administrator_login_password = var.sql-password
  public_network_access_enabled = true
}

resource "azurerm_mssql_database" "db" {
  name           = "tf-db"
  server_id      = azurerm_mssql_server.db.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  # license_type   = "LicenseIncluded"
  # max_size_gb    = 4
  read_scale     = false
  sku_name       = "Basic"
  storage_account_type = "GRS"
  # zone_redundant = true

  threat_detection_policy {
    state = "Enabled"
    email_addresses = [var.sql-threat-email]
  }
}

resource "azurerm_mssql_server_transparent_data_encryption" "db" {
  server_id = azurerm_mssql_server.db.id
}

resource "azurerm_mssql_virtual_network_rule" "db" {
  name      = "tf-db-vnet"
  server_id = azurerm_mssql_server.db.id
  subnet_id = azurerm_subnet.vnet.id
}

# App Service Plan + Slots
resource "azurerm_app_service_plan" "asp" {
  name                = "tf-asp"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  kind = "Linux"
  reserved = true

  sku {
    tier = "Standard"
    size = "S1"
  }
}

resource "azurerm_app_service" "app" {
  name                = "tf-app-lobex"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.asp.id

  https_only = true

  site_config {
    always_on = false
    dotnet_framework_version = "v5.0"
    ftps_state = "Disabled"
    http2_enabled = true
  }

  app_settings = {
    "SOME_KEY" = "some-value"
  }
}

resource "azurerm_app_service_virtual_network_swift_connection" "app" {
  app_service_id = azurerm_app_service.app.id
  subnet_id      = azurerm_subnet.vnet.id
}

resource "azurerm_app_service_slot" "app-staging" {
  name                = "staging"
  app_service_name    = azurerm_app_service.app.name
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.asp.id

  https_only = true

  site_config {
    always_on = false
    dotnet_framework_version = "v5.0"
    http2_enabled = true
    websockets_enabled = false
  }

  app_settings = {
    "SOME_KEY" = "some-value-3"
  }
}
