import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-alertas-relevantes',
  templateUrl: './alertas-relevantes.component.html',
  styleUrls: ['./alertas-relevantes.component.scss']
})
export class AlertasRelevantesComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    formAlertasRelevantes: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    seguimientoSemanalGestionObraAlertaId = 0;
    gestionAlertas: any;
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
