import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router, NavigationEnd } from '@angular/router';
import { Observable } from 'rxjs';
import { AutenticacionService } from '../core/_services/autenticacion/autenticacion.service';
import { Location } from '@angular/common';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  currentRoute: any;
  route: string;
  constructor(
    private router: Router,
    private location: Location,
    private authenticationService: AutenticacionService,

  ) {
    
    }
   
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
    const currentUser = this.authenticationService.actualUserValue;
    let url=this.location.path();
    let urlSec=url.split("/");
    if(urlSec.length>0)
    {
      url="/"+urlSec[1];
    }
    //console.log("reviso perfil "+url + "-" +this.location.path());
    
      //console.log(currentUser);
    // console.log(this.authenticationService);
    let rutasmenu=[];
      currentUser.menus.forEach(element => {
        rutasmenu.push(element.rutaFormulario);
      });
    if (currentUser) {
      // logged in so return true
      //validate permission over menu
      if(url!='/inicio' && url!='/home' && url!='/')
      {
        console.log(rutasmenu);
        if(rutasmenu.includes(url))
        {
          return true;
        }
        else{
          //not permision
          console.log("sin permisos al menu "+url);
          //this.router.navigate(['/home'], {}); //cuando redirecciono, se vuelve loco
          //return this.router.parseUrl('/home');
          return false;
        }
      }
      else
      {
        return true;
      }                  
    }
    else {
      // not logged in so redirect to home
      //this.router.navigate(['/inicio'], {});
      //console.log("no logeado");
      return false;
    }
  }

}
