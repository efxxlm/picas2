import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FaseDosPagosRendimientosService } from 'src/app/core/_services/faseDosPagosRendimientos/fase-dos-pagosRendimientos.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-dialog-cargar-report-rend-rpr',
  templateUrl: './dialog-cargar-report-rend-rpr.component.html',
  styleUrls: ['./dialog-cargar-report-rend-rpr.component.scss']
})
export class DialogCargarReportRendRprComponent implements OnInit {
  addressForm = new FormGroup({
    documentoFile: new FormControl()
  });
  typeFile = 'Rendimientos'
  boton: string = "Cargar";
  archivo: string;
  refresh: boolean = false
  constructor(private router: Router, public dialog: MatDialog, 
    private fb: FormBuilder, 
    public matDialogRef: MatDialogRef<DialogCargarReportRendRprComponent>, 
    @Inject(MAT_DIALOG_DATA) public data: any,
    private faseDosPagosRendimientosSvc: FaseDosPagosRendimientosService) { }

  ngOnInit(): void {
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText }
    });
  }
  openDialogNoConfirmar (modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data : { modalTitle, modalText}
    });
  };
  openDialogConfirmar (modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data: { modalTitle, modalText, siNoBoton: true }
    })

    confirmarDialog.afterClosed().subscribe((response) => {
      if (response === true) {
        let pFile = this.addressForm.get('documentoFile').value

        this.faseDosPagosRendimientosSvc
          .uploadFileToValidate(pFile, this.typeFile, true)
          .subscribe((response: any) => {
            this.openDialog('', 'La información ha sido guardada exitosamente')
          })
      }
    })
  };
  fileName(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.archivo = event.target.files[0].name;
      this.addressForm.patchValue({
        documentoFile: file
      });
    }
  }

  onSubmit() {
    this.refresh = true
    const pContrato = new FormData();
    let pFile = this.addressForm.get('documentoFile').value;
    let extFile = pFile.name.split('.')
    extFile = extFile[extFile.length - 1]
    if (extFile === 'xlsx') {
      this.uploadWorkSheetFile(pFile);
    } else {
      this.openDialog('', '<b>El tipo de archivo que esta intentando cargar no es permitido en la plataforma.<br>El tipo de documento soportado es .xlsx</b>');
      return;
    }   
  }

  private uploadWorkSheetFile(pFile: any) {
    this.faseDosPagosRendimientosSvc
      .uploadFileToValidate(pFile, this.typeFile, false)
      .subscribe((response: any) => {
        const performanceSumm = response.data;
        if (performanceSumm.cantidadDeRegistrosInvalidos > 0) {
          this.openDialog(
            'Validación de registro',
            `${this.templateModal(performanceSumm, false)}`
          );
        }else{
          this.openDialogConfirmar(
            'Validación de registro',
            `${this.templateModal(performanceSumm, true)}`
          );
        }
      });
  }

  
  private templateModal(data, valid: boolean):string{
    let userMessage = ""
    if(valid){
      userMessage = "<b>¿Desea realizar el cargue de los reportes de pagos?</b>"
    }else{
      userMessage = `<b>No se permite el cargue, ya que el archivo tiene registros
      inválidos. Ajuste el archivo y cargue de nuevo</b>`
    }

    return ` <br>Número de registros en el archivo: <b>${data.cantidadDeRegistros}</b>
    Número de registros válidos: <b>${data.cantidadDeRegistrosValidos}</b><br>
    Número de registros inválidos: <b>${data.cantidadDeRegistrosInvalidos}</b><br><br>
    ${userMessage}`
  }


  close() {
    this.matDialogRef.close(this.refresh);
  }

}
