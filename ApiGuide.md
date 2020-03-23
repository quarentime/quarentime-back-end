# API Guide
This document is a definition of what you should expect when working with the HTTP APIs on the Quarentime backend. 

## 1. Authentication
All requests must be authenticated using firebase and they must provide the access token on the Authorization header as a bearer token. 

eg.
```bash
curl --location --request GET 'https://[BASE_URL]/User/Survey' \
--header 'Authorization: Bearer TOKEN_HASH'
```
Requests that doesn't provide an access token will receive a 401 response code. Further information can be found inspecting the contents of the header `www-authenticate`.

Please, note that your access token must contain the claim `user_id` as we'll be using it to identify the user logged to the apps. Usually firebase issues this claim by default.

## 2. Requests

## 2.1 Authorization header
Every request to the api must provide the `Authorization` header with a valid access token.

## 2.1 Get
Get requests must be executed as usual.

## 2.2 Post, Put, and Patch
Verbs that allow you to inform a request body must provide a json request body and the `Content-Type: application/json` header.

```bash
curl --location --request POST 'http://localhost:8080/User/PersonalInformation' \
--header 'Content-Type: application/json' \
--header 'Authorization: Bearer XXXXXXX' \
--data-raw '{
	"email": "edmar@quarentime.org",
	"name": "Edmar Souza",
	"age": 35,
	"days_in_quarantine": 0,
	"days_in_post_quarantine": 0
}'
```

## 3. Responses
In order to create a consistent API experience, all endpoints will respond in the same way:

### 3.1 Successful requests
Successful requests will return a 200 response code with the following format:

```json
{
    "result": {
        ...
    },
    "request_id": "c8427f9d-022f-45bb-af78-b6f834ec4869"
}
```

Where:
- `result`: is the actual response content, this can be a json object containing user information (for instance), or plain text with a message. 
- `request_id`: this is the id of that request in particular, used for internal debuging

### 3.2 Non successful requests
Requests resulting on an error will issue a response code different from 200 (eg. 404), and will contain a field `error_code` containing a string representing the underlying error. 
Eg.

```json
{
    "request_id": "6f8fc4c3-fb8f-4a4d-8b25-c50d38c04c99",
    "error_code": "not_found"
}
```

### 3.3 Validation errors
Requests causing a validation error (eg. sending a string value for a integer property) will return a HTTP 400 and the following response body:

```json
{
    "result": {
        "age": "Unexpected character encountered while parsing value: c. Path 'age', line 4, position 9.,Unexpected character encountered while parsing value: a. Path 'age', line 4, position 9."
    },
    "request_id": "1cfcdfa9-9f4b-4ad5-994d-9a6267443ef0"
}
```
The `result` field is a dictionary containing further information on what caused the error. 