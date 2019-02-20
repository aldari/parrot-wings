import { NgModule, LOCALE_ID } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import localeRu from '@angular/common/locales/ru';
import { registerLocaleData } from '@angular/common';

import { AuthService } from '../../auth.service';
import { UserService } from '../auth/register/user.service';
import { HeaderService } from './header.service';
import { RequestInterceptorService } from '../../request-interceptor.service';

import { AdminLayoutComponent } from './pages/admin-layout/admin-layout.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { SharedModule } from '../shared/shared.module';
import { AppRoutingModule } from '../../app.routing';
import { AccountBalanceService } from './account-balance.service';
import { TransactionApiService } from '../transaction/services/transaction-api.service';
import { UserDataSource } from '../transaction/services/transaction-data-source.service';

registerLocaleData(localeRu);

@NgModule({
    declarations: [ AdminLayoutComponent, PageNotFoundComponent ],
    imports: [ SharedModule, AppRoutingModule ],
    exports: [ AppRoutingModule ],
    providers: [
        AuthService,
        UserService,
        HeaderService,
        TransactionApiService,
        AccountBalanceService,
        { provide: LOCALE_ID, useValue: 'en-US' },
        { provide: HTTP_INTERCEPTORS, useClass: RequestInterceptorService, multi: true },
        { provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: { duration: 4000 } }
    ]
})
export class CoreModule {}
