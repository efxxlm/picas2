import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ValidarInformeFinalService } from 'src/app/core/_services/validarInformeFinal/validar-informe-final.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Report } from 'src/app/_interfaces/proyecto-final.model';
import { FormObservacionesReciboSatisfaccionComponent } from '../form-observaciones-recibo-satisfaccion/form-observaciones-recibo-satisfaccion.component';
import { TablaInformeFinalAnexosComponent } from '../tabla-informe-final-anexos/tabla-informe-final-anexos.component';


@Component({
  selector: 'app-validar-informe',
  templateUrl: './validar-informe.component.html',
  styleUrls: ['./validar-informe.component.scss']
})
export class ValidarInformeComponent implements OnInit {

  id: string;
  report: Report;
  @ViewChild(TablaInformeFinalAnexosComponent ) childTablaInformeFinalAnexos: TablaInformeFinalAnexosComponent ; 
  @ViewChild(FormObservacionesReciboSatisfaccionComponent ) childFormObsReciboASatisfaccion: FormObservacionesReciboSatisfaccionComponent ; 

  existeObservacionApoyo = false;
  existeObservacionSupervision = false;
  existeObservacionCumplimiento = false;
  mostrarDetalleAnexos = false; //Mostrar cuando esta en estado de aprobación 5 y no tiene observaciones de interventoria
  mostrarDetalleObservacionesRecibo = false; //Mostrar cuando esta en estado de aprobación 5 y no tiene observaciones de interventoria
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private validarInformeFinalProyectoService: ValidarInformeFinalService,
    public dialog: MatDialog,
  ) { }

  ngOnDestroy(): void {
    if(this.childFormObsReciboASatisfaccion && this.childTablaInformeFinalAnexos){
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
    }
  };

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params.id;
      this.getInformeFinalByProyecto(this.id);
    })
  }

  getInformeFinalByProyecto(id:string) {
    this.validarInformeFinalProyectoService.getInformeFinalByProyecto(id)
    .subscribe(report => {
      this.report = report[0];
      if(report[0].proyecto.informeFinal[0].informeFinalObservaciones.length > 0){
        this.existeObservacionApoyo = true;
      }
      if(report[0].proyecto.informeFinal[0].informeFinalObservacionesSupervisor.length > 0){
        this.existeObservacionSupervision = true;
      }
      if(report[0].proyecto.informeFinal[0].informeFinalObservacionesCumplimiento.length > 0){
        this.existeObservacionCumplimiento = true;
      }
      if(report[0].proyecto.informeFinal[0].estadoCumplimiento === '2' && report[0].proyecto.informeFinal[0].tieneObservacionesInterventoria == false){
        this.mostrarDetalleAnexos = true;
      }
      if(report[0].proyecto.informeFinal[0].estadoCumplimiento === '2' && report[0].proyecto.informeFinal[0].tieneObservacionesCumplimiento == false){
        this.mostrarDetalleObservacionesRecibo = true;
      }
    });
  }
  
}