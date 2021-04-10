import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { PolizaGarantiaService, ContratoPoliza, InsertPoliza } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { EstadoPolizaCodigo } from 'src/app/_interfaces/gestionar-polizas-garantias.interface';
import humanize from 'humanize-plus';
import { EstadosRevision, PerfilCodigo } from 'src/app/_interfaces/estados-actualizacion-polizas.interface';
@Component({
  selector: 'app-gestionar-polizas',
  templateUrl: './gestionar-polizas.component.html',
  styleUrls: ['./gestionar-polizas.component.scss']
})
export class GestionarPolizasComponent implements OnInit, OnDestroy {

    addressForm = this.fb.group({
        nombre: [ null, Validators.compose([ Validators.required, Validators.minLength(1), Validators.maxLength(50)]) ],
        numeroPoliza: [ null, Validators.compose([ Validators.required, Validators.minLength(2), Validators.maxLength(20) ]) ],
        numeroCertificado: [ null, Validators.compose([ Validators.required, Validators.minLength(2), Validators.maxLength(20)]) ],
        fecha: [ null, Validators.required ],
        seguros: this.fb.array( [] ),
        polizasYSeguros: [ null, Validators.required ],
        cumpleAsegurado: [ null, Validators.required ],
        cumpleBeneficiario: [ null, Validators.required],
        cumpleAfianzado: [ null, Validators.required],
        reciboDePago: [ null, Validators.required],
        condicionesGenerales: [ null, Validators.required],
        fechaRevision: [ null, Validators.required],
        estadoRevision: [ null, Validators.required],
        fechaAprob: [ null, Validators.required],
        responsableAprob: [ null, Validators.required ],
        observacionesGenerales: [ null ]
    });
    listaPerfilCodigo = PerfilCodigo;
    estadosRevision = EstadosRevision;
    polizasYSegurosArray: Dominio[] = [];
    listaTipoSolicitudContrato: Dominio[] = [];
    listaTipoDocumento: Dominio[] = [];
    estadoArray: any[];
    estadosPoliza = EstadoPolizaCodigo;
    contrato: any;
    contratoPolizaId = 0;
    polizaListaChequeoId = 0;
    polizaObservacionId = 0;
    polizaActualizacionRevisionAprobacionObservacion = 0;
    realizoPeticion = false;
    estaEditando = false;
    listaUsuarios: any[] = [];
    minDate: Date;
    editorStyle = {
        height: '100px'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };

    get seguros() {
        return this.addressForm.get( 'seguros' ) as FormArray;
    }

    constructor(
        private routes: Router,
        private polizaSvc: PolizaGarantiaService,
        private fb: FormBuilder,
        public dialog: MatDialog,
        private activatedRoute: ActivatedRoute,
        private commonSvc: CommonService )
    {
        this.minDate = new Date();
        this.commonSvc.getUsuariosByPerfil( this.listaPerfilCodigo.fiduciaria )
            .subscribe( getUsuariosByPerfil => this.listaUsuarios = getUsuariosByPerfil );
        this.commonSvc.listaGarantiasPolizas()
            .subscribe( listaGarantiasPolizas => this.polizasYSegurosArray = listaGarantiasPolizas );
        this.commonSvc.listaTipodocumento()
            .subscribe( listaTipodocumento => this.listaTipoDocumento = listaTipodocumento )
        this.commonSvc.listaEstadoRevision()
            .subscribe( estadoRevision => this.estadoArray = estadoRevision );
        this.commonSvc.listaTipoSolicitudContrato()
            .subscribe( response => this.listaTipoSolicitudContrato = response );
        this.polizaSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
            .subscribe(
                response => {
                    this.contrato = response;
                    console.log( this.contrato )
                }
            )
    }

    ngOnInit(): void {
    }

    ngOnDestroy(): void {
        if ( this.addressForm.dirty === true && this.realizoPeticion === false) {
            this.openDialogConfirmar( '', '<b>¿Desea guardar la información registrada?</b>' );
        }
    };

    openDialogConfirmar(modalTitle: string, modalText: string) {
        const confirmarDialog = this.dialog.open(ModalDialogComponent, {
          width: '30em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        confirmarDialog.afterClosed()
            .subscribe( response => {
                if ( response === true ) {
                    this.onSubmit();
                }
            } );
    }

    // evalua tecla a tecla
    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
    }

    getTipoSolicitud( codigo: string ) {
        if ( this.listaTipoSolicitudContrato.length > 0 ) {
            const tipo = this.listaTipoSolicitudContrato.find( tipo => tipo.codigo === codigo );

            if ( tipo !== undefined ) {
                return tipo.nombre;
            }
        }
    }

    getTipoDocumento( codigo: string ) {
        if ( this.listaTipoDocumento.length > 0 ) {
            const documento = this.listaTipoDocumento.find( documento => documento.codigo === codigo );

            if ( documento !== undefined ) {
                return documento.nombre;
            }
        }
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
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

    async getvalues( listCodigo: string[] ) {
        const listaSeguros = [ ...listCodigo ];

        if ( this.seguros.length > 0 ) {
            this.seguros.controls.forEach( ( control, indexControl ) => {
                const seguroIndex = listaSeguros.findIndex( codigo => codigo === control.get( 'tipoGarantiaCodigo' ).value );
                const seguro = listaSeguros.find( codigo => codigo === control.get( 'tipoGarantiaCodigo' ).value );

                if ( seguroIndex !== -1 ) {
                    listaSeguros.splice( seguroIndex, 1 );
                }

                if ( seguro === undefined ) {
                    this.seguros.removeAt( indexControl );
                    listaSeguros.splice( seguroIndex, 1 );
                }
            } );
        }

        for ( const codigo of listaSeguros ) {
            const seguro = this.polizasYSegurosArray.find( seguro => seguro.codigo === codigo );

            if ( seguro !== undefined ) {
                this.seguros.push( this.fb.group(
                    {
                        contratoPolizaId: [ 0 ],
                        polizaGarantiaId: [ 0 ],
                        nombre: [ seguro.nombre ],
                        tipoGarantiaCodigo: [ seguro.codigo ],
                        esIncluidaPoliza: [ null, Validators.required ],
                        vigencia: [ null, Validators.required ],
                        vigenciaAmparo: [ null, Validators.required ],
                        valorAmparo: [ null, Validators.required ]
                    }
                ) )
            }
        }
    }

    checkSeguros() {
        let registroCompletoSeguros = false;

        if ( this.seguros.length > 0 ) {
            let registroCompleto = 0;
            this.seguros.controls.forEach( control => {
                if ( control.get( 'esIncluidaPoliza' ).value !== null ) {
                    if ( control.get( 'esIncluidaPoliza' ).value === true ) {
                        if (control.get( 'vigencia' ).value !== null && control.get( 'vigenciaAmparo' ).value !== null && control.get( 'valorAmparo' ).value !== null ) {
                            registroCompleto++;
                        }
                    }

                    if ( control.get( 'esIncluidaPoliza' ).value === false ) {
                        registroCompleto++;
                    }
                }
            } );
            
            if ( registroCompleto === this.seguros.length ) {
                registroCompletoSeguros = true;
            }
        }

        return registroCompletoSeguros;
    }

    checkEsIncluidaPoliza( value: boolean, index: number ) {
        if ( value === false ) {
            this.seguros.controls[ index ].get( 'vigencia' ).setValue( null );
            this.seguros.controls[ index ].get( 'vigenciaAmparo' ).setValue( null );
            this.seguros.controls[ index ].get( 'valorAmparo' ).setValue( null );
        }
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open(ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        const getPolizaGarantia = () => {
            const listSeguros = [];

            if ( this.seguros.length > 0 ) {
                this.seguros.controls.forEach( control => {
                    listSeguros.push(
                        {
                            polizaGarantiaId: control.get( 'polizaGarantiaId' ).value,
                            contratoPolizaId: control.get( 'contratoPolizaId' ).value,
                            tipoGarantiaCodigo: control.get( 'tipoGarantiaCodigo' ).value,
                            esIncluidaPoliza: control.get( 'esIncluidaPoliza' ).value,
                            vigencia: control.get( 'vigencia' ).value,
                            vigenciaAmparo: control.get( 'vigenciaAmparo' ).value,
                            valorAmparo: control.get( 'valorAmparo' ).value
                        }
                    )
                } )
            }

            return listSeguros.length > 0 ? listSeguros : null;
        }
        const getPolizaListaChequeo = () => {
            if (    this.addressForm.get( 'cumpleAsegurado' ).value === null
                    && this.addressForm.get( 'cumpleBeneficiario' ).value === null
                    && this.addressForm.get( 'cumpleAfianzado' ).value === null
                    && this.addressForm.get( 'reciboDePago' ).value === null
                    && this.addressForm.get( 'condicionesGenerales' ).value === null )
            {
                return null;
            }
            return [
                {
                    polizaListaChequeoId: this.polizaListaChequeoId,
                    contratoPolizaId: this.contratoPolizaId,
                    cumpleDatosAseguradoBeneficiario: this.addressForm.get( 'cumpleAsegurado' ).value,
                    cumpleDatosBeneficiarioGarantiaBancaria: this.addressForm.get( 'cumpleBeneficiario' ).value,
                    cumpleDatosTomadorAfianzado: this.addressForm.get( 'cumpleAfianzado' ).value,
                    tieneReciboPagoDatosRequeridos: this.addressForm.get( 'reciboDePago' ).value,
                    tieneCondicionesGeneralesPoliza: this.addressForm.get( 'condicionesGenerales' ).value
                }
            ];
        }
        const getHistorialObservaciones = () => {
            return [
                {
                    polizaObservacionId: this.polizaObservacionId,
                    contratoPolizaId: this.contratoPolizaId,
                    fechaRevision: this.addressForm.get( 'fechaRevision' ).value !== null ? new Date( this.addressForm.get( 'fechaRevision' ).value ).toISOString() : new Date().toISOString(),
                    estadoRevisionCodigo: this.addressForm.get( 'estadoRevision' ).value !== null ? this.addressForm.get( 'estadoRevision' ).value : this.estadosPoliza.enRevision,
                    fechaAprobacion: this.addressForm.get( 'fechaAprob' ).value !== null ? new Date( this.addressForm.get( 'fechaAprob' ).value ).toISOString() : null,
                    responsableAprobacionId: this.addressForm.get( 'responsableAprob' ).value,
                    observacionGeneral: this.addressForm.get( 'observacionesGenerales' ).value !== null ? this.addressForm.get( 'observacionesGenerales' ).value : null
                }
            ];
        }

        const pContrato = {
            contratoId: this.contrato.contratoId,
            contratoPoliza: [
                {
                    contratoId: this.contrato.contratoId,
                    contratoPolizaId: this.contratoPolizaId,
                    nombreAseguradora: this.addressForm.get( 'nombre' ).value,
                    numeroPoliza: this.addressForm.get( 'numeroPoliza' ).value,
                    numeroCertificado: this.addressForm.get( 'numeroCertificado' ).value,
                    fechaExpedicion: this.addressForm.get( 'fecha' ).value,
                    estadoPolizaCodigo: this.addressForm.get( 'estadoRevision' ).value !== null ? ( this.addressForm.get( 'estadoRevision' ).value === this.estadosPoliza.sinRadicacion ? this.estadosPoliza.polizaDevuelta : this.estadosPoliza.enRevision ) : this.estadosPoliza.enRevision,
                    polizaGarantia: getPolizaGarantia(),
                    polizaListaChequeo: getPolizaListaChequeo(),
                    polizaObservacion: getHistorialObservaciones()
                }
            ]
        }

        this.polizaSvc.createEditContratoPoliza( pContrato )
            .subscribe(
                response => {
                    this.realizoPeticion = true;
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then(
                        () => this.routes.navigate( [ '/generarPolizasYGarantias' ] )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
