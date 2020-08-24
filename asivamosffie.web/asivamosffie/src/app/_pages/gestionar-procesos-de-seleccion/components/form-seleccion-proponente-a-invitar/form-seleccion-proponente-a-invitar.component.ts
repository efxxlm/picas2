import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { min } from 'rxjs/operators';
import { ProcesoSeleccion } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';

@Component({
  selector: 'app-form-seleccion-proponente-a-invitar',
  templateUrl: './form-seleccion-proponente-a-invitar.component.html',
  styleUrls: ['./form-seleccion-proponente-a-invitar.component.scss']
})
export class FormSeleccionProponenteAInvitarComponent implements OnInit {

  @Input() procesoSeleccion: ProcesoSeleccion;
  @Output() guardar: EventEmitter<any> = new EventEmitter(); 

  addressForm = this.fb.group({
    cuantosProponentes: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2), Validators.min(1), Validators.max(99)])
    ],
    url: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(2), Validators.min(1), Validators.max(99)])
    ]
  });

  constructor(private fb: FormBuilder) {}
  ngOnInit(): void {
    
  }

  onSubmit() {
    console.log(this.addressForm.value);

    // this.procesoSeleccion.procesoSeleccionId = this.addressForm.get('procesoSeleccionId').value,
    // this.procesoSeleccion.evaluacionDescripcion = this.addressForm.get('descricion').value,
    // this.procesoSeleccion.urlSoporteEvaluacion = this.addressForm.get('url').value,
    
    //console.log(procesoS);
    this.guardar.emit(null);
  }

  cargarRegistro(){

    // this.addressForm.get('procesoSeleccionId').setValue( this.procesoSeleccion.procesoSeleccionId );
    // this.addressForm.get('descricion').setValue( this.procesoSeleccion.evaluacionDescripcion );
    // this.addressForm.get('url').setValue( this.procesoSeleccion.urlSoporteEvaluacion );

  }

}
