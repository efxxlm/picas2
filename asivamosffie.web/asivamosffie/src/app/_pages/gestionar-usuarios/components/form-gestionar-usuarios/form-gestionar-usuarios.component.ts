import { delay } from 'rxjs/operators';
import { GestionarUsuariosService } from './../../../../core/_services/gestionarUsuarios/gestionar-usuarios.service';
import { Localizacion } from './../../../../core/_services/common/common.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import humanize from 'humanize-plus';

@Component({
  selector: 'app-form-gestionar-usuarios',
  templateUrl: './form-gestionar-usuarios.component.html',
  styleUrls: ['./form-gestionar-usuarios.component.scss']
})
export class FormGestionarUsuariosComponent implements OnInit {

    perfil: any;
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
    listaAsignaciones: Dominio[] = [];
    listaRoles: { nombre: string, perfilId: number }[] = [];
    listaContratos: { contratoId: number, numeroContrato: string }[] = [];
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
        this.getDataUsuario();
        this.formUsuario.get( 'correo' ).valueChanges
            .pipe(
                delay( 5000 )
            )
            .subscribe(
                email => {
                    if ( this.formUsuario.get( 'correo' ).dirty === true && this.formUsuario.get( 'correo' ).value !== null ) {
                        const pUsuario = { email };

                        this.gestionarUsuariosSvc.validateExistEmail( pUsuario )
                            .subscribe(
                                response => {
                                    if ( response === true && this.formUsuario.get( 'correo' ).value === email ) {
                                        this.openDialog( '', '<b>El correo ya fue utilizado, por favor verifique la información.</b>' );
                                        this.formUsuario.get( 'correo' ).setValue( null );
                                    }
                                }
                            );
                    }
                }
            );
    }

    ngOnInit(): void {
    }

    getDataUsuario() {
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
       // Get data campo Procedencia
        this.commonSvc.listaProcedencia()
            .subscribe(
                listaProcedencia => {
                    const procedencia = listaProcedencia.find( procedencia => procedencia.codigo === '1' );
                    if ( procedencia !== undefined ) {
                        this.procedenciaFfieCodigo = procedencia.codigo;
                    }

                    this.listaProcedencia = listaProcedencia;
                    // Get data campo tipo de documento de identificacion
                    this.commonSvc.listaTipodocumento()
                        .subscribe( listaTipodocumento => {
                            this.listaTipoDocumento = listaTipodocumento;
                            // Get data campo departamento
                            this.commonSvc.listaDepartamentos()
                                .subscribe( listaDepartamentos => {
                                    this.listaDepartamento = listaDepartamentos;
                                    // Get data campo dependencia
                                    this.commonSvc.listaDependencia()
                                        .subscribe( listaDependencia => {
                                            this.listaDependencia = listaDependencia;
                                            // Get data campo grupo
                                            this.commonSvc.listaGrupo()
                                                .subscribe( listaGrupo => {
                                                    this.listaGrupo = listaGrupo;
                                                    // Get data roles
                                                    this.gestionarUsuariosSvc.getListPerfil()
                                                    .subscribe( listaPerfil => {
                                                        this.listaRoles = listaPerfil;
                                                        // Get data campo rol asignado al contrato
                                                        this.commonSvc.listaTipoAsignaciones()
                                                            .subscribe(
                                                                listaTipoAsignaciones => {
                                                                    this.listaAsignaciones = listaTipoAsignaciones;
                                                                    // Get data usuario
                                                                    if ( this.esRegistroNuevo === false ) {
                                                                        this.gestionarUsuariosSvc.getUsuario( this.activatedRoute.snapshot.params.id )
                                                                            .subscribe(
                                                                                async getUsuario => {
                                                                                    this.perfil = getUsuario;
                                                                                    this.usuarioId = getUsuario.usuarioId;
                                                                                    console.log( getUsuario );
                                                                                    const contratosAsignados = [];

                                                                                    if ( getUsuario.contratosAsignados !== undefined ) {
                                                                                        if ( getUsuario.contratosAsignados.length > 0 ) {
                                                                                            getUsuario.contratosAsignados.forEach( contrato => contratosAsignados.push( contrato.contratoId ) );
                                                                                            
                                                                                        }
                                                                                    }

                                                                                    if ( getUsuario.tipoAsignacionCodigo !== undefined ) {
                                                                                        this.getlistaContratos( getUsuario.tipoAsignacionCodigo );
                                                                                        this.listaAsignaciones = this.listaAsignaciones.filter( asignacion => asignacion.codigo === getUsuario.tipoAsignacionCodigo );
                                                                                    }

                                                                                    this.formUsuario.setValue(
                                                                                        {
                                                                                            procedencia: getUsuario.procedenciaCodigo !== undefined ? this.listaProcedencia.find( procedencia => procedencia.codigo === getUsuario.procedenciaCodigo ).codigo : null,
                                                                                            primerNombre: getUsuario.primerNombre !== undefined ? getUsuario.primerNombre : null,
                                                                                            segundoNombre: getUsuario.segundoNombre !== undefined ? getUsuario.segundoNombre : null,
                                                                                            primerApellido: getUsuario.primerApellido !== undefined ? getUsuario.primerApellido : null,
                                                                                            segundoApellido: getUsuario.segundoApellido !== undefined ? getUsuario.segundoApellido : null,
                                                                                            tipoDocumento: getUsuario.tipoDocumentoCodigo !== undefined ? this.listaTipoDocumento.find( documento => documento.codigo === getUsuario.tipoDocumentoCodigo ).codigo : null,
                                                                                            numeroIdentificacion: getUsuario.numeroIdentificacion !== undefined ? getUsuario.numeroIdentificacion : null,
                                                                                            correo: getUsuario.email !== undefined ? getUsuario.email : null,
                                                                                            telefonoFijo: null,
                                                                                            telefonoCelular: null,
                                                                                            departamento: null,
                                                                                            municipio: null,
                                                                                            fechaCreacion: getUsuario.fechaCreacion !== undefined ? getUsuario.fechaCreacion : null,
                                                                                            fechaExpiracion: getUsuario.fechaExpiracion !== undefined ? getUsuario.fechaExpiracion : null,
                                                                                            urlSoporte: getUsuario.urlSoporteDocumentacion !== undefined ? getUsuario.urlSoporteDocumentacion : null,
                                                                                            observaciones: getUsuario.observaciones !== undefined ? getUsuario.observaciones : null,
                                                                                            dependencia: getUsuario.dependenciaCodigo !== undefined ? this.listaDependencia.find( dependencia => dependencia.codigo === getUsuario.dependenciaCodigo ).codigo : null,
                                                                                            tieneGrupo: getUsuario.tieneGrupo !== undefined ? getUsuario.tieneGrupo : null,
                                                                                            grupo: getUsuario.grupoCodigo !== undefined ? this.listaGrupo.find( grupo => grupo.codigo === getUsuario.grupoCodigo ).codigo : null,
                                                                                            tieneContratos: getUsuario.tieneContratoAsignado !== undefined ? getUsuario.tieneContratoAsignado : null,
                                                                                            rol: getUsuario.perfil !== undefined ? this.listaRoles.find( rol => rol.perfilId === getUsuario.perfil.perfilId ).perfilId : null,
                                                                                            tipoAsignacionCodigo: getUsuario.tipoAsignacionCodigo !== undefined ? this.listaAsignaciones.find( asignacion => asignacion.codigo === getUsuario.tipoAsignacionCodigo ).codigo : null,
                                                                                            contratos: contratosAsignados.length > 0 ? contratosAsignados : null
                                                                                        }
                                                                                    );

                                                                                    if ( getUsuario.municipioId !== undefined ) {
                                                                                        this.commonSvc.listMunicipiosByIdMunicipio( getUsuario.municipioId )
                                                                                            .subscribe(
                                                                                                listMunicipiosByIdMunicipio => {
                                                                                                    this.formUsuario.get( 'departamento' ).setValue( this.listaDepartamento.find( departamento => departamento.localizacionId === listMunicipiosByIdMunicipio[0].idPadre ) );
                                                                                                    this.formUsuario.get( 'municipio' ).setValue( listMunicipiosByIdMunicipio[0] );
                                                                                                }
                                                                                            );
                                                                                    }
                                                                                }
                                                                            );
                                                                    }
                                                                }
                                                            );
                                                    } );
                                                } );
                                        } );
                                } );
                        } );
                }
            );
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
                telefonoFijo: [ null, [ Validators.pattern( '[- +()0-9]+' ), Validators.required ] ],
                telefonoCelular: [ null, [ Validators.pattern( '[- +()0-9]+' ), Validators.required ] ],
                departamento: [ null, Validators.required ],
                municipio: [ null, Validators.required ],
                fechaCreacion: [ null, Validators.required ],
                fechaExpiracion: [ null, Validators.required ],
                urlSoporte: [ null, Validators.required ],
                observaciones: [ null, Validators.required ],
                dependencia: [ null, Validators.required ],
                tieneGrupo: [ null, Validators.required ],
                grupo: [ null, Validators.required ],
                tieneContratos: [ null, Validators.required ],
                rol: [ null, Validators.required ],
                tipoAsignacionCodigo: [ null, Validators.required ],
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

    getlistaAsignaciones( listaContratos: any[] ) {
        if ( listaContratos !== null ) {
            if ( listaContratos.length === 0 ) {
                this.commonSvc.listaTipoAsignaciones()
                    .subscribe( listaTipoAsignaciones => {
                        this.listaAsignaciones = listaTipoAsignaciones;
                        this.formUsuario.controls[ 'tipoAsignacionCodigo' ].enable();
                    } );
            } else {
                this.formUsuario.controls[ 'tipoAsignacionCodigo' ].disable();
            }
        }
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    getlistaContratos( tipoAsignacion: string ) {
        this.gestionarUsuariosSvc.getContratoByTipo( tipoAsignacion, this.usuarioId )
            .subscribe( getContratoByTipo => this.listaContratos = getContratoByTipo );
    }

    getMunicipiosByDepartamento( departamento: Localizacion ) {
        this.listaMunicipio = [];

        if ( departamento !== null ) {
            this.formUsuario.get( 'municipio' ).setValue( null );
            this.commonSvc.listaMunicipiosByIdDepartamento( departamento.localizacionId )
                .subscribe( listaMunicipiosByIdDepartamento => this.listaMunicipio = listaMunicipiosByIdDepartamento );
        }
    }

    getValidateNumberPhone( controlName: string ) {
        if ( this.formUsuario.get( controlName ).valid === false ) {
            this.formUsuario.get( controlName ).setValue( null );
            return true;
        } else {
            return false;
        }
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
        const contratosAsignados = [];

        if ( this.formUsuario.get( 'contratos' ).value !== null ) {
            if ( this.formUsuario.get( 'contratos' ).value.length > 0 ) {
                const contratosSeleccionados: number[] = this.formUsuario.get( 'contratos' ).value;
                contratosSeleccionados.forEach( contrato => {
                    const contratoLista = this.listaContratos.find( registro => registro.contratoId === contrato );

                    if ( contratoLista !== undefined ) {
                        contratosAsignados.push( contratoLista );
                    }
                } );
            }
        }

        if ( this.formUsuario.get( 'telefonoFijo' ).value !== null ) {

            if ( this.formUsuario.get( 'telefonoFijo' ).value.length < 7 ) {
                this.openDialog( '', '<b>El numero de teléfono fijo no debe ser menor a 7 digitos</b>' );
            }

            if ( this.getValidateNumberPhone( 'telefonoFijo' ) === true ) {
                this.openDialog( '', '<b>Debe Ingresar un numero de teléfono fijo valido.</b>' );
                this.formUsuario.get( 'telefonoFijo' ).setValue( null );
            }

        }

        if ( this.formUsuario.get( 'telefonoCelular' ).value !== null ) {

            if ( this.formUsuario.get( 'telefonoCelular' ).value.length < 10 ) {
                this.openDialog( '', '<b>El numero de teléfono celular no debe ser menor a 10 digitos</b>' );
            }

            if ( this.getValidateNumberPhone( 'telefonoCelular' ) === true ) {
                this.openDialog( '', '<b>Debe Ingresar un numero de teléfono celular valido.</b>' );
                this.formUsuario.get( 'telefonoCelular' ).setValue( null );
            }

        }

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
            municipioId: this.formUsuario.get( 'municipio' ).value !== null ? this.formUsuario.get( 'municipio' ).value.localizacionId : null,
            fechaCreacion: this.formUsuario.get( 'fechaCreacion' ).value,
            fechaExpiracion: this.formUsuario.get( 'fechaExpiracion' ).value,
            urlSoporteDocumentacion: this.formUsuario.get( 'urlSoporte' ).value,
            observaciones: this.formUsuario.get( 'observaciones' ).value,
            dependenciaCodigo: this.formUsuario.get( 'dependencia' ).value,
            tieneGrupo: this.formUsuario.get( 'tieneGrupo' ).value,
            grupoCodigo: this.formUsuario.get( 'grupo' ).value,
            PerfilId: this.formUsuario.get( 'rol' ).value,
            tipoAsignacionCodigo: this.formUsuario.get( 'tipoAsignacionCodigo' ).value,
            tieneContratoAsignado: this.formUsuario.get( 'tieneContratos' ).value,
            contratosAsignados: contratosAsignados.length > 0 ? contratosAsignados : null
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
