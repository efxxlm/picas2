import { Component, Inject, OnInit } from '@angular/core'
import { FormBuilder, Validators } from '@angular/forms'
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { Respuesta } from 'src/app/core/_services/common/common.service'
import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrar-informe-final-proyecto.service'
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
  estaEditando = false

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
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService,
    @Inject(MAT_DIALOG_DATA) public data,
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
    // console.log(this.data.informe);
    this.estaEditando = true;
    this.observaciones.value.informeFinalInterventoriaId = this.data.informe.informeFinalInterventoriaId
    this.observaciones.value.esCalificacion = true
    if (this.data.informe.informeFinalInterventoriaObservacionesId != null) {
      this.observaciones.value.informeFinalInterventoriaObservacionesId = this.data.informe.informeFinalInterventoriaObservacionesId
    }
    this.dialog.getDialogById('dialogObservaciones').close({ observaciones: this.observaciones.value, id: this.data.informe.informeFinalInterventoriaId });
    //this.createEditInformeFinalInterventoriaObservacion(this.observaciones.value)
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }

  createEditInformeFinalInterventoriaObservacion(pObservaciones: any) {
    this.registrarInformeFinalProyectoService
      .createEditInformeFinalInterventoriaObservacion(pObservaciones)
      .subscribe((respuesta: Respuesta) => {
        this.openDialog('', respuesta.message)
        this.dialog.getDialogById('dialogObservaciones').close()
        return
      })
  }

  getInformeFinalInterventoriaObservacionByInformeFinalObservacion(id: number) {
    this.registrarInformeFinalProyectoService
      .getInformeFinalInterventoriaObservacionByInformeFinalObservacion(id)
      .subscribe(responseData => {
        if(responseData != null){
          this.observaciones.patchValue(responseData)
        }
      })
  }
}
