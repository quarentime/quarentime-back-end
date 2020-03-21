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