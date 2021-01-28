import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-form-costo-variable-terc-gog',
  templateUrl: './form-costo-variable-terc-gog.component.html',
  styleUrls: ['./form-costo-variable-terc-gog.component.scss']
})
export class FormCostoVariableTercGogComponent implements OnInit {
  addressForm: FormGroup;
  tipoDescuentoArray = [
    { name: 'Retefuente', value: '1' },
    { name: 'Rete ICA', value: '2' }
  ];
  constructor ( private fb: FormBuilder ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
    this.addressForm.get( 'numeroDescuentos' ).valueChanges
    .subscribe( value => {
      this.descuentos.clear();
      for ( let i = 0; i < Number(value); i++ ) {
        this.descuentos.push( 
          this.fb.group(
            {
              tipoDescuento: [ null ],
              valorDescuento: [ '' ],
            }
          ) 
        )
      }
    } )
  }
  get descuentos () {
    return this.addressForm.get( 'descuentos' ) as FormArray;
  };

  crearFormulario () {
    this.addressForm = this.fb.group({
      aplicarDescuentos:[null],
      numeroDescuentos: [ '' ],
      descuentos: this.fb.array([])
    });
  };
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }
  eliminarDescuento ( numerodesc: number ) {
    this.descuentos.removeAt( numerodesc );
    this.addressForm.patchValue({
      numeroDescuentos: `${ this.descuentos.length }`
    });
  };
  onSubmit() {
    console.log(this.addressForm.value);
  }
}
