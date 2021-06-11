import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params, Router, UrlSegment } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { EstadosSolicitudLiquidacionContractual, EstadosSolicitudLiquidacionContractualCodigo, ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId, TipoObservacionLiquidacionContrato, TipoObservacionLiquidacionContratoCodigo } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';
import { InformeFinal } from 'src/app/_interfaces/informe-final';

@Component({
  selector: 'app-verificar-informe-gtlc',
  templateUrl: './verificar-informe-gtlc.component.html',
  styleUrls: ['./verificar-informe-gtlc.component.scss']
})
export class VerificarInformeGtlcComponent implements OnInit {

  proyectoId: number;
  contratacionId: number;
  datosTabla = [];
  data: any;
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  listaTipoObservacionLiquidacionContratacion: TipoObservacionLiquidacionContrato = TipoObservacionLiquidacionContratoCodigo;
  registroCompleto : string;
  informeFinal: InformeFinal;
  esRegistroNuevo: boolean;
  esVerDetalle: boolean;

  constructor(
    private route: ActivatedRoute,
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService,
    private routes: Router
  ) { 
    this.route.params.subscribe((params: Params) => {
      this.proyectoId = params.proyectoId;
      this.contratacionId = params.id;
    });
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'verificarInformeFinal' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = true;
          return;
      }
      if ( urlSegment.path === 'verDetalleEditarInformeFinal' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = false;
          return;
      }
      if ( urlSegment.path === 'verDetalleInformeFinal' ) {
          this.esVerDetalle = true;
          return;
      }
    });
  }

  ngOnInit(): void {
    this.getInformeFinalByProyectoId(this.proyectoId, this.contratacionId);
  }


  getInformeFinalByProyectoId(proyectoId: number, contratacionId: number) {
    this.registerContractualLiquidationRequestService.getInformeFinalByProyectoId(proyectoId, contratacionId, this.listaMenu.gestionarSolicitudLiquidacionContratacion).subscribe(report => {
      if(report != null){
        report.forEach(element => {
          this.datosTabla.push({
            nombreContratistaInterventoria: element.nombreContratistaInterventoria,
            nombreContratistaObra: element.nombreContratistaObra,
            numeroContratoInterventoria: element.numeroContratoInterventoria,
            numeroContratoObra: element.numeroContratoObra,
            informeFinal: element.informeFinal
          });
        })
        if(this.datosTabla.length > 0){
          this.data = this.datosTabla[0];
          this.informeFinal = this.data.informeFinal;
          this.registroCompleto = this.data.registroCompleto ? 'Completo' : 'Incompleto';
        }
      }
    });
  }

  redirectToParent(): void{
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if(urlSegment.path.includes("Requisitos")){
        this.routes.navigate(['/gestionarTramiteLiquidacionContractual/', urlSegment.path, this.contratacionId ]);
        return;
      }
    });
  }
}
