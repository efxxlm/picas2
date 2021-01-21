import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { EstadosProcesoSeleccion, ProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';

@Component({
  selector: 'app-form-evaluacion',
  templateUrl: './form-evaluacion.component.html',
  styleUrls: ['./form-evaluacion.component.scss']
})
export class FormEvaluacionComponent {

  @Input() procesoSeleccion: ProcesoSeleccion;
  @Input() editar:boolean;
  @Output() guardar: EventEmitter<any> = new EventEmitter(); 
  estadosProcesoSeleccion = EstadosProcesoSeleccion;

  addressForm = this.fb.group({
    procesoSeleccionId: [],
    descricion: [null, Validators.required],
    url: [null, Validators.required]
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

  constructor(private fb: FormBuilder) {}

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

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

  onSubmit() {
    console.log(this.addressForm.value);

    this.procesoSeleccion.procesoSeleccionId = this.addressForm.get('procesoSeleccionId').value,
    this.procesoSeleccion.evaluacionDescripcion = this.addressForm.get('descricion').value,
    this.procesoSeleccion.urlSoporteEvaluacion = this.addressForm.get('url').value,
    
    //console.log(procesoS);
    this.guardar.emit(null);
  }

  cargarRegistro(){
    

    this.addressForm.get('procesoSeleccionId').setValue( this.procesoSeleccion.procesoSeleccionId );
    this.addressForm.get('descricion').setValue( this.procesoSeleccion.evaluacionDescripcion );
    this.addressForm.get('url').setValue( this.procesoSeleccion.urlSoporteEvaluacion );

  }
}
