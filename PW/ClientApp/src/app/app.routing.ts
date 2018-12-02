import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { AuthGuard } from './auth-guard.service';
import { HistoryComponent } from './history/history.component';
import { TransactionComponent } from './transaction/transaction.component';
import { LastTransactionComponent } from './last-transaction/last-transaction.component';
import { TestComponent } from './test/test.component';

const appRoutes: Routes = [
    {
        path: '',
        component: AdminLayoutComponent,
        children: [
            { path: '', component: TransactionComponent, canActivate: [ AuthGuard ] },
            { path: 'last-transaction', component: LastTransactionComponent, canActivate: [ AuthGuard ] },
            { path: 'history', component: HistoryComponent, canActivate: [ AuthGuard ] },
            { path: 'test', component: TestComponent }
        ]
    },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    {
        path: '**',
        component: PageNotFoundComponent
    }
];

export const AppRoutingProviders: any[] = [];

export const AppRouting: ModuleWithProviders = RouterModule.forRoot(appRoutes);
