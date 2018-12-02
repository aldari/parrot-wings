import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatExpansionModule } from '@angular/material/expansion';
import { registerLocaleData } from '@angular/common';
import localeRu from '@angular/common/locales/ru';
import { MAT_MOMENT_DATE_FORMATS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule, MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { MatProgressButtons } from 'mat-progress-buttons';
import { MatCardModule } from '@angular/material/card';

import { AppComponent } from './app.component';
import { AppRouting } from './app.routing';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AdminGuard } from './admin-guard.service';
import { AuthGuard } from './auth-guard.service';
import { AuthService } from './auth.service';
import { ValidateService } from './shared/validate.service';
import { OnlyNumberDirective } from './shared/only-number.directive';
import { OrderByPipe } from './shared/orderby.pipe';

// tslint:disable-next-line:max-line-length
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { DialogsService } from './shared/dialog.service';
import { HeaderService } from './header.service';
import { RequestInterceptorService } from './request-interceptor.service';
import { CapitalizePipe } from './shared/capitalize.pipe';
import { TransactionComponent } from './transaction/transaction.component';
import { HistoryComponent } from './history/history.component';
import { UserService } from './users/services/user.service';
import { RecipientService } from './transaction/recipient.service';
import { TransactionService } from './history/transactions.service';
import { LastTransactionComponent } from './last-transaction/last-transaction.component';
import { LastTransactionService } from './last-transaction/last-transactions.service';
import { TestComponent } from './test/test.component';

registerLocaleData(localeRu);

export const MY_FORMATS = {
    parse: {
        dateInput: 'L'
    },
    display: {
        dateInput: 'L',
        monthYearLabel: 'MMM YYYY',
        dateA11yLabel: 'LL',
        monthYearA11yLabel: 'MMMM YYYY'
    }
};

@NgModule({
    declarations: [
        AppComponent,
        AdminLayoutComponent,
        LoginComponent,
        RegisterComponent,
        PageNotFoundComponent,
        OnlyNumberDirective,
        OrderByPipe,
        CapitalizePipe,
        ConfirmDialogComponent,
        TransactionComponent,
        HistoryComponent,
        LastTransactionComponent,
        TestComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        AppRouting,
        MatInputModule,
        MatDatepickerModule,
        MatCheckboxModule,
        MatRadioModule,
        MatSelectModule,
        MatTabsModule,
        MatDialogModule,
        MatExpansionModule,
        MatButtonModule,
        MatSlideToggleModule,
        MatAutocompleteModule,
        MatTableModule,
        MatPaginatorModule,
        MatSortModule,
        MatProgressSpinnerModule,
        MatIconModule,
        MatSnackBarModule,
        MatProgressButtons,
        MatCardModule
    ],
    providers: [
        AuthService,
        AuthGuard,
        AdminGuard,
        UserService,
        ValidateService,
        HeaderService,
        DialogsService,
        TransactionService,
        LastTransactionService,
        RecipientService,
        { provide: LOCALE_ID, useValue: 'ru-RU' },
        { provide: MAT_DATE_LOCALE, useValue: 'ru-RU' },
        { provide: DateAdapter, useClass: MomentDateAdapter, deps: [ MAT_DATE_LOCALE ] },
        { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
        { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
        { provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: { duration: 4000 } }
    ],
    entryComponents: [ ConfirmDialogComponent ],
    bootstrap: [ AppComponent ]
})
export class AppModule {}
