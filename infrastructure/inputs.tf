variable "resource_group_name" {
  default = "tf-rg"
}

variable "location" {
  default ="eastus2"
}

variable "sql-login" {
  type = string
}

variable "sql-password" {
  type = string
}

variable "sql-threat-email" {
  type = string
}
