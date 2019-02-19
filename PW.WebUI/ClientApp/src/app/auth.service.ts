import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { map, catchError } from 'rxjs/operators';

import { environment } from '../environments/environment';
import { Observable, throwError } from 'rxjs';

@Injectable()
export class AuthService {
    authKey = 'auth';

    constructor(private http: HttpClient, private router: Router) {}

    login(email: string, password: string): any {
        const url = environment.apiUrl + '/api/auth/login';
        const data = {
            email: email,
            password: password,
            grant_type: 'password'
        };

        return this.http
            .post(url, JSON.stringify(data), {
                headers: new HttpHeaders({
                    'Content-Type': 'application/json'
                })
            })
            .pipe(
                map((response) => {
                    const auth = response;
                    this.setAuth(auth);
                    return auth;
                }),
                catchError((error) => this._handleError(error))
            );
    }

    logout(): boolean {
        this.setAuth(null);
        this.router.navigate([ '/login' ]);
        return true;
    }

    // Converts a Json object to urlencoded format
    toUrlEncodedString(data: any) {
        let body = '';
        // tslint:disable-next-line:forin
        for (const key in data) {
            if (body.length) {
                body += '&';
            }
            body += key + '=';
            body += encodeURIComponent(data[key]);
        }
        return body;
    }

    // Persist auth into localStorage or removes it if a NULL argument is given
    setAuth(auth: any): boolean {
        if (auth) {
            localStorage.setItem(this.authKey, JSON.stringify(auth));
        } else {
            localStorage.removeItem(this.authKey);
        }
        return true;
    }

    // Retrieves the auth JSON object (or NULL if none)
    getAuth(): any {
        const i = localStorage.getItem(this.authKey);
        if (i) {
            return JSON.parse(i);
        } else {
            return null;
        }
    }

    // Returns TRUE if the user is logged in, FALSE otherwise.
    isLoggedIn(): boolean {
        return localStorage.getItem(this.authKey) != null;
    }

    private _handleError(err: HttpErrorResponse | any): Observable<any> {
        const errorMsg = err.message || 'Error: Unable to complete request.';
        return throwError(errorMsg);
    }
}
