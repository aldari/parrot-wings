import { AuthService } from '../auth.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ValidateService } from '../shared/validate.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: [ './login.component.css' ]
})
export class LoginComponent implements OnInit {
    loginError = false;

    constructor(private router: Router, private authService: AuthService, private validateService: ValidateService) {}

    ngOnInit() {
        if (this.authService.isLoggedIn()) {
            this.router.navigate([ '/' ]);
        }
    }

    onSubmit(form: NgForm) {
        if (form.valid) {
            const email = form.value.email;
            const password = form.value.password;
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
        } else {
            this.validateService.validateAllFields(form.form);
        }
    }
}
