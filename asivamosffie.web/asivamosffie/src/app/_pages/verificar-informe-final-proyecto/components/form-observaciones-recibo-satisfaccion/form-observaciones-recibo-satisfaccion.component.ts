import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { ValidarInformeFinalService } from 'src/app/core/_services/validarInformeFinal/validar-informe-final.service'
import { Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-form-observaciones-recibo-satisfaccion',
  templateUrl: './form-observaciones-recibo-satisfaccion.component.html',
  styleUrls: ['./form-observaciones-recibo-satisfaccion.component.scss']
})
export class FormObservacionesReciboSatisfaccionComponent implements OnInit {
  @Input() report: Report;
  @Output() formCompleto = new EventEmitter<boolean>(true);
  estaEditando = false;
  informeFinalId = 0;
  tieneObservacionesValidacion = null;
  observaciones: FormGroup;
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
    private validarInformeFinalService: ValidarInformeFinalService
    ) {}

  ngOnInit(): void {
    this.buildForm();
  }

  private buildForm() {
    this.observaciones = this.fb.group({ 
      informeFinalObservacionesId: [null, Validators.required],
      informeFinalId: [null, Validators.required],
      observaciones: [null, Validators.required],
      esSupervision: [null, Validators.required],
      eliminado: [null, Validators.required],
      archivado: [null, Validators.required],
      tieneObservacionesValidacion: [null, Validators.required],
    });
    console.log("ue paso? ",this.report.proyecto);
    if (this.report.proyecto.informeFinal.length > 0) {
      this.observaciones.get("informeFinalId").setValue(this.report.proyecto.informeFinal[0].informeFinalId);
      this.observaciones.get("esSupervision").setValue(true);
      if(this.report.proyecto.informeFinal[0].tieneObservacionesValidacion == null){
        this.observaciones.get("tieneObservacionesValidacion").setValue(false);
      }else{
        this.observaciones.get("tieneObservacionesValidacion").setValue(this.report.proyecto.informeFinal[0].tieneObservacionesValidacion);
      }
      //this.observaciones.value.setValue({informeFinalId: (this.report.proyecto.informeFinal[0].informeFinalId), esSupervision: true});
      if(this.report.proyecto.informeFinal[0].informeFinalObservaciones.length>0){
        this.observaciones.patchValue(this.report.proyecto.informeFinal[0].informeFinalObservaciones[0]);
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

  onSubmit() {
    console.log(this.observaciones.value);
    this.estaEditando = true;
    this.observaciones.markAllAsTouched();
    this.estaEditando = true;
    this.createEditObservacionInformeFinal(this.observaciones.value, this.observaciones.value.tieneObservacionesValidacion);
  }

  createEditObservacionInformeFinal(informeFinalObservacion: any, tieneObservaciones: boolean) {
    console.log("esta enviando: ",tieneObservaciones);
    this.validarInformeFinalService.createEditObservacionInformeFinal(informeFinalObservacion, tieneObservaciones).subscribe((respuesta: Respuesta) => {
      this.openDialog('', respuesta.message);
    });
  }
}
