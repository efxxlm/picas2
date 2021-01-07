import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-registro-fotografico',
  templateUrl: './registro-fotografico.component.html',
  styleUrls: ['./registro-fotografico.component.scss']
})
export class RegistroFotograficoComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    seguimientoSemanalId: number;
    seguimientoSemanalRegistroFotograficoId: number;
    reporteFotografico: any;
    formRegistroFotografico: FormGroup = this.fb.group({
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
            this.seguimientoSemanalRegistroFotograficoId =  this.seguimientoSemanal.seguimientoSemanalRegistroFotografico.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalRegistroFotografico[0].seguimientoSemanalRegistroFotograficoId : 0;

            if ( this.seguimientoSemanal.seguimientoSemanalRegistroFotografico.length > 0 ) {
                this.reporteFotografico = this.seguimientoSemanal.seguimientoSemanalRegistroFotografico[0];
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
        console.log( this.formRegistroFotografico.value );
    }

}
