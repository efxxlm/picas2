import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-form-reporte-actividades-realizadas',
  templateUrl: './form-reporte-actividades-realizadas.component.html',
  styleUrls: ['./form-reporte-actividades-realizadas.component.scss']
})
export class FormReporteActividadesRealizadasComponent implements OnInit {

    @Input() esSiguienteSemana: boolean;
    @Input() esVerDetalle = false;
    @Input() reporteActividad: any;
    @Output() reporteDeActividades = new EventEmitter();
    formActividadesRealizadas: FormGroup;
    formActividadesRealizadasSiguienteSemana: FormGroup;
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
        this.crearFormularioSiguienteSemana();
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        this.formActividadesRealizadas = this.fb.group({
            actividadTecnica: [ null ],
            actividadLegal: [ null ],
            actividadAdministrativaFinanciera: [ null ]
        });
    }

    crearFormularioSiguienteSemana() {
        this.formActividadesRealizadasSiguienteSemana = this.fb.group({
            actividadTecnicaSiguiente: [ null ],
            actividadLegalSiguiente: [ null ],
            actividadAdministrativaFinancieraSiguiente: [ null ]
        });
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
        if ( this.esSiguienteSemana === true ) {
            this.reporteDeActividades.emit(
                {
                    esSiguienteSemana: this.esSiguienteSemana,
                    reporteActividades: this.formActividadesRealizadasSiguienteSemana.value
                }
            );
        } else {
            this.reporteDeActividades.emit(
                {
                    esSiguienteSemana: this.esSiguienteSemana,
                    reporteActividades: this.formActividadesRealizadas.value
                }
            );
        }
    }

}
