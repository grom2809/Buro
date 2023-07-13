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
import { RegUser } from 'src/app/models/regUser';

export const DATE_FORMAT = {
  parse: {
    dateInput: "YYYY-MM-DD"
  },
  display: {
    dateInput: "DD.MM.YYYY",
    monthYearLabel: "MMM YYYY",
    dateA11yLabel: "YYYY-MM-DD",
    monthYearA11yLabel: "MMMM YYYY"
  }
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
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
export class LoginComponent {

  maxDate: Date;

  currentUser: User | null;
  regUser: RegUser;
  isDatePickerErrorShowed: boolean = false;
  header = "Ошибка";
  message = "";
  visible = false;

  public get isLoggedIn() : boolean{
    return this.as.isAuthenticated();
  }

  constructor(private as: AuthService, private us: UserstoreService, private router: Router) {
    this.currentUser = null;
    this.regUser = new RegUser("","","","");
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18 );
  }

  ngOnInit(): void {
    if(this.as.isAuthenticated())
      this.updateUser();
    this.regUser = new RegUser("","","","");
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18 );
  }

  datePickerFocus(){
    this.isDatePickerErrorShowed = true;
  }

  login(email: string, password: string){
    this.as.login(email, password).subscribe(
      res => { this.updateUser(); }, 
      (err: HttpErrorResponse) => this.handleError(err));
  }

  register(name: string, birthdate: string, email: string, password: string){
    let parts = birthdate.split('.');
    birthdate = parts[2]+'-'+parts[1]+'-'+parts[0];
    this.us.createUser(name, birthdate, email, password).subscribe( 
      (event) => {
        if (event.type === HttpEventType.Response) {
          this.login(email, password);
        }
      },
      (err: HttpErrorResponse) => this.handleError(err));
  }

  updateUser(): void{
    this.currentUser = null;
    let id = this.as.getCurrentUserId();
    if(!!id)
    {
      this.us.getUser(id).subscribe(
        (res: User) => {
          this.currentUser = res;
          this.as.updateUserId();
          this.router.navigate(['/' + this.currentUser.role.toLowerCase()]);
        }),
        (err: HttpErrorResponse) => this.handleError(err);
    }
  }

  handleError(error: HttpErrorResponse): void {
    this.message = error.error.message;
    this.header = "Ошибка " + error.status;
    this.visible = !this.visible;
  }
}
