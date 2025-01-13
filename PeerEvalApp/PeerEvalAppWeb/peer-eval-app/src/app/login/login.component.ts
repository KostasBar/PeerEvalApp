import { AuthService } from "../auth.service";

export class LoginComponent {
  username: string = '';
  password: string = '';

  constructor(private authService: AuthService) {}

  onSubmit() {
    this.authService.login(this.username, this.password).subscribe({
      next: (response) => {
        console.log('Login successful', response);
        // Handle redirection or token storage here
      },
      error: (error) => {
        console.error('Login failed', error);
      }
    });
  }
}
