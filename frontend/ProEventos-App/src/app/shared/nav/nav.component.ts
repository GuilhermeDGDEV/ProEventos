import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
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

  constructor(private router: Router, public accountService: AccountService) { }

  public logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/user/login');
  }

  public showMenu(): boolean {
    return this.router.url !== '/user/login';
  }

}
