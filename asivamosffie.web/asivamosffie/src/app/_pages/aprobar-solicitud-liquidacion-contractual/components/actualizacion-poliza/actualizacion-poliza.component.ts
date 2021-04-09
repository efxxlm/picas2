import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Params } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { EstadosRevision, TipoActualizacion, TipoActualizacionCodigo } from 'src/app/_interfaces/estados-actualizacion-polizas.interface';
import { ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId, TipoObservacionLiquidacionContrato, TipoObservacionLiquidacionContratoCodigo } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';
import humanize from 'humanize-plus';

@Component({
  selector: 'app-actualizacion-poliza',
  templateUrl: './actualizacion-poliza.component.html',
  styleUrls: ['./actualizacion-poliza.component.scss']
})
export class ActualizacionPolizaComponent implements OnInit {

  listaTipoObservacionLiquidacionContratacion: TipoObservacionLiquidacionContrato = TipoObservacionLiquidacionContratoCodigo;
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;

  @Input() contratoPolizaId : number;
  @Input() contratoPolizaActualizacionId : number;
  @Input() contratacionProyectoId : number;
  @Input() esVerDetalle: boolean;
  @Input() data: any;
  @Output() semaforoActualizacionPoliza = new EventEmitter<string>();

  contratoPoliza: any;
  polizasYSegurosArray: Dominio[] = [];
  responsable: any;
  listaPolizas = [];

  //razón y tipo de actualización
  contratoPolizaActualizacion: any;
  contratoPolizaActualizacionSeguro: any;
  contratoPolizaActualizacionListaChequeo: any;
  contratoPolizaActualizacionRevisionAprobacionObservacion: any[] = [];
  razonActualizacion: any;
  fechaExpedicion:any;
  razonActualizacionArray : Dominio[] = [];
  tipoActualizacionArray: Dominio[] = [];
  listaTipoDocumento: Dominio[] = [];
  listaTipoActualizacion: TipoActualizacion = TipoActualizacionCodigo;
  estadoArray: Dominio[] = [];

  segurosRazon = [];
  seguros = [];
  polizasyseguros = [];
  //observaciones
  tieneObservaciones: any;
  observacionesEspecificas: any;
  //lista de chequeo
  listaChequeoData : any;
  //revisión
  contadorRevision = 0;
  estadosRevision = EstadosRevision;
  dataRevision: any;
  displayedColumns: string[] = ['fechaRevision', 'observacion', 'estadoRevisionCodigo'];
  dataSource = new MatTableDataSource();

  constructor(
    private route: ActivatedRoute,
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService,
    private commonSvc: CommonService
  ) {
    this.getContratoPoliza();
  }

   ngOnInit(): void {
  }  

  async getContratoPoliza() {
    this.polizasYSegurosArray = await this.commonSvc.listaGarantiasPolizas().toPromise();
    this.razonActualizacionArray = await this.commonSvc.listaRazonActualizacion().toPromise();
    this.tipoActualizacionArray = await this.commonSvc.listaTipoActualizacion().toPromise();
    this.listaTipoDocumento = await this.commonSvc.listaTipodocumento().toPromise();
    this.commonSvc.listaEstadoRevision()
    .subscribe( listaEstadoRevision => this.estadoArray = listaEstadoRevision );

    this.registerContractualLiquidationRequestService.getContratoPoliza( this.contratoPolizaId , this.listaMenu.aprobarSolicitudLiquidacionContratacion, this.contratacionProyectoId)
        .subscribe(
            response => {
                this.contratoPoliza = response;
                this.listaPolizas = this.contratoPoliza.polizaGarantia;
                this.responsable = this.contratoPoliza.userResponsableAprobacion;
                if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
                  if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                      this.contratoPolizaActualizacion = this.contratoPoliza.contratoPolizaActualizacion[0];
                      this.contratoPolizaActualizacionId = this.contratoPolizaActualizacion.contratoPolizaActualizacionId;
                      this.razonActualizacion = this.contratoPolizaActualizacion.razonActualizacionCodigo !== undefined ? this.razonActualizacionArray.find( razon => razon.codigo === this.contratoPolizaActualizacion.razonActualizacionCodigo ).codigo : null;
                      this.fechaExpedicion = this.contratoPolizaActualizacion.fechaExpedicionActualizacionPoliza !== undefined ? new Date( this.contratoPolizaActualizacion.fechaExpedicionActualizacionPoliza ) : null;
                      
                      this.tieneObservaciones =  this.contratoPolizaActualizacion.tieneObservacionEspecifica !== undefined ? this.contratoPolizaActualizacion.tieneObservacionEspecifica : null;
                      this.observacionesEspecificas = this.contratoPolizaActualizacion.observacionEspecifica !== undefined ? this.contratoPolizaActualizacion.observacionEspecifica : null;

                    //Razón y tipo de actualización / vigencias
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
                                  //seguros acordeón vgencias y valor
                                  const seguroVigencias = {
                                    nombre: this.polizasYSegurosArray.find( poliza => poliza.codigo === seguro.tipoSeguroCodigo ).nombre,
                                    codigo: seguro.tipoSeguroCodigo,
                                    tieneSeguro: seguro.tieneFechaSeguro,
                                    fechaSeguro: seguro.fechaSeguro !== undefined ? new Date( seguro.fechaSeguro ) : null,
                                    tieneFechaAmparo: seguro.tieneFechaVigenciaAmparo,
                                    fechaAmparo: seguro.fechaVigenciaAmparo !== undefined ? new Date( seguro.fechaVigenciaAmparo ) : null,
                                    tieneValorAmparo: seguro.tieneValorAmparo,
                                    valorAmparo: seguro.valorAmparo !== undefined ? seguro.valorAmparo : null 
                                  }
                                  this.seguros.push(
                                    seguroVigencias
                                  );
                                  if ( poliza !== undefined ) {
                                    const element = {
                                      contratoPolizaActualizacionSeguroId: seguro.contratoPolizaActualizacionSeguroId,
                                      nombre: poliza.nombre,
                                      codigo: poliza.codigo,
                                      tipoActualizacion: actualizacionSeleccionada
                                    
                                    };
                                    this.segurosRazon.push(
                                        element
                                    );
                                  }
                              }
                              this.polizasyseguros = segurosSeleccionados;
                          }
                      }
                    //lista de chequeo
                    if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo !== undefined ) {
                      if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo.length > 0 ) {
                          this.contratoPolizaActualizacionListaChequeo = this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo[ 0 ];
                          const listaChequeoData = {
                            cumpleAsegurado:this.contratoPolizaActualizacionListaChequeo.cumpleDatosAseguradoBeneficiario !== undefined ? this.contratoPolizaActualizacionListaChequeo.cumpleDatosAseguradoBeneficiario : null,
                            cumpleBeneficiario:this.contratoPolizaActualizacionListaChequeo.cumpleDatosBeneficiarioGarantiaBancaria !== undefined ? this.contratoPolizaActualizacionListaChequeo.cumpleDatosBeneficiarioGarantiaBancaria : null,
                            cumpleAfianzado:this.contratoPolizaActualizacionListaChequeo.cumpleDatosTomadorAfianzado !== undefined ? this.contratoPolizaActualizacionListaChequeo.cumpleDatosTomadorAfianzado : null,
                            reciboDePago: this.contratoPolizaActualizacionListaChequeo.tieneReciboPagoDatosRequeridos !== undefined ? this.contratoPolizaActualizacionListaChequeo.tieneReciboPagoDatosRequeridos : null,
                            condicionesGenerales: this.contratoPolizaActualizacionListaChequeo.tieneCondicionesGeneralesPoliza !== undefined ? this.contratoPolizaActualizacionListaChequeo.tieneCondicionesGeneralesPoliza : null,
                          }
                          this.listaChequeoData = listaChequeoData;
                      }
                    }
                    //revisión
                    if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion !== undefined ) {
                      if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion.length > 0 ) {
                          this.contratoPolizaActualizacionRevisionAprobacionObservacion = this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion;
                          this.dataSource = new MatTableDataSource( this.contratoPolizaActualizacionRevisionAprobacionObservacion );

                          this.contadorRevision = this.contratoPolizaActualizacionRevisionAprobacionObservacion.length;
  
                          const revision = this.contratoPolizaActualizacionRevisionAprobacionObservacion.filter( revision => revision.estadoSegundaRevision === this.estadosRevision.aprobacion );
  
                          if ( revision.length > 0 ) {
                              const ultimaRevision = revision[ revision.length - 1 ];
  
                              if ( this.contratoPolizaActualizacionRevisionAprobacionObservacion[ this.contratoPolizaActualizacionRevisionAprobacionObservacion.length - 1 ] === ultimaRevision ) {
                                  this.contadorRevision--;
                                  const dataRevision = {
                                    fechaRevision: ultimaRevision.segundaFechaRevision !== undefined ? new Date( ultimaRevision.segundaFechaRevision ) : null,
                                    estadoRevision: ultimaRevision.estadoSegundaRevision !== undefined ? ultimaRevision.estadoSegundaRevision : null,
                                    fechaAprob: ultimaRevision.fechaAprobacion !== undefined ? ultimaRevision.fechaAprobacion : null,
                                    responsableAprob: ultimaRevision.responsableAprobacionId !== undefined ? ultimaRevision.responsableAprobacionId : null,
                                    observacionesGenerales: ultimaRevision.observacionGeneral !== undefined ? ultimaRevision.observacionGeneral : null
                                  }
                                  this.dataRevision = dataRevision;
                              }
                          }
                      }
                    }
                    
                    this.semaforoActualizacionPoliza.emit(this.contratoPoliza.registroCompleto ? 'Completo' : 'Incompleto');

                  }
              }
            }
        )
    }

  getPoliza( codigo: string ) {
    if ( this.polizasYSegurosArray.length > 0 ) {
        const poliza = this.polizasYSegurosArray.find( poliza => poliza.codigo === codigo );

        if ( poliza !== undefined ) {
            return poliza.nombre;
        }
    }
  }

  getResponsable() {
      if ( this.responsable !== undefined ) {
          return `${ this.firstLetterUpperCase( this.responsable.primerNombre ) } ${ this.firstLetterUpperCase( this.responsable.primerApellido ) }`
      }
  }

  firstLetterUpperCase( texto:string ) {
    if ( texto !== undefined ) {
        return humanize.capitalize( String( texto ).toLowerCase() );
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

  getTipoDocumento( codigo: string ) {
    if ( this.listaTipoDocumento.length > 0 ) {
        const tipo = this.listaTipoDocumento.find( tipo => tipo.codigo === codigo );

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
