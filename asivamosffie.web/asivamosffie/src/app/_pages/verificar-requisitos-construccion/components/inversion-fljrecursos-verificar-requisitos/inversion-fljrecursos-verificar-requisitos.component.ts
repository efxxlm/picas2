import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-inversion-fljrecursos-verificar-requisitos',
  templateUrl: './inversion-fljrecursos-verificar-requisitos.component.html',
  styleUrls: ['./inversion-fljrecursos-verificar-requisitos.component.scss']
})
export class InversionFljrecursosVerificarRequisitosComponent implements OnInit, OnChanges {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required],
    construccionObservacionId:[],
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
  estaEditando = false;

  constructor(
              private dialog: MatDialog, 
              private fb: FormBuilder,
              private commonService: CommonService,
              private faseUnoConstruccionService: FaseUnoConstruccionService,
              ) 
  { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.contratoConstruccion)
      this.ngOnInit();
  }

  ngOnInit(): void {

    if (this.contratoConstruccion) {

      this.addressForm.get('tieneObservaciones').setValue(this.contratoConstruccion.tieneObservacionesFlujoInversionApoyo)
      this.addressForm.get('observaciones').setValue(this.contratoConstruccion.observacionFlujoInversionApoyo ? this.contratoConstruccion.observacionFlujoInversionApoyo.observaciones : null)
      this.addressForm.get('construccionObservacionId').setValue(this.contratoConstruccion.observacionFlujoInversionApoyo ? this.contratoConstruccion.observacionFlujoInversionApoyo.construccionObservacionId : null)

      //this.validarSemaforo();
    }

  }

  validarSemaforo() {

    this.contratoConstruccion.semaforoFlujo = "sin-diligenciar";

    if (this.addressForm.value.tieneObservaciones === true || this.addressForm.value.tieneObservaciones === false) {
      this.contratoConstruccion.semaforoFlujo = 'completo';

      if (this.addressForm.value.tieneObservaciones === true && !this.addressForm.value.observaciones)
        this.contratoConstruccion.semaforoFlujo = 'en-proceso';
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
  
  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  descargar() {
    this.estaEditando = true;
    this.commonService.getFileById(this.contratoConstruccion.archivoCargueIdFlujoInversion)
      .subscribe(respuesta => {
        let documento = "FlujoInversion.xlsx";
        var text = documento,
          blob = new Blob([respuesta], { type: 'application/octet-stream' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/octet-stream', anchor.download, anchor.href].join(':');
        anchor.click();
      });
  }

  guardarFlujo() {

    let construccion = {
      contratoConstruccionId: this.contratoConstruccionId,
      tieneObservacionesFlujoInversionApoyo: this.addressForm.value.tieneObservaciones,

      construccionObservacion: [
        {
          construccionObservacionId: this.addressForm.value.construccionObservacionId,
          contratoConstruccionId: this.contratoConstruccionId,
          tipoObservacionConstruccion: TiposObservacionConstruccion.FlujoInversion,
          esSupervision: false,
          esActa: false,
          observaciones: this.addressForm.value.observaciones,

        }
      ]
    }

    console.log();

    this.faseUnoConstruccionService.createEditObservacionFlujoInversion(construccion)
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message);
        if (respuesta.code == "200")
          this.createEdit.emit(true);
      })
  }
  
}
