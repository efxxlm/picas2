import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ValidarCumplimientoInformeFinalService } from 'src/app/core/_services/validarCumplimientoInformeFinal/validar-cumplimiento-informe-final.service';
import { Report } from 'src/app/_interfaces/proyecto-final.model';
import { Respuesta } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-form-observaciones-informe-final-anexos',
  templateUrl: './form-observaciones-informe-final-anexos.component.html',
  styleUrls: ['./form-observaciones-informe-final-anexos.component.scss']
})
export class FormObservacionesInformeFinalAnexosComponent implements OnInit {

  @Input() report: Report;
  estaEditando = false;
  informeFinalId = 0;
  tieneObservacionesInterventoria = null;
  observaciones: FormGroup;
  noGuardado=true; 
  existe_historial = false;

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
      tieneObservacionesInterventoria: [null, Validators.required],
      esGrupoNovedadesInterventoria: [null, Validators.required],
    });

    if (this.report.proyecto.informeFinal.length > 0) {
      this.observaciones.get("informeFinalId").setValue(this.report.proyecto.informeFinal[0].informeFinalId);
      this.observaciones.get("esGrupoNovedadesInterventoria").setValue(true);
      if(this.report.proyecto.informeFinal[0].tieneObservacionesInterventoria != null){
        this.observaciones.get("tieneObservacionesInterventoria").setValue(this.report.proyecto.informeFinal[0].tieneObservacionesInterventoria);
      }
      if(this.report.proyecto.informeFinal[0].informeFinalObservacionesInterventoria.length>0){
        this.observaciones.patchValue(this.report.proyecto.informeFinal[0].informeFinalObservacionesInterventoria[0]);
      }
      this.estaEditando = true;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
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
    this.createEditObservacionInformeFinalInterventoria(this.observaciones.value, this.observaciones.value.tieneObservacionesCumplimiento, test);
  }

  
  createEditObservacionInformeFinalInterventoria(informeFinalObservacion: any, tieneObservaciones: boolean, test: boolean) {
    this.validarCumplimientoInformeFinalService.createEditObservacionInformeFinalInterventoria(informeFinalObservacion, tieneObservaciones).subscribe((respuesta: Respuesta) => {
      if(!test){
        this.openDialog('', respuesta.message);
      }
    });
  }
}
