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
    if (role === 'User') {
      this.menu = [
        { text: "Evaluation Cycles User", routerLink: "user-evaluation-cycles" },
        { text: "Evaluations", routerLink: "evaluations-desk" },
        { text: "Evaluation Cycles", routerLink: "evaluation-cycles" },
        { text: "Reports", routerLink: "evaluations-report"},
        {text: "Users - Groups", routerLink: "handle-users-groups"}
      ];
    } else {
      this.menu = [
        { text: "Evaluation Cycles", routerLink: "evaluation-cycles" },
        { text: "Reports", routerLink: "evaluations-report"},
        {text: "Users - Groups", routerLink: "handle-users-groups"}
      ];
    }
  }
}
