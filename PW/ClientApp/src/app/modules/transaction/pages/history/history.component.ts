import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

import { Observable, of, merge, Subscription } from 'rxjs';
import { tap, debounceTime, distinctUntilChanged, switchMap, catchError, map, filter } from 'rxjs/operators';

import { TransactionDataSource } from './transactions-datasource.service';
import { TransactionService } from './transactions.service';
import { RecipientService } from '../transaction/recipient.service';

@Component({
    selector: 'app-history',
    templateUrl: './history.component.html',
    styleUrls: [ './history.component.css' ]
})
export class HistoryComponent implements OnInit, OnDestroy, AfterViewInit {
    dataSource: TransactionDataSource;
    displayedColumns = [ 'amount', 'credit', 'correspondent', 'transactionDate' ];
    filterForm: FormGroup;
    filteredData$: Observable<any>;
    loadTransactionsSubscription: Subscription;
    counterSubscription: Subscription;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    constructor(
        private transactionService: TransactionService,
        private recipientService: RecipientService,
        private fb: FormBuilder
    ) {}

    ngOnInit() {
        this.dataSource = new TransactionDataSource(this.transactionService);
        this.dataSource.loadLessons(null, '', null, null, 'transactionDate', 'asc', 0, 3);

        this.filterForm = this.fb.group({
            filterAmount: [ null, [ Validators.min(1) ] ],
            filterCorrespondent: [ null ],
            filterDateFrom: [ null ],
            filterDateTo: [ null ]
        });

        const correspondentChanges$ = this.filterForm
            .get('filterCorrespondent')
            .valueChanges.pipe(debounceTime(400), distinctUntilChanged());

        const noSearchStringCase = correspondentChanges$.pipe(filter((val: string) => val.length === 0), map(() => []));
        const callServiceCase = correspondentChanges$.pipe(
            filter((val: string) => val.length > 0),
            switchMap((value: string) =>
                this.recipientService.getList(value).pipe(
                    map((v: { users: any[] }) => v.users),
                    catchError((err: any) => {
                        return of([]);
                    })
                )
            )
        );
        this.filteredData$ = merge(noSearchStringCase, callServiceCase);
    }

    ngAfterViewInit() {
        // reset the paginator after sorting
        this.sort.sortChange.pipe(tap(() => (this.paginator.pageIndex = 0)));
        this.counterSubscription = this.dataSource.counter$
            .pipe(
                tap((count) => {
                    this.paginator.length = count;
                })
            )
            .subscribe();

        this.loadTransactionsSubscription = merge(this.sort.sortChange, this.paginator.page)
            .pipe(tap(() => this.loadTransactionsPage()))
            .subscribe();
    }

    loadTransactionsPage() {
        this.dataSource.loadLessons(
            this.filterForm.value.filterAmount,
            this.filterForm.value.filterCorrespondent,
            this.filterForm.value.filterDateFrom,
            this.filterForm.value.filterDateTo,
            this.sort.active,
            this.sort.direction,
            this.paginator.pageIndex,
            this.paginator.pageSize
        );
    }

    public clearFilterForm() {
        this.filterForm.reset();
        this.loadTransactionsPage();
    }

    public filter() {
        this.paginator.pageIndex = 0;
        this.loadTransactionsPage();
    }

    displayFn(recipient?: any): string | undefined {
        return recipient ? recipient.name : undefined;
    }

    ngOnDestroy(): void {
        this.loadTransactionsSubscription.unsubscribe();
        this.counterSubscription.unsubscribe();
    }
}
