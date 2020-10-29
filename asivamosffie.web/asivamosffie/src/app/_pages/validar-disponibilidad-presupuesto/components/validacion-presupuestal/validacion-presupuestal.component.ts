import { Component, Inject, OnInit, Pipe } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DevolverPorValidacionComponent } from '../devolver-por-validacion/devolver-por-validacion.component';
import { Router, ActivatedRoute } from '@angular/router';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { DomSanitizer } from '@angular/platform-browser';
import { RechasadaPorValidacionComponent } from '../rechasada-por-validacion/rechasada-por-validacion.component';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TipoDDP } from 'src/app/core/_services/budgetAvailability/budget-availability.service';

@Component({
  selector: 'app-validacion-presupuestal',
  templateUrl: './validacion-presupuestal.component.html',
  styleUrls: ['./validacion-presupuestal.component.scss']
})
export class ValidacionPresupuestalComponent implements OnInit {

  constructor(public dialog: MatDialog,private disponibilidadServices: DisponibilidadPresupuestalService,
    private route: ActivatedRoute,
    private router: Router,private sanitized: DomSanitizer,
    ) { }
    detailavailabilityBudget:any=null;
    esModificacion=false;
    pTipoDDP=TipoDDP;

  ngOnInit(): void {
    console.log(this.pTipoDDP.DDP_tradicional);
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.disponibilidadServices.GetDetailAvailabilityBudgetProyect(id).subscribe(listas => {
        console.log(listas);
        if(listas.length>0)
        {
          this.detailavailabilityBudget=listas[0];        
        }                
      });
    }
  }
  download()
  {
    console.log(this.detailavailabilityBudget);
    this.disponibilidadServices.GenerateDDP(this.detailavailabilityBudget.id).subscribe((listas:any) => {
      console.log(listas);
      const documento = `DDP ${ this.detailavailabilityBudget.id }.pdf`;
        const text = documento,
          blob = new Blob([listas], { type: 'application/pdf' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
        anchor.click();
    });
  }

  openDialogDevolver(tipo,numero,tipoSolicitud) {
    this.dialog.open(DevolverPorValidacionComponent, {
      width: '70em',data:{solicitudID:this.route.snapshot.paramMap.get('id'),tipo:tipo,numeroSolicitud:numero,tipoSolicitud:tipoSolicitud}
    });
  }
  validar() {
    this.disponibilidadServices.SetValidarValidacionDDP(this.route.snapshot.paramMap.get('id')).subscribe(listas => {
      this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
      });
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef= this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {

        this.router.navigate(['/validarDisponibilidadPresupuesto']);

    });
  }

}
