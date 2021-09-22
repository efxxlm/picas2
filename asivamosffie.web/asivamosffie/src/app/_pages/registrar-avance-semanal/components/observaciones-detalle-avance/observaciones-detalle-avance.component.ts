import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-observaciones-detalle-avance',
  templateUrl: './observaciones-detalle-avance.component.html',
  styleUrls: ['./observaciones-detalle-avance.component.scss']
})
export class ObservacionesDetalleAvanceComponent implements OnInit {
  formObservaciones: FormGroup = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null]
  });

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
  estaEditando = false;

  constructor(private dialog: MatDialog, private fb: FormBuilder) {}

  ngOnInit(): void {}

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
    console.log(this.formObservaciones.value);
    this.estaEditando = true;
  }
}
