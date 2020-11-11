import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-manejo-anticipo-artc',
  templateUrl: './manejo-anticipo-artc.component.html',
  styleUrls: ['./manejo-anticipo-artc.component.scss']
})
export class ManejoAnticipoArtcComponent implements OnInit, OnChanges {

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
  @Input() contratacion: any;
  @Input() contratoConstruccionId: any;

  @Output() createEdit = new EventEmitter();

  constructor ( private dialog: MatDialog, 
                private fb: FormBuilder )
  { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.contratacion)
      this.ngOnInit();
  }

  ngOnInit(): void {
    if (this.contratacion) {

      this.addressForm.get('tieneObservaciones') //Por integrar
      this.addressForm.get('observaciones') //Por integrar
      this.addressForm.get('construccionObservacionId').setValue(this.contratacion.observacionManejoAnticipo ? this.contratacion.observacionManejoAnticipo.construccionObservacionId : null)

      this.validarSemaforo();
    }
  }

  validarSemaforo() {

    this.contratacion.semaforoManejo = "sin-diligenciar";

    if (this.addressForm.value.tieneObservaciones === true || this.addressForm.value.tieneObservaciones === false) {
      this.contratacion.semaforoManejo = 'completo';

      if (this.addressForm.value.tieneObservaciones === true && !this.addressForm.value.observaciones)
        this.contratacion.semaforoManejo = 'en-proceso';
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  };

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  };

  guardarManejo() {

    let construccion = {
      contratoConstruccionId: this.contratoConstruccionId,
      tieneObservacionesManejoAnticipoApoyo: this.addressForm.value.tieneObservaciones,

      construccionObservacion: [
        {
          construccionObservacionId: this.addressForm.value.construccionObservacionId,
          contratoConstruccionId: this.contratoConstruccionId,
          tipoObservacionConstruccion: TiposObservacionConstruccion.ManejoAnticipo,
          esSupervision: true,
          esActa: false,
          observaciones: this.addressForm.value.observaciones,

        }
      ]
    }

    console.log( construccion );

  }

}
