
az login
# az group create -n swa-easytokens -l westcentralus
# az group deployment create --resource-group swa-easytokens --template-file ./templates/service.template.json --parameters ./templates/service.parameters.json
az group deployment create --resource-group swa-easytokens --template-file ./templates/swa-easytokens-master.template.json --parameters ./parameters/swa-easytokens-parameters.json

