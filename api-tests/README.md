**This is the api tests README**

## Local Test Execution

### Prerequisites
* Install Newman as API tests are executed using Newman by [Postman](https://github.com/postmanlabs/newman)
* Install Postman
* Get the API_KEY from Firebase

To run the tests, you must do the following

1. `cd api-tests/`
1. In the Terminal, add the env var `QUARENTIME_FIREBASE_TOKEN``
    1. `export QUARENTIME_FIREBASE_API_KEY=${API_KEY}`
1. Install Newman locally
    1. `npm install -g newman`
    1. `npm install -g newman-reporter-htmlextra`
1. Run ` newman run basic-test-suite/Quarentime-API-Tests.postman_collection.json  -e basic-test-suite/api_env_vars.json -r htmlextra,csv,cli --env-var "API_TOKEN=$QUARENTIME_FIREBASE_API_KEY`

### Extending the tests
1. Open Postman locally
1. Import `basic-test-suite/Quarentime-API-Tests.postman_collection.json`
1. For the existing requests, go to the Tests tab and add all the validations required
    1. There are 2 types of validations being done
        1. `Validate Status Code`
        1. `Validate Response`
    1. Examples:
        1. `pm.test("Validate Status Code", function() {pm.response.to.have.status(200);});`
        1. `pm.test("Validate Response", function() {var jsonData = pm.response.json();var jsonDataResult = jsonData.result;pm.expect(jsonDataResult.email).to.eql("test_new_user@quarentime.org");});`
1. Add new requests with proper validations accordingly
1. Add new variations of the same request --> //TODO
1. After doing all the changes in Postman, export the collection back to `basic-test-suite/Quarentime-API-Tests.postman_collection.json`
1. Push to branch


### TODOs
* Improve token generation
* Setup package.json to not have to install newman manually
* Add tests
* Integrate with Continuous Integration

