import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params, UrlSegment } from '@angular/router';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { ReprogrammingService } from 'src/app/core/_services/reprogramming/reprogramming.service';
import * as moment from 'moment';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-validar-ajuste',
  templateUrl: './validar-ajuste.component.html',
  styleUrls: ['./validar-ajuste.component.scss']
})
export class ValidarAjusteComponent implements OnInit {

  detalleId: number;
  ajusteProgramacion: any;
  ajusteProgramacionInfo: any;
  novedad: NovedadContractual;
  fechaFinalizacionContrato : any;
  fechaEstimadaFinalizacion : Date;
  presupuestoAdicionalSolicitado = 0; //X novedad
  esAdicion: boolean = false;
  modificaFecha: boolean = false;
  esVerDetalle: boolean = false;
  esRegistroNuevo: boolean = false;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private reprogrammingService: ReprogrammingService,
    private contractualNoveltyService: ContractualNoveltyService

  ) {
    if (this.router.getCurrentNavigation().extras.replaceUrl) {
      this.router.navigateByUrl('/validarAjusteProgramacion');
      return;
    };
    if (this.router.getCurrentNavigation().extras.state){
      this.ajusteProgramacionInfo = this.router.getCurrentNavigation().extras.state.ajusteProgramacion
    }
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'validar' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = true;
          return;
      }
      if ( urlSegment.path === 'verDetalleEditar' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = false;
          return;
      }
      if ( urlSegment.path === 'verDetalle' ) {
          this.esVerDetalle = true;
          return;
      }
    });
   }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;
      this.reprogrammingService.getAjusteProgramacionById( this.detalleId )
        .subscribe( ajuste => {
          console.log(ajuste);
          this.ajusteProgramacion = ajuste;
          this.contractualNoveltyService.getNovedadContractualById(this.ajusteProgramacionInfo?.novedadContractualId)
          .subscribe(respuesta => {
            this.novedad = respuesta;
          });
        });
    });
  }

}
