import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-observaciones-validfsp',
  templateUrl: './dialog-observaciones-validfsp.component.html',
  styleUrls: ['./dialog-observaciones-validfsp.component.scss']
})
export class DialogObservacionesValidfspComponent implements OnInit {
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
  estaEditando = false;
  constructor(public matDialogRef: MatDialogRef<DialogObservacionesValidfspComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private fb: FormBuilder) { }

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
    this.estaEditando = true;
    console.log(this.addressForm.value);
  }

}
