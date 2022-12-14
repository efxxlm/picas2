import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-manejo-residuos-peligrosos',
  templateUrl: './manejo-residuos-peligrosos.component.html',
  styleUrls: ['./manejo-residuos-peligrosos.component.scss']
})
export class ManejoResiduosPeligrososComponent implements OnInit {

    @Input() formManejoResiduosPeligrosos: FormGroup;
    @Input() residuosPeligrosos: any;
    @Input() esVerDetalle = false;
    manejoResiduosPeligrosos: any;
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
        if ( this.residuosPeligrosos !== undefined && this.residuosPeligrosos.length > 0 ) {
            this.manejoResiduosPeligrosos = this.residuosPeligrosos[0].manejoResiduosPeligrososEspeciales;
            if ( this.manejoResiduosPeligrosos !== undefined ) {
                this.formManejoResiduosPeligrosos.setValue(
                    {
                        manejoResiduosPeligrososEspecialesId: this.manejoResiduosPeligrosos.manejoResiduosPeligrososEspecialesId,
                        estanClasificados:  this.manejoResiduosPeligrosos.estanClasificados !== undefined
                                            ? this.manejoResiduosPeligrosos.estanClasificados : null,
                        requiereObservacion:    this.manejoResiduosPeligrosos.requiereObservacion !== undefined
                                                ? this.manejoResiduosPeligrosos.requiereObservacion : null,
                        observacion:    this.manejoResiduosPeligrosos.observacion !== undefined
                                        ? this.manejoResiduosPeligrosos.observacion : null,
                        urlRegistroFotografico: this.manejoResiduosPeligrosos.urlRegistroFotografico !== undefined
                                                ? this.manejoResiduosPeligrosos.urlRegistroFotografico : ''
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
