import { Injectable } from '@angular/core';
import { CanLoad, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { AutenticacionService } from '../core/_services/autenticacion/autenticacion.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticateGuard implements CanLoad {

  constructor(
    private autenticacionService: AutenticacionService,
    private activatedRoute: ActivatedRoute,
  ){}

  canLoad(
    route: Route,
    segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {

      let url = this.activatedRoute.snapshot.url;
      console.log( url, route.path );

      this.autenticacionService.tienePermisos(`/${ route.path }`)
        .subscribe( permisos => {
          console.log( permisos )
        });

    return true;
  }
}
