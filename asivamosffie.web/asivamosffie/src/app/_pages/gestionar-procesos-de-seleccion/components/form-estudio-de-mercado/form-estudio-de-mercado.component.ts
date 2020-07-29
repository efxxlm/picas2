import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { ProcesoSeleccion, ProcesoSeleccionCotizacion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';


@Component({
  selector: 'app-form-estudio-de-mercado',
  templateUrl: './form-estudio-de-mercado.component.html',
  styleUrls: ['./form-estudio-de-mercado.component.scss']
})
export class FormEstudioDeMercadoComponent implements OnInit {

  @Input() procesoSeleccion: ProcesoSeleccion;
  @Output() guardar: EventEmitter<any> = new EventEmitter(); 

  addressForm: FormGroup = this.fb.group({});
    

  get cotizaciones() {
    return this.addressForm.get('cotizaciones') as FormArray;
  }

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

  createFormulario(){
    return this.fb.group({
      cuantasCotizaciones: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(2)])
      ],
      cotizaciones: this.fb.array([
        this.fb.group({
          procesoSeleccionCotizacionId: [],
          nombreOrganizacion: [null, Validators.compose([
            Validators.required, Validators.minLength(2), Validators.maxLength(50)])
          ],
          valor: [null, Validators.compose([
            Validators.required, Validators.minLength(4), Validators.maxLength(20)])
          ],
          descripcion: [null, Validators.required],
          url: [null, Validators.required]
        })
      ])
    });
  }

  constructor(private fb: FormBuilder) {}
  ngOnInit(): void {
    this.addressForm = this.createFormulario();
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  CambioNumeroCotizantes() {
    const Formcotizaciones = this.addressForm.value;
    if (Formcotizaciones.cuantasCotizaciones > this.cotizaciones.length && Formcotizaciones.cuantasCotizaciones < 100) {
      while (this.cotizaciones.length < Formcotizaciones.cuantasCotizaciones) {
        this.cotizaciones.push( this.createCotizacion() );
      }
    } else if (Formcotizaciones.cuantasCotizaciones <= this.cotizaciones.length && Formcotizaciones.cuantasCotizaciones >= 0) {
      while (this.cotizaciones.length > Formcotizaciones.cuantasCotizaciones) {
        this.borrarArray(this.cotizaciones, this.cotizaciones.length - 1);
      }
    }
  }

  createCotizacion(): FormGroup {
    return this.fb.group({
      nombreOrganizacion: [null, Validators.compose([
        Validators.required, Validators.minLength(2), Validators.maxLength(50)])
      ],
      valor: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(20)])
      ],
      descripcion: [null, Validators.required],
      url: [null, Validators.required]
    });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  onSubmit() {

    let listaCotizaciones = this.addressForm.get('cotizaciones') as FormArray;

    this.procesoSeleccion.procesoSeleccionCotizacion = [];

    listaCotizaciones.controls.forEach( control => {
      let cotizacion: ProcesoSeleccionCotizacion = {
        descripcion: control.get('descripcion').value,
        procesoSeleccionId: this.procesoSeleccion.procesoSeleccionId,
        nombreOrganizacion: control.get('descripcion').value,
        procesoSeleccionCotizacionId: control.get('procesoSeleccionCotizacionId').value,
        urlSoporte: control.get('url').value,
        valorCotizacion: control.get('valor').value,
      }
      this.procesoSeleccion.procesoSeleccionCotizacion.push( cotizacion );
    })

    this.guardar.emit(null);
  }
}
