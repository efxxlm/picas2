import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-form-amortizacion-anticipo',
  templateUrl: './form-amortizacion-anticipo.component.html',
  styleUrls: ['./form-amortizacion-anticipo.component.scss']
})
export class FormAmortizacionAnticipoComponent implements OnInit {

    addressForm = this.fb.group({
      porcentajeAmortizacion: [null, Validators.required],
      valorAmortizacion: [ { value: null, disabled: true } , Validators.required]
    });

    constructor( private fb: FormBuilder ){
        this.addressForm.get( 'porcentajeAmortizacion' ).valueChanges
            .subscribe(
                value => {
                    const exampleValue = 30000000;
                    const porcentajeCalculo = value / 100;
                    const valorAmortizacion = exampleValue * porcentajeCalculo;
                    this.addressForm.get( 'valorAmortizacion' ).setValue( valorAmortizacion );
                }
            );
    }

    ngOnInit(): void {
    }

    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }

    numberValidate( value: any ) {
        if ( value > 100 ) {
            this.addressForm.get( 'porcentajeAmortizacion' ).setValue( 100 );
        }
        if ( value < 0 ) {
            this.addressForm.get( 'porcentajeAmortizacion' ).setValue( 0 );
        }
    }

    onSubmit() {
    }

}
