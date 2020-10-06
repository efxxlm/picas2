import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DevolverPorValidacionComponent } from '../devolver-por-validacion/devolver-por-validacion.component';
import { CancelarDdpComponent } from '../cancelar-ddp/cancelar-ddp.component';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-gestionar-ddp',
  templateUrl: './gestionar-ddp.component.html',
  styleUrls: ['./gestionar-ddp.component.scss']
})
export class GestionarDdpComponent implements OnInit {
  detailavailabilityBudget: any;

  constructor(public dialog: MatDialog,private disponibilidadServices: DisponibilidadPresupuestalService,
    private route: ActivatedRoute,
    private router: Router) { }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
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
    this.disponibilidadServices.CreateDDP(this.detailavailabilityBudget[0].id).subscribe(listas => {
      console.log(listas);
      this.detailavailabilityBudget=listas;
    });
  }

  openDialogDevolver() {
    let dialogRef = this.dialog.open(DevolverPorValidacionComponent, {
      width: '70em'
    });
    dialogRef.componentInstance.id = this.detailavailabilityBudget[0].id;
    
  }

  openDialogCancelarDDP() {
    let dialogRef = this.dialog.open(CancelarDdpComponent, {
      width: '70em'
    });
    dialogRef.componentInstance.id = this.detailavailabilityBudget[0].id;
  }

}
