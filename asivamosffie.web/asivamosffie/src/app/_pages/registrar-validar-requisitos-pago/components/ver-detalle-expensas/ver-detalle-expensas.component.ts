import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-ver-detalle-expensas',
  templateUrl: './ver-detalle-expensas.component.html',
  styleUrls: ['./ver-detalle-expensas.component.scss']
})
export class VerDetalleExpensasComponent implements OnInit {

    listaEstadoSolicitudPago: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    tipoSolicitudCodigo: any = {};
    solicitudPago: any;

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
                        }
                    );
              }
            );
    }

    ngOnInit(): void {
    }

}
