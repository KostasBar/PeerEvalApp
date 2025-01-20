import { Component, effect, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthServiceService } from '../../services/auth-service.service';
import { LoggedInUser } from '../../interfaces/user';
import { Mappers } from '../../utils/mappers';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  authService = inject(AuthServiceService);
  user = this.authService.user;

  logout() {
    this.authService.logout();
    location.reload()
    // console.log(this.user?.toString())
  }
}
