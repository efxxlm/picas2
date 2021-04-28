import { CommonService } from './../../../../core/_services/common/common.service';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-dialog-cargar-acta',
  templateUrl: './dialog-cargar-acta.component.html',
  styleUrls: ['./dialog-cargar-acta.component.scss']
})
export class DialogCargarActaComponent implements OnInit {

    formCargarActa: FormGroup;
    modalidadContratoArray: Dominio[] = [];
    listaModalidadCodigo: any = {};
    archivo: string;
    tipoArchivoPermitido = 'pdf';

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private matDialogRef: MatDialogRef<DialogCargarActaComponent>,
        @Inject(MAT_DIALOG_DATA) public data )
    {
        this.commonSvc.modalidadesContrato()
            .subscribe( response => {
                this.modalidadContratoArray = response;
                console.log( this.modalidadContratoArray );
                this.modalidadContratoArray.forEach( modalidad => {
                    if ( modalidad.codigo === '1' ) {
                        this.listaModalidadCodigo.tipoA = modalidad.codigo;
                    }
                    if ( modalidad.codigo === '2' ) {
                        this.listaModalidadCodigo.tipoB = modalidad.codigo;
                    }
                } );
            } );
        this.crearFormulario();
    }

    ngOnInit(): void {
        console.log( this.data );
    }

    crearFormulario() {
        this.formCargarActa = this.fb.group({
            rutaCargaActaTerminacionContrato: [ null, Validators.required ],
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
        if ( this.data.registro.modalidadCodigo === this.listaModalidadCodigo.tipoA ) {
            const pContratacionProyecto = new FormData();
            pContratacionProyecto.append( 'rutaCargaActaTerminacionContrato', this.formCargarActa.get( 'rutaCargaActaTerminacionContrato' ).value );
            pContratacionProyecto.append( 'contratacionProyectoId', this.data.registro.contratacionProyectoId );
            this.avanceSemanalSvc.uploadContractTerminationCertificate( pContratacionProyecto )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.matDialogRef.close();
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
        }

        if ( this.data.registro.modalidadCodigo === this.listaModalidadCodigo.tipoB ) {
            const inputNode: any = document.getElementById('file');
            if ( inputNode.files[0] === undefined ) {
                return;
            }

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
                        response => {
                            this.openDialog( '', `<b>${ response.message }</b>` );
                            this.matDialogRef.close();
                        },
                        err => this.openDialog( '', `<b>${ err.message }</b>` )
                    );
            } else {
                this.openDialog('', '<b>El tipo de archivo que esta intentando cargar no es permitido en la plataforma.<br>El tipo de documento soportado es .pdf</b>');
                return;
            }
        }
    }

}
