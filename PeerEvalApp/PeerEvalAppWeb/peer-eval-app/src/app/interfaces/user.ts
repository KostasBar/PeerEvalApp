export interface UserLogin {
    email: string,
    password: string
}

export interface LoggedInUser{
    id: string,
    email: string,
    role: string
}

export interface LoginResponse {
    token: {
      token: string;
    };
  }