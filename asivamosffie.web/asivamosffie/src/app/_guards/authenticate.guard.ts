import { MatDialog } from '@angular/material/dialog';
import { Injectable } from '@angular/core';
import { CanLoad, Route, UrlSegment, ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AutenticacionService } from '../core/_services/autenticacion/autenticacion.service';
import { ModalDialogComponent } from '../shared/components/modal-dialog/modal-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class AuthenticateGuard implements CanLoad {

  constructor(
    private autenticacionService: AutenticacionService,
    private activatedRoute: ActivatedRoute,
    private dialog: MatDialog,
    private routes: Router
  ){}

  openDialog( modalTitle: string, modalText: string ) {
    this.dialog.open( ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText }
    });
}

    canLoad(
        route: Route,
        segments: UrlSegment[] ): Observable<boolean> | Promise<boolean> | boolean
    {

        return new Promise( ( resolve, reject ) => {
            this.autenticacionService.tienePermisos(`/${ route.path }`)
            .subscribe(
                permisos => {
                    if ( permisos === null ) {
                        this.openDialog( '', '<b>Acceso denegado.<br>Debe loguearse para acceder a las funcionalidades del sistema</b>' );
                        this.routes.navigate( [ 'home' ] );
                        reject( false );
                    } else {
                        route.data = permisos;
                        resolve( true );
                    } 
                }
            );
        } )

    }

}
