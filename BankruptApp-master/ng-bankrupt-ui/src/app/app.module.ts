import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HttpClientModule } from '@angular/common/http'
import { MatCardModule } from '@angular/material/card'
import { MatInputModule } from '@angular/material/input'
import { MatButtonModule } from '@angular/material/button'
import { MatTableModule } from '@angular/material/table'
import { MatFormFieldModule } from '@angular/material/form-field';
import { AdminComponent } from './components/admin/admin.component';
import { UsersComponent } from './components/users/users.component'
import { AUTH_API_URL, STORE_API_URL } from './app-injection-tokens';
import { JwtModule } from '@auth0/angular-jwt';
import { ACCESS_TOKEN_KEY } from './services/auth.service';
import { environment } from 'src/environments/environment';

import { MatSidenavModule } from '@angular/material/sidenav';
import {MatTabsModule} from '@angular/material/tabs';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatMomentDateModule } from "@angular/material-moment-adapter";
import { LoginComponent } from './components/login/login.component';
import { AccountComponent } from './components/account/account.component';
import { FormsModule } from '@angular/forms';
import { ToastModule } from '@coreui/angular';

export function tokenGetter(){
  return localStorage.getItem(ACCESS_TOKEN_KEY);
}

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    UsersComponent,
    LoginComponent,
    AccountComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,

    HttpClientModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatFormFieldModule,
    MatSidenavModule,
    BrowserModule,
    MatTabsModule,
    MatDatepickerModule,
    MatButtonModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatMomentDateModule,
    FormsModule,
    ToastModule,

    JwtModule.forRoot({
      config: {
        tokenGetter,
        allowedDomains: environment.allowedDomains
      }
    })
  ],
  providers: [{
      provide: AUTH_API_URL,
      useValue: environment.authApi
    },
    {
      provide: STORE_API_URL,
      useValue: environment.storeApi
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
