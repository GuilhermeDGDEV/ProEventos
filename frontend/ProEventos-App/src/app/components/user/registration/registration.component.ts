import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder, AbstractControlOptions, AbstractControl, FormControl } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {
  form: FormGroup = new FormGroup({});

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
      primeiroNome: ['', Validators.required],
      ultimoNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      usuario: ['', Validators.required],
      senha: ['', [Validators.required, Validators.minLength(6)]],
      senhaConfirm: ['', Validators.required],
      termosConfirm: ['', Validators.requiredTrue],
    }, formOptions);
  }

  public isInvalid(formControl: FormControl | AbstractControl): boolean {
    return formControl?.errors && formControl?.touched;
  }
}
