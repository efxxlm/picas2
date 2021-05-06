import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { OrdenPagoService } from './../../../../core/_services/ordenPago/orden-pago.service';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ListaMenu, ListaMenuId } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-form-verificar-orden-giro',
  templateUrl: './form-verificar-orden-giro.component.html',
  styleUrls: ['./form-verificar-orden-giro.component.scss']
})
export class FormVerificarOrdenGiroComponent implements OnInit {

    listaMenu: ListaMenu = ListaMenuId;
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
            if ( urlSegment.path === 'verificarOrdenGiro' ) {
                this.esRegistroNuevo = true;
                return;
            }
            if ( urlSegment.path === 'verDetalle' ) {
                this.esVerDetalle = true;
                return;
            }
            if ( urlSegment.path === 'verificarOrdenGiroExpensas' || urlSegment.path === 'editarOrdenGiroExpensas' ) {
                this.esExpensas = true;
                return;
            }
            if ( urlSegment.path === 'verDetalleExpensas' ) {
                this.esExpensas = true;
                this.esVerDetalle = true;
                return;
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
