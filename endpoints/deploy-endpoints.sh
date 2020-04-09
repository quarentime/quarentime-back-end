#!/bin/bash
original_project=$(gcloud config get-value project)
echo "Current Project: $original_project"

project=$1

gcloud config set project $project
echo "Current Project: $(gcloud config get-value project)"

echo "Enabling services..."
gcloud services enable servicemanagement.googleapis.com
gcloud services enable servicecontrol.googleapis.com
gcloud services enable endpoints.googleapis.com

echo "Deploying configuration"

original_url=$(gcloud run services describe quarentime-gateway --platform managed | grep https)
service_url=${original_url#https://}

echo $service_url

echo "We should run some template tool here to fill the config.yaml in the gaps"

gcloud endpoints services deploy config.yaml 
gcloud services enable $service_url


echo "Aquire configuration id"
original_id=$(gcloud endpoints services describe $service_url | grep id:)
configuration_id=${original_id#  id: }
echo $configuration_id

chmod +x gcloud_build_image
./gcloud_build_image -s $service_url \
    -p $project -c $configuration_id

gcloud run deploy quarentime-gateway \
  --image="gcr.io/quarentime-prod/endpoints-runtime-serverless:$service_url-$configuration_id" \
  --set-env-vars=ESPv2_ARGS=--cors_preset=basic \
  --allow-unauthenticated \
  --platform managed

gcloud config set project $original_project
echo "Current Project: $(gcloud config get-value project)"