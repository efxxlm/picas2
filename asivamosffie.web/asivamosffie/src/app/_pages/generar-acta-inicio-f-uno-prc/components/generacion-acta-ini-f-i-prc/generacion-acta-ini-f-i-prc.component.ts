import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-generacion-acta-ini-f-i-prc',
  templateUrl: './generacion-acta-ini-f-i-prc.component.html',
  styleUrls: ['./generacion-acta-ini-f-i-prc.component.scss']
})
export class GeneracionActaIniFIPreconstruccionComponent implements OnInit {
  maxDate: Date;
  public numContrato = "A886675445";//valor quemado
  public fechaContrato = "20/06/2020";//valor quemado
  public mesPlazoIni: number = 10;
  public diasPlazoIni: number = 25;

  addressForm = this.fb.group({});
  dataDialog: {
    modalTitle: string,
    modalText: string
  };

  constructor(private router: Router,public dialog: MatDialog, private fb: FormBuilder) {
    this.maxDate = new Date();
  }
  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '37em',
      data: { modalTitle, modalText }
    });
  }
  openDialog2(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });
  }
  editorStyle = {
    height: '50%'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  crearFormulario() {
    return this.fb.group({
      fechaActaInicioFUnoPreconstruccion: [null, Validators.required],
      fechaPrevistaTerminacion: [null, Validators.required],
      mesPlazoEjFase1: [null, Validators.required],
      diasPlazoEjFase1: [null, Validators.required],
      mesPlazoEjFase2: [null, Validators.required],
      diasPlazoEjFase2: [null, Validators.required],
      observacionesEspeciales: [null]
    })
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }
  number(e: { keyCode: any; }) {
    const tecla = e.keyCode;
    if (tecla === 8) { return true; } // Tecla de retroceso (para poder borrar)
    if (tecla === 48) { return true; } // 0
    if (tecla === 49) { return true; } // 1
    if (tecla === 50) { return true; } // 2
    if (tecla === 51) { return true; } // 3
    if (tecla === 52) { return true; } // 4
    if (tecla === 53) { return true; } // 5
    if (tecla === 54) { return true; } // 6
    if (tecla === 55) { return true; } // 7
    if (tecla === 56) { return true; } // 8
    if (tecla === 57) { return true; } // 9
    const patron = /1/; // ver nota
    const te = String.fromCharCode(tecla);
    return patron.test(te);
  }
  onSubmit() {
    console.log(this.addressForm.value);
    this.openDialog2('La información ha sido guardada exitosamente.', "");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
  }
}
