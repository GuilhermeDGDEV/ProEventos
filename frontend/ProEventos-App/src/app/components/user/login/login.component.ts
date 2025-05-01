import { Component } from '@angular/core';
import { UserLogin } from '@app/models/identity/UserLogin';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { faUsers } from '@fortawesome/free-solid-svg-icons';
import { AccountService } from '@app/services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  public model: UserLogin = {} as UserLogin;

  public faUsers: IconDefinition = faUsers;

  constructor(
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  public login(): void {
    this.spinner.show();
    this.accountService.login(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/dashboard');
      },
      error: err => {
        if (err.status == 401) {
          this.toastr.error('Usuário ou senha inválido');
        } else {
          console.log(err);
        }
      }
    }).add(() => this.spinner.hide());
  }

}
