import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray } from '@angular/forms';

@Component({
  selector: 'app-manejo-residuos-construccion',
  templateUrl: './manejo-residuos-construccion.component.html',
  styleUrls: ['./manejo-residuos-construccion.component.scss']
})
export class ManejoResiduosConstruccionComponent implements OnInit {

    @Input() formManejoResiduosConstruccion: FormGroup;
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
    booleanosActividadRelacionada: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];

    get gestorResiduos() {
        return this.formManejoResiduosConstruccion.get( 'gestorResiduos' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder ) { }

    ngOnInit(): void {
    }

    validateNumber( value: string, campoForm: string ) {
        if ( isNaN( Number( value ) ) === true ) {
            this.formManejoResiduosConstruccion.get( campoForm ).setValue( '' );
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

    addGestor() {
        this.gestorResiduos.push(
            this.fb.group(
                {
                    gestorResiduosConstruccion: [ '' ],
                    presentaPermisoAmbientalValido: [ null ],
                    urlSoporte: [ '' ]
                }
            )
        );
    }

    deleteGestor( index: number ) {
        this.gestorResiduos.removeAt( index );
    }

}
