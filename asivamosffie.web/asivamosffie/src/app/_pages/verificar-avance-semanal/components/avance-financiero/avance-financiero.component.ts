import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-avance-financiero',
  templateUrl: './avance-financiero.component.html',
  styleUrls: ['./avance-financiero.component.scss']
})
export class AvanceFinancieroComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    formAvanceFinanciero: FormGroup = this.fb.group({
      tieneObservaciones: [ null, Validators.required ],
      observaciones: [ null ]
    });
    tablaHistorial = new MatTableDataSource();
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    dataHistorial: any[] = [
        {
            fechaRevision: new Date(),
            responsable: 'Apoyo a la supervisión',
            historial: '<p>Se recomienda que en cada actividad se especifique el responsable.</p>'
        }
    ];
    seguimientoSemanalId: number;
    seguimientoSemanalAvanceFinancieroId: number;
    avanceFinanciero: any;
    editorStyle = {
        height: '100px'
    };
    config = {
      toolbar: [
        ['bold', 'italic', 'underline'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ indent: '-1' }, { indent: '+1' }],
        [{ align: [] }],
      ]
    };

    constructor( private fb: FormBuilder ) { }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalAvanceFinancieroId =  this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero[0].seguimientoSemanalAvanceFinancieroId : 0;

            if ( this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero.length > 0 ) {
                this.avanceFinanciero = this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero[0];
            }
            this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
        }
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }

    guardar() {
        console.log( this.formAvanceFinanciero.value );
    }

}
