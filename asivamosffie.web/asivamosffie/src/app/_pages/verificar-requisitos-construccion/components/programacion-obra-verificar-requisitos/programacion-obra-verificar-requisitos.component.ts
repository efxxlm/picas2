import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-programacion-obra-verificar-requisitos',
  templateUrl: './programacion-obra-verificar-requisitos.component.html',
  styleUrls: ['./programacion-obra-verificar-requisitos.component.scss']
})
export class ProgramacionObraVerificarRequisitosComponent implements OnInit {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required],
    construccionObservacionId: [],
  });

  editorStyle = {
    height: '100px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  @Input() observacionesCompleted;
  @Input() contratoConstruccion: any;
  @Input() contratoConstruccionId: any;

  @Output() createEdit = new EventEmitter();

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private commonService: CommonService,
    private faseUnoConstruccionService: FaseUnoConstruccionService
  ) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.contratoConstruccion)
      this.ngOnInit();
  }

  ngOnInit(): void {

    console.log(this.contratoConstruccion.observacionProgramacionObraApoyo);

    this.addressForm.get('tieneObservaciones').setValue(this.contratoConstruccion.tieneObservacionesProgramacionObraApoyo)
    this.addressForm.get('observaciones').setValue(this.contratoConstruccion.observacionProgramacionObraApoyo ? this.contratoConstruccion.observacionProgramacionObraApoyo.observaciones : null)
    this.addressForm.get('construccionObservacionId').setValue(this.contratoConstruccion.observacionProgramacionObraApoyo ? this.contratoConstruccion.observacionProgramacionObraApoyo.construccionObservacionId : null)


    //this.validarSemaforo();

  }

  validarSemaforo() {

    this.contratoConstruccion.semaforoProgramacion = "sin-diligenciar";

    if (this.addressForm.value.tieneObservaciones === true || this.addressForm.value.tieneObservaciones === false) {
      this.contratoConstruccion.semaforoProgramacion = 'completo';

      if (this.addressForm.value.tieneObservaciones === true && !this.addressForm.value.observaciones)
        this.contratoConstruccion.semaforoProgramacion = 'en-proceso';
    }
  }

  

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p>');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li>');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  };

  descargar() {
    this.commonService.getFileById(this.contratoConstruccion.archivoCargueIdProgramacionObra)
      .subscribe(respuesta => {
        let documento = "ProgramacionObra.xlsx";
        var text = documento,
          blob = new Blob([respuesta], { type: 'application/octet-stream' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/octet-stream', anchor.download, anchor.href].join(':');
        anchor.click();
      });
  }

  guardarProgramacion() {

    let construccion = {
      contratoConstruccionId: this.contratoConstruccionId,
      tieneObservacionesProgramacionObraApoyo: this.addressForm.value.tieneObservaciones,

      construccionObservacion: [
        {
          construccionObservacionId: this.addressForm.value.construccionObservacionId,
          contratoConstruccionId: this.contratoConstruccionId,
          tipoObservacionConstruccion: TiposObservacionConstruccion.ProgramacionObra,
          esSupervision: false,
          esActa: false,
          observaciones: this.addressForm.value.observaciones,

        }
      ]
    }

    console.log(); 

    this.faseUnoConstruccionService.createEditObservacionProgramacionObra(construccion)
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message);
        if (respuesta.code == "200")
          this.createEdit.emit(true);
      })
  }

}
