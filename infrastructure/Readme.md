This is the infrastructure for the Example LOB App services.

The terraform plan is used for two purposes:
1. To manage the infrastructure
2. To enable the CD pipeline to query for infrastructure details (reduce the amount of config duplication)

The terraform state is stored in a dedicated storage account in Azure that is only accessed 
during infrastructure operations and is 100% separate from running infrastructure for the services.

## Database Firewall Exceptions

Firewall exceptions can be added in the `firewall_rules.tg` file, separate from the main infrastructure.