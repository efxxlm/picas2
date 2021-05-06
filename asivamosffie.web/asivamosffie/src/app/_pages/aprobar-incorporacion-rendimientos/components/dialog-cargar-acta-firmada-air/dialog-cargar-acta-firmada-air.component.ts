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

  minuteUrlForm: FormGroup;
  boton: string = "Cargar";
  archivo: string;
  tipoArchivoPermitido = 'pdf';
  constructor(private router: Router, public dialog: MatDialog, private fb: FormBuilder, 
    public matDialogRef: MatDialogRef<DialogCargarActaFirmadaAirComponent>, 
    @Inject(MAT_DIALOG_DATA) public data: any,
    private faseDosPagosRendimientosSvc: FaseDosPagosRendimientosService) { }

  ngOnInit(): void {
    this.buidForm()
  }

  buidForm() {
    this.minuteUrlForm = this.fb.group({
      resourceId : this.data.registerId,
      fileName: [null, Validators.required]
    });
}

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }
  
  onSubmit() {
    if ( this.minuteUrlForm.invalid ) {
        return;
    }
  //  this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
    let pFile = this.minuteUrlForm.value.fileName;
    pFile = pFile.split('.');
    if ( pFile.length > 0 && pFile[pFile.length - 1] === this.tipoArchivoPermitido  ) {
       
        this.faseDosPagosRendimientosSvc.uploadMinutes( this.minuteUrlForm.value )
            .subscribe(response =>{
              this.openDialog( '', `<b>${ response.message }</b>` );
              this.close();
            }
             , err => this.openDialog( '', `<b>${ err.message }</b>` )
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
