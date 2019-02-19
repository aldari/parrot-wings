import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { AuthService } from '../../../../auth.service';
import { HeaderService } from '../../header.service';
import { AccountBalanceService } from '../../account-balance.service';

@Component({
    selector: 'app-admin-layout',
    templateUrl: './admin-layout.component.html',
    styleUrls: [ './admin-layout.component.css' ]
})
export class AdminLayoutComponent implements OnInit {
    name: string;

    constructor(
        private router: Router,
        private authService: AuthService,
        private headerService: HeaderService,
        public accountService: AccountBalanceService
    ) {}

    ngOnInit() {
        this.headerService.userTitleChanged.subscribe(
            (name) => {
                this.name = name;
            },
            (error) => {
                console.log(error);
            }
        );

        this.headerService.getUserTitle();
    }

    logout() {
        if (this.authService.logout()) {
            this.router.navigate([ '/login' ]);
        }
    }
}
