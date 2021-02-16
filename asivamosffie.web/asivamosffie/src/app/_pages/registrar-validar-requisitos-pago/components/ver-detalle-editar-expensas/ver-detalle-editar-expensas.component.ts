import { ActivatedRoute } from '@angular/router';
import { CommonService } from './../../../../core/_services/common/common.service';
import { Component, OnInit } from '@angular/core';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-ver-detalle-editar-expensas',
  templateUrl: './ver-detalle-editar-expensas.component.html',
  styleUrls: ['./ver-detalle-editar-expensas.component.scss']
})
export class VerDetalleEditarExpensasComponent implements OnInit {

    tipoSolicitudCodigo: any = {};
    solicitudPago: any;
    registroCompletoAcordeones = {
        registroCompletoListaChequeo: false
    };
    estadoSemaforoAcordeon = {
        estadoSemaforoListaChequeo: 'sin-diligenciar',
        estadoSemaforoSoporteSolicitud: 'en-alerta'
    };

    constructor(
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private activatedRoute: ActivatedRoute )
    {
        this.commonSvc.tiposDeSolicitudes()
            .subscribe(
                solicitudes => {
                    for ( const solicitud of solicitudes ) {
                        if ( solicitud.codigo === '1' ) {
                            this.tipoSolicitudCodigo.contratoObra = solicitud.codigo;
                        }
                        if ( solicitud.codigo === '2' ) {
                            this.tipoSolicitudCodigo.contratoInterventoria = solicitud.codigo;
                        }
                        if ( solicitud.codigo === '3' ) {
                            this.tipoSolicitudCodigo.expensas = solicitud.codigo;
                        }
                        if ( solicitud.codigo === '4' ) {
                            this.tipoSolicitudCodigo.otrosCostos = solicitud.codigo;
                        }
                    }
                    this.registrarPagosSvc.getSolicitudPago( this.activatedRoute.snapshot.params.id )
                        .subscribe(
                            response => {
                                console.log( response );
                                this.solicitudPago = response;
                                // Get semaforo lista de chequeo
                                const solicitudPagoListaChequeo: any[] = this.solicitudPago.solicitudPagoListaChequeo;
                                let totalRegistroCompleto = 0;
                    
                                if ( solicitudPagoListaChequeo.length > 0 ) {
                                    solicitudPagoListaChequeo.forEach( listaChequeo => {
                                        if ( listaChequeo.registroCompleto === true ) {
                                            totalRegistroCompleto++;
                                        }
                                    } );
                        
                                    if ( totalRegistroCompleto > 0 && totalRegistroCompleto < solicitudPagoListaChequeo.length ) {
                                        this.estadoSemaforoAcordeon.estadoSemaforoListaChequeo = 'en-proceso';
                                    }
                                    if ( totalRegistroCompleto > 0 && totalRegistroCompleto === solicitudPagoListaChequeo.length ) {
                                        this.estadoSemaforoAcordeon.estadoSemaforoListaChequeo = 'completo';
                                        this.registroCompletoAcordeones.registroCompletoListaChequeo = true
                                    }
                                }
                                // Get semaforo soporte de la solicitud
                                const solicitudPagoSoporteSolicitud = this.solicitudPago.solicitudPagoSoporteSolicitud[0];

                                if ( this.registroCompletoAcordeones.registroCompletoListaChequeo === true ) {
                                    if ( solicitudPagoSoporteSolicitud === undefined ) {
                                        this.estadoSemaforoAcordeon.estadoSemaforoSoporteSolicitud = 'sin-diligenciar';
                                    }
                                    if ( solicitudPagoSoporteSolicitud !== undefined ) {
                                        if ( solicitudPagoSoporteSolicitud.registroCOmpleto === false ) {
                                            this.estadoSemaforoAcordeon.estadoSemaforoSoporteSolicitud = 'en-proceso';
                                        }
    
                                        if ( solicitudPagoSoporteSolicitud.registroCompleto === true ) {
                                            this.estadoSemaforoAcordeon.estadoSemaforoSoporteSolicitud = 'completo';
                                        }
                                    }
                                }
                            }
                        );
                }
            );
    }

    ngOnInit(): void {
    }

}
