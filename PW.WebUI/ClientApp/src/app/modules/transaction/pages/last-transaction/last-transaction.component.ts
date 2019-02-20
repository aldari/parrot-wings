import { Component, OnInit } from '@angular/core';
import { TransactionApiService } from '../../services/transaction-api.service';
import { UserDataSource } from '../../services/transaction-data-source.service';

@Component({
    selector: 'app-last-transaction',
    templateUrl: './last-transaction.component.html',
    styleUrls: [ './last-transaction.component.css' ]
})
export class LastTransactionComponent implements OnInit {
    dataSource: UserDataSource = new UserDataSource(this.transactionApiService);
    displayedColumns = [ 'amount', 'accountId', 'transactionDate', 'accumulateSum', 'repeatTransaction' ];

    constructor(private transactionApiService: TransactionApiService) {}

    ngOnInit() {
        this.dataSource.load();
    }
}
