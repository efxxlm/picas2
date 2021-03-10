import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-descargar-orden-giro',
  templateUrl: './dialog-descargar-orden-giro.component.html',
  styleUrls: ['./dialog-descargar-orden-giro.component.scss']
})
export class DialogDescargarOrdenGiroComponent implements OnInit {

    formDate: FormGroup = this.fb.group(
        {
            inicialDate: [ null, Validators.required ],
            finalDate: [ null, Validators.required ]
        }
    );

    constructor(
        private fb: FormBuilder,
        @Inject(MAT_DIALOG_DATA) public data )
    { }

    ngOnInit(): void {
    }

    descargar() {
        console.log( this.formDate );
    }

    btnDisabled() {
        if ( this.formDate.get( 'inicialDate' ).value !== null && this.formDate.get( 'finalDate' ).value !== null ) {
            return false;
        } else {
            return true;
        }
    }

}
