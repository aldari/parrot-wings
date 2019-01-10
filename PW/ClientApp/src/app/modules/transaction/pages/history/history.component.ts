import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

import { merge, Observable, EMPTY } from 'rxjs';
import { tap, startWith, debounceTime, distinctUntilChanged, switchMap, catchError } from 'rxjs/operators';

import { TransactionDataSource } from './transactions-datasource.service';
import { TransactionService } from './transactions.service';
import { RecipientService } from '../transaction/recipient.service';

@Component({
    selector: 'app-history',
    templateUrl: './history.component.html',
    styleUrls: [ './history.component.css' ]
})
export class HistoryComponent implements OnInit, AfterViewInit {
    dataSource: TransactionDataSource;
    displayedColumns = [ 'amount', 'credit', 'correspondent', 'transactionDate' ];
    filterForm: FormGroup;
    filteredData$: Observable<any>;

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

        const correspondentChanges$ = this.filterForm.get('filterCorrespondent').valueChanges;
        this.filteredData$ = correspondentChanges$.pipe(
            startWith(''),
            debounceTime(400),
            distinctUntilChanged(),
            switchMap((value: string) =>
                this.recipientService.getList(value).pipe(
                    catchError(() => {
                        return EMPTY;
                    })
                )
            )
        );
    }

    ngAfterViewInit() {
        // reset the paginator after sorting
        this.sort.sortChange.pipe(tap(() => (this.paginator.pageIndex = 0)));
        this.dataSource.counter$
            .pipe(
                tap((count) => {
                    this.paginator.length = count;
                })
            )
            .subscribe();

        merge(this.sort.sortChange, this.paginator.page).pipe(tap(() => this.loadLessonsPage())).subscribe();
    }

    loadLessonsPage() {
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
        this.loadLessonsPage();
    }

    public filter() {
        this.loadLessonsPage();
    }

    displayFn(recipient?: any): string | undefined {
        return recipient ? recipient.name : undefined;
    }
}
