import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from '../shared/shared.module';

@NgModule({
    declarations: [ LoginComponent, RegisterComponent ],
    imports: [ CommonModule, BrowserAnimationsModule, ReactiveFormsModule, AuthRoutingModule, SharedModule ]
})
export class AuthModule {}
