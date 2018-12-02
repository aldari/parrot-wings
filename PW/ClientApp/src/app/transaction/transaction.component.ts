import { Observable, EMPTY } from 'rxjs';
import { switchMap, debounceTime, distinctUntilChanged, startWith, catchError } from 'rxjs/operators';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { ButtonOpts } from 'mat-progress-buttons';

import { Transaction } from './transaction.model';
import { RecipientService } from './recipient.service';
import { TransactionService } from '../history/transactions.service';
import { forceOptionValidator } from './forceOptionValidator';

@Component({
    selector: 'app-transaction',
    templateUrl: './transaction.component.html',
    styleUrls: [ './transaction.component.css' ]
})
export class TransactionComponent implements OnInit {
    filteredData$: Observable<any>;
    transactionForm: FormGroup;
    @ViewChild(FormGroupDirective) myForm;
    errMsg: string;

    constructor(
        private recipientService: RecipientService,
        private transactionService: TransactionService,
        private fb: FormBuilder,
        public snackBar: MatSnackBar
    ) {}

    spinnerButtonOptions: ButtonOpts = {
        active: false,
        text: 'Add Transacton',
        spinnerSize: 18,
        raised: true,
        buttonColor: 'primary',
        spinnerColor: 'accent',
        fullWidth: false,
        disabled: false
    };

    ngOnInit() {
        this.transactionForm = this.fb.group({
            amount: [ '', [ Validators.required, Validators.min(1) ] ],
            recipient: [ '', [ forceOptionValidator() ] ]
        });

        this.errMsg = '';
        this.filteredData$ = this.transactionForm.get('recipient').valueChanges.pipe(
            startWith('*'),
            // filter((val: string) => val.length > 0),
            debounceTime(400),
            distinctUntilChanged(),
            switchMap((value: string) =>
                this.recipientService.getList(value).pipe(
                    catchError((err: any) => {
                        this.errMsg = err.message;
                        return EMPTY;
                    })
                )
            )
        );
    }

    save() {
        if (!this.transactionForm.valid) {
            return;
        }

        this.spinnerButtonOptions.active = true;
        this.spinnerButtonOptions.text = 'Loading ...';
        this.errMsg = '';

        const transaction = new Transaction();
        transaction.amount = this.transactionForm.value['amount'];
        transaction.recipient = this.transactionForm.value['recipient']['id'];

        this.transactionService.addTransaction(transaction).subscribe(
            () => {
                this.myForm.resetForm();
                this.snackBar.open('Transaction successfuly sent');
            },
            (error) => {
                this.errMsg = error;
                this.spinnerButtonOptions.active = false;
                this.spinnerButtonOptions.text = 'Add Transacton';
            },
            () => {
                this.spinnerButtonOptions.active = false;
                this.spinnerButtonOptions.text = 'Add Transacton';
            }
        );
    }

    displayFn(recipient?: any): string | undefined {
        return recipient ? recipient.name : undefined;
    }
}
