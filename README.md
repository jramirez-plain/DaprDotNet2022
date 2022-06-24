# DaprDotNet2022
Dapr example for DotNet2022

# Sample

dapr run --app-id capacityplanner --app-port 5151 --log-as-json --log-level debug --metrics-port 9000 --config C:\Users\crecuero\source\repos\DaprDotNet2022\dapr\configuration\configuration.yaml --components-path C:\Users\crecuero\source\repos\DaprDotNet2022\dapr\components -- dotnet run

dapr run --app-id catalog --app-port 5195 --dapr-grpc-port 50001 --dapr-http-port 3500 --log-as-json --log-level debug --metrics-port 9001 --placement-host-address 127.0.0.1:6050 --config C:\Users\crecuero\source\repos\DaprDotNet2022\dapr\configuration\configuration.yaml --components-path C:\Users\crecuero\source\repos\DaprDotNet2022\dapr\components -- dotnet run

## Send capacity forecast messages:

Message to publish:

Historical Forecast:

``` json
{
  "id": "2e45ec17-c2c4-4254-a18c-c1190fd32dd0",
  "source": "historical",
  "specversion": "1.0",
  "type": "com.dapr.event.sent",
  "traceparent": "00-216deef472a0836071147d9eb13bcb76-d230a75ad0ca60f4-00",
  "datacontenttype": "application/json",
  "data": {
      "hotelCode": "1",
      "date": "2022-07-01",
      "historicalLevel": 6
  }
}
```

Reservation Trend:

``` json
{
  "id": "4b45ec17-b2c2-2332-b15c-c5670fd32d55",
  "source": "reservations",
  "specversion": "1.0",
  "type": "com.dapr.event.sent",
  "traceparent": "00-216deef472a0836071147d9eb13bcb76-d230a75ad0ca60f4-00",
  "datacontenttype": "application/json",
  "data": {
      "hotelCode": "1",
      "date": "2022-07-01",
      "estimatedReservations": 90
  }
}
```

Direct forecast:

``` json
{
  "id": "2e45ec17-c2c4-4254-a18c-c1190fd32dd0",
  "source": "historical",
  "specversion": "1.0",
  "type": "com.dapr.event.sent",
  "traceparent": "00-216deef472a0836071147d9eb13bcb76-d230a75ad0ca60f4-00",
  "datacontenttype": "application/json",
  "data": {
    "hotelCode": "1",
    "date": "2022-07-01",
    "occupancyPercentage": 0.69,
    "confidenceRate": 0.7
  }
}
```

# deploy

We need to prepare our az cli to manage Azure Conatiner Apps. To do that we need to add a new extension.

``` Bash
 az extension add --name containerapp --upgrade
```

The second step is prepare our subscription to user Azure Container Apps

``` Bash
  az login
  az provider register --namespace Microsoft.App
  az provider register --namespace Microsoft.OperationalInsights
```
  

Now we can create our environment to deploy our apps:

First we need to create the resource group where we are going to deploy out apps:

``` Bash
az group create --name DotNet2022 --location northeurope
``` 

Now we need and environment to deploy our apps, but we need a Log Analytics workspace to use with our environment. We can let az cli to create one if we don't set one previously, or we can create our workspace and set up the creation of the environment with out workspace

``` Bash
  az containerapp env create --name DaprApps -g DotNet2022 -l northeurope
```

or

``` Bash (pending...)
  az monitor log-analytics workspace create -g DotNet2022 -n workspace-DaprApps -l northeurope
  az containerapp env create --name DaprApps -g DotNet2022 -l northeurope --logs-workspace-id --logs-workspace-key
```

## Deploy Application in ContainerApp

Now, we need to deploy our applciation using Dapr. To acomplish this actions we need to deploy first the components of dapr used for our application, and then we can deploy some revision of our application

### Deploy components

We need to define and configure our componets, and we can use a yaml file to setup, and use that definition file to deploy.

For example we have a state component based on keyvault, like that:

``` yaml

  name: localsecretstore
  componentType: secretstores.azure.keyvault
  version: v1
  metadata:
  - name: vaultName
    value: "kvDotNet2022"
  - name: azureTenantId
    value: "3a465f8a-b004-45a3-a74e-2b479766bd54"
  - name: azureClientId
    value: "49e8c131-0c22-4030-a34d-3f390f6869bd"
  - name: azureClientSecret
    secretRef: clientsecret
  secrets:
  - name: clientsecret
    value: "notelovoyadecir"
  scopes:
  - capacityplanner

``` 

To create our new component, we will use the yaml definition directly with az cli tu have available that component dor  our applications

``` Bash
  az containerapp env dapr-component set --name DaprApps -g DotNet2022 --dapr-component-name mysecretstore --yaml .\deploy\mysecretstore.yaml
  az containerapp env dapr-component set --name DaprApps -g DotNet2022 --dapr-component-name mystatestore --yaml .\deploy\mystatestore.yaml
``` 

We can list or available components in our environment with that command

``` bash
  az containerapp env dapr-component list --name DaprApps -g DotNet2022 --output table
```

## Deploy Capacity Planner

Now that we have an Azure Components App environment ready, we can start deploying our application. There are several ways to deploy, we will use yaml definition files to set up out application and we use az cli to deploy it.


``` bash
  az containerapp create -n capacityplanner -g DotNet2022 --environment DaprApps --yaml .\deploy\capacityplanner.yml
```

If we want to access to the service, we need to enable an ingress, that could be internal or external and we need a target port for the ingress

```bash
  az containerapp ingress enable -n capacityplanner -g DotNet2022 --type internal --target-port 80
``` 

