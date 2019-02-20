import { throwError, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Transaction } from '../pages/transaction/transaction.model';
import { environment } from '../../../../environments/environment';
import { LastTranasctionItem } from '../models/last-transaction-item.model';

@Injectable()
export class TransactionApiService {
    private baseUrl = environment.apiUrl + '/api/';

    constructor(private http: HttpClient) {}

    getRecipients(filter: string) {
        return this.http.get(this.baseUrl + `recipient/${filter}/`, this.getRequestOptions()).pipe(
            catchError((error) => {
                return throwError('Can not proceed transaction');
            })
        );
    }

    getTransactionHistoryItems(
        amount,
        correspondent,
        from,
        to,
        sortColumn,
        sortOrder = 'asc',
        pageIndex: number,
        pageSize: number
    ) {
        let params = new HttpParams();
        if (amount) {
            params = params.append('amount', amount);
        }
        if (correspondent) {
            params = params.append('correspondent', correspondent);
        }
        if (from) {
            params = params.append('from', from);
        }
        if (to) {
            params = params.append('to', to);
        }
        if (sortColumn) {
            params = params.append('sortColumn', sortColumn);
        }
        if (sortOrder) {
            params = params.append('sortOrder', sortOrder);
        }
        if (pageIndex) {
            params = params.append('pageIndex', pageIndex.toString());
        }
        if (pageSize) {
            params = params.append('pageSize', pageSize.toString());
        }

        return this.http
            .get(this.baseUrl + 'transaction/', {
                headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
                params
            })
            .pipe(
                catchError((error) => {
                    return throwError(error.message);
                })
            );
    }

    addTransaction(transaction: Transaction) {
        return this.http
            .post(this.baseUrl + 'transaction/', JSON.stringify(transaction), this.getRequestOptions())
            .pipe(
                catchError((error) => {
                    return throwError(error.error.message);
                })
            );
    }

    getLastTransactions(): Observable<LastTranasctionItem[]> {
        return this.http.get<LastTranasctionItem[]>(this.baseUrl + 'transaction/last', this.getRequestOptions()).pipe(
            catchError((error) => {
                return throwError(error.error || 'Server error');
            })
        );
    }

    getBalance(): Observable<number> {
        return this.http.get<number>(this.baseUrl + 'register/balance', this.getRequestOptions()).pipe(
            catchError((error) => {
                return throwError(error.error || 'Server error');
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
