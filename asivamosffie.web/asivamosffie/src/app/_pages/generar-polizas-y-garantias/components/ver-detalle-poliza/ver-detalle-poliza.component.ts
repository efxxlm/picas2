import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import humanize from 'humanize-plus';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { EstadosRevision, PerfilCodigo } from 'src/app/_interfaces/estados-actualizacion-polizas.interface';
import { EstadoPolizaCodigo } from 'src/app/_interfaces/gestionar-polizas-garantias.interface';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-ver-detalle-poliza',
  templateUrl: './ver-detalle-poliza.component.html',
  styleUrls: ['./ver-detalle-poliza.component.scss']
})
export class VerDetallePolizaComponent implements OnInit {

    addressForm = this.fb.group({
        nombre: [ null, Validators.compose([ Validators.required, Validators.minLength(1), Validators.maxLength(200)]) ],
        numeroPoliza: [ null, Validators.compose([ Validators.required, Validators.minLength(2), Validators.maxLength(100) ]) ],
        numeroCertificado: [ null, Validators.compose([ Validators.required, Validators.minLength(2), Validators.maxLength(100)]) ],
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
    ultimaRevision: any;
    listaUsuarios: any[] = [];
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
        this.getContratoPoliza();
    }

    ngOnInit(): void {
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
                                    this.ultimaRevision = this.polizaObservacion[ this.polizaObservacion.length - 1 ];
                                }
                            }
                        }
                    }
                }
            )
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

    getEstado( codigo: string ) {
        if ( this.estadoArray.length > 0 ) {
            const estado = this.estadoArray.find( estado => estado.codigo === codigo );

            if ( estado !== undefined ) {
                return estado.nombre;
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

    getSeguro( codigo: string ) {
        if ( this.polizasYSegurosArray.length > 0 ) {
            const seguro = this.polizasYSegurosArray.find( seguro => seguro.codigo === codigo );

            if ( seguro !== undefined ) {
                return seguro.nombre;
            }
        }
    }

    getResponsable( usuarioId: number ) {
        if ( this.listaUsuarios.length > 0 ) {
            const responsable = this.listaUsuarios.find( responsable => responsable.usuarioId === usuarioId );

            if ( responsable !== undefined ) {
                return `${ this.firstLetterUpperCase( responsable.primerNombre ) } ${ this.firstLetterUpperCase( responsable.primerApellido ) }`;
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

}
