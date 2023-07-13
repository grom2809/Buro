import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { AUTH_API_URL } from '../app-injection-tokens';
import { Token } from '../models/token';

export const ACCESS_TOKEN_KEY = 'userstore_access_token';
export const USER_ID = 'userstore_current_user_id';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private userId = new BehaviorSubject('');
  currentUserId  = this.userId.asObservable();
  updateUserId() {
    let id = this.getCurrentUserId();
    this.userId.next(!!id ? id : "");
  }
  
  constructor(
    private http: HttpClient,
    @Inject(AUTH_API_URL) private apiUrl: string,
    private jwtHelper: JwtHelperService,
    private router: Router
  ) { }

  login(email: string, password: string): Observable<Token>{
    return this.http.post<Token>(this.apiUrl + '/Auth/login', {email, password})
    .pipe(tap((token: Token) => 
    {
      localStorage.setItem(ACCESS_TOKEN_KEY, <string>token.access_token); 
      localStorage.setItem(USER_ID, <string>token.user_id);
      this.updateUserId(); 
    }))
  }

  isAuthenticated(): boolean{
    var token = localStorage.getItem(ACCESS_TOKEN_KEY);
    return !!token && !this.jwtHelper.isTokenExpired(token);
  }

  getCurrentRole(): string{
    var token = localStorage.getItem(ACCESS_TOKEN_KEY);
    if(!!token)
      return JSON.parse(atob(token.split('.')[1])).role.toLowerCase();
    return "";
  }

  getCurrentUserId(): string | null{
    return localStorage.getItem(USER_ID);
  }

  logout(): void{
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(USER_ID);
    this.router.navigate(['']);
  }
  
}
