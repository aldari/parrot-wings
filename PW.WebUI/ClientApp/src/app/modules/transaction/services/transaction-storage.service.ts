import { Injectable } from '@angular/core';

@Injectable()
export class TransactionStorageService {
    public recipientId: string;
    public recipientName: string;
    public amount: number;
    public hasValue = false;

    TransactionStorageService() {}
}
