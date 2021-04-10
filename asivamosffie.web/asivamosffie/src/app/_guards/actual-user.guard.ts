import { Injectable } from '@angular/core';
import { CanLoad, Route, UrlSegment, ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AutenticacionService } from '../core/_services/autenticacion/autenticacion.service';

@Injectable({
  providedIn: 'root'
})
export class ActualUserGuard implements CanLoad {

  constructor(
    private autenticacionService: AutenticacionService,
    private routes: Router ) {};

  canLoad(
    route: Route,
    segments: UrlSegment[] ): Observable<boolean> | Promise<boolean> | boolean
  {

    return new Promise<boolean>( async ( resolve, reject ) => {
      const checkActualUser = await this.autenticacionService.checkActualUser();

      if ( checkActualUser === true ) {
        resolve( true );
      }
      if ( checkActualUser === false ) {
        this.routes.navigate( [ 'inicio' ] );
        reject( false );
      }
    } );
  }
  
}
