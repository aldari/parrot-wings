import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';

import { PageNotFoundComponent } from './modules/core/pages/page-not-found/page-not-found.component';
import { AdminLayoutComponent } from './modules/core/pages/admin-layout/admin-layout.component';
import { TransactionComponent } from './modules/transaction/pages/transaction/transaction.component';
import { LastTransactionComponent } from './modules/transaction/pages/last-transaction/last-transaction.component';
import { HistoryComponent } from './modules/transaction/pages/history/history.component';
import { TestComponent } from './modules/transaction/pages/test/test.component';
import { AuthGuard } from './modules/shared/guards/auth-guard.service';

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
    {
        path: '**',
        component: PageNotFoundComponent
    }
];

@NgModule({
    imports: [ RouterModule.forRoot(appRoutes, { preloadingStrategy: PreloadAllModules }) ],
    exports: [ RouterModule ]
})
export class AppRoutingModule {}
