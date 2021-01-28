import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-otros-manejos',
  templateUrl: './otros-manejos.component.html',
  styleUrls: ['./otros-manejos.component.scss']
})
export class OtrosManejosComponent implements OnInit {

    @Input() formOtrosManejos: FormGroup;
    @Input() otrosManejos: any;
    @Input() esVerDetalle = false;
    otros: any;
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
        if ( this.otrosManejos !== undefined && this.otrosManejos.length > 0 ) {
            this.otros = this.otrosManejos[0].manejoOtro;
            if ( this.otros !== undefined ) {
                this.formOtrosManejos.setValue(
                    {
                        manejoOtroId: this.otros.manejoOtroId,
                        fechaActividad: this.otros.fechaActividad !== undefined ? this.otros.fechaActividad : null,
                        actividad: this.otros.actividad !== undefined ? this.otros.actividad : null,
                        urlSoporteGestion: this.otros.urlSoporteGestion !== undefined ? this.otros.urlSoporteGestion : ''
                    }
                );
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
