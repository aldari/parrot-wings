import { Router } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { AuthService } from '../../../../auth.service';
import { HeaderService } from '../../header.service';
import { AccountBalanceService } from '../../account-balance.service';
import { TransactionApiService } from '../../../transaction/services/transaction-api.service';
import { tap } from 'rxjs/operators';

@Component({
    selector: 'app-admin-layout',
    templateUrl: './admin-layout.component.html',
    styleUrls: [ './admin-layout.component.css' ]
})
export class AdminLayoutComponent implements OnInit, OnDestroy {
    name: string;
    titleChangedSubscription: Subscription;

    constructor(
        private router: Router,
        private authService: AuthService,
        private headerService: HeaderService,
        public accountService: AccountBalanceService,
        private transactionApiService: TransactionApiService
    ) {}

    ngOnInit() {
        this.titleChangedSubscription = this.headerService.userTitleChanged$.subscribe(
            (name) => {
                this.name = name;
            },
            (error) => {
                console.log(error);
            }
        );
        this.headerService.getUserTitle();

        this.transactionApiService
            .getBalance()
            .pipe(tap((result: any) => this.accountService.setBalance(result.balance)))
            .subscribe();
    }

    logout() {
        if (this.authService.logout()) {
            this.router.navigate([ '/login' ]);
        }
    }

    ngOnDestroy(): void {
        if (this.titleChangedSubscription) this.titleChangedSubscription.unsubscribe();
    }
}
