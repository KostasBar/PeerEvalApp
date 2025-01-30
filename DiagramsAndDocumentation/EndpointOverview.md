# PeerEvalAppAPI
## Version: v1

### /api/Evaluation/submit

#### POST
##### Summary:
Submits a new evaluation based on the provided input data.

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Successfully submitted the evaluation. |
| 400 | The submission failed due to bad request (e.g. validation errors). |
| 404 | The requested resource was not found (e.g. related entity missing). |
| 500 | An unexpected error occurred during the submission process. |

---
### /api/Evaluation/evaluations-by-group/{groupId}/{cycleId}

#### GET
##### Summary:

Retrieves evaluations by group and cycle IDs.

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| groupId | path | The ID of the group to retrieve evaluations for. | Yes | integer |
| cycleId | path | The ID of the cycle to retrieve evaluations for. | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Successfully retrieved the evaluations. |
| 400 | An error occurred while fetching the evaluations. |
| 404 | The requested evaluations were not found. |

---
### /api/EvaluationCycle/newcycle

#### POST
##### Summary:

Initiates a new evaluation cycle with the specified details.

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | The evaluation cycle was successfully created. |
| 400 | There was an error during the creation of the evaluation cycle. |
| 409 | An evaluation cycle with the same properties already exists. |

---
### /api/EvaluationCycle/updatecycle

#### POST
##### Summary:

Updates an existing evaluation cycle with new information.

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | The evaluation cycle was successfully updated and returned. |
| 400 | There was a problem with the input parameters or during the update process. |
| 404 | The evaluation cycle to update was not found. |

---
### /api/EvaluationCycle/evaluationCycleExists

#### GET
##### Summary:

Checks if there is an active evaluation cycle.

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Successfully retrieved the evaluation cycle status. |
| 400 | An error occurred while trying to retrieve the evaluation cycle. |

---
### /api/EvaluationCycle/get-all-cycles

#### GET
##### Summary:

Retrieves all available evaluation cycles.

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Successfully retrieved the list of all evaluation cycles. |
| 400 | An error occurred while trying to retrieve the list of evaluation cycles. |

---
### /api/Group/GetAllGroups

#### GET
##### Summary:

Retrieves a list of all groups.

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Successfully retrieved the list of groups. |
| 400 | An error occurred while trying to retrieve the list of groups. |
| 404 | The requested groups could not be found. |

---
### /api/Group/AddGroup

#### POST
##### Summary:

Adds a new group to the system.

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Successfully added the new group. |
| 404 | The group already exists in the system. |
| 500 | An error occurred while trying to add the new group. |

---
### /api/User/login

#### POST
##### Summary:

Authenticates a user and returns a JWT token.

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Returns the JWT token on successful authentication. |
| 401 | If the credentials are invalid or authentication fails. |

---
### /api/User/register

#### POST
##### Summary:

Registers a new user with the provided details.

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | If the user is successfully registered. |
| 400 | If there is an error in the registration process. |
| 409 | If a user with the same email or username already exists. |

---
### /api/User/evaluations-for-user/{id}

#### GET
##### Summary:

Retrieves evaluations associated with a specific user.

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path | The ID of the user whose evaluations are to be retrieved. | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Returns a list of evaluations for the user. |
| 404 | If no user with the specified ID exists. |
| 500 | If an error occurs during the process of retrieving evaluations. |

---
### /api/User/user-to-evaluate/{id}

#### GET
##### Summary:

Retrieves users that a specific user can evaluate

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path | The evaluator to be | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Returns a list of users to be evaluated from the specific user. |
| 404 | If no user with the specified ID exists. |
| 500 | If an error occurs during the process of retrieving users. |

---
### /api/User/users-by-group/{groupId}

#### GET
##### Summary:

Retrieves a list of users belonging to a specific group.

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| groupId | path | The ID of the group for which to retrieve the users. | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Successfully retrieved the users in the specified group. |
| 500 | An error occurred while processing the request to retrieve the users. |

---
### /api/User/update-user/{userId}

#### PUT
##### Summary:

Updates an existing user's details.
Receives user data from the client and applies updates.

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| userId | path | The ID of the user to update. | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |
