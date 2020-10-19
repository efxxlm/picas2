import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-observaciones-programacion',
  templateUrl: './dialog-observaciones-programacion.component.html',
  styleUrls: ['./dialog-observaciones-programacion.component.scss']
})
export class DialogObservacionesProgramacionComponent implements OnInit {

  formObservacion: FormGroup;
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
  constructor(private fb: FormBuilder,
    private dialog: MatDialog,
    private dialogRef: MatDialogRef<DialogObservacionesProgramacionComponent>,
    @Inject(MAT_DIALOG_DATA) public data) {
    this.crearFormulario();
  }

  ngOnInit(): void {
  }
  crearFormulario() {
    this.formObservacion = this.fb.group({
      observaciones: [null]
    });
  };

  textoLimpio(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  textoLimpioMessage(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };
  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data : { modalTitle, modalText }
    });
  };
  
  guardar() {
    this.dialogRef.close({ data: this.formObservacion.get('observaciones').value });
    this.openDialog("","La información ha sido guardada exitosamente");
  }
}
