import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AccountBalanceService {
    private internalBalance = 500;
    private balance: BehaviorSubject<number> = new BehaviorSubject<number>(this.internalBalance);

    public balance$: Observable<number> = this.balance.asObservable();

    constructor() {}

    private setBalance(newValue: number) {
        this.internalBalance = newValue;
        this.balance.next(this.internalBalance);
    }

    public reduce(byValue: number) {
        this.internalBalance -= byValue;
        this.balance.next(this.internalBalance);
    }
}
