import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-envio-autorizacion',
  templateUrl: './dialog-envio-autorizacion.component.html',
  styleUrls: ['./dialog-envio-autorizacion.component.scss']
})
export class DialogEnvioAutorizacionComponent implements OnInit {
  addressForm = this.fb.group({});
  constructor(public matDialogRef: MatDialogRef<DialogEnvioAutorizacionComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }
  crearFormulario() {
    return this.fb.group({
      urlSoporte:[null, Validators.required],
    })
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }
  onSubmit() {
    console.log(this.addressForm.value);
  }
}
