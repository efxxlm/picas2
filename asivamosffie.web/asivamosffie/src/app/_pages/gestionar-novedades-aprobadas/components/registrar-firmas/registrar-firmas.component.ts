import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-registrar-firmas',
  templateUrl: './registrar-firmas.component.html',
  styleUrls: ['./registrar-firmas.component.scss']
})
export class RegistrarFirmasComponent implements OnInit {

  addressForm = this.fb.group({
    continuarProceso: [null, Validators.required],
    fechaEnvioActaFirmaContratistaObra: [null, Validators.required],
    fechaFirmaContratistaObra: [null, Validators.required],
    fechaEnvioActaFirmaContratistaInterventoria: [null, Validators.required],
    fechaFirmaContratistaInterventoria: [null, Validators.required],
    fechaEnvioActaFirmaApoyoSupervision: [null, Validators.required],
    fechaFirmaApoyoSupervision: [null, Validators.required],
    fechaEnvioFirmaSupervisor: [null, Validators.required],
    fechaFirmaSupervisor: [null, Validators.required],
    razones: [null, Validators.required],
    urlFirmas: [null, Validators.required]
  });

  estaEditando = false;

  editorStyle = {
    height: '75px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  estadoDelProcesoArray = [
    { name: 'Aprobada', value: '1' },
    { name: 'En revisión de gestión contractual', value: '2' }
  ];
  nombreAbogadoArray = [
    { name: 'Laura Andrea Osorio Martínez', value: '1' },
    { name: 'Laura Andrea Osorio Martínez 2', value: '2' }
  ];



  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
  ) { }

  ngOnInit(): void {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
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

  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }
}
