import { Component, Inject, OnInit } from '@angular/core'
import { FormBuilder, Validators } from '@angular/forms'
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { Router } from '@angular/router'
import { ProcesosContractualesService } from 'src/app/core/_services/procesosContractuales/procesos-contractuales.service'
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component'

@Component({
  selector: 'app-dialog-observaciones',
  templateUrl: './dialog-observaciones.component.html',
  styleUrls: ['./dialog-observaciones.component.scss']
})
export class DialogObservacionesComponent implements OnInit {
  observacion = this.fb.group({
    procesosContractualesObservacionId: [null, Validators.required],
    tipoSolicitudCodigo: [null, Validators.required],
    solicitudId: [null, Validators.required],
    tieneObservacion: [null, Validators.required],
    observacion: [null, Validators.required],
    tipoObservacionCodigo: [null, Validators.required],
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
    private routes: Router,
    private procesosContractualesSvc: ProcesosContractualesService,
    @Inject(MAT_DIALOG_DATA) public data,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    console.log(this.data);
    /*if(this.data.informeFinalObservacion != null){
      this.observaciones.patchValue(this.data.informeFinalObservacion[0])
    }else{
      this.getInformeFinalInterventoriaObservacionByInformeFinalObservacion(
        this.data.informe.informeFinalInterventoriaObservacionesId
      )
    }
    this.getObservacionesByInformeFinalInterventoriaId(this.data.informe.informeFinalInterventoriaId);*/
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength())
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

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
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
        () => this.routes.navigate( [ '/procesosContractuales' ] )
      );
    });
  }

  onSubmit() {
    this.estaEditando = true;
    this.observacion.value.tipoSolicitudCodigo = this.data.tipoSolicitudCodigo;
    this.observacion.value.solicitudId = this.data.solicitudId;
    this.observacion.value.tieneObservacion = true;
    this.observacion.value.tipoObservacionCodigo = '1'; //defecto devolución, aún no existe tipo dominio, si crean mas, modificar esto
    this.devolver(this.observacion);
  }

  devolver ( elemento: any ) {
    this.procesosContractualesSvc.devolverProcesosContractuales(elemento.value)
      .subscribe(
        response => {
          this.dialog.closeAll();
          this.openDialog( '', `<b>${response.message}</b>` );
        },
        err => this.openDialog( '', `<b>${err.message}</b>` )
      );
  }

}
