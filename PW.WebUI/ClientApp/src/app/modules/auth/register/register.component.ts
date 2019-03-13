import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

import { UserService } from './user.service';
import { User } from './user.model';
import { CrossFieldErrorMatcher } from './cross-field-error-matcher';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: [ './register.component.css' ]
})
export class RegisterComponent implements OnInit, OnDestroy {
    loaderFlag: boolean;
    registrationForm: FormGroup;
    errMsg: string;
    @ViewChild(FormGroupDirective) registerForm;
    apiSubscription: Subscription;
    public errorMatcher = new CrossFieldErrorMatcher();

    constructor(private userService: UserService, public snackBar: MatSnackBar, private fb: FormBuilder) {}

    ngOnInit(): void {
        this.registrationForm = this.fb.group(
            {
                fullName: [ '', [ Validators.required ] ],
                email: [ '', [ Validators.required, Validators.email ] ],
                password: [ '', [ Validators.required, Validators.minLength(6) ] ],
                confirmPassword: [ '', [ Validators.required ] ]
            },
            { validator: this.passwordValidator }
        );
    }

    register() {
        if (!this.registrationForm.valid) {
            return;
        }

        const user = new User();
        user.fullName = this.registrationForm.value.fullName;
        user.email = this.registrationForm.value.email;
        user.password = this.registrationForm.value.password;
        user.confirmPassword = this.registrationForm.value.confirmPassword;

        this.loaderFlag = true;
        this.errMsg = '';
        this.apiSubscription = this.userService.register(user).subscribe(
            () => {
                this.loaderFlag = false;
                this.snackBar.open('User has registered');
                this.registerForm.reset(this.registerForm.value);
            },
            (error) => {
                this.loaderFlag = false;
                this.errMsg = error[''];
            }
        );
    }

    passwordValidator(form: FormGroup) {
        const condition = form.get('password').value !== form.get('confirmPassword').value;

        return condition ? { passwordsDoNotMatch: true } : null;
    }

    ngOnDestroy() {
        if (this.apiSubscription) this.apiSubscription.unsubscribe();
    }
}
