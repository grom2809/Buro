import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  private allowedUrl = new Set<string>(["/", "/account"]);

  constructor(private as: AuthService, private router: Router) {
  }

  canActivate():boolean {
    if(!this.as.isAuthenticated()){
      this.router.navigate(['']);
    }
    let role = "/"+this.as.getCurrentRole();
    let url = window.location.pathname.toLowerCase();//this.router.location.path();//.toLowerCase();
    
    if(url!==role && !this.allowedUrl.has(url))
      this.router.navigate([".."]);
    return true;
  }
  
}
