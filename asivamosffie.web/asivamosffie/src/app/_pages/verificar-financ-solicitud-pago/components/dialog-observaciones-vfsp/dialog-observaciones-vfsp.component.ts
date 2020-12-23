import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-observaciones-vfsp',
  templateUrl: './dialog-observaciones-vfsp.component.html',
  styleUrls: ['./dialog-observaciones-vfsp.component.scss']
})
export class DialogObservacionesVfspComponent implements OnInit {
  addressForm = this.fb.group({});
  constructor(public matDialogRef: MatDialogRef<DialogObservacionesVfspComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }
  crearFormulario() {
    return this.fb.group({
      observaciones:[null, Validators.required],
      puedeSubsanar:[null, Validators.required]
    })
  }
  maxLength(e: any, n: number) {
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
