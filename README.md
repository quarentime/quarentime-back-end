## To build a specific project as a docker container

```
$ docker build -t quarentime-contact-circle -f ./ContactCircle.Api.Dockerfile .
```

## To build all projects as containers

```
$ ./build-containers.sh
$ docker image list | grep quarentime-
```

## To run a built docker container

```
$ docker run -d -p 8080:8080 <container_name>
```

## To submit a build to GCP cloud build

```
$ gcloud builds submit --config cloudbuild.yaml --substitutions=_IMAGE_NAME=quarentime-user-api,_PROJECT=User.Api
```

## To deploy a version using Cloud Run

```
```