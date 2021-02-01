import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-form-amortizacion',
  templateUrl: './form-amortizacion.component.html',
  styleUrls: ['./form-amortizacion.component.scss']
})
export class FormAmortizacionComponent implements OnInit {

    @Input() esVerDetalle = false;
    addressForm: FormGroup;
    detalleForm = this.fb.group({
        porcentajeAmortizacion: [null, Validators.required],
        valorAmortizacion: [ { value: null, disabled: true } , Validators.required]
    });
    editorStyle = {
      height: '45px',
      overflow: 'auto'
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
        private fb: FormBuilder,
        private routes: Router,
        private dialog: MatDialog )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        return this.fb.group({
          tieneObservaciones: [null, Validators.required],
          observaciones:[null, Validators.required],
        })
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

    onSubmit() {
      console.log(this.addressForm.value);
    }

}
