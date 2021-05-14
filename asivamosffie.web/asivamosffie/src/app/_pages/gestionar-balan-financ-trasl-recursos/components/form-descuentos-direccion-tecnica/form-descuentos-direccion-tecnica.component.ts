import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-form-descuentos-direccion-tecnica',
  templateUrl: './form-descuentos-direccion-tecnica.component.html',
  styleUrls: ['./form-descuentos-direccion-tecnica.component.scss']
})
export class FormDescuentosDireccionTecnicaComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    formDescuentosDireccionTecnica = this.fb.group(
        {
            fases: this.fb.array( [] )
        }
    )

    get fases() {
        return this.formDescuentosDireccionTecnica.get( 'fases' ) as FormArray;
    }

    constructor( private fb: FormBuilder ) { }

    ngOnInit(): void {
    }

}
