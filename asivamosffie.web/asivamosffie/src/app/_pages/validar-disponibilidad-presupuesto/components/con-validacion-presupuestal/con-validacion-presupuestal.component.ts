import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { TipoDDP } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { FuenteFinanciacionService } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CancelarDdpComponent } from '../cancelar-ddp/cancelar-ddp.component';

@Component({
  selector: 'app-con-validacion-presupuestal',
  templateUrl: './con-validacion-presupuestal.component.html',
  styleUrls: ['./con-validacion-presupuestal.component.scss']
})
export class ConValidacionPresupuestalComponent implements OnInit {

  esRechazada:boolean=false;
  esNovedad: boolean = false;
  esGenerar: boolean = false;
  esCancelada: boolean = false;
  esLiberacion: boolean = false;

  novedadId;

  constructor(public dialog: MatDialog,private disponibilidadServices: DisponibilidadPresupuestalService,
    private route: ActivatedRoute,
    private router: Router,private sanitized: DomSanitizer,
    private fuenteFinanciacionService: FuenteFinanciacionService,
    private contractualNoveltyService: ContractualNoveltyService
    ) {
      this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
        if ( urlSegment.path === 'rechazada' ) {
            this.esRechazada = true;
            return;
        }else if(urlSegment.path === 'conDisponibilidadPresupuestal'){
            this.esGenerar = true;
            return;
        }else if(urlSegment.path === 'conDisponibilidadCancelada'){
          this.esCancelada = true;
          return;
        }else if(urlSegment.path === 'conLiberacionSaldo'){
          this.esLiberacion = true;
          return;
        }
    } );
    }
    detailavailabilityBudget:any=null;
    datosContratoProyectoModificadosXNovedad: any;
    esModificacion=false;
    pTipoDDP=TipoDDP;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    const esNovedad = this.route.snapshot.paramMap.get('esNovedad');
    const esGenerar = this.esGenerar === true ? 'true':'false';
    this.esNovedad = esNovedad == "true" ? true : false;
    const novedadId = this.route.snapshot.paramMap.get('novedadId');
    this.novedadId = this.route.snapshot.paramMap.get('novedadId');
    if (id) {
      if(this.esLiberacion != true){
        this.disponibilidadServices.GetDetailAvailabilityBudgetProyectNew(id, esNovedad, novedadId,this.esGenerar)
        .subscribe(listas => {
          console.log(listas);
          this.detailavailabilityBudget=listas[0];
          if(this.detailavailabilityBudget != null){
            if(this.detailavailabilityBudget?.tipoSolicitudCodigo === this.pTipoDDP.DDP_especial){
              this.detailavailabilityBudget?.aportantes.forEach(element => {
                this.fuenteFinanciacionService.GetListFuentesFinanciacionByDisponibilidadPresupuestalid(this.detailavailabilityBudget?.id).subscribe(lista => {
                  console.log(lista);
                  if(lista.length > 0){
                    element.fuentesFinanciacion = lista;
                  }
              });
              });
            }
            this.contractualNoveltyService.getDatosContratoProyectoModificadosXNovedad(0, this.detailavailabilityBudget.contratoId).subscribe(respuesta => {
              this.datosContratoProyectoModificadosXNovedad = respuesta;
            });
          }
        });
      }else{
        this.disponibilidadServices.GetDetailAvailabilityBudgetProyectHistorical(id, esNovedad, novedadId,this.esGenerar)
        .subscribe(listas => {
          console.log(listas);
          this.detailavailabilityBudget=listas[0];
          if(this.detailavailabilityBudget != null){
            if(this.detailavailabilityBudget?.tipoSolicitudCodigo === this.pTipoDDP.DDP_especial){
              this.detailavailabilityBudget?.aportantes.forEach(element => {
                this.fuenteFinanciacionService.GetListFuentesFinanciacionByDisponibilidadPresupuestalid(this.detailavailabilityBudget?.id).subscribe(lista => {
                  if(lista.length > 0){
                    element.fuentesFinanciacion = lista;
                  }
              });
              });
            }
            this.contractualNoveltyService.getDatosContratoProyectoModificadosXNovedad(0, this.detailavailabilityBudget.contratoId).subscribe(respuesta => {
              this.datosContratoProyectoModificadosXNovedad = respuesta;
            });
          }
      });
      }
    }
  }


  download()
  {
    console.log(this.detailavailabilityBudget);
    const esNovedad = this.route.snapshot.paramMap.get('esNovedad');
    const novedadId = this.route.snapshot.paramMap.get('novedadId');
    this.disponibilidadServices.GenerateDDP(this.detailavailabilityBudget.id, esNovedad, novedadId,true,this.esLiberacion).subscribe((listas:any) => {
      console.log(listas);
      const documento = `${ this.detailavailabilityBudget.numeroDDP  }.pdf`;
        const text = documento,
          blob = new Blob([listas], { type: 'application/pdf' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef= this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      if(result === true)
      {

      }
    });
  }

  openDialogCancelarDDP() {
    let dialogRef = this.dialog.open(CancelarDdpComponent, {
      width: '70em'
    });
    dialogRef.componentInstance.id = this.detailavailabilityBudget.id;
    dialogRef.componentInstance.tipo = this.detailavailabilityBudget.tipoSolicitudEspecial;
    dialogRef.componentInstance.nSolicitud = this.detailavailabilityBudget.numeroSolicitud;
    dialogRef.componentInstance.esNovedad = this.esNovedad;
    dialogRef.componentInstance.registroPresupuestalId = this.novedadId;

    dialogRef.afterClosed().subscribe(result => {
      this.router.navigate(["/generarDisponibilidadPresupuestal"], {});
    });

  }
}
