import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { WorkingDeskComponent } from "./components/working-desk/working-desk.component";
import { Subscription } from 'rxjs';
import { AuthServiceService } from './services/auth-service.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoginComponent, WorkingDeskComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  access_token: string | null = null;
  private tokenSubscription!: Subscription;
  router = inject(Router);

  constructor(private authService: AuthServiceService) {}

  ngOnInit(): void {
    // Subscribe to access token changes
    this.tokenSubscription = this.authService.accessToken$.subscribe((token) => {
      this.access_token = token;
      this.refreshComponentIfNeeded();
    });
  }

  ngOnDestroy(): void {
    // Unsubscribe to avoid memory leaks
    if (this.tokenSubscription) {
      this.tokenSubscription.unsubscribe();
    }
  }

  private refreshComponentIfNeeded(): void {
    if (!this.access_token) {
      // Redirect to login or handle component refresh logic
      this.router.navigate([''])
      console.log('Token missing or expired. Redirecting to login...');
    } else {
      console.log('Token exists:', this.access_token);
    }
  }
}
