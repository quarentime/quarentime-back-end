#!/bin/bash
original_project=$(gcloud config get-value project)
echo "Current Project: $original_project"

gcloud config set project quarentime
echo "Current Project: $(gcloud config get-value project)"

service=$1
echo "Deploying cloud run service $service using tag: develop"

gcloud run deploy quarentime-$service  \
--image gcr.io/quarentime-registry/quarentime-$service:develop \
--platform managed --allow-unauthenticated

gcloud config set project $original_project
echo "Current Project: $(gcloud config get-value project)"