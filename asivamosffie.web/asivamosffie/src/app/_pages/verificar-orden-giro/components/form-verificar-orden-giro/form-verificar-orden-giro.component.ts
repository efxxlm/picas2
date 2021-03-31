import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { OrdenPagoService } from './../../../../core/_services/ordenPago/orden-pago.service';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-verificar-orden-giro',
  templateUrl: './form-verificar-orden-giro.component.html',
  styleUrls: ['./form-verificar-orden-giro.component.scss']
})
export class FormVerificarOrdenGiroComponent implements OnInit {

    solicitudPago: any;
    contrato: any;
    esRegistroNuevo = false;
    esVerDetalle = false;
    esExpensas = false;
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

}
