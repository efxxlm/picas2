import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';

@Component({
  selector: 'app-form-generar-orden-giro',
  templateUrl: './form-generar-orden-giro.component.html',
  styleUrls: ['./form-generar-orden-giro.component.scss']
})
export class FormGenerarOrdenGiroComponent implements OnInit {

    solicitudPago: any;
    contrato: any;
    modalidadContratoArray: Dominio[] = [];

    constructor(
        private activatedRoute: ActivatedRoute,
        private ordenPagoSvc: OrdenPagoService,
        private commonSvc: CommonService )
    {
        this.commonSvc.modalidadesContrato()
        .subscribe( response => {
            this.modalidadContratoArray = response;
            this.ordenPagoSvc.getSolicitudPagoBySolicitudPagoId( this.activatedRoute.snapshot.params.id )
                .subscribe(
                    response => {
                        this.solicitudPago = response;
                        this.contrato = response[ 'contratoSon' ];
                        console.log( this.solicitudPago, this.contrato );
                    }
                );
        } );
    }

    ngOnInit(): void {
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.modalidadContratoArray.length > 0 ) {
            const modalidad = this.modalidadContratoArray.filter( modalidad => modalidad.codigo === modalidadCodigo );
            return modalidad[0].nombre;
        }
    }

}
