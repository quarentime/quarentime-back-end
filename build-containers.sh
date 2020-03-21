#!/bin/bash
docker build -t quarentime-user-api -f ./User.Api.Dockerfile .
docker build -t quarentime-contact-circle-api -f ./ContactCircle.Api.Dockerfile .
docker build -t quarentime-risk-profile-api -f ./RiskProfile.Api.Dockerfile .
docker build -t quarentime-location-api -f ./Location.Api.Dockerfile .