import { Component, OnInit } from '@angular/core';
import { AbstractControl, AbstractControlOptions, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  showSpinner: boolean = false;

  get f(): any {
    return this.form.controls;
  }

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void {
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('senha', 'senhaConfirm')
    };

    this.form = this.fb.group({
      titulo: ['', Validators.required],
      primeiroNome: ['', Validators.required],
      ultimoNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', Validators.required],
      funcao: ['', Validators.required],
      descricao: ['', Validators.required],
      senha: [''],
      senhaConfirm: [''],
    }, formOptions);
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  public isInvalid(formControl: FormControl | AbstractControl): boolean {
    return formControl?.errors && formControl?.touched;
  }

  public save(): void {
    this.showSpinner = !this.showSpinner;
  }

}
