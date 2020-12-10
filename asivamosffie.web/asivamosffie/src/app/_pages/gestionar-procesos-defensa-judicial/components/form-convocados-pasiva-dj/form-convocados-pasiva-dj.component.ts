import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-form-convocados-pasiva-dj',
  templateUrl: './form-convocados-pasiva-dj.component.html',
  styleUrls: ['./form-convocados-pasiva-dj.component.scss']
})
export class FormConvocadosPasivaDjComponent implements OnInit {
  formContratista: FormGroup;
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
  tiposIdentificacionArray = [
    { name: 'Cedula de ciudadanía', value: '1' },
    { name: 'Cedula de extranjería', value: '2' },
  ];
  departamentoArray = [
    { name: 'Antioquia', value: '1' },
    { name: 'Atlantico', value: '2' },
  ];
  municipioArray = [
    { name: 'Soledad', value: '1' },
    { name: 'Amalfi', value: '2' },
  ];
  tipoAccionArray = [
    { name: 'Reparacion Directa', value: '1' },
    { name: 'Reparacion Indirecta', value: '2' },
  ];
  jurisdiccionArray = [
    { name: 'Ordinaria', value: '1' },
    { name: 'Extraordinaria', value: '2' },
  ];
  intanciasArray = [
    { name: 'Primera instancia', value: '1' },
    { name: 'Segunda instancia', value: '2' },
  ];
  constructor ( private fb: FormBuilder ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
    this.formContratista.get( 'numeroContratos' ).valueChanges
      .subscribe( value => {
        this.perfiles.clear();
        for ( let i = 0; i < Number(value); i++ ) {
          this.perfiles.push( 
            this.fb.group(
              {
                nomConvocado: [ null ],
                tipoIdentificacion: [ null ],
                numIdentificacion: [ null ],
                despacho: [ null ],
                departamento: [ null ],
                municipio: [ null ],
                radicadoDespacho: [ null ],
                fechaRadicadoDespacho: [ null ],
                accionAEvitar: [ null ],
                etapaProcesoFFIE: [ null ],
                caducidad: [ null ]
              }
            ) 
          )
        }
      } )
  };

	  get perfiles () {
		return this.formContratista.get( 'perfiles' ) as FormArray;
	  };

  get numeroRadicado () {
    let numero;
    Object.values( this.formContratista.controls ).forEach( control => {
      if ( control instanceof FormArray ) {
        Object.values( control.controls ).forEach( control => {
          numero = control.get( 'numeroRadicadoFfieAprobacionCv' ) as FormArray;
        } )
      }
    } )
    return numero;
  };
  textoLimpio (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  maxLength (e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  crearFormulario () {
    this.formContratista = this.fb.group({
      numeroContratos: [ '' ],
      perfiles: this.fb.array([])
    });
  };

  eliminarPerfil ( numeroPerfil: number ) {
    this.perfiles.removeAt( numeroPerfil );
    this.formContratista.patchValue({
      numeroContratos: `${ this.perfiles.length }`
    });
  };

  agregarNumeroRadicado () {
    this.numeroRadicado.push( this.fb.control( '' ) )
  }

  eliminarNumeroRadicado ( numeroRadicado: number ) {
    this.numeroRadicado.removeAt( numeroRadicado );
  };

  guardar () {
    console.log( this.formContratista );
  }

}
