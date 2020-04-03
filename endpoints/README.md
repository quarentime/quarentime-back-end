## Initial setup

### Deploy ESPv2 Beta to Cloud Run

```
gcloud run deploy quarentime-gateway \
    --image="gcr.io/endpoints-release/endpoints-runtime-serverless:2" \
    --allow-unauthenticated \
    --platform managed \
    --region=europe-west1
```

### Convert open api 3 to swagger 2

```
npm install -g api-spec-converter

api-spec-converter --from=openapi_3 --to=swagger_2 --syntax=yaml --order=alpha https://quarentime-user-api-rcgccs4vga-ew.a.run.app/swagger/v1/swagger.json > config.yaml
```

### Enable services
```
gcloud services enable servicemanagement.googleapis.com
gcloud services enable servicecontrol.googleapis.com
gcloud services enable endpoints.googleapis.com
```

## Redeploying the configuration

### Deploy the configuration

```
gcloud endpoints services deploy config.yaml 
gcloud services enable quarentime-gateway-rcgccs4vga-ew.a.run.app
```

### Build custom image
```
chmod +x gcloud_build_image
./gcloud_build_image -s quarentime-gateway-rcgccs4vga-ew.a.run.app \
    -p quarentime -c 2020-04-04r5
```

### Redeploy image
```
gcloud run deploy quarentime-gateway \
  --image="gcr.io/quarentime/endpoints-runtime-serverless:quarentime-gateway-rcgccs4vga-ew.a.run.app-2020-04-04r5" \
  --set-env-vars=ESPv2_ARGS=--cors_preset=basic \
  --allow-unauthenticated \
  --platform managed
```
### Permissions
```
gcloud run services add-iam-policy-binding quarentime-user-api \
  --member "serviceAccount:quarentime-compute@developer.gserviceaccount.com" \
  --role "roles/run.invoker" \
  --platform managed
```  

### To redirect a call to a service other than the root leve

Add the x-google-backend to the specific operation where you want to specify a different endpoint. eg.
```
  /User/Contacts:
    get:
      operationId: GetContacts
      x-google-backend:
        address: https://quarentime-user-api-rcgccs4vga-ew.a.run.app
        path_translation: APPEND_PATH_TO_ADDRESS
      parameters: []
      produces:
        - text/plain
        - application/json
        - text/json
      responses:
        '200':
          description: Success
          schema:
            $ref: '#/definitions/ContactIEnumerableResponse'
      tags:
        - User
```