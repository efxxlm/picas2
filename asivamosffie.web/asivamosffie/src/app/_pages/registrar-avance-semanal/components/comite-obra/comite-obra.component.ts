import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-comite-obra',
  templateUrl: './comite-obra.component.html',
  styleUrls: ['./comite-obra.component.scss']
})
export class ComiteObraComponent implements OnInit {

    formComiteObra: FormGroup;

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        this.formComiteObra = this.fb.group({
            fechaComiteObra: [ null ],
            urlSoporte: [ null ]
        });
    }

    guardar() {
        console.log( this.formComiteObra.value );
    }

}
