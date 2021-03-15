import { GestionarUsuariosService } from './../../../../core/_services/gestionarUsuarios/gestionar-usuarios.service';
import { Localizacion } from './../../../../core/_services/common/common.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-gestionar-usuarios',
  templateUrl: './form-gestionar-usuarios.component.html',
  styleUrls: ['./form-gestionar-usuarios.component.scss']
})
export class FormGestionarUsuariosComponent implements OnInit {

    esRegistroNuevo: boolean;
    formUsuario: FormGroup;
    procedenciaFfieCodigo: string;
    usuarioId = 0;
    listaProcedencia: Dominio[] = [];
    listaTipoDocumento: Dominio[] = [];
    listaDepartamento: Localizacion[] = [];
    listaMunicipio: Localizacion[] = [];
    listaDependencia: Dominio[] = [];
    listaGrupo: Dominio[] = [];
    listaRoles: { nombre: string, perfilId: number }[] = [];
    listaContratos: { nombre: string, codigo: string }[] = [
        { nombre: 'N801801', codigo: '1' },
        { nombre: 'J208208', codigo: '2' }
    ];
    editorStyle = {
        height: '50%'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };

    constructor(
        private activatedRoute: ActivatedRoute,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private commonSvc: CommonService,
        private gestionarUsuariosSvc: GestionarUsuariosService )
    {
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
            if ( urlSegment.path === 'crearUsuario' ) {
                this.esRegistroNuevo = true;
                return;
            }
            if ( urlSegment.path === 'editarUsuario' ) {
                this.esRegistroNuevo = false;
                return;
            }
        } );
        this.formUsuario = this.crearFormulario();
        /*
            listaDepartamentos
            listaMunicipiosByIdDepartamento
        */
        this.commonSvc.listaProcedencia()
            .subscribe(
                listaProcedencia => {
                    const procedencia = listaProcedencia.find( procedencia => procedencia.codigo === '1' );
                    if ( procedencia !== undefined ) {
                        this.procedenciaFfieCodigo = procedencia.codigo;
                    }

                    this.listaProcedencia = listaProcedencia;
                }
            );
        this.commonSvc.listaTipodocumento()
            .subscribe( listaTipodocumento => this.listaTipoDocumento = listaTipodocumento );
        this.commonSvc.listaDepartamentos()
            .subscribe( listaDepartamentos => this.listaDepartamento = listaDepartamentos );
        this.commonSvc.listaDependencia()
            .subscribe( listaDependencia => this.listaDependencia = listaDependencia );
        this.commonSvc.listaGrupo()
            .subscribe( listaGrupo => this.listaGrupo = listaGrupo );
        this.gestionarUsuariosSvc.getListPerfil()
            .subscribe( listaPerfil => this.listaRoles = listaPerfil );
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        return this.fb.group(
            {
                procedencia: [ null, Validators.required ],
                primerNombre: [ null, Validators.required ],
                segundoNombre: [ null, Validators.required ],
                primerApellido: [ null, Validators.required ],
                segundoApellido: [ null, Validators.required ],
                tipoDocumento: [ null, Validators.required ],
                numeroIdentificacion: [ null, Validators.required ],
                correo: [ null, Validators.required ],
                telefonoFijo: [ null, Validators.required ],
                telefonoCelular: [ null, Validators.required ],
                departamento: [ null, Validators.required ],
                municipio: [ null, Validators.required ],
                fechaCreacion: [ null, Validators.required ],
                fechaExpiracion: [ null, Validators.required ],
                urlSoporte: [ null, Validators.required ],
                observaciones: [ null, Validators.required ],
                dependencia: [ null, Validators.required ],
                grupo: [ null, Validators.required ],
                rol: [ null, Validators.required ],
                contratos: [ null, Validators.required ]
            }
        );
    }

    getRolSeleccionado( perfilId: number ) {
        if ( this.listaRoles.length > 0 ) {
            const rol = this.listaRoles.find( rol => rol.perfilId === perfilId );

            if ( rol !== undefined ) {
                return rol.nombre;
            }
        }
    }

    getMunicipiosByDepartamento( localizacionId: string ) {
        this.listaMunicipio = [];
        this.commonSvc.listaMunicipiosByIdDepartamento( localizacionId )
            .subscribe( listaMunicipiosByIdDepartamento => this.listaMunicipio = listaMunicipiosByIdDepartamento );
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
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

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }

    guardar() {
        console.log( this.formUsuario );

        const pUsuario = {
            usuarioId: this.usuarioId,
            procedenciaCodigo: this.formUsuario.get( 'procedencia' ).value,
            primerNombre: this.formUsuario.get( 'primerNombre' ).value,
            segundoNombre: this.formUsuario.get( 'segundoNombre' ).value,
            primerApellido: this.formUsuario.get( 'primerApellido' ).value,
            segundoApellido: this.formUsuario.get( 'segundoApellido' ).value,
            tipoDocumentoCodigo: this.formUsuario.get( 'tipoDocumento' ).value,
            numeroIdentificacion: this.formUsuario.get( 'numeroIdentificacion' ).value,
            email: this.formUsuario.get( 'correo' ).value,
            telefonoFijo: this.formUsuario.get( 'telefonoFijo' ).value,
            telefonoCelular: this.formUsuario.get( 'telefonoCelular' ).value,
            municipioId: this.formUsuario.get( 'municipio' ).value,
            fechaCreacion: this.formUsuario.get( 'fechaCreacion' ).value,
            fechaExpiracion: this.formUsuario.get( 'fechaExpiracion' ).value,
            urlSoporteDocumentacion: this.formUsuario.get( 'urlSoporte' ).value,
            observaciones: this.formUsuario.get( 'observaciones' ).value,
            dependenciaCodigo: this.formUsuario.get( 'dependencia' ).value,
            grupoCodigo: this.formUsuario.get( 'grupo' ).value,
            PerfilId: this.formUsuario.get( 'rol' ).value
        }

        this.gestionarUsuariosSvc.createEditUsuario( pUsuario )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/gestionUsuarios' ] ) );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
