import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-devolver-sol-pago-gog',
  templateUrl: './dialog-devolver-sol-pago-gog.component.html',
  styleUrls: ['./dialog-devolver-sol-pago-gog.component.scss']
})
export class DialogDevolverSolPagoGogComponent implements OnInit {
  addressForm = this.fb.group({});
  editorStyle = {
    height: '45px',
    overflow: 'auto'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  usuarioDevolverArray = [
    { name: 'Equipo de facturación', value: '1' }
  ];
  constructor(private fb: FormBuilder, public matDialogRef: MatDialogRef<DialogDevolverSolPagoGogComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }
  crearFormulario() {
    return this.fb.group({
      usuarioADevolver :[null, Validators.required],
      observaciones:[null, Validators.required]
    })
  }
  maxLength(e: any, n: number) {
    console.log(e.editor.getLength()+" "+n);
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n-1, e.editor.getLength());
    }
  }
  textoLimpio(texto,n) {
    if (texto!=undefined) {
      return texto.getLength() > n ? n : texto.getLength();
    }
  }
  onSubmit() {
    console.log(this.addressForm.value);
  }

  close() {
    this.matDialogRef.close('aceptado');
  }
}
