import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';

import { TransactionRow } from './TransactionRow';
import { TransactionService } from './transactions.service';
import { FilteredResponse } from './filtered-response.model';
import { Moment } from 'moment';

export class TransactionDataSource implements DataSource<TransactionRow> {
    private transactionsSubject = new BehaviorSubject<TransactionRow[]>([]);
    private countSubject = new BehaviorSubject<number>(0);
    private loadingSubject = new BehaviorSubject<boolean>(false);

    public counter$ = this.countSubject.asObservable();
    public loading$ = this.loadingSubject.asObservable();

    constructor(private transactionService: TransactionService) {}

    connect(collectionViewer: CollectionViewer): Observable<TransactionRow[]> {
        return this.transactionsSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.transactionsSubject.complete();
        this.loadingSubject.complete();
    }

    loadLessons(
        amount = 0,
        correspondent = '',
        from: Moment,
        to: Moment,
        sortColumn = '',
        sortDirection = 'asc',
        pageIndex = 0,
        pageSize = 3
    ) {
        this.loadingSubject.next(true);

        this.transactionService
            .getList(
                amount,
                correspondent ? correspondent['id'] : null,
                from ? from.toISOString() : null,
                to ? to.toISOString() : null,
                sortColumn,
                sortDirection,
                pageIndex,
                pageSize
            )
            .pipe(catchError(() => of([])), finalize(() => this.loadingSubject.next(false)))
            .subscribe((response: FilteredResponse) => {
                this.transactionsSubject.next(response.transactions);
                this.countSubject.next(response.count);
            });
    }
}
