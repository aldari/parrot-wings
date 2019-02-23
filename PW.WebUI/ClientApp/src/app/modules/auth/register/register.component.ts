import { FormBuilder, FormGroup, Validators, FormGroupDirective } from '@angular/forms';
import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { UserService } from './user.service';
import { User } from './user.model';
import { CustomValidators } from '../../../shared/valid';
import { Subscription } from 'rxjs';

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

    ngOnDestroy() {
      if (this.apiSubscription) this.apiSubscription.unsubscribe();
    }
}
