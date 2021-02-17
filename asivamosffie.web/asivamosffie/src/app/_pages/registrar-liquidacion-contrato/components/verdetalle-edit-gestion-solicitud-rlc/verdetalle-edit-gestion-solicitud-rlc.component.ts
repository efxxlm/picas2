import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-verdetalle-edit-gestion-solicitud-rlc',
  templateUrl: './verdetalle-edit-gestion-solicitud-rlc.component.html',
  styleUrls: ['./verdetalle-edit-gestion-solicitud-rlc.component.scss']
})
export class VerdetalleEditGestionSolicitudRlcComponent implements OnInit {
  addressForm = this.fb.group({
    fechaEnvioFirmaContratista: [null, Validators.required],
    fechaFirmaParteContratista: [null, Validators.required],
    fechaEnvioFirmaFiduciaria: [null, Validators.required],
    fechaFirmaParteFiduciaria: [null, Validators.required],
    observaciones: [null],
    urlSoporte : [null, Validators.required]
  });
  estaEditando = false;
  editorStyle = {
    height: '50px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  constructor(private fb: FormBuilder,public dialog: MatDialog) { }

  ngOnInit(): void {
  }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if ( texto ){
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

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }
  onSubmit() {
    this.estaEditando = true;
  }
}
