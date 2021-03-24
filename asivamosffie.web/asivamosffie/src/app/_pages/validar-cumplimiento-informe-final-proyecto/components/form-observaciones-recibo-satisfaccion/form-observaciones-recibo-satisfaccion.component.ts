import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Report } from 'src/app/_interfaces/proyecto-final.model';
import { ValidarCumplimientoInformeFinalService } from 'src/app/core/_services/validarCumplimientoInformeFinal/validar-cumplimiento-informe-final.service';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-form-observaciones-recibo-satisfaccion',
  templateUrl: './form-observaciones-recibo-satisfaccion.component.html',
  styleUrls: ['./form-observaciones-recibo-satisfaccion.component.scss']
})
export class FormObservacionesReciboSatisfaccionComponent implements OnInit {

  @Input() report: Report;

  estaEditando = false;
  informeFinalId = 0;
  tieneObservacionesCumplimiento = null;
  observaciones: FormGroup;
  noGuardado=true; 
  id : number;
  editorStyle = {
    height: '100px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }]
    ]
  };

  constructor(
    private dialog: MatDialog, private fb: FormBuilder,
    private routes: Router,
    private validarCumplimientoInformeFinalService: ValidarCumplimientoInformeFinalService
    ) {}

  ngOnInit(): void {
    this.buildForm();
  }

  private buildForm() {
    this.observaciones = this.fb.group({ 
      informeFinalObservacionesId: [null, Validators.required],
      informeFinalId: [null, Validators.required],
      observaciones: [null, Validators.required],
      eliminado: [null, Validators.required],
      archivado: [null, Validators.required],
      tieneObservacionesCumplimiento: [null, Validators.required],
      esGrupoNovedades: [null, Validators.required],
    });
    if (this.report.proyecto.informeFinal.length > 0) {
      this.observaciones.get("informeFinalId").setValue(this.report.proyecto.informeFinal[0].informeFinalId);
      this.observaciones.get("esGrupoNovedades").setValue(true);
      if(this.report.proyecto.informeFinal[0].tieneObservacionesCumplimiento != null){
        this.observaciones.get("tieneObservacionesCumplimiento").setValue(this.report.proyecto.informeFinal[0].tieneObservacionesCumplimiento);
      }if(this.report.proyecto.informeFinal[0].informeFinalObservacionesCumplimiento.length>0){
        this.observaciones.patchValue(this.report.proyecto.informeFinal[0].informeFinalObservacionesCumplimiento[0]);
      }
      this.id = this.report.proyecto.informeFinal[0].proyectoId;
      this.estaEditando = true;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
        () =>   this.routes.navigate(
                    [
                        '/validarCumplimientoInformeFinalProyecto/revisarInforme', this.id
                    ]
                )
    );
    });
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n - 1, e.editor.getLength());
    }
  }

  textoLimpio(evento: any, n: number) {
    if (evento !== undefined) {
      return evento.getLength() > n ? n : evento.getLength();
    } else {
      return 0;
    }
  }

  onSubmit(test: boolean) {
    this.noGuardado = false;
    console.log(this.observaciones.value);
    this.estaEditando = true;
    this.observaciones.markAllAsTouched();
    this.estaEditando = true;
    this.createEditObservacionInformeFinal(this.observaciones.value, this.observaciones.value.tieneObservacionesCumplimiento, test);
  }

  
  createEditObservacionInformeFinal(informeFinalObservacion: any, tieneObservaciones: boolean, test: boolean) {
    this.validarCumplimientoInformeFinalService.createEditObservacionInformeFinal(informeFinalObservacion, tieneObservaciones).subscribe((respuesta: Respuesta) => {
      if(!test){
        this.openDialog('', respuesta.message);
      }
    });
  }
}
