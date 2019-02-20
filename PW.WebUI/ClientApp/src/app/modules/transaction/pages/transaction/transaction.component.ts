import { Observable } from 'rxjs';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormGroupDirective } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { ButtonOpts } from 'mat-progress-buttons';

import { Transaction } from './transaction.model';
import { forceOptionValidator } from './forceOptionValidator';
import { AccountBalanceService } from '../../../core/account-balance.service';
import { RecipientAutocompleteService } from '../../services/recipient-autocomplete.service';
import { TransactionApiService } from '../../services/transaction-api.service';

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
        private recipientAutocompleteService: RecipientAutocompleteService,
        private transactionApiService: TransactionApiService,
        private fb: FormBuilder,
        public snackBar: MatSnackBar,
        public accountBalanceService: AccountBalanceService
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

        const searchString$ = this.transactionForm.get('recipient').valueChanges;
        this.filteredData$ = this.recipientAutocompleteService.getRecipientListAfterAutocompleteEdit(searchString$);
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

        this.transactionApiService.addTransaction(transaction).subscribe(
            () => {
                this.myForm.resetForm();
                this.accountBalanceService.reduce(transaction.amount);
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
