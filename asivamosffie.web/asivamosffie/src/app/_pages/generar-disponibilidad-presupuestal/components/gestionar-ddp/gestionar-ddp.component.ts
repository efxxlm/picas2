import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DevolverPorValidacionComponent } from '../devolver-por-validacion/devolver-por-validacion.component';
import { CancelarDdpComponent } from '../cancelar-ddp/cancelar-ddp.component';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TipoDDP } from 'src/app/core/_services/budgetAvailability/budget-availability.service';

@Component({
  selector: 'app-gestionar-ddp',
  templateUrl: './gestionar-ddp.component.html',
  styleUrls: ['./gestionar-ddp.component.scss']
})
export class GestionarDdpComponent implements OnInit {
  detailavailabilityBudget: any;
  esModificacion = false;
  pTipoDDP = TipoDDP;
  esNovedad;
  novedadId;

  constructor(public dialog: MatDialog, private disponibilidadServices: DisponibilidadPresupuestalService,
    private route: ActivatedRoute,
    private router: Router) { }

  openDialog(modalTitle: string, modalText: string, relocate = false) {
    let ref = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    if (relocate) {
      ref.afterClosed().subscribe(result => {
        this.router.navigate(["/generarDisponibilidadPresupuestal"], {});
      });
    }
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.esNovedad = this.route.snapshot.paramMap.get('esNovedad');
    this.novedadId = this.route.snapshot.paramMap.get('novedadId');
    if (id) {
      this.disponibilidadServices.GetDetailAvailabilityBudgetProyectNew(id, this.esNovedad, this.novedadId, true)
        .subscribe(listas => {
          console.log(listas);
          if (listas.length > 0) {
            this.detailavailabilityBudget = listas[0];
            this.validarBotonGenerarDDp(this.detailavailabilityBudget?.proyectos, this.detailavailabilityBudget?.tipoSolicitudCodigo);
          }
          else {
            this.openDialog('', 'Error al intentar recuperar los datos de la solicitud, por favor intenta nuevamente.');
          }

        });
    }
  }

  validarBotonGenerarDDp(proyectos: any[], tipoSolicitudCodigo: any){
    let cumpleCondiciones = true;
    //especial
    if(tipoSolicitudCodigo == "2"){
      return cumpleCondiciones;
    }
    if(proyectos != null){
      if(proyectos.length > 0){
        proyectos.forEach(proyecto => {
          proyecto.aportantes.forEach(apo => {
            apo.fuentesFinanciacion.forEach(fuente => {
              if(fuente.saldo_actual_de_la_fuente < fuente.valor_solicitado_de_la_fuente){
                cumpleCondiciones = false;
                return cumpleCondiciones;
              }
            });
          });
        });
      }else{
        cumpleCondiciones = false;
        return cumpleCondiciones;
      }
    }else{
      cumpleCondiciones = false;
      return cumpleCondiciones;
    }
    return cumpleCondiciones;
  }

  generarddp() {
    this.disponibilidadServices.CreateDDP(this.detailavailabilityBudget.id, this.esNovedad, this.novedadId)
      .subscribe(listas => {
        console.log(listas);
        //this.detailavailabilityBudget=listas;
        this.openDialog("", listas.message, true);
        if (listas.code == "200") {
          this.download(listas.data);
        }
      });
  }

  download(dato: any) {
    console.log(this.detailavailabilityBudget);
    console.log(dato);
    this.disponibilidadServices.GenerateDDP(this.detailavailabilityBudget.id, this.esNovedad, this.novedadId,false,false).subscribe((listas: any) => {
      console.log(listas);
      const documento = `${dato.numeroSolicitud ? dato.numeroSolicitud : 'DDP'}.pdf`;
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
    dialogRef.componentInstance.tipo = this.detailavailabilityBudget.tipoSolicitudEspecial;
    dialogRef.componentInstance.nSolicitud = this.detailavailabilityBudget.numeroSolicitud;
    dialogRef.afterClosed().subscribe(result => {
      this.router.navigate(["/generarDisponibilidadPresupuestal"], {});
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
