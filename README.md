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

## To run all tests in the solution

```
dotnet test
```

## To submit a build to GCP Cloud Build

```
$ gcloud builds submit --config cloudbuild.yaml --substitutions=_IMAGE_NAME=<image-name>,_PROJECT=<Project.Name>
```

### To submit a build to GCP Cloud Build specifying a docker image tag:

```
$ gcloud builds submit --config cloudbuild.yaml --substitutions=_IMAGE_NAME=<image-name>:<tag>,_PROJECT=<Project.Name>
```

## To deploy a version using Cloud Run

```
gcloud run deploy <project-name>  --image gcr.io/quarentime/<image-name> --platform managed
```

