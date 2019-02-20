import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DigitOnlyModule } from '@uiowa/digit-only';

import { HistoryComponent } from './pages/history/history.component';
import { LastTransactionComponent } from './pages/last-transaction/last-transaction.component';
import { TransactionComponent } from './pages/transaction/transaction.component';
import { SharedModule } from '../shared/shared.module';
import { RecipientAutocompleteService } from './services/recipient-autocomplete.service';

@NgModule({
    declarations: [ TransactionComponent, LastTransactionComponent, HistoryComponent ],
    imports: [ CommonModule, FormsModule, BrowserAnimationsModule, ReactiveFormsModule, SharedModule, DigitOnlyModule ],
    providers: [ RecipientAutocompleteService ]
})
export class TransactionModule {}
