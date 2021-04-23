import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router, UrlSegment } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { EstadosSolicitudLiquidacionContractual, EstadosSolicitudLiquidacionContractualCodigo, ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId, TipoObservacionLiquidacionContrato, TipoObservacionLiquidacionContratoCodigo } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';
import { InformeFinal } from 'src/app/_interfaces/informe-final';

@Component({
  selector: 'app-aprobar-informe-final',
  templateUrl: './aprobar-informe-final.component.html',
  styleUrls: ['./aprobar-informe-final.component.scss']
})
export class AprobarInformeFinalComponent implements OnInit {

  proyectoId: number;
  contratacionProyectoId: number;
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
      this.contratacionProyectoId = params.id;
    });
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'aprobarInformeFinal' ) {
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
    this.getInformeFinalByProyectoId(this.proyectoId, this.contratacionProyectoId);
  }


  getInformeFinalByProyectoId(proyectoId: number, contratacionProyectoId: number) {
    this.registerContractualLiquidationRequestService.getInformeFinalByProyectoId(proyectoId, contratacionProyectoId, this.listaMenu.aprobarSolicitudLiquidacionContratacion).subscribe(report => {
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
        this.routes.navigate(['/aprobarSolicitudLiquidacionContractual/', urlSegment.path, this.contratacionProyectoId ]);
        return;
      }
    });
  }
  
}
