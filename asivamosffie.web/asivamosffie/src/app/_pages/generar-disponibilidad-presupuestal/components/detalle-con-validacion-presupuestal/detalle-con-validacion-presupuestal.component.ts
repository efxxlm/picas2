import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TipoDDP } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CancelarDdpComponent } from '../cancelar-ddp/cancelar-ddp.component';
import { DevolverPorValidacionComponent } from '../devolver-por-validacion/devolver-por-validacion.component';

@Component({
  selector: 'app-detalle-con-validacion-presupuestal',
  templateUrl: './detalle-con-validacion-presupuestal.component.html',
  styleUrls: ['./detalle-con-validacion-presupuestal.component.scss']
})
export class DetalleConValidacionPresupuestalComponent implements OnInit {
  detailavailabilityBudget: any;
  esModificacion=false;
  pTipoDDP=TipoDDP;

  constructor(public dialog: MatDialog,private disponibilidadServices: DisponibilidadPresupuestalService,
    private route: ActivatedRoute,
    private router: Router) { }

  openDialog(modalTitle: string, modalText: string,relocate=false) {
    let ref=this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if(relocate)
    {
      ref.afterClosed().subscribe(result => {        
        this.router.navigate(["/generarDisponibilidadPresupuestal"], {});      
    });
    }
  }
  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.disponibilidadServices.GetDetailAvailabilityBudgetProyect(id).subscribe(listas => {
        console.log(listas);
        if(listas.length>0)
        {
          this.detailavailabilityBudget=listas[0];
        }
        else{
          this.openDialog('','Error al intentar recuperar los datos de la solicitud, por favor intenta nuevamente.');
        }
        
      });
    }    
  }
  generarddp(){
    
        this.download();
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

  openDialogDevolver() {
    let dialogRef = this.dialog.open(DevolverPorValidacionComponent, {
      width: '70em'
    });
    dialogRef.componentInstance.id = this.detailavailabilityBudget.id;
    dialogRef.afterClosed().subscribe(result => {        
      this.router.navigate(["/generarDisponibilidadPresupuestal"], {});      
    });
  }

  openDialogCancelarDDP() {
    let dialogRef = this.dialog.open(CancelarDdpComponent, {
      width: '70em'
    });
    dialogRef.componentInstance.id = this.detailavailabilityBudget.id;
    dialogRef.afterClosed().subscribe(result => {        
      this.router.navigate(["/generarDisponibilidadPresupuestal"], {});      
    });
    
  }

}
