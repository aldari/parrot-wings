import { FormBuilder, FormGroup, Validators, FormGroupDirective, ValidatorFn } from '@angular/forms';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { UserService } from './user.service';
import { User } from './user.model';
import { CustomValidators } from '../../../shared/valid';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: [ './register.component.css' ]
})
export class RegisterComponent implements OnInit {
    loaderFlag: boolean;
    registrationForm: FormGroup;
    errMsg: string;
    @ViewChild(FormGroupDirective) registerForm;

    constructor(private userService: UserService, public snackBar: MatSnackBar, private fb: FormBuilder) {}

    ngOnInit(): void {
        this.registrationForm = this.fb.group(
            {
                fullName: [ '', [ Validators.required ] ],
                email: [ '', [ Validators.required, Validators.email ] ],
                password: [ '', [ Validators.required ] ],
                passwordConfirm: [ '', [ Validators.required ] ]
            },
            { validator: CustomValidators.passwordMatchValidator }
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

        this.loaderFlag = true;
        this.errMsg = '';
        this.userService.register(user).subscribe(
            () => {
                this.loaderFlag = false;
                this.snackBar.open('User has registered');
                //this.registerForm.resetForm();
                this.registerForm.reset(this.registerForm.value);
            },
            (error) => {
                this.loaderFlag = false;
                this.errMsg = error[''];
            }
        );
    }
}
