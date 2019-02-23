import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TransactionApiService } from '../../services/transaction-api.service';
import { UserDataSource } from '../../services/transaction-data-source.service';
import { LastTranasctionItem } from '../../models/last-transaction-item.model';
import { TransactionStorageService } from '../../services/transaction-storage.service';
import { Transaction } from '../transaction/transaction.model';

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
        const pass = new Transaction();
        pass.amount = transaction.amount;
        pass.recipient = transaction.accountId;
        this.transactionStorageService.transaction = pass;
        this.router.navigate([ '/' ]);
    }
}
