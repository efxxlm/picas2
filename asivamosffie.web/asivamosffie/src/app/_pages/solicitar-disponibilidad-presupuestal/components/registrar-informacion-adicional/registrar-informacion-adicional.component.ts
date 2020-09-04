import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { FormBuilder, Validators, FormArray } from '@angular/forms';

@Component({
  selector: 'app-registrar-informacion-adicional',
  templateUrl: './registrar-informacion-adicional.component.html',
  styleUrls: ['./registrar-informacion-adicional.component.scss']
})
export class RegistrarInformacionAdicionalComponent implements OnInit {

  addressForm = this.fb.group({
    plazoMeses: [null, Validators.required],
    plazoDias: [null, Validators.required],
    objeto: [null, Validators.required],
  });

  editorStyle = {
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      console.log(params);
      const id = params.id;
      // const a = this.service.getService(id;
      // console.log(a);
    });
  }

  // evalua tecla a tecla
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
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  onSubmit() {
    console.log(this.addressForm.value);
  }

}
