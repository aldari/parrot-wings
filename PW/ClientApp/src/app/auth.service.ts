import { Subject, Observable } from 'rxjs';
import { Injectable, EventEmitter, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { DOCUMENT } from '@angular/platform-browser';
import { map } from 'rxjs/operators';

import { environment } from '../environments/environment';

@Injectable()
export class AuthService {
    authKey = 'auth';

    constructor(private http: HttpClient, @Inject(DOCUMENT) private document, private router: Router) {}

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
                    'Content-Type': 'application/json',
                    Tenant: this.document.location.hostname
                })
            })
            .pipe(
                map((response) => {
                    const auth = response;
                    this.setAuth(auth);
                    return auth;
                })
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

    // public refreshToken(): Observable<string>  {
    //     return this.login(this.username, this.password)
    //     .map((data) => {
    //         return data['auth_token'];
    //     });
    // }
}
