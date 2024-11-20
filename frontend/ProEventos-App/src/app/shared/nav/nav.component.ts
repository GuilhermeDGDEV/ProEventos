import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faUsers } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.scss'
})
export class NavComponent {

  public isCollapsed: boolean = true;
  public faUsers: IconDefinition = faUsers;

  constructor(private router: Router) {}

  public showMenu(): boolean {
    return this.router.url !== '/user/login' && this.router.url !== '/user/registration';
  }

}
