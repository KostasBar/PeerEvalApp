# PeerEvalApp - Role-based Overview & Endpoints

Below is a concise description of the **Admin** and **User** functionalities in **PeerEvalApp**, along with some suggested endpoints that implement these capabilities.

---

## Admin Functionality

1. **Create & Open EvaluationCycles**  
   - **Description**: An Admin can create new evaluation cycles and set them to `Open`.  
   - **Endpoint**:  
     - `POST /api/EvaluationCycles` – Creates a new evaluation cycle (body contains title, start/end dates, status, etc.).

2. **Update EvaluationCycles**  
   - **Change Status**: An Admin can switch the status between `Open` and `Closed` (e.g., when the cycle is complete).  
   - **Postpone End Date**: The Admin can update the `EndDate` to extend the evaluation period.  
   - **Endpoints**:  
     - `PUT /api/EvaluationCycles/{id}` – Updates status (`Open` <-> `Closed`) and dates.

3. **Manage User Data**  
   - **Description**: The Admin can update user details such as first name, last name, email, group/role, manager, etc.  
   - **Endpoint**:  
     - `PUT /api/Users/{id}` – Updates user information.

4. **View Historical EvaluationCycles**  
   - **Description**: Admin can list or view details of past evaluation cycles for reference.  
   - **Endpoints**:  
     - `GET /api/EvaluationCycles` – Retrieves all evaluation cycles (including closed ones).  
     - `GET /api/EvaluationCycles/{id}` – Retrieves specific details about a single cycle.

---

## User Functionality

1. **View Active EvaluationCycle**  
   - **Description**: A User can check if there’s an active cycle for which they need to provide input.  
   - **Endpoint**:  
     - `GET /api/EvaluationCycles/active` – Retrieves information on the currently open evaluation cycle (if any).

2. **Review Past Results**  
   - **Description**: Users can see the grades or feedback from previous (closed) cycles.  
   - **Endpoint**:  
     - `GET /api/EvaluationCycles/past` – Retrieves a list of all past cycles for this user, including their final evaluations.

3. **Perform Evaluations**  
   - **Description**: Users can navigate to a dedicated page for the currently open cycle and submit evaluations for their colleagues (and optionally their manager).  
   - **Endpoints**:  
     - `POST /api/Evaluations` – Creates a new evaluation record (body includes evaluator, evaluatee, answers, etc.).  
     - `GET /api/Evaluations/{userId}` – Retrieves all evaluations for a particular user, or you could filter further.

---

By adhering to these endpoints and role-based distinctions, **PeerEvalApp** maintains a clear separation of concerns:
- **Admins** administer evaluation cycles, manage user data, and oversee historical data.  
- **Users** participate by evaluating others and reviewing their own evaluation results.
