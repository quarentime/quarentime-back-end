#!/bin/bash
original_project=$(gcloud config get-value project)
echo "Current Project: $original_project"

gcloud config set project quarentime-registry
echo "Current Project: $(gcloud config get-value project)"

tag=$1
echo "Deploying images with tag: $tag"
gcloud builds submit --config cloudbuild.yaml --substitutions=_IMAGE_NAME=quarentime-user-api:$tag,_PROJECT=User.Api
gcloud builds submit --config cloudbuild.yaml --substitutions=_IMAGE_NAME=quarentime-notification-api:$tag,_PROJECT=Notification.Api

gcloud config set project $original_project
echo "Current Project: $(gcloud config get-value project)"