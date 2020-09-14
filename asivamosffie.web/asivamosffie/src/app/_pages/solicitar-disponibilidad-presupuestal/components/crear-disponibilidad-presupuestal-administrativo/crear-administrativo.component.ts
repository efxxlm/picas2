import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-crear-administrativo',
  templateUrl: './crear-administrativo.component.html',
  styleUrls: ['./crear-administrativo.component.scss']
})
export class CrearDisponibilidadPresupuestalAdministrativoComponent implements OnInit {

  formulario = this.fb.group({
    objeto: [null, Validators.required],
    consecutivo: [null, Validators.required],

  })

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

  ) 
  {
  }

  ngOnInit(): void {
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

  enviarObjeto() {
     console.log( this.formulario.get('objeto').value );
    
  }

}
