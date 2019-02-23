import { Injectable } from '@angular/core';
import { Transaction } from '../pages/transaction/transaction.model';

@Injectable()
export class TransactionStorageService {
    public transaction: Transaction;
}
