import { Component } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { RouterOutlet } from '@angular/router';
import { MenuComponent } from "../menu/menu.component";

@Component({
  selector: 'app-working-desk',
  standalone: true,
  imports: [NavbarComponent, RouterOutlet, MenuComponent],
  templateUrl: './working-desk.component.html',
  styleUrl: './working-desk.component.css'
})
export class WorkingDeskComponent {
  
  
}
