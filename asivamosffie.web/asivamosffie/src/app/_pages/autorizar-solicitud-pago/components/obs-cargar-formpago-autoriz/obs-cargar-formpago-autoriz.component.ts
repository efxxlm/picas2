import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-obs-cargar-formpago-autoriz',
  templateUrl: './obs-cargar-formpago-autoriz.component.html',
  styleUrls: ['./obs-cargar-formpago-autoriz.component.scss']
})
export class ObsCargarFormpagoAutorizComponent implements OnInit {

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
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }
  crearFormulario() {
    return this.fb.group({
      tieneObservaciones: [null, Validators.required],
      observaciones:[null, Validators.required],
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
