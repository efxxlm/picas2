import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-form-descuentos-direccion-tecnica',
  templateUrl: './form-descuentos-direccion-tecnica.component.html',
  styleUrls: ['./form-descuentos-direccion-tecnica.component.scss']
})
export class FormDescuentosDireccionTecnicaComponent implements OnInit {
  formDescuentos: FormGroup;
  tiposDescuentoArray = [
    { name: '2x1000', value: '1' },
    { name: '4x1000', value: '2' },
  ];
  constructor(private fb: FormBuilder) { 
    this.crearFormulario();
  }

  ngOnInit(): void {
    this.formDescuentos.get( 'numeroDescuentos' ).valueChanges
    .subscribe( value => {
      this.descuentos.clear();
      for ( let i = 0; i < Number(value); i++ ) {
        this.descuentos.push( 
          this.fb.group(
            {
              tipoDescuento: [ null ],
              valorDescuento: [ null ]
            }
          ) 
        )
      }
    } )
  }
  get descuentos () {
		return this.formDescuentos.get( 'descuentos' ) as FormArray;
	  };
  crearFormulario () {
    this.formDescuentos = this.fb.group({
      aplicaDescuento: [ null],
      numeroDescuentos: [ '' ],
      valorAPagarDespues: [ '' ],
      descuentos: this.fb.array([])
    });
  };
  eliminarDescuento ( numeroDescuento: number ) {
    this.descuentos.removeAt( numeroDescuento );
    this.formDescuentos.patchValue({
      numeroDescuentos: `${ this.descuentos.length }`
    });
  };
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
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p>');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li>');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }
  guardar () {
    console.log( this.formDescuentos );
  }

}
