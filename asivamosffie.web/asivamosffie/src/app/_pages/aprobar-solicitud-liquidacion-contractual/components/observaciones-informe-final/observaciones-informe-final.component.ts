import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-observaciones-informe-final',
  templateUrl: './observaciones-informe-final.component.html',
  styleUrls: ['./observaciones-informe-final.component.scss']
})
export class ObservacionesInformeFinalComponent implements OnInit {

  estaEditando = false;
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
    private dialog: MatDialog, private fb: FormBuilder
    ) {}

  ngOnInit(): void {
    this.buildForm();
  }

  private buildForm() {
    this.observaciones = this.fb.group({ 
      tieneObservaciones: [null, Validators.required],
      observaciones: [null, Validators.required]
    });
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
  }
}
