import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-programacion-obra-artc',
  templateUrl: './programacion-obra-artc.component.html',
  styleUrls: ['./programacion-obra-artc.component.scss']
})
export class ProgramacionObraArtcComponent implements OnInit, OnChanges {

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
  
  constructor ( private dialog: MatDialog, 
                private fb: FormBuilder,
                private commonSvc: CommonService ) 
  { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.contratoConstruccion)
      this.ngOnInit();
  }

  ngOnInit(): void {

    this.addressForm.get('tieneObservaciones').setValue(this.contratoConstruccion.tieneObservacionesProgramacionObraApoyo)
    this.addressForm.get('observaciones').setValue(this.contratoConstruccion.observacionProgramacionObra ? this.contratoConstruccion.observacionProgramacionObra.observaciones : null)
    this.addressForm.get('construccionObservacionId').setValue(this.contratoConstruccion.observacionProgramacionObra ? this.contratoConstruccion.observacionProgramacionObra.construccionObservacionId : null)


    this.validarSemaforo();

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
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  };

  descargar() {
    this.commonSvc.getFileById(this.contratoConstruccion.archivoCargueIdProgramacionObra)
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

    console.log( construccion );
  }


}
