import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-reporte-actividades',
  templateUrl: './reporte-actividades.component.html',
  styleUrls: ['./reporte-actividades.component.scss']
})
export class ReporteActividadesComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    formResumenGeneral: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalReporteActividadId: number;
    reporteActividad: any;
    semaforoReporte = 'sin-diligenciar';
    semaforoActividad = 'sin-diligenciar';
    semaforoActividadSiguiente = 'sin-diligenciar';
    editorStyle = {
        height: '45px'
    };
    config = {
        toolbar: [
          ['bold', 'italic', 'underline'],
          [{ list: 'ordered' }, { list: 'bullet' }],
          [{ indent: '-1' }, { indent: '+1' }],
          [{ align: [] }],
        ]
    };

    constructor() { }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalReporteActividadId =  this.seguimientoSemanal.seguimientoSemanalReporteActividad.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalReporteActividad[0].seguimientoSemanalReporteActividadId : 0;

            if ( this.seguimientoSemanal.seguimientoSemanalReporteActividad.length > 0 ) {
                this.reporteActividad = this.seguimientoSemanal.seguimientoSemanalReporteActividad[0];
            }
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

}
