import { throwError as observableThrowError, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { Transaction } from '../transaction/transaction.model';

@Injectable()
export class LastTransactionService {
    private baseUrl = environment.apiUrl + '/api/transaction/last';

    constructor(private http: HttpClient) {}

    getLastTransactionsList(): any {
        return this.http.get(this.baseUrl, this.getRequestOptions()).pipe(catchError(this.handleError));
    }

    private getRequestOptions() {
        const requestOptions = {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        };
        return requestOptions;
    }

    private handleError(error: HttpErrorResponse) {
        return observableThrowError(error.error || 'Server error');
    }
}
