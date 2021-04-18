import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CreatePolizaGarantia, CreatePolizaObservacion, EditPoliza, InsertPoliza, PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { EstadoPolizaCodigo } from 'src/app/_interfaces/gestionar-polizas-garantias.interface';
import { EstadosRevision, PerfilCodigo } from 'src/app/_interfaces/estados-actualizacion-polizas.interface';
import humanize from 'humanize-plus';
@Component({
  selector: 'app-editar-en-revision',
  templateUrl: './editar-en-revision.component.html',
  styleUrls: ['./editar-en-revision.component.scss']
})
export class EditarEnRevisionComponent implements OnInit, OnDestroy {

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
    estadoArray: Dominio[];
    estadosPoliza = EstadoPolizaCodigo;
    contrato: any;
    contratoPoliza: any;
    polizaListaChequeoId = 0;
    polizaObservacion: any[] = [];
    polizaObservacionId = 0;
    contadorRevision = 0;
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
        this.getContratoPoliza();
    }

    ngOnInit(): void {
    }

    ngOnDestroy(): void {
        if ( this.addressForm.dirty === true && this.realizoPeticion === false) {
            this.openDialogConfirmar( '', '<b>¿Desea guardar la información registrada?</b>' );
        }
    }

    getContratoPoliza() {
        this.polizaSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
            .subscribe(
                async response => {
                    this.contrato = response;
                    console.log( this.contrato )
                    this.listaUsuarios = await this.commonSvc.getUsuariosByPerfil( this.listaPerfilCodigo.fiduciaria ).toPromise();
                    this.polizasYSegurosArray = await this.commonSvc.listaGarantiasPolizas().toPromise();
                    this.listaTipoDocumento = await this.commonSvc.listaTipodocumento().toPromise();
                    this.estadoArray = await this.commonSvc.listaEstadoRevision().toPromise();
                    this.listaTipoSolicitudContrato = await this.commonSvc.listaTipoSolicitudContrato().toPromise();

                    if ( this.contrato.contratoPoliza !== undefined ) {
                        if ( this.contrato.contratoPoliza.length > 0 ) {
                            this.contratoPoliza = this.contrato.contratoPoliza[ 0 ];

                            this.addressForm.get( 'nombre' ).setValue( this.contratoPoliza.nombreAseguradora !== undefined ? this.contratoPoliza.nombreAseguradora : null );
                            this.addressForm.get( 'numeroPoliza' ).setValue( this.contratoPoliza.numeroPoliza !== undefined ? this.contratoPoliza.numeroPoliza : null );
                            this.addressForm.get( 'numeroCertificado' ).setValue( this.contratoPoliza.numeroCertificado !== undefined ? this.contratoPoliza.numeroCertificado : null );
                            this.addressForm.get( 'fecha' ).setValue( this.contratoPoliza.fechaExpedicion !== undefined ? new Date( this.contratoPoliza.fechaExpedicion ) : null );

                            // GET seguros
                            if ( this.contratoPoliza.polizaGarantia !== undefined ) {
                                if ( this.contratoPoliza.polizaGarantia.length > 0 ) {
                                    const polizaGarantia = this.contratoPoliza.polizaGarantia;
                                    let listSeguroCodigo = [];

                                    for ( const seguro of polizaGarantia ) {
                                        const poliza = this.polizasYSegurosArray.find( poliza => poliza.codigo === seguro.tipoGarantiaCodigo );

                                        if ( poliza !== undefined ) {
                                            listSeguroCodigo.push( poliza.codigo );

                                            this.seguros.push( this.fb.group(
                                                {
                                                    polizaGarantiaId: [ seguro.polizaGarantiaId ],
                                                    nombre: [ poliza.nombre ],
                                                    tipoGarantiaCodigo: [ poliza.codigo ],
                                                    esIncluidaPoliza: [ seguro.esIncluidaPoliza !== undefined ? seguro.esIncluidaPoliza : null, Validators.required ],
                                                    vigencia: [ seguro.vigencia !== undefined ? new Date( seguro.vigencia ) : null, Validators.required ],
                                                    vigenciaAmparo: [ seguro.vigenciaAmparo !== undefined ? new Date( seguro.vigenciaAmparo ) : null, Validators.required ],
                                                    valorAmparo: [ seguro.valorAmparo !== undefined ? seguro.valorAmparo : null, Validators.required ]
                                                }
                                            ) )
                                        }
                                    }

                                    if ( listSeguroCodigo.length > 0 ) {
                                        this.addressForm.get( 'polizasYSeguros' ).setValue( listSeguroCodigo );
                                    }
                                }
                            }

                            // GET lista de chequeo
                            if ( this.contratoPoliza.polizaListaChequeo !== undefined ) {
                                if ( this.contratoPoliza.polizaListaChequeo.length > 0 ) {
                                    const polizaListaChequeo = this.contratoPoliza.polizaListaChequeo[ 0 ];
                                    this.polizaListaChequeoId = polizaListaChequeo.polizaListaChequeoId;

                                    this.addressForm.get( 'cumpleAsegurado' ).setValue( polizaListaChequeo.cumpleDatosAseguradoBeneficiario !== undefined ? polizaListaChequeo.cumpleDatosAseguradoBeneficiario : null );
                                    this.addressForm.get( 'cumpleBeneficiario' ).setValue( polizaListaChequeo.cumpleDatosBeneficiarioGarantiaBancaria !== undefined ? polizaListaChequeo.cumpleDatosBeneficiarioGarantiaBancaria : null );
                                    this.addressForm.get( 'cumpleAfianzado' ).setValue( polizaListaChequeo.cumpleDatosTomadorAfianzado !== undefined ? polizaListaChequeo.cumpleDatosTomadorAfianzado : null );
                                    this.addressForm.get( 'reciboDePago' ).setValue( polizaListaChequeo.tieneReciboPagoDatosRequeridos !== undefined ? polizaListaChequeo.tieneReciboPagoDatosRequeridos : null );
                                    this.addressForm.get( 'condicionesGenerales' ).setValue( polizaListaChequeo.tieneCondicionesGeneralesPoliza !== undefined ? polizaListaChequeo.tieneCondicionesGeneralesPoliza : null );
                                }
                            }

                            // GET revision y aprobacion
                            if ( this.contratoPoliza.polizaObservacion !== undefined ) {
                                if ( this.contratoPoliza.polizaObservacion.length > 0 ) {
                                    this.polizaObservacion = this.contratoPoliza.polizaObservacion;
                                    const revision = this.polizaObservacion.filter( revision => revision.estadoRevisionCodigo === this.estadosRevision.aprobacion );
                                    this.contadorRevision = this.polizaObservacion.length;

                                    if ( revision.length > 0 ) {
                                        const ultimaRevision = revision[ revision.length - 1 ];
            
                                        if ( this.polizaObservacion[ this.polizaObservacion.length - 1 ] === ultimaRevision ) {
                                            this.contadorRevision--;

                                            this.polizaObservacionId = ultimaRevision.polizaObservacionId;
                                            this.addressForm.get( 'fechaRevision' ).setValue( ultimaRevision.fechaRevision !== undefined ? new Date( ultimaRevision.fechaRevision ) : null );
                                            this.addressForm.get( 'estadoRevision' ).setValue( ultimaRevision.estadoRevisionCodigo !== undefined ? ultimaRevision.estadoRevisionCodigo : null );
                                            this.addressForm.get( 'fechaAprob' ).setValue( ultimaRevision.fechaAprobacion !== undefined ? new Date( ultimaRevision.fechaAprobacion ) : null );
                                            this.addressForm.get( 'responsableAprob' ).setValue( ultimaRevision.responsableAprobacionId !== undefined ? ultimaRevision.responsableAprobacionId : null );
                                            this.addressForm.get( 'observacionesGenerales' ).setValue( ultimaRevision.observacion !== undefined ? ultimaRevision.observacion : null );
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            )
    }

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
                            contratoPolizaId: this.contratoPoliza.contratoPolizaId,
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
                    contratoPolizaId: this.contratoPoliza.contratoPolizaId,
                    cumpleDatosAseguradoBeneficiario: this.addressForm.get( 'cumpleAsegurado' ).value,
                    cumpleDatosBeneficiarioGarantiaBancaria: this.addressForm.get( 'cumpleBeneficiario' ).value,
                    cumpleDatosTomadorAfianzado: this.addressForm.get( 'cumpleAfianzado' ).value,
                    tieneReciboPagoDatosRequeridos: this.addressForm.get( 'reciboDePago' ).value,
                    tieneCondicionesGeneralesPoliza: this.addressForm.get( 'condicionesGenerales' ).value
                }
            ];
        }
        const getHistorialObservaciones = () => {
            let fechaAprobacion = this.addressForm.get( 'fechaAprob' ).value !== null ? new Date( this.addressForm.get( 'fechaAprob' ).value ).toISOString() : null;
            let responsableAprobacionId = this.addressForm.get( 'responsableAprob' ).value;

            if ( this.addressForm.get( 'estadoRevision' ).value !== null ) {
                if ( this.addressForm.get( 'estadoRevision' ).value === this.estadosRevision.devuelta ) {
                    fechaAprobacion = null;
                    responsableAprobacionId = null;
                    this.polizaObservacionId = 0;
                }
            }

            return [
                {
                    polizaObservacionId: this.polizaObservacionId,
                    contratoPolizaId: this.contratoPoliza.contratoPolizaId,
                    fechaRevision: this.addressForm.get( 'fechaRevision' ).value !== null ? new Date( this.addressForm.get( 'fechaRevision' ).value ).toISOString() : new Date().toISOString(),
                    estadoRevisionCodigo: this.addressForm.get( 'estadoRevision' ).value !== null ? this.addressForm.get( 'estadoRevision' ).value : this.estadosPoliza.enRevision,
                    fechaAprobacion,
                    responsableAprobacionId,
                    observacion: this.addressForm.get( 'observacionesGenerales' ).value !== null ? this.addressForm.get( 'observacionesGenerales' ).value : null
                }
            ];
        }

        const pContrato = {
            contratoId: this.contrato.contratoId,
            contratoPoliza: [
                {
                    contratoId: this.contrato.contratoId,
                    contratoPolizaId: this.contratoPoliza.contratoPolizaId,
                    nombreAseguradora: this.addressForm.get( 'nombre' ).value,
                    numeroPoliza: this.addressForm.get( 'numeroPoliza' ).value,
                    numeroCertificado: this.addressForm.get( 'numeroCertificado' ).value,
                    fechaExpedicion: this.addressForm.get( 'fecha' ).value !== null ? new Date( this.addressForm.get( 'fecha' ).value ).toISOString() : null,
                    fechaAprobacion: this.addressForm.get( 'fechaAprob' ).value !== null ? new Date( this.addressForm.get( 'fechaAprob' ).value ).toISOString() : null,
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
