import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-rechazar-solicitud-vfsp',
  templateUrl: './dialog-rechazar-solicitud-vfsp.component.html',
  styleUrls: ['./dialog-rechazar-solicitud-vfsp.component.scss']
})
export class DialogRechazarSolicitudVfspComponent implements OnInit {
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
  constructor(public matDialogRef: MatDialogRef<DialogRechazarSolicitudVfspComponent>, @Inject(MAT_DIALOG_DATA) public data: any, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }
  crearFormulario() {
    return this.fb.group({
      fechaRadicacionSAC:[null, Validators.required],
      numeroRadicacionSAC:[null, Validators.required],
      observaciones:[null, Validators.required]
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
