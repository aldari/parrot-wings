import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import localeRu from '@angular/common/locales/ru';

import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';

import { AppComponent } from './app.component';
import { AppRouting } from './app.routing';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AuthService } from './auth.service';
import { OnlyNumberDirective } from './shared/only-number.directive';

// tslint:disable-next-line:max-line-length
import { HeaderService } from './header.service';
import { RequestInterceptorService } from './request-interceptor.service';
import { TransactionService } from './modules/transaction/pages/history/transactions.service';
import { LastTransactionService } from './modules/transaction/pages/last-transaction/last-transactions.service';
import { RecipientService } from './modules/transaction/pages/transaction/recipient.service';
import { AuthModule } from './modules/auth/auth.module';
import { UserService } from './modules/auth/register/user.service';
import { TransactionModule } from './modules/transaction/transaction.module';
import { SharedModule } from './modules/shared/shared.module';

registerLocaleData(localeRu);

@NgModule({
    declarations: [ AppComponent, AdminLayoutComponent, PageNotFoundComponent, OnlyNumberDirective ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        ReactiveFormsModule,
        RouterModule,
        AppRouting,
        AuthModule,
        TransactionModule,
        SharedModule
    ],
    providers: [
        AuthService,
        UserService,
        HeaderService,
        TransactionService,
        LastTransactionService,
        RecipientService,
        { provide: LOCALE_ID, useValue: 'en-US' },
        { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
        { provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: { duration: 4000 } }
    ],
    bootstrap: [ AppComponent ]
})
export class AppModule {}
