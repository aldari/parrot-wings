import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Transaction } from '../transaction/transaction.model';
import { environment } from '../../../../../environments/environment';

@Injectable()
export class TransactionService {
    private baseUrl = environment.apiUrl + '/api/transaction/';

    constructor(private http: HttpClient) {}

    getList(amount, correspondent, from, to, sortColumn, sortOrder = 'asc', pageIndex: number, pageSize: number) {
        let params = new HttpParams();
        params = params
            .append('amount', amount)
            .append('correspondent', correspondent)
            .append('from', from)
            .append('to', to)
            .append('sortColumn', sortColumn)
            .append('sortOrder', sortOrder)
            .append('pageIndex', pageIndex.toString())
            .append('pageSize', pageSize.toString());

        return this.http
            .get(this.baseUrl, { headers: new HttpHeaders({ 'Content-Type': 'application/json' }), params })
            .pipe(
                catchError((error) => {
                    return throwError(error.message);
                })
            );
    }

    addTransaction(transaction: Transaction) {
        return this.http
            .post(environment.apiUrl + '/api/transaction/', JSON.stringify(transaction), this.getRequestOptions())
            .pipe(
                catchError((error) => {
                    return throwError(error.error.message);
                })
            );
    }

    private getRequestOptions() {
        const requestOptions = {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        };
        return requestOptions;
    }
}
