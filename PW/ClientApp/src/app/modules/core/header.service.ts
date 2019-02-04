import { Injectable } from '@angular/core';
import * as JWT from 'jwt-decode';
import { Subject } from 'rxjs';

@Injectable()
export class HeaderService {
    public userTitleChanged = new Subject<string>();

    public getUserTitle() {
        const fullName = JWT(localStorage.getItem('auth'))['FullName'];
        this.userTitleChanged.next(`${fullName}`);
    }
}
