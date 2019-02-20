import { Injectable } from '@angular/core';
import * as JWT from 'jwt-decode';
import { Subject, Observable } from 'rxjs';

@Injectable()
export class HeaderService {
    private userTitleChanged = new Subject<string>();
    public userTitleChanged$ = this.userTitleChanged.asObservable();

    public getUserTitle() {
        const fullName = JWT(localStorage.getItem('auth'))['FullName'];
        this.userTitleChanged.next(`${fullName}`);
    }
}
