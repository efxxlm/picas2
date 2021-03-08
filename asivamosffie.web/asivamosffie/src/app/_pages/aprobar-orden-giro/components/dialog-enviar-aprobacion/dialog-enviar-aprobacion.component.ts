import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-enviar-aprobacion',
  templateUrl: './dialog-enviar-aprobacion.component.html',
  styleUrls: ['./dialog-enviar-aprobacion.component.scss']
})
export class DialogEnviarAprobacionComponent implements OnInit {

    formUrl: FormGroup = this.fb.group( { urlSoporte: [ null, Validators.required ] } );

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        @Inject(MAT_DIALOG_DATA) public data )
    { }

    ngOnInit(): void {
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '40em',
          data : { modalTitle, modalText }
        });
    }

    guardar() {
        console.log( this.formUrl );
    }

}
