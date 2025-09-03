# Azure Deployment Guide for ISMT College Website

This guide provides step-by-step instructions for deploying the ISMT College website to Azure as part of the cloud migration assignment.

## Prerequisites

- Azure subscription
- Azure CLI installed and configured
- .NET 6.0 SDK
- Git repository access

## Step 1: Azure Resource Group Setup

### Create Resource Group
```bash
az group create --name ISMTCollege-RG --location "East US"
```

### Verify Resource Group
```bash
az group show --name ISMTCollege-RG
```

## Step 2: Azure Database for PostgreSQL Setup

### Create PostgreSQL Flexible Server
```bash
az postgres flexible-server create \
  --resource-group ISMTCollege-RG \
  --name ismtcollege-postgres \
  --admin-user postgresadmin \
  --admin-password "YourSecurePassword123!" \
  --sku-name Standard_B1ms \
  --tier Burstable \
  --storage-size 32 \
  --version 14 \
  --location "East US"
```

### Create Database
```bash
az postgres flexible-server db create \
  --resource-group ISMTCollege-RG \
  --server-name ismtcollege-postgres \
  --database-name ismtcollege
```

### Configure Firewall Rules
```bash
# Allow Azure services
az postgres flexible-server firewall-rule create \
  --resource-group ISMTCollege-RG \
  --name ismtcollege-postgres \
  --rule-name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

# Allow your IP address
az postgres flexible-server firewall-rule create \
  --resource-group ISMTCollege-RG \
  --name ismtcollege-postgres \
  --rule-name AllowMyIP \
  --start-ip-address YOUR_IP_ADDRESS \
  --end-ip-address YOUR_IP_ADDRESS
```

## Step 3: Azure App Service Setup

### Create App Service Plan
```bash
az appservice plan create \
  --resource-group ISMTCollege-RG \
  --name ISMTCollege-Plan \
  --sku B1 \
  --is-linux
```

### Create Web App
```bash
az webapp create \
  --resource-group ISMTCollege-RG \
  --plan ISMTCollege-Plan \
  --name ismtcollege-webapp \
  --runtime "DOTNETCORE:6.0"
```

### Configure App Settings
```bash
# Get PostgreSQL connection string
az postgres flexible-server show \
  --resource-group ISMTCollege-RG \
  --name ismtcollege-postgres \
  --query "connectionString" \
  --output tsv

# Set connection string in App Service
az webapp config appsettings set \
  --resource-group ISMTCollege-RG \
  --name ismtcollege-webapp \
  --settings \
    ConnectionStrings__DefaultConnection="Host=ismtcollege-postgres.postgres.database.azure.com;Database=ismtcollege;Username=postgresadmin;Password=YourSecurePassword123!;Port=5432;SSL Mode=Require;Trust Server Certificate=true;" \
    ASPNETCORE_ENVIRONMENT="Production"
```

## Step 4: Database Migration

### Update Connection String Locally
Update `appsettings.json` with Azure PostgreSQL connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=ismtcollege-postgres.postgres.database.azure.com;Database=ismtcollege;Username=postgresadmin;Password=YourSecurePassword123!;Port=5432;SSL Mode=Require;Trust Server Certificate=true;"
  }
}
```

### Run Migrations
```bash
dotnet ef database update
```

## Step 5: Deploy Application

### Publish Application
```bash
dotnet publish -c Release -o ./publish
```

### Deploy to Azure
```bash
az webapp deployment source config-zip \
  --resource-group ISMTCollege-RG \
  --name ismtcollege-webapp \
  --src ./publish.zip
```

### Alternative: Deploy from Git
```bash
az webapp deployment source config \
  --resource-group ISMTCollege-RG \
  --name ismtcollege-webapp \
  --repo-url "YOUR_GIT_REPO_URL" \
  --branch main \
  --manual-integration
```

## Step 6: Verify Deployment

### Check App Service Status
```bash
az webapp show \
  --resource-group ISMTCollege-RG \
  --name ismtcollege-webapp \
  --query "defaultHostName" \
  --output tsv
```

### Test Application
- Navigate to your web app URL
- Test all pages (Home, About, Courses, Contact)
- Submit a test contact form
- Verify data is stored in PostgreSQL

## Step 7: Monitoring and Scaling

### Enable Application Insights
```bash
az monitor app-insights component create \
  --app ismtcollege-insights \
  --location "East US" \
  --resource-group ISMTCollege-RG \
  --application-type web
```

### Configure Auto-scaling
```bash
az monitor autoscale create \
  --resource-group ISMTCollege-RG \
  --resource ismtcollege-webapp \
  --resource-type Microsoft.Web/sites \
  --name ISMTCollege-autoscale \
  --min-count 1 \
  --max-count 3 \
  --count 1
```

## Step 8: Security and Compliance

### Enable HTTPS
```bash
az webapp update \
  --resource-group ISMTCollege-RG \
  --name ismtcollege-webapp \
  --https-only true
```

### Configure Authentication (Optional)
```bash
az webapp auth update \
  --resource-group ISMTCollege-RG \
  --name ismtcollege-webapp \
  --enabled true \
  --action LoginWithAzureActiveDirectory
```

## Cost Optimization

### Monitor Costs
```bash
az consumption usage list \
  --start-date 2025-01-01 \
  --end-date 2025-01-31
```

### Optimize Resources
- Use Basic (B1) tier for development
- Scale up only when needed
- Monitor database performance

## Troubleshooting

### Common Issues

1. **Connection String Errors**
   - Verify firewall rules
   - Check SSL settings
   - Ensure database exists

2. **Deployment Failures**
   - Check build logs
   - Verify runtime version
   - Check app settings

3. **Database Connection Issues**
   - Verify PostgreSQL server is running
   - Check firewall rules
   - Verify credentials

### Useful Commands

```bash
# View app logs
az webapp log tail --name ismtcollege-webapp --resource-group ISMTCollege-RG

# Restart app service
az webapp restart --name ismtcollege-webapp --resource-group ISMTCollege-RG

# Check app settings
az webapp config appsettings list --name ismtcollege-webapp --resource-group ISMTCollege-RG
```

## Cleanup (When Assignment is Complete)

```bash
# Delete all resources
az group delete --name ISMTCollege-RG --yes --no-wait
```

## Assignment Deliverables

✅ **ASP.NET Core MVC Project with PostgreSQL integration**
✅ **appsettings.json with sample PostgreSQL connection string**
✅ **EF Core migration files**
✅ **Azure deployment ready**
✅ **Local development setup**
✅ **Complete documentation**

## Next Steps for Production

1. **CI/CD Pipeline**: Set up Azure DevOps or GitHub Actions
2. **Monitoring**: Configure alerts and dashboards
3. **Backup**: Set up automated database backups
4. **Security**: Implement additional security measures
5. **Performance**: Optimize database queries and caching

## Support Resources

- [Azure App Service Documentation](https://docs.microsoft.com/en-us/azure/app-service/)
- [Azure Database for PostgreSQL](https://docs.microsoft.com/en-us/azure/postgresql/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
