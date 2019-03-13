import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AuthService } from '../../../auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: [ './login.component.css' ]
})
export class LoginComponent implements OnInit, OnDestroy {
    signinForm: FormGroup;
    @ViewChild(FormGroupDirective) loginForm;
    loginError = false;
    apiSubscription: Subscription;

    constructor(private router: Router, private authService: AuthService, private fb: FormBuilder) {}

    ngOnInit() {
        this.signinForm = this.fb.group({
            email: [ '', [ Validators.required, Validators.email ] ],
            password: [ '', [ Validators.required ] ]
        });
        // if (this.authService.isLoggedIn()) {
        //     this.router.navigate([ '/' ]);
        // }
    }

    login() {
        if (!this.signinForm.valid) {
            return;
        }

        const email = this.signinForm.value.email;
        const password = this.signinForm.value.password;
        this.authService.login(email, password).subscribe(
            () => {
                this.loginError = false;
                this.router.navigate([ '/' ]);
            },
            (err) => {
                this.loginError = true;
                console.log(err);
            }
        );
    }

    ngOnDestroy(): void {
        if (this.apiSubscription) this.apiSubscription.unsubscribe();
    }
}
