import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-cargar-acta',
  templateUrl: './dialog-cargar-acta.component.html',
  styleUrls: ['./dialog-cargar-acta.component.scss']
})
export class DialogCargarActaComponent implements OnInit {

    formCargarActa: FormGroup;
    archivo: string;
    tipoArchivoPermitido = 'pdf';

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        @Inject(MAT_DIALOG_DATA) public data )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
        console.log( this.data );
    }

    crearFormulario() {
        this.formCargarActa = this.fb.group({
            fileCargarActa: [ null, Validators.required ]
        });
    }

    fileName() {
        const inputNode: any = document.getElementById('file');
        this.archivo = inputNode.files[0].name;
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '40em',
          data : { modalTitle, modalText }
        });
    }

    guardar() {
        const inputNode: any = document.getElementById('file');
        if ( this.formCargarActa.invalid === true || inputNode.files[0] === undefined ) {
            return;
        }
        console.log( inputNode.files[0] );
        if (inputNode.files[0].size > 1048576) {
            this.openDialog('', '<b>El tamaño del archivo es superior al permitido, debe subir un archivo máximo de 1MB.</b>');
            return;
        }
        let pFile = inputNode.files[0];
        pFile = pFile.name.split('.');
        pFile = pFile[pFile.length - 1];
        if ( pFile === this.tipoArchivoPermitido ) {
            const pContratacionProyecto = new FormData();
            pContratacionProyecto.append( 'pFile', inputNode.files[0] );
            pContratacionProyecto.append( 'contratacionProyectoId', this.data.registro.contratacionProyectoId );
            this.avanceSemanalSvc.uploadContractTerminationCertificate( pContratacionProyecto )
                .subscribe(
                    response => this.openDialog( '', `<b>${ response.message }</b>` ),
                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                );
        } else {
            this.openDialog('', '<b>El tipo de archivo que esta intentando cargar no es permitido en la plataforma.<br>El tipo de documento soportado es .pdf</b>');
            return;
        }
    }

}
