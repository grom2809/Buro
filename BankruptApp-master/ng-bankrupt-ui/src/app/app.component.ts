import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from './models/user';
import { AuthService, USER_ID } from './services/auth.service';
import { UserstoreService } from './services/userstore.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  title = 'ng-bankrupt-ui';

  currentUser: User;
  userName: string = "";
  header = "Ошибка";
  message = "";
  visible = false;

  public get isLoggedIn() : boolean{
    return this.as.isAuthenticated();
  }

  constructor(private as: AuthService, private us: UserstoreService) {
    this.currentUser = new User("", "", "", "", "");
    this.refreshUser();
  }

  ngOnInit(): void {
    this.refreshUser();
  }

  logout(){
    this.as.logout();
  }

  refreshUser(): void{
    this.as.currentUserId.subscribe(() => {
      let id = localStorage.getItem(USER_ID);
      if(!id) return;
      this.currentUser = new User("", "", "", "", "");
      this.us.getUser(id).subscribe(
          (res: User) => 
          {
            this.currentUser = res;
            this.userName = res.name;
          },
          (err: HttpErrorResponse) => this.handleError(err));
    });
  }

  handleError(error: HttpErrorResponse): void {
    this.message = error.error.message;
    this.header = "Ошибка " + error.status;
    this.visible = !this.visible;
  }
}
