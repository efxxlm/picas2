import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-accord-vigencias-valor',
  templateUrl: './accord-vigencias-valor.component.html',
  styleUrls: ['./accord-vigencias-valor.component.scss']
})
export class AccordVigenciasValorComponent implements OnInit {

    addressForm = this.fb.group(
        {
            seguros: this.fb.array( [
                this.fb.group(
                    {
                        nombre: [ 'Buen manejo y correcta inversión del anticipo' ],
                        vigenciaPoliza: [ new Date() ],
                        tieneSeguro: [ true ],
                        fechaSeguro: [ null, Validators.required ],
                        vigenciaAmparo: [ new Date() ],
                        tieneFechaAmparo: [ true ],
                        fechaAmparo: [ null, Validators.required ],
                        valor: [ 45000000 ],
                        tieneValorAmparo: [ true ],
                        valorAmparo: [ null, Validators.required ]
                    }
                )
            ] ),
            // vigenciaActualizadaPoliza: [null, Validators.required],
            // vigenciaActualizadaAmparo: [null, Validators.required],
            // valorActualizadoAmparo: [null, Validators.required]
        }
    );
    estaEditando = false;

    get seguros() {
        return this.addressForm.get( 'seguros' ) as FormArray;
    }

    constructor(
        private dialog: MatDialog,
        private fb: FormBuilder )
    { }

    ngOnInit(): void {
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
    }

    onSubmit() {
        this.estaEditando = true;
    }

}
