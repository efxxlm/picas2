import { FormArray, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { EstadosRevision, PerfilCodigo, TipoActualizacionCodigo } from 'src/app/_interfaces/estados-actualizacion-polizas.interface';
import humanize from 'humanize-plus';
import { ActualizarPolizasService } from 'src/app/core/_services/actualizarPolizas/actualizar-polizas.service';

@Component({
  selector: 'app-actualizacion-poliza-rlc',
  templateUrl: './actualizacion-poliza-rlc.component.html',
  styleUrls: ['./actualizacion-poliza-rlc.component.scss']
})
export class ActualizacionPolizaRlcComponent implements OnInit {

    @Input() contrato: any;
    contratoPoliza: any;
    esRegistroNuevo: boolean;
    esVerDetalle: boolean;
    contratoPolizaActualizacion: any;
    contratoPolizaActualizacionSeguro: any;
    contratoPolizaActualizacionListaChequeo: any;
    contratoPolizaActualizacionRevisionAprobacionObservacion: any[] = [];
    estadosRevision = EstadosRevision;
    listaPerfilCodigo = PerfilCodigo;
    listaTipoActualizacion = TipoActualizacionCodigo;
    listaUsuarios: any[] = [];
    listaTipoSolicitudContrato: Dominio[] = [];
    polizasYSegurosArray: Dominio[] = [];
    listaTipoDocumento: Dominio[] = [];
    razonActualizacionArray : Dominio[] = []
    tipoActualizacionArray: Dominio[] = [];
    estadoArray: Dominio[] = [];
    responsable: any;
    dataSource = new MatTableDataSource();
    ultimaRevisionAprobada: any;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'polizaYSeguros',
      'responsableAprobacion'
    ]
    detalleRazon = this.fb.group({
        contratoPolizaActualizacionId: [ 0 ],
        razonActualizacion: [ null ],
        fechaExpedicion: [ null ],
        polizasYSeguros: [ null ],
        segurosRazon: this.fb.array( [] ),
        seguros: this.fb.array( [] )
    });
    addressForm = this.fb.group( {
        contratoPolizaActualizacionId: [ 0 ],
        contratoPolizaActualizacionListaChequeoId: [ 0 ],
        cumpleAsegurado: [ null ],
        cumpleBeneficiario: [ null ],
        cumpleAfianzado: [ null ],
        reciboDePago: [ null ],
        condicionesGenerales: [ null ],
        fechaRevision: [ null ],
        estadoRevision: [ null ],
        fechaAprob: [ null ],
        responsableAprob: [ null ]
    } );

    get segurosRazon() {
        return this.detalleRazon.get( 'segurosRazon' ) as FormArray;
    }
    get seguros() {
        return this.detalleRazon.get( 'seguros' ) as FormArray;
    }

    constructor(
        private commonSvc: CommonService,
        private fb: FormBuilder,
        private actualizarPolizaSvc: ActualizarPolizasService )
    { }

    ngOnInit(): void {
        this.getContratoPoliza();
    }

    async getContratoPoliza() {
        this.listaUsuarios = await this.commonSvc.getUsuariosByPerfil( this.listaPerfilCodigo.fiduciaria ).toPromise();
        this.polizasYSegurosArray = await this.commonSvc.listaGarantiasPolizas().toPromise();
        this.listaTipoSolicitudContrato = await this.commonSvc.listaTipoSolicitudContrato().toPromise();
        this.listaTipoDocumento = await this.commonSvc.listaTipodocumento().toPromise();
        this.estadoArray = await this.commonSvc.listaEstadoRevision().toPromise();
        this.razonActualizacionArray = await this.commonSvc.listaRazonActualizacion().toPromise();
        this.tipoActualizacionArray = await this.commonSvc.listaTipoActualizacion().toPromise();

        this.actualizarPolizaSvc.getContratoPoliza( this.contrato.contratoPoliza[ 0 ].contratoPolizaId , false)
            .subscribe(
                response => {
                    this.contratoPoliza = response;
                    this.responsable = this.contratoPoliza.userResponsableAprobacion;
                    console.log( this.contratoPoliza );

                    if ( this.contratoPoliza.polizaObservacion.length > 0 ) {
                        const polizaAprobada: any[] = this.contratoPoliza.polizaObservacion.filter( polizaAprobada => polizaAprobada.estadoRevisionCodigo === this.estadosRevision.aprobacion );

                        if ( polizaAprobada.length > 0 ) {
                            this.ultimaRevisionAprobada = polizaAprobada[ polizaAprobada.length - 1 ];
                        }
                    }

                    if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
                        if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                            this.contratoPolizaActualizacion = this.contratoPoliza.contratoPolizaActualizacion[ 0 ];
                        }
                    }

                    this.dataSource = new MatTableDataSource( this.contratoPoliza.polizaGarantia );
                    this.getRazonTipoActualizacion();
                }
            )
    }

    getRazonTipoActualizacion() {
        this.polizasYSegurosArray.forEach( ( poliza, index ) => {
            const garantia = this.contratoPoliza.polizaGarantia.find( garantia => garantia.tipoGarantiaCodigo === poliza.codigo );

            if ( garantia === undefined ) {
                this.polizasYSegurosArray.splice( index, 1 );
            }
        } )

        if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
            if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                this.detalleRazon.get( 'contratoPolizaActualizacionId' ).setValue( this.contratoPolizaActualizacion.contratoPolizaActualizacionId );
                this.detalleRazon.get( 'razonActualizacion' ).setValue( this.contratoPolizaActualizacion.razonActualizacionCodigo !== undefined ? this.razonActualizacionArray.find( razon => razon.codigo === this.contratoPolizaActualizacion.razonActualizacionCodigo ).codigo : null );
                this.detalleRazon.get( 'fechaExpedicion' ).setValue( this.contratoPolizaActualizacion.fechaExpedicionActualizacionPoliza !== undefined ? new Date( this.contratoPolizaActualizacion.fechaExpedicionActualizacionPoliza ) : null )

                if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro !== undefined ) {
                    if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro.length > 0 ) {
                        this.contratoPolizaActualizacionSeguro = this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro;
                        const segurosSeleccionados = [];

                        for ( const seguro of this.contratoPolizaActualizacionSeguro ) {
                            const poliza = this.polizasYSegurosArray.find( poliza => poliza.codigo === seguro.tipoSeguroCodigo );
                            const actualizacionSeleccionada = [];

                            if ( seguro.tieneFechaSeguro === true ) {
                                actualizacionSeleccionada.push( this.listaTipoActualizacion.seguros );
                            }
                            if ( seguro.tieneFechaVigenciaAmparo === true ) {
                                actualizacionSeleccionada.push( this.listaTipoActualizacion.fecha );
                            }
                            if ( seguro.tieneValorAmparo === true ) {
                                actualizacionSeleccionada.push( this.listaTipoActualizacion.valor );
                            }

                            segurosSeleccionados.push( seguro.tipoSeguroCodigo );

                            if ( poliza !== undefined ) {
                                this.segurosRazon.push( this.fb.group(
                                    {
                                        contratoPolizaActualizacionSeguroId: [ seguro.contratoPolizaActualizacionSeguroId ],
                                        nombre: [ poliza.nombre ],
                                        codigo: [ poliza.codigo ],
                                        tipoActualizacion: [ actualizacionSeleccionada ]
                                    }
                                ) )
                            }
                        }

                        this.detalleRazon.get( 'polizasYSeguros' ).setValue( segurosSeleccionados );
                    }
                }
            }
        }
        this.getVigenciaValor();
    }

    getVigenciaValor() {

        if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
            if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro !== undefined ) {
                    if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro.length > 0 ) {
                        const polizaGarantia: any[] = this.contratoPoliza.polizaGarantia.length > 0 ? this.contratoPoliza.polizaGarantia : [];

                        for ( const seguro of this.contratoPolizaActualizacionSeguro ) {
                            const seguroPoliza = polizaGarantia.find( seguroPoliza => seguroPoliza.tipoGarantiaCodigo === seguro.tipoSeguroCodigo );

                            let semaforo = 'sin-diligenciar';

                            if ( seguro.registroCompletoSeguro === false ) {
                                semaforo = 'en-proceso';
                            }
                            if ( seguro.registroCompletoSeguro === true ) {
                                semaforo = 'completo';
                            }

                            if ( seguroPoliza !== undefined ) {
                                this.seguros.push( this.fb.group(
                                    {
                                        semaforo,
                                        seguroPoliza,
                                        nombre: [ this.polizasYSegurosArray.find( poliza => poliza.codigo === seguro.tipoSeguroCodigo ).nombre ],
                                        codigo: [ seguro.tipoSeguroCodigo ],
                                        tieneSeguro: [ seguro.tieneFechaSeguro ],
                                        fechaSeguro: [ seguro.fechaSeguro !== undefined ? new Date( seguro.fechaSeguro ) : null ],
                                        tieneFechaAmparo: [ seguro.tieneFechaVigenciaAmparo ],
                                        fechaAmparo: [ seguro.fechaVigenciaAmparo !== undefined ? new Date( seguro.fechaVigenciaAmparo ) : null ],
                                        tieneValorAmparo: [ seguro.tieneValorAmparo ],
                                        valorAmparo: [ seguro.valorAmparo !== undefined ? seguro.valorAmparo : null ]
                                    }
                                ) )
                            }
                        }
                    }
                }
            }
        }
        this.getListaChequeo();
    }

    getListaChequeo() {
        if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
            if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo !== undefined ) {
                    if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo.length > 0 ) {
                        this.contratoPolizaActualizacionListaChequeo = this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo[ 0 ];

                        this.addressForm.get( 'contratoPolizaActualizacionListaChequeoId' ).setValue( this.contratoPolizaActualizacionListaChequeo.contratoPolizaActualizacionListaChequeoId );
                        this.addressForm.get( 'cumpleAsegurado' ).setValue( this.contratoPolizaActualizacionListaChequeo.cumpleDatosAseguradoBeneficiario !== undefined ? this.contratoPolizaActualizacionListaChequeo.cumpleDatosAseguradoBeneficiario : null );
                        this.addressForm.get( 'cumpleBeneficiario' ).setValue( this.contratoPolizaActualizacionListaChequeo.cumpleDatosBeneficiarioGarantiaBancaria !== undefined ? this.contratoPolizaActualizacionListaChequeo.cumpleDatosBeneficiarioGarantiaBancaria : null );
                        this.addressForm.get( 'cumpleAfianzado' ).setValue( this.contratoPolizaActualizacionListaChequeo.cumpleDatosTomadorAfianzado !== undefined ? this.contratoPolizaActualizacionListaChequeo.cumpleDatosTomadorAfianzado : null );
                        this.addressForm.get( 'reciboDePago' ).setValue( this.contratoPolizaActualizacionListaChequeo.tieneReciboPagoDatosRequeridos !== undefined ? this.contratoPolizaActualizacionListaChequeo.tieneReciboPagoDatosRequeridos : null );
                        this.addressForm.get( 'condicionesGenerales' ).setValue( this.contratoPolizaActualizacionListaChequeo.tieneCondicionesGeneralesPoliza !== undefined ? this.contratoPolizaActualizacionListaChequeo.tieneCondicionesGeneralesPoliza : null );
                    }
                }
            }
        }
        this.getRevision();
    }

    getRevision() {
        if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
            if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                this.contratoPolizaActualizacion = this.contratoPoliza.contratoPolizaActualizacion[ 0 ];
                this.addressForm.get( 'contratoPolizaActualizacionId' ).setValue( this.contratoPolizaActualizacion.contratoPolizaActualizacionId );

                if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion !== undefined ) {
                    if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion.length > 0 ) {
                        this.contratoPolizaActualizacionRevisionAprobacionObservacion = this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion;

                        const revision = this.contratoPolizaActualizacionRevisionAprobacionObservacion.filter( revision => revision.estadoSegundaRevision === this.estadosRevision.aprobacion );

                        if ( revision.length > 0 ) {
                            const ultimaRevision = revision[ revision.length - 1 ];

                            if ( this.contratoPolizaActualizacionRevisionAprobacionObservacion[ this.contratoPolizaActualizacionRevisionAprobacionObservacion.length - 1 ] === ultimaRevision ) {

                                this.addressForm.get( 'fechaRevision' ).setValue( ultimaRevision.segundaFechaRevision !== undefined ? new Date( ultimaRevision.segundaFechaRevision ) : null )
                                this.addressForm.get( 'estadoRevision' ).setValue( ultimaRevision.estadoSegundaRevision !== undefined ? ultimaRevision.estadoSegundaRevision : null )
                                this.addressForm.get( 'fechaAprob' ).setValue( ultimaRevision.fechaAprobacion !== undefined ? ultimaRevision.fechaAprobacion : null )
                                this.addressForm.get( 'responsableAprob' ).setValue( ultimaRevision.responsableAprobacionId !== undefined ? ultimaRevision.responsableAprobacionId : null )
                            }
                        }
                    }
                }
            }
        }
    }

    getTipoSolicitudContrato( tipoSolicitudCodigo: string ) {
        if ( this.listaTipoSolicitudContrato.length > 0 ) {
            const solicitud = this.listaTipoSolicitudContrato.find( solicitud => solicitud.codigo === tipoSolicitudCodigo );

            if ( solicitud !== undefined ) {
                return solicitud.nombre;
            }
        }
    }

    getPoliza( codigo: string ) {
        if ( this.polizasYSegurosArray.length > 0 ) {
            const poliza = this.polizasYSegurosArray.find( poliza => poliza.codigo === codigo );

            if ( poliza !== undefined ) {
                return poliza.nombre;
            }
        }
    }

    getResponsable( usuarioId: number ) {
        const responsable = this.listaUsuarios.find( usuario => usuario.usuarioId === usuarioId );

        if ( responsable !== undefined ) {
            return `${ this.firstLetterUpperCase( responsable.primerNombre ) } ${ this.firstLetterUpperCase( responsable.primerApellido ) }`;
        }
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
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

    getRazonActualizacion( codigo: string ) {
        if ( this.razonActualizacionArray.length > 0 ) {
            const razon = this.razonActualizacionArray.find( razon => razon.codigo === codigo );

            if ( razon !== undefined ) {
                return razon.nombre;
            }
        }
    }

    getSeguros( codigo: string ) {
        if ( this.polizasYSegurosArray.length > 0 ) {
            const seguro = this.polizasYSegurosArray.find( seguro => seguro.codigo === codigo );

            if ( seguro !== undefined ) {
                return seguro.nombre;
            }
        }
    }

    getTipoActualizacion( codigo: string ) {
        if ( this.tipoActualizacionArray.length > 0 ) {
            const tipo = this.tipoActualizacionArray.find( tipo => tipo.codigo === codigo );

            if ( tipo !== undefined ) {
                return tipo.nombre;
            }
        }
    }

    getEstadoRevision( codigo: string ) {
        if ( this.estadoArray.length > 0 ) {
            const estado = this.estadoArray.find( estado => estado.codigo === codigo );

            if ( estado !== undefined ) {
                return estado.nombre;
            }
        }
    }

}
