import { Component, Inject, OnInit } from '@angular/core'
import { FormBuilder, Validators } from '@angular/forms'
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { Respuesta } from 'src/app/core/_services/common/common.service'
import { VerificarInformeFinalService } from 'src/app/core/_services/verificarInformeFinal/verificar-informe-final.service'
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
    esCalificacion: [null, Validators.required],
    esApoyo: [null, Validators.required],
    fechaCreacion: [null, Validators.required],
    archivado: [null, Validators.required],
  })

  observacionesSupervisor = this.fb.group({
    informeFinalInterventoriaObservacionesId: [null, Validators.required],
    informeFinalInterventoriaId: [null, Validators.required],
    observaciones: [null, Validators.required],
    esSupervision: [null, Validators.required],
    esCalificacion: [null, Validators.required],
    esApoyo: [null, Validators.required],
    fechaCreacion: [null, Validators.required],
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
    private validarInformeFinalService: VerificarInformeFinalService,
    @Inject(MAT_DIALOG_DATA) public data,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    console.log("Crear nuevo form: ",this.data);
    if(this.data.informeFinalObservacion != null){
      this.observaciones.patchValue(this.data.informeFinalObservacion[0])
    }
    else{
      this.getInformeFinalInterventoriaObservacionByInformeFinalObservacion(
          this.data.informe.informeFinalInterventoriaObservacionesId
        )
    }
    if(this.data.informe.estadoValidacion === '4' || this.data.informe.estadoValidacion === '6'){
      this.observacionesSupervisor.patchValue(this.data.informe.observacionVigenteSupervisor)
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
    this.observaciones.value.informeFinalInterventoriaId = this.data.informe.informeFinalInterventoriaId
    this.observaciones.value.esApoyo = true;
    if(this.data.informe){
      this.data.informe.estadoValidacion === '6' ? this.observaciones.value.archivado = false : true;
    }else{
      this.observaciones.value.archivado = false;
    }
    if (this.data.informe.informeFinalInterventoriaObservacionesId != null) {
      this.observaciones.value.informeFinalInterventoriaObservacionesId = this.data.informe.informeFinalInterventoriaObservacionesId
    }
    this.dialog.getDialogById('dialogObservaciones').close({ observaciones: this.observaciones.value, id: this.data.informe.informeFinalInterventoriaId, tieneObservacionNoCumple: true, archivado: this.observaciones.value.archivado });
    this.openDialog('', '<b>La informaci??n ha sido guardada exitosamente.</b>');
  }


  createEditInformeFinalInterventoriaObservacion(pObservaciones: any) {
    this.validarInformeFinalService
      .createEditInformeFinalInterventoriaObservacion(pObservaciones)
      .subscribe((respuesta: Respuesta) => {
        this.openDialog('', respuesta.message)
        this.dialog.getDialogById('dialogObservacionesSupervisor').close()
        return
      })
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

  getInformeFinalInterventoriaObservacionByInformeFinalInterventoria(id: number) {
    this.validarInformeFinalService
      .getInformeFinalInterventoriaObservacionByInformeFinalInterventoria(id)
      .subscribe(responseData => {
        if(responseData != null){
          this.observaciones.patchValue(responseData)
        }
      })
  }
}
