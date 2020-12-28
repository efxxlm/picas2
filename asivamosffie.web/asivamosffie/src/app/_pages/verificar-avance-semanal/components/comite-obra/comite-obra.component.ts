import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-comite-obra',
  templateUrl: './comite-obra.component.html',
  styleUrls: ['./comite-obra.component.scss']
})
export class ComiteObraComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    numeroComiteObra: string;
    seguimientoSemanalId: number;
    seguimientoSemanalRegistrarComiteObraId: number;
    gestionComiteObra: any;
    formComiteObra: FormGroup = this.fb.group({
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
            this.seguimientoSemanalRegistrarComiteObraId =  this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra[0].seguimientoSemanalRegistrarComiteObraId : 0;
            if ( this.seguimientoSemanal.comiteObraGenerado !== undefined ) {
                this.numeroComiteObra = this.seguimientoSemanal.comiteObraGenerado;
            }

            if ( this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra.length > 0 ) {
                this.gestionComiteObra = this.seguimientoSemanal.seguimientoSemanalRegistrarComiteObra[0];
                this.numeroComiteObra = this.gestionComiteObra.numeroComite;
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
        console.log( this.formComiteObra.value );
    }

}
