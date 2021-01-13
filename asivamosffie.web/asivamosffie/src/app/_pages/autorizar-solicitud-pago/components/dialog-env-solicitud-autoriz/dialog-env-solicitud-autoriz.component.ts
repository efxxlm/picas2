import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-env-solicitud-autoriz',
  templateUrl: './dialog-env-solicitud-autoriz.component.html',
  styleUrls: ['./dialog-env-solicitud-autoriz.component.scss']
})
export class DialogEnvSolicitudAutorizComponent implements OnInit {
  addressForm = this.fb.group({});
  constructor(private fb: FormBuilder, public matDialogRef: MatDialogRef<DialogEnvSolicitudAutorizComponent>, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }  
  crearFormulario() {
    return this.fb.group({
      fechaRadicacionSAC: [null, Validators.required],
      numeroRadicacionSAC:[null, Validators.required],
      urlSoporte:[null, Validators.required],
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
}
