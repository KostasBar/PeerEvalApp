import { Component, inject } from '@angular/core';
import { MenuEntry } from '../../interfaces/menu-entry';
import { AuthServiceService } from '../../services/auth-service.service';
import { LoggedInUser } from '../../interfaces/user';
import { Mappers } from '../../utils/mappers';
import { jwtDecode } from 'jwt-decode';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [RouterLink, RouterLinkActive ],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent {
  menu: MenuEntry[] = [];
  authService = inject(AuthServiceService);

  ngOnInit():void{
    const access_token = localStorage.getItem('access_token');
    if (access_token){
      const user = Mappers.mapDecodedTokenToLoggedInUser(jwtDecode(access_token));
      this.updateMenuBasedOnRole(user.role);
    }
  }

  /**
   * Sets the menu based on the user's role (user or admin)
   * @param role - The user's role
   */
  private updateMenuBasedOnRole(role: string): void {
    if (role === 'user') {
      this.menu = [
        { text: "Evaluation Cycles", routerLink: "user-evaluation-cycles" },
        { text: "Evaluations", routerLink: "users-to-evaluate" }
      ];
    } else {
      this.menu = [
        { text: "Evaluation Cycles", routerLink: "user-evaluation-cycles" },
        { text: "Handle Users", routerLink: "users-to-evaluate" }
      ];
    }
  }
}
