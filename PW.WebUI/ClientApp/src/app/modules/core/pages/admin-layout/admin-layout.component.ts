import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { AuthService } from '../../../../auth.service';
import { AccountBalanceService } from '../../account-balance.service';
import { HeaderService } from '../../header.service';

@Component({
    selector: 'app-admin-layout',
    templateUrl: './admin-layout.component.html',
    styleUrls: [ './admin-layout.component.css' ]
})
export class AdminLayoutComponent implements OnInit, OnDestroy {
    name: string;
    balanceChangedSubscription: Subscription;
    titleChangedSubscription: Subscription;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private authService: AuthService,
        private headerService: HeaderService,
        public accountBalanceService: AccountBalanceService
    ) {}

    ngOnInit() {
        this.accountBalanceService.setBalance(this.route.snapshot.data.balanceData.balance);
        this.titleChangedSubscription = this.headerService.userTitleChanged$.subscribe(
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

    ngOnDestroy(): void {
        this.titleChangedSubscription.unsubscribe();
    }
}
