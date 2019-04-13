import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

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
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressButtonsModule } from 'mat-progress-buttons';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';

import { AdminGuard } from './guards/admin-guard.service';
import { AuthGuard } from './guards/auth-guard.service';
import { OnlyNumberDirective } from './directives/only-number.directive';
import { NumberDirective } from './directives/numbers-only.directive';

@NgModule({
    declarations: [ OnlyNumberDirective, NumberDirective ],
    imports: [
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
        MatProgressButtonsModule,
        MatCardModule,
        MatFormFieldModule,
        MatMomentDateModule,
        MatSidenavModule,
        MatToolbarModule,
        MatListModule
    ],
    exports: [
        CommonModule,
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
        MatProgressButtonsModule,
        MatCardModule,
        MatFormFieldModule,
        MatMomentDateModule,
        OnlyNumberDirective,
        NumberDirective,
        MatSidenavModule,
        MatToolbarModule,
        MatListModule
    ],
    providers: [ AdminGuard, AuthGuard ]
})
export class SharedModule {}
