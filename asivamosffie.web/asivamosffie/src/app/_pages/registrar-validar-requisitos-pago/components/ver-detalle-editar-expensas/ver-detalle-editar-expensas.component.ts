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
              }
            );
        this.registrarPagosSvc.getSolicitudPago( this.activatedRoute.snapshot.params.id )
            .subscribe(
                response => {
                    console.log( response );
                }
            );
    }

    ngOnInit(): void {
    }

}
