import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, of, empty } from 'rxjs';
import { mergeMap, catchError, tap } from 'rxjs/operators';

import { TransactionApiService } from './transaction-api.service';

@Injectable({
    providedIn: 'root'
})
export class BalanceResolverService implements Resolve<any> {
    constructor(private transactionApiService: TransactionApiService) {}

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Observable<never> {
        return this.transactionApiService.getBalance().pipe(
            catchError((error) => {
                return empty();
            }),
            mergeMap((something) => {
                if (something) {
                    return of(something);
                } else {
                    return empty();
                }
            })
        );
    }
}
