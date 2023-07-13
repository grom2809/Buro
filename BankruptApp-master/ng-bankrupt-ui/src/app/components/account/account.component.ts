import { Component } from '@angular/core';
import {
  DateAdapter,
  MAT_DATE_LOCALE,
  MAT_DATE_FORMATS
} from "@angular/material/core";
import { Router } from '@angular/router';
import { User } from '../../models/user';
import { AuthService } from '../../services/auth.service';
import { UserstoreService } from '../../services/userstore.service';
import { DatePipe } from "@angular/common";
import { MomentDateAdapter } from "@angular/material-moment-adapter";
import { HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { EmailValidator } from '@angular/forms';

export const DATE_FORMAT = {
  parse: {
    dateInput: "DD.MM.YYYY"
  },
  display: {
    dateInput: "DD.MM.YYYY",
    monthYearLabel: "MMM YYYY",
    dateA11yLabel: "YYYY-MM-DD",
    monthYearA11yLabel: "MMMM YYYY"
  }
}

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE]
    },
    { provide: MAT_DATE_FORMATS, useValue: DATE_FORMAT },
    { provide: MAT_DATE_LOCALE, useValue: 'ru-RU' },
    DatePipe
  ]
})
export class AccountComponent {

  maxDate: Date | undefined;
  id: string|null = null;
  currentUser: User;
  dateToPass: Date = new Date();
  header = "Ошибка";
  message = "";
  visible = false;
  isDatePickerErrorShowed: boolean = false;

  constructor(private as: AuthService, private us: UserstoreService, private router: Router) {
    this.updateMaxDate();
    this.currentUser = new User("1","1","1","1","1");
    this.id = this.as.getCurrentUserId();
  }

  ngOnInit(): void {
    this.updateMaxDate();
    this.id = this.as.getCurrentUserId();
    this.refreshUser();
  }

  datePickerFocus(){
    this.isDatePickerErrorShowed = true;
  }

  updateMaxDate(){
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  updateUser(name: string, birthdate: string, email: string, password: string){
    if(!this.id)
      return;
    password = password.trim();

    let parts = birthdate.split('.');
    birthdate = parts[2]+'-'+parts[1]+'-'+parts[0];
    this.us.updateUser(this.id, name, birthdate, email, password).subscribe(
      (event) => {
        if (event.type === HttpEventType.Response) {
          this.as.updateUserId();
        }
      },
      (err: HttpErrorResponse) => this.handleError(err));
  }

  refreshUser(){
    if(!!this.id)
    {
      this.us.getUser(this.id).subscribe(
        (res: User) => 
        {
          this.currentUser = res;
          let parts = res.birthdate.split('.');
          this.dateToPass = new Date(parts[2]+"-"+parts[1]+"-"+parts[0]);
        },
        (err: HttpErrorResponse) => this.handleError(err));
    }
  }

  back(): void {
    this.router.navigate(['/' + this.as.getCurrentRole()]);
  }

  deleteAccount() {
    if(confirm("Вы уверены, что хотите удалить свой аккаунт?")) {
      let id = this.as.getCurrentUserId();
      if(!id) return;
      this.us.deleteAccount(id).subscribe(
        (event) => {
          if (event.type === HttpEventType.Response) {
            this.as.logout();
          }
        },
        (err: HttpErrorResponse) => this.handleError(err)
      );
    }
  }

  handleError(error: HttpErrorResponse): void {
    this.message = error.error.message;
    this.header = "Ошибка " + error.status;
    this.visible = !this.visible;
  }
}
