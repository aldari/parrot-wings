import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TransactionApiService } from '../../services/transaction-api.service';
import { UserDataSource } from '../../services/transaction-data-source.service';

@Component({
    selector: 'app-last-transaction',
    templateUrl: './last-transaction.component.html',
    styleUrls: [ './last-transaction.component.css' ]
})
export class LastTransactionComponent implements OnInit {
    dataSource: UserDataSource = new UserDataSource(this.transactionApiService);
    displayedColumns = [ 'amount', 'accountId', 'transactionDate', 'accumulateSum' ];

    constructor(private transactionApiService: TransactionApiService, private router: Router) {}

    ngOnInit() {
        this.dataSource.load();
    }
}
