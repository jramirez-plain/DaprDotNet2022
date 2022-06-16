# DaprDotNet2022
Dapr example for DotNet2022

# Sample

## Send capacity forecast messages:
``` Bash
dapr publish --publish-app-id capacityplanner --pubsub servicebus-pubsub --topic capacity-forecast --data '{"specversion" : "1.0", "type" : "com.dapr.cloudevent.sent", "source" : "Historical Forecaster", "subject" : "Forecast", "id" : "someCloudEventId", "time" : "2022-06-28T09:00:00Z", "datacontenttype" : "application/cloudevents+json", "data" : {"date": "2022-07-01T00:00:00Z", "historicalLevel": "5"}}'
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

