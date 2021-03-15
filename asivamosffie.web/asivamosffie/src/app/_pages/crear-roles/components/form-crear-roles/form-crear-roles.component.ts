import { CrearRolesService } from './../../../../core/_services/crearRoles/crear-roles.service';
import { from } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, Renderer2 } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { delay } from 'rxjs/operators';

@Component({
  selector: 'app-form-crear-roles',
  templateUrl: './form-crear-roles.component.html',
  styleUrls: ['./form-crear-roles.component.scss']
})
export class FormCrearRolesComponent implements OnInit {

    esRegistroNuevo: boolean;
    formRoles: FormGroup;
    listaEstadoFuncionalidad: any = {};
    listaFuncionalidad = {
        faseInicio: [],
        faseSeguimiento: [],
        faseCierre: []
    }
    perfilId = 0;

    constructor(
        private activatedRoute: ActivatedRoute,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private crearRolesSvc: CrearRolesService,
        private commonSvc: CommonService )
    {
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
            if ( urlSegment.path === 'nuevoRol' ) {
                this.esRegistroNuevo = true;
                return;
            }
            if ( urlSegment.path === 'editarRol' ) {
                this.esRegistroNuevo = false;
                return;
            }
        } );
        this.formRoles = this.crearFormulario();
        this.commonSvc.listaFaseMenu()
            .subscribe(
                listaFaseMenu => {
                    this.crearRolesSvc.getMenu()
                        .subscribe(
                            getMenu => {

                                listaFaseMenu.forEach( lista => {
                                    if ( lista.codigo === '1' ) {
                                        this.listaEstadoFuncionalidad.faseInicio = lista.codigo;
                                    }
                                    if ( lista.codigo === '2' ) {
                                        this.listaEstadoFuncionalidad.faseSeguimiento = lista.codigo;
                                    }
                                    if ( lista.codigo === '3' ) {
                                        this.listaEstadoFuncionalidad.faseCierre = lista.codigo;
                                    }
                                } );

                                getMenu.forEach( menu => {
                                    if ( menu.faseCodigo === this.listaEstadoFuncionalidad.faseInicio ) {
                                        this.listaFuncionalidad.faseInicio.push( menu );
                                    }

                                    if ( menu.faseCodigo === this.listaEstadoFuncionalidad.faseSeguimiento ) {
                                        this.listaFuncionalidad.faseSeguimiento.push( menu );
                                    }

                                    if ( menu.faseCodigo === this.listaEstadoFuncionalidad.faseCierre ) {
                                        this.listaFuncionalidad.faseCierre.push( menu );
                                    }
                                } );

                                console.log( this.listaFuncionalidad );
                            }
                        );
                }
            );

            this.formRoles.get( 'nombreRol' ).valueChanges
                .pipe(
                    delay( 2000 )
                )
                .subscribe(
                    value => {
                        if ( this.formRoles.get( 'nombreRol' ).dirty === true && this.formRoles.get( 'nombreRol' ).value !== null ) {
                            this.crearRolesSvc.validateExistNamePerfil( value )
                                .subscribe(
                                    response => {
                                        if ( response === true && this.formRoles.get( 'nombreRol' ).value === value ) {
                                            this.openDialog( '', '<b>El nombre del rol ya fue utilizado, por favor verifique la información.</b>' );
                                            this.formRoles.get( 'nombreRol' ).setValue( null );
                                        }
                                    }
                                );
                        }
                    }
                );
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        return this.fb.group(
            {
                nombreRol: [ null, Validators.required ],
                registrosFaseInicio: this.fb.group(
                    {
                        registros: this.fb.array( [] )
                    }
                ),
                registrosFaseSeguimiento: this.fb.group(
                    {
                        registros: this.fb.array( [] )
                    }
                ),
                registrosFaseFinal: this.fb.group(
                    {
                        registros: this.fb.array( [] )
                    }
                )
            }
        );
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '30em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    guardar() {
        console.log( this.formRoles );
        const getMenuPerfil = () => {
            const listaMenuPerfil = [];

            this.formRoles.get( 'registrosFaseInicio' ).get( 'registros' ).value.forEach( registro => {
                if ( !( registro.crear === null && registro.consultar === null && registro.modificar === null && registro.eliminar === null ) ) {
                    listaMenuPerfil.push(
                        {
                            menuPerfilId: 0,
                            menuId: registro.menuId,
                            perfilId: this.perfilId,
                            tienePermisoCrear: registro.crear === null ? false : registro.crear,
                            tienePermisoLeer: registro.consultar === null ? false : registro.consultar,
                            tienePermisoEditar: registro.modificar === null ? false : registro.modificar,
                            tienePermisoEliminar: registro.eliminar === null ? false : registro.eliminar
                        }
                    );
                }
            } );
            this.formRoles.get( 'registrosFaseSeguimiento' ).get( 'registros' ).value.forEach( registro => {
                if ( !( registro.crear === null && registro.consultar === null && registro.modificar === null && registro.eliminar === null ) ) {
                    listaMenuPerfil.push(
                        {
                            menuPerfilId: 0,
                            menuId: registro.menuId,
                            perfilId: this.perfilId,
                            tienePermisoCrear: registro.crear === null ? false : registro.crear,
                            tienePermisoLeer: registro.consultar === null ? false : registro.consultar,
                            tienePermisoEditar: registro.modificar === null ? false : registro.modificar,
                            tienePermisoEliminar: registro.eliminar === null ? false : registro.eliminar
                        }
                    );
                }
            } );
            this.formRoles.get( 'registrosFaseFinal' ).get( 'registros' ).value.forEach( registro => {
                if ( !( registro.crear === null && registro.consultar === null && registro.modificar === null && registro.eliminar === null ) ) {
                    listaMenuPerfil.push(
                        {
                            menuPerfilId: 0,
                            menuId: registro.menuId,
                            perfilId: this.perfilId,
                            tienePermisoCrear: registro.crear === null ? false : registro.crear,
                            tienePermisoLeer: registro.consultar === null ? false : registro.consultar,
                            tienePermisoEditar: registro.modificar === null ? false : registro.modificar,
                            tienePermisoEliminar: registro.eliminar === null ? false : registro.eliminar
                        }
                    );
                }
            } );

            return listaMenuPerfil;
        }

        const pPerfil = {
            perfilId: this.perfilId,
            nombre: this.formRoles.get( 'nombreRol' ).value,
            menuPerfil: getMenuPerfil()
        }

        this.crearRolesSvc.createEditRolesPermisos( pPerfil )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/crearRoles' ] ) );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
