# PeerEvalAppAPI

## OpenAPI Version: 3.0.1
## Version: v1

## Paths

### `/api/Evaluation/submit` (POST)
- **Tags**: Evaluation
- **RequestBody**: Accepts JSON with `SubmitEvaluationDTO`
- **Responses**:
  - **200**: OK

### `/api/EvaluationCycle/newcycle` (POST)
- **Tags**: EvaluationCycle
- **Summary**: Initiates a new evaluation cycle with the specified details.
- **RequestBody**: Contains `InitiateCycleDTO` detailing the new cycle.
- **Responses**:
  - **200**: The evaluation cycle was successfully created.
  - **409**: An evaluation cycle with the same properties already exists.
  - **400**: There was an error during the creation of the evaluation cycle.

### `/api/EvaluationCycle/updatecycle` (POST)
- **Tags**: EvaluationCycle
- **Summary**: Updates an existing evaluation cycle with new information.
- **RequestBody**: Contains `UpdateCycleDTO` for updating the cycle.
- **Responses**:
  - **200**: The evaluation cycle was successfully updated and returned.
  - **404**: The evaluation cycle to update was not found.
  - **400**: There was a problem with the input parameters or during the update process.

### `/api/User/login` (POST)
- **Tags**: User
- **Summary**: Authenticates a user and returns a JWT token.
- **RequestBody**: User's login credentials (`UserLogInDTO`).
- **Responses**:
  - **200**: Returns the JWT token on successful authentication.
  - **401**: If the credentials are invalid or authentication fails.

### `/api/User/register` (POST)
- **Tags**: User
- **Summary**: Registers a new user with the provided details.
- **RequestBody**: User's sign up information (`UserSignUpDTO`).
- **Responses**:
  - **200**: If the user is successfully registered.
  - **409**: If a user with the same email or username already exists.
  - **400**: If there is an error in the registration process.

### `/api/User/evaluations-for-user/{id}` (GET)
- **Tags**: User
- **Summary**: Retrieves evaluations associated with a specific user.
- **Parameters**:
  - `id`: The ID of the user whose evaluations are to be retrieved.
- **Responses**:
  - **200**: Returns a list of evaluations for the user.
  - **404**: If no user with the specified ID exists.
  - **500**: If an error occurs during the process of retrieving evaluations.

### `/api/User/user-to-evaluate/{id}` (GET)
- **Tags**: User
- **Summary**: Retrieves users that a specific user can evaluate.
- **Parameters**:
  - `userId`: The evaluator to be.
  - `id`: ID path parameter.
- **Responses**:
  - **200**: Returns a list of users to be evaluated from the specific user.
  - **404**: If no user with the specified ID exists.
  - **500**: If an error occurs during the process of retrieving users.

## Components Schemas
- **Evaluation**
- **EvaluationAnswer**
- **EvaluationAnswerDTO**
- **EvaluationCycle**
- **Group**
- **InitiateCycleDTO**
- **Question**
- **SubmitEvaluationDTO**
- **UpdateCycleDTO**
- **User**
- **UserLogInDTO**
- **UserRole**
- **UserSignUpDTO**

Each schema details specific properties and types required for requests and responses throughout the API.
