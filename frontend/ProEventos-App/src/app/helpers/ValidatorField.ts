import { AbstractControl, FormControl, FormGroup } from '@angular/forms';

export class ValidatorField {
    static mustMatch(controlName: string, matchingControlName: string): any {
        return (group: AbstractControl) => {
            const formGroup = group as FormGroup;
            const control = formGroup.controls[controlName];
            const matchingControl = formGroup.controls[matchingControlName];

            if (matchingControl.errors && !matchingControl.errors['mustMatch']) {
                return null;
            }
            
            if ((control.value ?? '') !== (matchingControl.value ?? '')) {
                matchingControl.setErrors({ mustMatch: true });
            } else {
                matchingControl.setErrors(null);
            }

            return null;
        };
    }

    static cssValidator(campoForm: FormControl): object {
        return { 'is-invalid': campoForm.errors && campoForm.touched };
    }
}