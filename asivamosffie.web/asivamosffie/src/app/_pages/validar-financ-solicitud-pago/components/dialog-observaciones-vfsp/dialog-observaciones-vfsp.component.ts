import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-observaciones-vfsp',
  templateUrl: './dialog-observaciones-vfsp.component.html',
  styleUrls: ['./dialog-observaciones-vfsp.component.scss']
})
export class DialogObservacionesVfspComponent implements OnInit {

    addressForm: FormGroup;
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
    estaEditando = false;

    constructor(
        public matDialogRef: MatDialogRef<DialogObservacionesVfspComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private fb: FormBuilder )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        this.addressForm.setValue(
            {
                observaciones: this.data.registro.get( 'observacion' ).value,
                tieneSubsanacion: this.data.registro.get( 'tieneSubsanacion' ).value
            }
        );
    }

    crearFormulario() {
        return this.fb.group({
            observaciones: [ null, Validators.required ],
            tieneSubsanacion: [ null, Validators.required ]
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
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      this.matDialogRef.close( this.addressForm.value );
    }

}
