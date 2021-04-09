import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import moment from 'moment';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';

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
        private obsOrdenGiro: ObservacionesOrdenGiroService,
        @Inject(MAT_DIALOG_DATA) public data )
    { }

    ngOnInit(): void {
    }

    btnDisabled() {
        if ( this.formDate.get( 'inicialDate' ).value !== null && this.formDate.get( 'finalDate' ).value !== null ) {
            return false;
        } else {
            return true;
        }
    }

    getOrdenGiroAprobadas() {
        const pDescargarOrdenGiro = {
            registrosAprobados: false,
            fechaInicial: new Date( this.formDate.get( 'inicialDate' ).value ).toISOString(),
            fechaFinal: new Date( this.formDate.get( 'finalDate' ).value ).toISOString()
        }

        this.obsOrdenGiro.getListOrdenGiro( pDescargarOrdenGiro )
        .subscribe(
            response => {
                const fechaInicial = moment( this.formDate.get( 'inicialDate' ).value ).format( 'DD/MM/YYYY' );
                const fechaFinal = moment( this.formDate.get( 'finalDate' ).value ).format( 'DD/MM/YYYY' );
                const nombreDocumento = `Listado órdenes de giro para tramitar ${ fechaInicial } - ${ fechaFinal }`;
                const blob = new Blob( [ response ], { type: 'application/pdf' } );
                const anchor = document.createElement('a');

                anchor.download = nombreDocumento;
                anchor.href = window.URL.createObjectURL( blob );
                anchor.dataset.downloadurl = ['application/vnd.openxmlformats-officedocument.wordprocessingml.document', anchor.download, anchor.href].join(':');
                anchor.click();
            }
        );
    }

}
