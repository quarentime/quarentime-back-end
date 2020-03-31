**This is the api tests README**

## Local Test Execution

### Prerequisites
* Install Newman as API tests are executed using Newman by [Postman](https://github.com/postmanlabs/newman)
* Install Postman

To run the tests, you must do the following

1. `cd api-tests/`
1. In the Terminal, add the env var `QUARENTIME_FIREBASE_TOKEN``
    1. `export QUARENTIME_FIREBASE_TOKEN=%YOUR_TOKEN`
1. Install Newman locally
    1. `npm install -g newman`
    1. `npm install newman-reporter-html`
1. Run `newman run basic-test-suite/Quarentime.api_tests_postman_collection.json  -e basic-test-suite/api_env_vars.json -r html,cli,junit --env-var "token=$QUARENTIME_FIREBASE_TOKEN"`

### Extending the tests
1. Open Postman locally
1. Import `basic-test-suite/Quarentime.api_tests_postman_collection.json`
1. For the existing requests, go to the Tests tab and add all the validations required
1. Add new requests with proper validations accordingly
1. To add new variations of the same request
1. After doing all the changes in Postman, export the collection back to `newman run basic-test-suite/Quarentime.api_tests_postman_collection.json  -e basic-test-suite/api_env_vars.json -r html,cli,junit --env-var "token=$QUARENTIME_FIREBASE_TOKEN"`
1. Push to branch
