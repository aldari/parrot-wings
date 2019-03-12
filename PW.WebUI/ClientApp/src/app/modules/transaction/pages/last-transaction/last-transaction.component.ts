import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TransactionApiService } from '../../services/transaction-api.service';
import { UserDataSource } from '../../services/transaction-data-source.service';
import { LastTranasctionItem } from '../../models/last-transaction-item.model';
import { TransactionStorageService } from '../../services/transaction-storage.service';

@Component({
    selector: 'app-last-transaction',
    templateUrl: './last-transaction.component.html',
    styleUrls: [ './last-transaction.component.css' ]
})
export class LastTransactionComponent implements OnInit {
    dataSource: UserDataSource = new UserDataSource(this.transactionApiService);
    displayedColumns = [ 'amount', 'accountId', 'transactionDate', 'accumulateSum', 'repeatTransaction' ];

    constructor(
        private transactionApiService: TransactionApiService,
        private transactionStorageService: TransactionStorageService,
        private router: Router
    ) {}

    ngOnInit() {
        this.dataSource.load();
    }

    public repeat(transaction: LastTranasctionItem) {
        this.transactionStorageService.hasValue = true;
        this.transactionStorageService.recipientId = transaction.accountId;
        this.transactionStorageService.recipientName = transaction.accountName;
        this.transactionStorageService.amount = transaction.amount;
        this.router.navigate([ '/' ]);
    }
}
