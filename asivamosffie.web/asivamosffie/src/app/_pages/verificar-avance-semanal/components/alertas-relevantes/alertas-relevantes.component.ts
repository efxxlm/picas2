import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-alertas-relevantes',
  templateUrl: './alertas-relevantes.component.html',
  styleUrls: ['./alertas-relevantes.component.scss']
})
export class AlertasRelevantesComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    formAlertasRelevantes: FormGroup = this.fb.group({
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
    seguimientoSemanalGestionObraId: number;
    seguimientoSemanalGestionObraAlertaId = 0;
    gestionAlertas: any;
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
            this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;

            if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                    && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAlerta.length > 0 )
            {
                this.gestionAlertas = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAlerta[0];
                if ( this.gestionAlertas !== undefined ) {
                    this.seguimientoSemanalGestionObraAlertaId = this.gestionAlertas.seguimientoSemanalGestionObraAlertaId;
                }
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
        console.log( this.formAlertasRelevantes.value );
    }

}
