import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { VerificarInformeFinalService } from 'src/app/core/_services/verificarInformeFinal/verificar-informe-final.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Report } from 'src/app/_interfaces/proyecto-final.model';
import { FormObservacionesReciboSatisfaccionComponent } from '../form-observaciones-recibo-satisfaccion/form-observaciones-recibo-satisfaccion.component';
import { TablaInformeFinalAnexosComponent } from '../tabla-informe-final-anexos/tabla-informe-final-anexos.component';

@Component({
  selector: 'app-verificar-informe',
  templateUrl: './verificar-informe.component.html',
  styleUrls: ['./verificar-informe.component.scss']
})
export class VerificarInformeComponent implements OnInit {

  id: string;
  report: Report;
  @ViewChild(TablaInformeFinalAnexosComponent ) childTablaInformeFinalAnexos: TablaInformeFinalAnexosComponent ; 
  @ViewChild(FormObservacionesReciboSatisfaccionComponent ) childFormObsReciboASatisfaccion: FormObservacionesReciboSatisfaccionComponent ; 

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private verificarInformeFinalProyectoService: VerificarInformeFinalService,
    public dialog: MatDialog,
  ) { }

  ngOnDestroy(): void {
    //Sólo satisfacción
    if (this.childFormObsReciboASatisfaccion.noGuardado===true && this.childFormObsReciboASatisfaccion.observaciones.dirty && !this.childTablaInformeFinalAnexos.noGuardado) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        if(result === true)
        {
          this.childFormObsReciboASatisfaccion.onSubmit(false); 
          this.ngOnInit();
        }           
      });
    }
    //Sólo tabla anexos
    else if (!(this.childFormObsReciboASatisfaccion.noGuardado===true && this.childFormObsReciboASatisfaccion.observaciones.dirty) && this.childTablaInformeFinalAnexos.noGuardado) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        if(result === true)
        {
          this.childTablaInformeFinalAnexos.onSubmit(true); 
          this.ngOnInit();
        }           
      });
    }
    //ambos
    else if (this.childFormObsReciboASatisfaccion.noGuardado===true && this.childFormObsReciboASatisfaccion.observaciones.dirty && this.childTablaInformeFinalAnexos.noGuardado) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        if(result === true)
        {
            this.childFormObsReciboASatisfaccion.onSubmit(true); //solo muestre una vez la ventana de éxito
            this.childTablaInformeFinalAnexos.onSubmit(true); //cambiar pop up
            this.ngOnInit();
        }           
      });
    }
  };

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params.id;
      this.getInformeFinalByProyecto(this.id);
    })
  }
  getInformeFinalByProyecto(id:string) {
    this.verificarInformeFinalProyectoService.getInformeFinalByProyecto(id)
    .subscribe(report => {
      this.report = report[0];
    });
  }

}
