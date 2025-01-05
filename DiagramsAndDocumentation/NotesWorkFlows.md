# WorkFlows
Workflows of the PeerEvalApp from UI to Backend.
Used for base to build with.
---

## Login
 - Login Page
   1. Insert Data
   2. Submit - FrontEnd Authentication - Check for mandadory fields
   3. Create JSON -> Call the API -> LogIn Controller 
   4. Controller -> FromBody -> LoginDTO -> Call LogInService (Insert User)
   5. UserRepository -> InsertUser

## Home Page
 - Table with Older Evaluation Grades (If not admin)
   1. Get Call to User Controller (Evaluations where EvaluatedUser = UserEmail)
   2. Create GetEvaluationsDTO
   3. Service -> UserRepo -> GetEvaluations -> EvaluationsDTO
   4. User sees: 
         |evaluation cycle |  evaluation | avg grade per evaluation|
         |-|-|-|
 - Table Older or New Evaluations If Exist
   1. Get Call to User Controller
   2. Create GetEvaluationsDTO
   3. Service -> UserRepo -> GetAllEvaluations -> EvaluationsDTO
   4. User sees: 
         |evaluation cycle |  evaluation | open/closed|
         |-|-|-|
  - Button to Create new Evaluation (if admin) 
  
 - Menu
   - Profile
   - Evaluations (if not an admin)
    

## Profile 
  - Update Profile
     1. Update Details -> Post to User Controller
     2. Controller FromBody UpdateUserDTO
     3. UpdateService -> UserRepository -> UpdateUser -> UpdatedUser

## Evaluations Page
 1. Get Call UserController -> Active evaluations -> Service -> Repo List<Evaluations>
 2. If no active Evaluation -> Just message that there are no active Evaluations
 3. Else -> List of users to evaluate
 4. Click on user -> EvaluationForThisUser

## Evaluation Page For User
 1. Show Evaluation Page for this user
 2. OnSubmit -> EvaluationController -> FromBody NewEvalDTO
 3. Service -> EvalRepo -> SubmitEval -> CompletedEvaluation