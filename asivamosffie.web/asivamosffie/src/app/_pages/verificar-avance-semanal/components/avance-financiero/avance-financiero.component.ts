import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-avance-financiero',
  templateUrl: './avance-financiero.component.html',
  styleUrls: ['./avance-financiero.component.scss']
})
export class AvanceFinancieroComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    formAvanceFinanciero: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalAvanceFinancieroId: number;
    avanceFinanciero: any;
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
            this.seguimientoSemanalAvanceFinancieroId =  this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero[0].seguimientoSemanalAvanceFinancieroId : 0;

            if ( this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero.length > 0 ) {
                this.avanceFinanciero = this.seguimientoSemanal.seguimientoSemanalAvanceFinanciero[0];
            }
        }
    }

    textoLimpio(texto: string) {
        if ( texto ){
            const textolimpio = texto.replace(/<[^>]*>/g, '');
            return textolimpio.length;
        }
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
          e.editor.deleteText(n, e.editor.getLength());
        }
    }

}
