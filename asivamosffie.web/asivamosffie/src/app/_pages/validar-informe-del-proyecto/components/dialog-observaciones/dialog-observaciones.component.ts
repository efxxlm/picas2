import { Component, Inject, OnInit } from '@angular/core'
import { FormBuilder, Validators } from '@angular/forms'
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { Respuesta } from 'src/app/core/_services/common/common.service'
import { ValidarInformeFinalService } from 'src/app/core/_services/validarInformeFinal/validar-informe-final.service'
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component'


@Component({
  selector: 'app-dialog-observaciones',
  templateUrl: './dialog-observaciones.component.html',
  styleUrls: ['./dialog-observaciones.component.scss']
})
export class DialogObservacionesComponent implements OnInit {

  observaciones = this.fb.group({
    informeFinalInterventoriaObservacionesId: [null, Validators.required],
    informeFinalInterventoriaId: [null, Validators.required],
    observaciones: [null, Validators.required],
    esSupervision: [true, Validators.required],
    esCalificacion: [null, Validators.required],
    esApoyo: [null, Validators.required],
  })

  editorStyle = {
    height: '100px'
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
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data,
    private validarInformeFinalService: ValidarInformeFinalService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    if(this.data.informeFinalObservacion != null){
      this.observaciones.patchValue(this.data.informeFinalObservacion[0])
    }else{
      this.getInformeFinalInterventoriaObservacionByInformeFinalObservacion(
        this.data.informe.informeFinalInterventoriaObservacionesId
      )
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength())
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0
    let posicion = 0
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias
      posicion += subcadena.length
    }
    return contadorConcurrencias
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p>')
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li>')

    if (texto) {
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '')
      return textolimpio.length + saltosDeLinea
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    })
  }

  onSubmit() {
    this.observaciones.value.informeFinalInterventoriaId = this.data.informe.informeFinalInterventoriaId
    this.observaciones.value.esSupervision = true
    if (this.data.informe.informeFinalInterventoriaObservacionesId != null) {
      this.observaciones.value.informeFinalInterventoriaObservacionesId = this.data.informe.informeFinalInterventoriaObservacionesId
    }
    this.dialog.getDialogById('dialogObservaciones').close({ observaciones: this.observaciones.value, id: this.data.informe.informeFinalInterventoriaId });
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

  getInformeFinalInterventoriaObservacionByInformeFinalObservacion(id: number) {
    this.validarInformeFinalService
      .getInformeFinalInterventoriaObservacionByInformeFinalObservacion(id)
      .subscribe(responseData => {
        if(responseData != null){
          this.observaciones.patchValue(responseData)
        }
      })
  }
}
