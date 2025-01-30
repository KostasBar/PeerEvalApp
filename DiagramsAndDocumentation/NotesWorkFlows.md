# PeerEvalApp - Role-based Overview & Endpoints

Below is a concise description of the **Admin** and **User** functionalities in **PeerEvalApp**, along with some suggested endpoints that implement these capabilities.

---

## Admin Functionality

1. **Create & Open EvaluationCycles**  
   - **Description**: An Admin can create new evaluation cycles and set them to `Open`.  
   - **Endpoint**:  
     - `POST /api/EvaluationCycle/newcycle` – Creates a new evaluation cycle (body contains title and duration).

2. **Update EvaluationCycles**  
   - **Change Status**: An Admin can switch the status to `Closed` (e.g., when the cycle is complete).  
   - **Postpone End Date**: The Admin can update the `EndDate` to extend the evaluation period.  
   - **Endpoint**:  
     - `POST /api/EvaluationCycle/updatecycle` – Updates status (`Open` <-> `Closed`) and dates.

3. **Manage User Data**  
   - **Description**: The Admin can update user details such as password, group and role.  
   - **Endpoint**:  
     - `PUT /api/User/update-user/{userId}` – Updates user information.

4. **View Historical EvaluationCycles**  
   - **Description**: Admin can view details of past evaluation cycles for reference.  
   - **Endpoint**:  
     - `GET /api/EvaluationCycle/get-all-cycles` – Retrieves all evaluation cycles (including closed ones).
5. **View Evaluation Reports**
   - **Description**: Admin has access to the evaluations of any group and any cycle. The admin can see the average score of each employee.
   - **Endpoint**: 
     - `GET /api/Evaluation/evaluations-by-group/{groupId}/{cycleId}` - Retrieves evaluations by group and cycle IDs.

---

## User Functionality

1. **View Active EvaluationCycle**  
   - **Description**: A User can check if there’s an active cycle for which they need to provide input.  
   - **Endpoint**:  
     - `GET /api/EvaluationCycle/evaluationCycleExists` – Retrieves information on the currently open evaluation cycle (if any).

2. **Review Past Results**  
   - **Description**: Users can see the grades or feedback from previous (closed) cycles.  
   - **Endpoint**:  
     - `GET /api/User/evaluations-for-user/{id}` – Retrieves a list of all past cycles for this user, including their final evaluations.

3. **Perform Evaluations**  
   - **Description**: Users can navigate to a dedicated page for the currently open cycle and submit evaluations for their colleagues (and optionally their manager).  
   - **Endpoints**:  
     - `GET /api/User/user-to-evaluate/{id}` – Retrieves users that a specific user can evaluate. 
     - `POST /api/Evaluation/submit` – Submits a new evaluation based on the provided input data.

---

By adhering to these endpoints and role-based distinctions, **PeerEvalApp** maintains a clear separation of concerns:
- **Admins** administer evaluation cycles, manage user data, and oversee historical data.  
- **Users** participate by evaluating others and reviewing their own evaluation results.
