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
  observacionesValidacion = null;
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
      informeFinalId: [null, Validators.required],
      proyectoId: [this.report.proyecto.proyectoId, Validators.required],
      observacionesValidacion: [null, Validators.required],
      tieneObservacionesValidacion: [null, Validators.required],
      estadoValidacion: [null, Validators.required],
    });
    if (this.report.proyecto.informeFinal.length > 0) {
      this.observaciones.patchValue(this.report.proyecto.informeFinal[0]);
      this.estaEditando = true;
    }
    this.formCompleto.emit(this.respuestaFormCompleto());
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
    this.editObservacionInformeFinal(this.observaciones.value);
  }

  editObservacionInformeFinal(informeFinal: any) {
    this.validarInformeFinalService.editObservacionInformeFinal(informeFinal).subscribe((respuesta: Respuesta) => {
      this.openDialog('', respuesta.message);
      this.formCompleto.emit(this.respuestaFormCompleto());
    });
  }

  respuestaFormCompleto() {
    if (this.observaciones.get('tieneObservacionesValidacion').valid) return true;
    else if (this.observaciones.get('observacionesValidacion').valid) return true;
    else return false;
  }
}
