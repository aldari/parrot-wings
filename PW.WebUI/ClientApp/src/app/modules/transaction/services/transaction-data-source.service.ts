import { DataSource } from '@angular/cdk/collections';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { finalize, catchError } from 'rxjs/operators';

import { TransactionApiService } from './transaction-api.service';

export class UserDataSource extends DataSource<any> {
    private lastTransactionSubject = new BehaviorSubject<any[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    public loading$ = this.loadingSubject.asObservable();

    constructor(private transactionApiService: TransactionApiService) {
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
        return this.transactionApiService
            .getLastTransactions()
            .pipe(catchError(() => of([])), finalize(() => this.loadingSubject.next(false)))
            .subscribe((data) => this.lastTransactionSubject.next(data));
    }
}
