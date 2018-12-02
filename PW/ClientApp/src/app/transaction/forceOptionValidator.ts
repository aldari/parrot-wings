import { AbstractControl, ValidatorFn } from '@angular/forms';

export function forceOptionValidator(): ValidatorFn {
    return (control: AbstractControl): any => {
        const selection = control.value;
        if (typeof selection === 'string') {
            return { forceOptionValidator: true };
        }
        return null;
    };
}
