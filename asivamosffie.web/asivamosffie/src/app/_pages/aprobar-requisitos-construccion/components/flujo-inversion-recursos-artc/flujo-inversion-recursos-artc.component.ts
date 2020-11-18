import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { FaseDosAprobarConstruccionService } from 'src/app/core/_services/faseDosAprobarConstruccion/fase-dos-aprobar-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';


@Component({
  selector: 'app-flujo-inversion-recursos-artc',
  templateUrl: './flujo-inversion-recursos-artc.component.html',
  styleUrls: ['./flujo-inversion-recursos-artc.component.scss']
})
export class FlujoInversionRecursosArtcComponent implements OnInit {

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

  constructor ( private dialog: MatDialog, 
                private fb: FormBuilder,
                private commonSvc: CommonService,
                private faseDosAprobarConstruccionSvc: FaseDosAprobarConstruccionService )
  { };

  ngOnInit(): void {

    if (this.contratoConstruccion) {
      this.addressForm.get('tieneObservaciones').setValue( this.contratoConstruccion.tieneObservacionesFlujoInversionSupervisor !== undefined ? this.contratoConstruccion.tieneObservacionesFlujoInversionSupervisor : null );
      this.addressForm.get('observaciones').setValue( this.contratoConstruccion.observacionFlujoInversionSupervisor !== undefined ? this.contratoConstruccion.observacionFlujoInversionSupervisor.observaciones : null );
      this.addressForm.get('construccionObservacionId').setValue(this.contratoConstruccion.observacionFlujoInversionSupervisor !== undefined ? this.contratoConstruccion.observacionFlujoInversionSupervisor.construccionObservacionId : null);
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
    if ( texto !== undefined ) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  }
  
  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  descargar() {
    this.commonSvc.getFileById(this.contratoConstruccion.archivoCargueIdFlujoInversion)
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
      tieneObservacionesFlujoInversionSupervisor: this.addressForm.value.tieneObservaciones,

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

    console.log( construccion );
    this.faseDosAprobarConstruccionSvc.createEditObservacionFlujoInversionSupervisor( construccion )
      .subscribe(
        response => this.openDialog( '', response.message ),
        err => this.openDialog( '', err.message )
      )

  }

}
