import { TransactionRow } from './transactionrow.model';

export class FilteredResponse {
    public transactions: TransactionRow[];
    public count: number;
}
