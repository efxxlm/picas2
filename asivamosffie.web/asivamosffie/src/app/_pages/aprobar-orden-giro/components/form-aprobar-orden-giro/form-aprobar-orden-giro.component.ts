import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';

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

    constructor(
        private activatedRoute: ActivatedRoute,
        private ordenGiroSvc: OrdenPagoService,
        private commonSvc: CommonService )
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
                response => {
                    this.solicitudPago = response;
                    this.contrato = response[ 'contratoSon' ];
                    console.log( this.solicitudPago );

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
