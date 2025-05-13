import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { UserUpdate } from '@app/models/identity/UserUpdate';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.scss'
})
export class PerfilComponent implements OnInit {

  public userUpdate: UserUpdate = {} as UserUpdate;

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
      userName: [''],
      titulo: ['NaoInformado', [Validators.required]],
      primeiroNome: ['', [Validators.required]],
      ultimoNome: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
      funcao: ['NaoInformado', [Validators.required]],
      descricao: ['', [Validators.required]],
      password: [''],
      confirmPassword: ['']
    }, formOptions);
  }

  public ngOnInit(): void {
    this.carregarUsuario();
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService.getUser().subscribe({
      next: (userRetorno: UserUpdate) => {
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
      },
      error: console.error
    }).add(() => this.spinner.hide());
  }

  public cssValidator(campoForm: FormControl): object {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }

  public onSubmit(): void {
    this.atualizarUsuario();
  }

  public atualizarUsuario(): void {
    this.userUpdate = { ...this.form.value };
    this.spinner.show();
    this.accountService.updateUser(this.userUpdate).subscribe({
      next: () => this.toastr.success('Usuário atualizado!', 'Sucesso'),
      error: (err) => {
        this.toastr.error(err.error);
        console.error(err);
      }
    }).add(() => this.spinner.hide());
  }

  public voltar(): void {
    history.back();
  }

}
