import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrarInformeFinal/registrar-informe-final-proyecto.service'
import { Report } from 'src/app/_interfaces/proyecto-final.model';
import { TablaInformeFinalAnexosComponent } from '../tabla-informe-final-anexos/tabla-informe-final-anexos.component';
import { FormReciboASatisfaccionComponent } from '../form-recibo-a-satisfaccion/form-recibo-a-satisfaccion.component';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-registrar-informe-final',
  templateUrl: './registrar-informe-final.component.html',
  styleUrls: ['./registrar-informe-final.component.scss']
})
export class RegistrarInformeFinalComponent implements OnInit {
  id: string;
  report: Report;
  mostrarInforme = true;
  mostrarDetalleAnexos = false;
  mostrarDetalleRecibo = false;
  @ViewChild(TablaInformeFinalAnexosComponent ) childTablaInformeFinalAnexos: TablaInformeFinalAnexosComponent ; 
  @ViewChild(FormReciboASatisfaccionComponent ) childFormReciboASatisfaccion: FormReciboASatisfaccionComponent ; 

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService,
    public dialog: MatDialog,
  ) { }

  ngOnDestroy(): void {
    //Sólo satisfacción
    if (this.childFormReciboASatisfaccion && this.childFormReciboASatisfaccion.noGuardado===true && this.childFormReciboASatisfaccion.addressForm.dirty && (!this.childTablaInformeFinalAnexos || !this.childTablaInformeFinalAnexos.noGuardado)) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        if(result === true)
        {
          this.childFormReciboASatisfaccion.onSubmit(false); 
          this.ngOnInit();
        }           
      });
    }
    //Sólo tabla anexos
    else if (this.childTablaInformeFinalAnexos && !(this.childFormReciboASatisfaccion.noGuardado===true && this.childFormReciboASatisfaccion.addressForm.dirty) && this.childTablaInformeFinalAnexos.noGuardado) {
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
    else if (this.childFormReciboASatisfaccion.noGuardado===true && this.childFormReciboASatisfaccion.addressForm.dirty && this.childTablaInformeFinalAnexos.noGuardado) {
      let dialogRef =this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle:"", modalText:"¿Desea guardar la información registrada?",siNoBoton:true }
      });   
      dialogRef.afterClosed().subscribe(result => {
        if(result === true)
        {
            this.childFormReciboASatisfaccion.onSubmit(true); //solo muestre una vez la ventana de éxito
            this.childTablaInformeFinalAnexos.onSubmit(true); //popup
            this.ngOnInit();
        }           
      });
    }
  };

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.id = params.id;
      this.getInformeFinalByProyecto(this.id);
    });
  }

  getInformeFinalByProyecto(id: string) {
    this.registrarInformeFinalProyectoService.getInformeFinalByProyecto(id).subscribe(report => {
      this.report = report[0];
      if(report[0].proyecto.informeFinal[0].estadoAprobacion === '6' && report[0].proyecto.informeFinal[0].tieneObservacionesAnyAnexo == false){
        this.mostrarDetalleAnexos = true;
      }
      if(report[0].proyecto.informeFinal[0].estadoAprobacion === '6' && report[0].proyecto.informeFinal[0].tieneObservacionesSupervisor == false){
        this.mostrarDetalleRecibo = true;
      }
    });
  }

  updateForm(data) {
    if (data) this.mostrarInforme = data;
    else this.mostrarInforme = false;
  }
}
