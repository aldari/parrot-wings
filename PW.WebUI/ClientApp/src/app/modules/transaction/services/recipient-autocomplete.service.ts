import { Injectable } from '@angular/core';
import { Observable, of, merge } from 'rxjs';
import { distinctUntilChanged, debounceTime, filter, switchMap, map, catchError } from 'rxjs/operators';
import { TransactionApiService } from './transaction-api.service';

@Injectable()
export class RecipientAutocompleteService {
    constructor(private transactionApiService: TransactionApiService) {}

    public getRecipientListAfterAutocompleteEdit(inputChanges$: Observable<any>): Observable<any> {
        const input$ = inputChanges$.pipe(debounceTime(400), distinctUntilChanged());

        const noSearchStringCase = input$.pipe(filter((val: string) => !val), map(() => []));
        const callServiceCase = input$.pipe(
            filter((val: any) => !!val && typeof val == 'string'),
            switchMap((value: string) =>
                this.transactionApiService.getRecipients(value).pipe(
                    map((v: { users: any[] }) => v.users),
                    catchError((err: any) => {
                        return of([]);
                    })
                )
            )
        );
        return merge(noSearchStringCase, callServiceCase);
    }
}
