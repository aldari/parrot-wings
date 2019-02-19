import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HistoryComponent } from './pages/history/history.component';
import { LastTransactionComponent } from './pages/last-transaction/last-transaction.component';
import { TransactionComponent } from './pages/transaction/transaction.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
    declarations: [ TransactionComponent, LastTransactionComponent, HistoryComponent ],
    imports: [ CommonModule, FormsModule, BrowserAnimationsModule, ReactiveFormsModule, SharedModule ]
})
export class TransactionModule {}
