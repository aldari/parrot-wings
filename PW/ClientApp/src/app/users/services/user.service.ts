import { throwError as observableThrowError, Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';

import { User } from '../model/user.model';
import { environment } from '../../../environments/environment';

@Injectable()
export class UserService {
    constructor(private http: HttpClient) {}

    private handleError(error: HttpErrorResponse) {
        return observableThrowError(error.error || 'Server error');
    }

    register(user: User): Observable<any> {
        const url = environment.apiUrl + '/api/register';
        return this.http.post(url, JSON.stringify(user), this.getRequestOptions()).pipe(catchError(this.handleError));
    }

    // returns a viable RequestOptions object to handle Json requests
    private getRequestOptions() {
        const requestOptions = {
            headers: new HttpHeaders({ 'Content-Type': 'application/json' })
        };
        return requestOptions;
    }
}
