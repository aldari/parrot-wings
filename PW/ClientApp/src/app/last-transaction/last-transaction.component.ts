import { Component, OnInit } from '@angular/core';
import { DataSource } from '@angular/cdk/collections';
import { Observable, BehaviorSubject, of } from 'rxjs';

import { LastTransactionService } from './last-transactions.service';
import { finalize, catchError } from 'rxjs/operators';

export class UserDataSource extends DataSource<any> {
    private lastTransactionSubject = new BehaviorSubject<any[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    public loading$ = this.loadingSubject.asObservable();

    constructor(private lastTransactionService: LastTransactionService) {
        super();
    }

    connect(): Observable<any> {
        return this.lastTransactionSubject.asObservable();
    }

    disconnect() {
        this.loadingSubject.complete();
    }

    load() {
        this.loadingSubject.next(true);
        return this.lastTransactionService
            .getLastTransactionsList()
            .pipe(catchError(() => of([])), finalize(() => this.loadingSubject.next(false)))
            .subscribe((data) => this.lastTransactionSubject.next(data));
    }
}

@Component({
    selector: 'app-last-transaction',
    templateUrl: './last-transaction.component.html',
    styleUrls: [ './last-transaction.component.css' ]
})
export class LastTransactionComponent implements OnInit {
    dataSource: UserDataSource = new UserDataSource(this.lastTransactionService);
    displayedColumns = [ 'amount', 'accountId', 'transactionDate', 'accumulateSum', 'repeatTransaction' ];

    constructor(private lastTransactionService: LastTransactionService) {}

    ngOnInit() {
        this.dataSource.load();
    }
}
