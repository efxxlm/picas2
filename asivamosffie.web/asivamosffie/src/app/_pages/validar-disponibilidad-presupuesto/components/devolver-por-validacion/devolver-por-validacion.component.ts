import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
@Component({
  selector: 'app-devolver-por-validacion',
  templateUrl: './devolver-por-validacion.component.html',
  styleUrls: ['./devolver-por-validacion.component.scss']
})
export class DevolverPorValidacionComponent implements OnInit {

  observaciones: FormControl;
  minDate: Date;

  editorStyle = {
    height: '100px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor(public dialog: MatDialog) {
    this.declararOnservaciones();
    this.minDate = new Date();
  }

  ngOnInit(): void {
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  private declararOnservaciones() {
    this.observaciones = new FormControl(null, [Validators.required]);
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  devolverSolicitud() {
    console.log(this.observaciones.value);
    this.openDialog('', 'La información ha sido guardada exitosamente.');
  }
}
