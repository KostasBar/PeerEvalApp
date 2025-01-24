export interface UserLogin {
    email: string,
    password: string
}

export interface LoggedInUser{
    id: string,
    email: string,
    firstname: string,
    lastname: string,
    role: string
}

export interface LoginResponse {
    token: {
      token: string;
    };
  }

export interface UsersToEvaluate{
  id: number,
  firstname: string,
  lastname: string,
  email: string
}

export interface SubmitUser{
  firstName: string,
  lastName: string,
  email: string,
  password: string,
  groupId: number,
}