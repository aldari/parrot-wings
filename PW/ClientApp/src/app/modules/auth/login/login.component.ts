import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { AuthService } from '../../../auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: [ './login.component.css' ]
})
export class LoginComponent implements OnInit {
    signinForm: FormGroup;
    @ViewChild(FormGroupDirective) loginForm;
    loginError = false;

    constructor(private router: Router, private authService: AuthService, private fb: FormBuilder) {}

    ngOnInit() {
        this.signinForm = this.fb.group({
            email: [ '', [ Validators.required, Validators.email ] ],
            password: [ '', [ Validators.required ] ]
        });
        if (this.authService.isLoggedIn()) {
            this.router.navigate([ '/' ]);
        }
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
                console.log(err);
                this.loginError = true;
            }
        );
    }
}
