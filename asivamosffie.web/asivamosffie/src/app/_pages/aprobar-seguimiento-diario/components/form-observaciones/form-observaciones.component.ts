import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-form-observaciones',
  templateUrl: './form-observaciones.component.html',
  styleUrls: ['./form-observaciones.component.scss']
})
export class FormObservacionesComponent {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observacion: [null, Validators.required]
  });

  minDate: Date;
  editorStyle = {
    height: '45px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog
  ) { }

  textoLimpio(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    this.addressForm.markAllAsTouched();
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }
}
