import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AutenticacionService } from '../core/_services/autenticacion/autenticacion.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authenticationService: AutenticacionService
  ) { }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    const currentUser = this.authenticationService.actualUserValue;
     console.log(currentUser);
    // console.log(this.authenticationService);

    if (currentUser) {
      // logged in so return true
      return true;
    }
    else {
      // not logged in so redirect to home
      this.router.navigate(['/'], {});
      return false;
    }
  }

}
