import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FaseDosPagosRendimientosService } from 'src/app/core/_services/faseDosPagosRendimientos/fase-dos-pagosRendimientos.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-cargar-acta-firmada-air',
  templateUrl: './dialog-cargar-acta-firmada-air.component.html',
  styleUrls: ['./dialog-cargar-acta-firmada-air.component.scss']
})
export class DialogCargarActaFirmadaAirComponent implements OnInit {

  addressForm = this.fb.group({
    urlSoporte: [null, Validators.required]
  });
  boton: string = "Cargar";
  archivo: string;
  formCargarActa: FormGroup;
  tipoArchivoPermitido = 'pdf';
  constructor(private router: Router, public dialog: MatDialog, private fb: FormBuilder, 
    public matDialogRef: MatDialogRef<DialogCargarActaFirmadaAirComponent>, 
    @Inject(MAT_DIALOG_DATA) public data: any,
    private faseDosPagosRendimientosSvc: FaseDosPagosRendimientosService) { }

  ngOnInit(): void {
    this.crearFormulario()
  }


  crearFormulario() {
    this.formCargarActa = this.fb.group({
        fileCargarActa: [ null, Validators.required ]
    });
}

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

  fileName() {
    const inputNode: any = document.getElementById('file');
    this.archivo = inputNode.files[0].name;
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
        this.faseDosPagosRendimientosSvc.uploadMinutes( this.data, pContratacionProyecto  )
            .subscribe(
                response => this.openDialog( '', `<b>${ response.message }</b>` ),
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    } else {
        this.openDialog('', `<b>El tipo de archivo que esta intentando cargar no es permitido en la plataforma.<br>
        El tipo de documento soportado es .pdf</b>`);
        return;
    }
}


  close() {
    this.matDialogRef.close('aceptado');
  }
}
