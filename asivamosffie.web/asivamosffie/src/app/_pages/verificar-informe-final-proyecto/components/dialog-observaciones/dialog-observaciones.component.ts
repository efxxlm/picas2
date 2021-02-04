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
    esSupervision: [null, Validators.required],
    esCalificacion: [true, Validators.required]
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
    private validarInformeFinalService: ValidarInformeFinalService,
    @Inject(MAT_DIALOG_DATA) public data,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    if (this.data.informe.informeFinalInterventoriaObservacionesId > 0 && this.data.informe.informeFinalInterventoriaObservacionesId != null) {
      /*this.getInformeFinalInterventoriaObservacionByInformeFinalObservacion(
        this.data.informe.informeFinalInterventoriaObservacionesId
      );*/
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength())
    }
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

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0
    let posicion = 0
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias
      posicion += subcadena.length
    }
    return contadorConcurrencias
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    })
  }

  onSubmit() {
    console.log(this.observaciones.value);
  }
}
