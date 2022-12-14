import { Component, Inject, OnInit } from '@angular/core'
import { FormGroup, FormControl, FormBuilder } from '@angular/forms'
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog'
import { Router } from '@angular/router'
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component'
import { FaseDosPagosRendimientosService } from '../../../../core/_services/faseDosPagosRendimientos/fase-dos-pagosRendimientos.service'

@Component({
  selector: 'app-dialog-cargar-report-pagos-rpr',
  templateUrl: './dialog-cargar-report-pagos-rpr.component.html',
  styleUrls: ['./dialog-cargar-report-pagos-rpr.component.scss']
})
export class DialogCargarReportPagosRprComponent implements OnInit {
  addressForm = new FormGroup({
    documentoFile: new FormControl()
  })
  typeFile = 'Pagos'
  boton: string = 'Cargar'
  archivo: string
  refresh: boolean = false
  constructor(
    private router: Router,
    public dialog: MatDialog,
    private fb: FormBuilder,
    private faseDosPagosRendimientosSvc: FaseDosPagosRendimientosService,
    public matDialogRef: MatDialogRef<DialogCargarReportPagosRprComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit(): void {}
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText }
    })
  }
  openDialogNoConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data: { modalTitle, modalText }
    })
  }
  openDialogConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data: { modalTitle, modalText, siNoBoton: true }
    })

    confirmarDialog.afterClosed().subscribe((response) => {
      if (response === true) {
        let pFile = this.addressForm.get('documentoFile').value

        this.faseDosPagosRendimientosSvc
          .validateUploadPaymentRegisterFile(pFile, true)
          .subscribe((response: any) => {
            this.openDialog('', 'La informaci??n ha sido guardada exitosamente')
          })
      }
    })
  }
  fileName(event: any) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0]
      this.archivo = event.target.files[0].name
      this.addressForm.patchValue({
        documentoFile: file
      })
    }
  }
  onSubmit() {
    this.refresh = true
    const pContrato = new FormData()

    let pFile = this.addressForm.get('documentoFile').value
    let extFile = pFile.name.split('.')
    extFile = extFile[extFile.length - 1]
    if (extFile === 'xlsx') {
      this.faseDosPagosRendimientosSvc
        .validateUploadPaymentRegisterFile(pFile, false)
        .subscribe((response: any) => {
          if (response.data.cantidadDeRegistrosInvalidos > 0) {
            this.openDialog(
              'Validaci??n de registro',
              ` <br>N??mero de registros en el archivo: <b>${response.data.cantidadDeRegistros}</b>
        N??mero de registros v??lidos: <b>${response.data.cantidadDeRegistrosValidos}</b><br>
        N??mero de registros inv??lidos: <b>${response.data.cantidadDeRegistrosInvalidos}</b><br><br>
        <b>No se permite el cargue, ya que el archivo tiene registros
inv??lidos. Ajuste el archivo y cargue de nuevo</b>
        `
            )
          } else {
            this.openDialogConfirmar(
              'Validaci??n de registro',
              ` <br>N??mero de registros en el archivo: <b>${response.data.cantidadDeRegistros}</b><br>
        N??mero de registros v??lidos: <b>${response.data.cantidadDeRegistrosValidos}</b><br>
        N??mero de registros inv??lidos: <b>${response.data.cantidadDeRegistrosInvalidos}</b><br><br>
        <b>??Desea realizar el cargue de los reportes de los rendimientos?</b>
        `
            )
          }
        })
    } else {
      this.openDialog(
        '',
        '<b>El tipo de archivo que esta intentando cargar no es permitido en la plataforma.<br>El tipo de documento soportado es .xlsx</b>'
      )
      return
    }
  }
  close() {
    this.matDialogRef.close(this.refresh)
  }
}
