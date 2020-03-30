**This is the api tests README**

## Local Test Execution

### Prerequisites
* Install Newman as API tests are executed using Newman by [Postman](https://github.com/postmanlabs/newman)
* Install Postman

To run the tests, you must do the following

1. `cd api-tests/`
1. open the file `api_env_vars.json` and replace `${FIREBASE_API_ACCESS_TOKEN}` with your personal token
1. Install Newman locally
1. Run `newman run basic-test-suite/Quarentime.api_tests_postman_collection.json  -e basic-test-suite/api_env_vars.json`

### Extending the tests
1. Open Postman locally
1. Import `basic-test-suite/Quarentime.api_tests_postman_collection.json`
1. For the existing requests, go to the Tests tab and add all the validations required
1. Add new requests with proper validations accordingly
1. To add new variations of the same request
1. After doing all the changes in Postman, export the collection back to `basic-test-suite/Quarentime.api_tests_postman_collection.json`
1. Push to branch
