import { throwError as observableThrowError, Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';

import { environment } from '../../../../../environments/environment';

@Injectable()
export class RecipientService {
    private baseUrl = environment.apiUrl + '/api/recipient/';
    constructor(private http: HttpClient) {}

    getList(filter: string) {
        return this.http.get(this.baseUrl + `${filter}/`, this.getRequestOptions()).pipe(catchError(this.handleError));
    }

    private handleError(error: HttpErrorResponse) {
        return throwError('Can not proceed transaction');
    }

    // returns a viable RequestOptions object to handle Json requests
    private getRequestOptions() {
        const requestOptions = {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        };
        return requestOptions;
    }
}
