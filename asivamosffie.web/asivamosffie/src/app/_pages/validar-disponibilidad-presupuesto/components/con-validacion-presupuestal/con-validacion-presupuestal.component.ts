import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { TipoDDP } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { FuenteFinanciacionService } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-con-validacion-presupuestal',
  templateUrl: './con-validacion-presupuestal.component.html',
  styleUrls: ['./con-validacion-presupuestal.component.scss']
})
export class ConValidacionPresupuestalComponent implements OnInit {

  esRechazada:boolean=false;
  esNovedad: boolean = false;
  constructor(public dialog: MatDialog,private disponibilidadServices: DisponibilidadPresupuestalService,
    private route: ActivatedRoute,
    private router: Router,private sanitized: DomSanitizer,
    private fuenteFinanciacionService: FuenteFinanciacionService
    ) {
      this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
        if ( urlSegment.path === 'rechazada' ) {
            this.esRechazada = true;
            return;
        }
    } );
    }
    detailavailabilityBudget:any=null;
    esModificacion=false;
    pTipoDDP=TipoDDP;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    const esNovedad = this.route.snapshot.paramMap.get('esNovedad');
    this.esNovedad = esNovedad == "true" ? true : false;
    const novedadId = this.route.snapshot.paramMap.get('novedadId');
    if (id) {
      this.disponibilidadServices.GetDetailAvailabilityBudgetProyectNew(id, esNovedad, novedadId)
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
          }
      });
    }
  }


  download()
  {
    console.log(this.detailavailabilityBudget);
    const esNovedad = this.route.snapshot.paramMap.get('esNovedad');
    const novedadId = this.route.snapshot.paramMap.get('novedadId');
    this.disponibilidadServices.GenerateDDP(this.detailavailabilityBudget.id, esNovedad, novedadId,true).subscribe((listas:any) => {
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

}
