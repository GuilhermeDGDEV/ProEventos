import { Component, OnInit } from '@angular/core';
import { AccountService } from './services/account.service';
import { User } from './models/identity/User';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  
  constructor(private accountService: AccountService) { }

  public ngOnInit(): void {
    this.setCurrentUser();
  }

  public setCurrentUser(): void {
    console.log('inicio');
    let user: User | null;

    if (localStorage.getItem('user'))
      user = JSON.parse(localStorage.getItem('user') ?? '{}');
    else
      user = null;

    if (user)
      this.accountService.setCurrentUser(user);
  }

}
