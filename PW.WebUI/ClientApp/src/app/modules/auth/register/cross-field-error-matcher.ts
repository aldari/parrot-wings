import { ErrorStateMatcher } from '@angular/material';
import { FormControl, FormGroupDirective, NgForm } from '@angular/forms';

export class CrossFieldErrorMatcher implements ErrorStateMatcher {
    isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
        return !!(
            ((control && control.invalid) || (form && form.hasError('passwordsDoNotMatch'))) &&
            (control.touched || (form && form.submitted))
        );
    }
}
