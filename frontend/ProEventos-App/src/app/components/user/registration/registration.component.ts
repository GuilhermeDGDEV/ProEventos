import { Component } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.scss'
})
export class RegistrationComponent {

  public user = {} as User;

  public form: FormGroup;

  get f(): any {
    return this.form.controls;
  }

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.mustMatch('password', 'confirmPassword')
    }

    this.form = this.fb.group({
      primeiroNome: ['', [Validators.required]],
      ultimoNome: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(4)]],
      confirmPassword: ['', [Validators.required]],
      termosDeUso: [false, Validators.requiredTrue]
    }, formOptions);
  }

  public cssValidator(campoForm: FormControl): object {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }

  public register(): void {
    this.spinner.show();
    this.user = { ...this.form.value };
    this.accountService.register(this.user).subscribe({
      next: () => this.router.navigateByUrl('/dashboard'),
      error: err => {
        if (err.status == 401) {
          this.toastr.error('Erro ao registrar usuário');
        } else {
          console.log(err);
        }
      }
    }).add(() => this.spinner.hide());
  }

}
