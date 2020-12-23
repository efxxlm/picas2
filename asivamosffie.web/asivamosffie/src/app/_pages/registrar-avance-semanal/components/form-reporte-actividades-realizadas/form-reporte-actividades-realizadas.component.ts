import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-form-reporte-actividades-realizadas',
  templateUrl: './form-reporte-actividades-realizadas.component.html',
  styleUrls: ['./form-reporte-actividades-realizadas.component.scss']
})
export class FormReporteActividadesRealizadasComponent implements OnInit {

    formActividadesRealizadas: FormGroup;
    @Input() esSiguienteSemana: boolean;
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

    constructor(
        private fb: FormBuilder )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
        console.log( this.esSiguienteSemana );
    }

    crearFormulario() {
        this.formActividadesRealizadas = this.fb.group({
            observacionTecnica: [ null ],
            observacionLegal: [ null ],
            observacionAdministrativa: [ null ]
        });
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

    guardar() {
        if ( this.esSiguienteSemana === true ) {
            console.log( 'esSiguienteSemana', this.formActividadesRealizadas.value );
        } else {
            console.log( 'noEsSiguienteSemana', this.formActividadesRealizadas.value );
        }
    }

}
