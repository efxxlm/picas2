import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-devolver-solicitud-interventor',
  templateUrl: './dialog-devolver-solicitud-interventor.component.html',
  styleUrls: ['./dialog-devolver-solicitud-interventor.component.scss']
})
export class DialogDevolverSolicitudInterventorComponent implements OnInit {

  formObservacion: FormGroup;

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
    ) {
    this.crearFormulario();
  }

  ngOnInit(): void { }

  crearFormulario() {
    this.formObservacion = this.fb.group({
      causaRechazo: [null, Validators.required]
    });
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if (texto) {
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }

  textoLimpioMessage(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data: { modalTitle, modalText }
    });
  };

  openDialogGuardar(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onClose(): void {
    this.dialog.closeAll();
  }

  guardar() {
    console.log(this.formObservacion.value);
    this.onClose();
    this.openDialogGuardar('', '<b>La información ha sido guardada exitosamente.</b>');
  }

}
