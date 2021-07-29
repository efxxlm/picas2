import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-form-aprobar-orden-giro',
  templateUrl: './form-aprobar-orden-giro.component.html',
  styleUrls: ['./form-aprobar-orden-giro.component.scss']
})
export class FormAprobarOrdenGiroComponent implements OnInit {

    solicitudPago: any;
    contrato: any;
    esRegistroNuevo = false;
    esVerDetalle = false;
    esExpensas = false;
    semaforoInformacionGeneral = 'sin-diligenciar';
    semaforoDetalleGiro = 'sin-diligenciar';
    listaModalidadContrato: Dominio[] = [];
    listaDetalleGiro: { contratacionProyectoId: number, llaveMen: string, fases: any[], semaforoDetalle: string }[] = [];

    constructor(
        private activatedRoute: ActivatedRoute,
        private ordenGiroSvc: OrdenPagoService,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        // Verificar si es registro nuevo o ver detalle/editar o ver detalle
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
          if ( urlSegment.path === 'aprobarOrdenGiro' ) {
            this.esRegistroNuevo = true;
          }
          if ( urlSegment.path === 'verDetalle' ) {
            this.esVerDetalle = true;
          }
          if ( urlSegment.path === 'aprobarOrdenGiroExpensas' || urlSegment.path === 'editarOrdenGiroExpensas' ) {
            this.esExpensas = true;
          }
          if ( urlSegment.path === 'verDetalleExpensas' ) {
              this.esExpensas = true;
              this.esVerDetalle = true;
          }
        } );
        // Get lista modalidades de contrato
        this.commonSvc.modalidadesContrato()
            .subscribe( modalidadesContrato => this.listaModalidadContrato = modalidadesContrato );
        // Get solicitud de pago y orden de giro
        this.ordenGiroSvc.getSolicitudPagoBySolicitudPagoId( this.activatedRoute.snapshot.params.id )
            .subscribe(
                async response => {
                    this.solicitudPago = response;
                    this.contrato = response[ 'contratoSon' ];
                    console.log( this.solicitudPago );

                        /*
                            Se crea un arreglo de proyectos asociados a una fase y unos criterios que estan asociados a esa fase y al proyecto para
                            el nuevo flujo de Orden de Giro el cual los acordeones de "Estrategia de pagos, Observaciones y Soporte de orden de giro" ya no son hijos del acordeon
                            "Detalle de giro" y el detalle de giro se diligencia por proyectos el cual tendra como hijo directo las fases y los criterios asociados a esa fase y al proyecto.
                        */
                        // Peticion asincrona de los proyectos por contratoId
                        const getProyectosByIdContrato: any[] = await this.registrarPagosSvc.getProyectosByIdContrato( this.solicitudPago.contratoId ).toPromise();
                        const LISTA_PROYECTOS: any[] = getProyectosByIdContrato[1];
                        const solicitudPagoFase: any[] = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase;

                        LISTA_PROYECTOS.forEach( proyecto => {
                            // Objeto Proyecto que se agregara al array listaDetalleGiro
                            const PROYECTO = {
                                semaforoDetalle: 'sin-diligenciar',
                                contratacionProyectoId: proyecto.contratacionProyectoId,
                                llaveMen: proyecto.llaveMen,
                                fases: []
                            }

                            const listFase = solicitudPagoFase.filter( fase => fase.contratacionProyectoId === proyecto.contratacionProyectoId )
                            if ( listFase.length > 0 ) {
                                listFase.forEach( fase => {
                                    fase.estadoSemaforo = 'sin-diligenciar'
                                    fase.estadoSemaforoCausacion = 'sin-diligenciar'
                                })
                            }
                            PROYECTO.fases = listFase

                            if ( PROYECTO.fases.length > 0 ) {
                                this.listaDetalleGiro.push( PROYECTO )
                            }
                        } )
                }
            );
    }

    ngOnInit(): void {
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.listaModalidadContrato.length > 0 ) {
            const modalidad = this.listaModalidadContrato.find( modalidad => modalidad.codigo === modalidadCodigo );
            
            if ( modalidad !== undefined ) {
                return modalidad.nombre;
            }
        }
    }

    checkSemaforoDetalle( listaSemaforosDetalle: any ) {
        const tieneSinDiligenciar = Object.values( listaSemaforosDetalle ).includes( 'sin-diligenciar' );
        const tieneEnProceso = Object.values( listaSemaforosDetalle ).includes( 'en-proceso' );
        const tieneCompleto = Object.values( listaSemaforosDetalle ).includes( 'completo' );

        if ( tieneEnProceso === true ) {
            this.semaforoDetalleGiro = 'en-proceso';
        }
        if ( tieneSinDiligenciar === true && tieneCompleto === true ) {
            this.semaforoDetalleGiro = 'en-proceso';
        }
        if ( tieneSinDiligenciar === false && tieneEnProceso === false && tieneCompleto === true ) {
            this.semaforoDetalleGiro = 'completo';
        }
    }

}
