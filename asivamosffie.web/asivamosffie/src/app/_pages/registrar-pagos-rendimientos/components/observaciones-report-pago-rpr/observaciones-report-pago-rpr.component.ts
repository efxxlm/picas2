import { Component, Inject, OnInit } from '@angular/core'
import { FormBuilder, Validators } from '@angular/forms'
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog'
import { FaseDosPagosRendimientosService } from '../../../../core/_services/faseDosPagosRendimientos/fase-dos-pagosRendimientos.service'
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component'

@Component({
  selector: 'app-observaciones-report-pago-rpr',
  templateUrl: './observaciones-report-pago-rpr.component.html',
  styleUrls: ['./observaciones-report-pago-rpr.component.scss']
})
export class ObservacionesReportPagoRprComponent implements OnInit {
  addressForm = this.fb.group({})
  editorStyle = {
    height: '45px',
    overflow: 'auto'
  }

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }]
    ]
  }
  constructor(
    private dialog: MatDialog,
    public matDialogRef: MatDialogRef<ObservacionesReportPagoRprComponent>,
    private faseDosPagosRendimientosSvc: FaseDosPagosRendimientosService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.addressForm = this.crearFormulario()
  }
  crearFormulario() {
    return this.fb.group({
      observaciones: [null, Validators.required]
    })
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n - 1, e.editor.getLength())
    }
  }
  textoLimpio(texto, n) {
    if (texto != undefined) {
      return texto.getLength() > n ? n : texto.getLength()
    }
  }
  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data: { modalTitle, modalText }
    })
  }
  onSubmit() {
    this.matDialogRef.close({
      data: this.addressForm.value.observaciones
    })
    this.openDialog('', '<b>La información ha sido guardada exitosamente</b>')
  }
}
