import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Params, Router, UrlSegment } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { EstadosSolicitudLiquidacionContractual, EstadosSolicitudLiquidacionContractualCodigo, ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId, TipoObservacionLiquidacionContrato, TipoObservacionLiquidacionContratoCodigo } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';
import { InformeFinal } from 'src/app/_interfaces/informe-final';

@Component({
  selector: 'app-validar-informe-final',
  templateUrl: './validar-informe-final.component.html',
  styleUrls: ['./validar-informe-final.component.scss']
})
export class ValidarInformeFinalComponent implements OnInit {

  proyectoId: number;
  contratacionProyectoId: number;
  data: any;
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  listaTipoObservacionLiquidacionContratacion: TipoObservacionLiquidacionContrato = TipoObservacionLiquidacionContratoCodigo;
  registroCompleto : string;
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
      if ( urlSegment.path === 'validarInformeFinal' ) {
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

  redirectToParent(): void{
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if(urlSegment.path.includes("Requisitos")){
        this.routes.navigate(['/registrarSolicitudLiquidacionContractual/', urlSegment.path, this.contratacionProyectoId ]);
        return;
      }
    });
  }


  getInformeFinalByProyectoId(proyectoId: number, contratacionProyectoId: number) {
    this.registerContractualLiquidationRequestService.getInformeFinalByProyectoId(proyectoId, contratacionProyectoId, this.listaMenu.registrarSolicitudLiquidacionContratacion).subscribe(report => {
      if(report != null){
        this.data = report[0];
        this.registroCompleto = this.data.registroCompleto ? 'Completo' : 'Incompleto';
      }
    });
  }

}
