import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Contrato, TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { ContratacionProyecto } from 'src/app/_interfaces/project-contracting';

@Component({
  selector: 'app-diagnostico-verificar-requisitos',
  templateUrl: './diagnostico-verificar-requisitos.component.html',
  styleUrls: ['./diagnostico-verificar-requisitos.component.scss']
})
export class DiagnosticoVerificarRequisitosComponent implements OnInit, OnChanges {
  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null],
    construccionObservacionId: []
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


  @Input() construccion: any;
  @Input() contratoConstruccionId: any;
  @Input() observacionesCompleted
  
  @Output() createEditDiagnostico = new EventEmitter();

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private faseUnoConstruccionService: FaseUnoConstruccionService,

  ) { }
  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes.contratacion)
      this.ngOnInit();
    console.log("c", this.construccion)
  }

  ngOnInit(): void {
    if (this.construccion) {

      
      this.addressForm.get('tieneObservaciones').setValue(this.construccion.tieneObservacionesDiagnosticoApoyo)
      this.addressForm.get('observaciones').setValue(this.construccion.observacionDiagnosticoApoyo ? this.construccion.observacionDiagnosticoApoyo.observaciones : null)
      //en edición el setvalue 0 genera la creacion de registros
      //this.addressForm.get('construccionObservacionId').setValue(0);
      this.addressForm.get('construccionObservacionId').setValue(this.construccion.observacionDiagnosticoApoyo?this.construccion.observacionDiagnosticoApoyo.construccionObservacionId:null);

      //this.validarSemaforo();
    }
  }

  validarSemaforo() {

    this.construccion.semaforoDiagnostico = "sin-diligenciar";

    if (this.addressForm.value.tieneObservaciones === true || this.addressForm.value.tieneObservaciones === false) {
      this.construccion.semaforoDiagnostico = 'completo';

      if (this.addressForm.value.tieneObservaciones === true && !this.addressForm.value.observaciones)
        this.construccion.semaforoDiagnostico = 'en-proceso';
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

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

  guardarDiagnostico() {

    let construccion = {
      contratoConstruccionId: this.contratoConstruccionId,
      tieneObservacionesDiagnosticoApoyo: this.addressForm.value.tieneObservaciones,

      construccionObservacion: [
        {
          construccionObservacionId: this.addressForm.value.construccionObservacionId,
          contratoConstruccionId: this.contratoConstruccionId,          
          tipoObservacionConstruccion: TiposObservacionConstruccion.Diagnostico,
          esSupervision: false,
          esActa: false,
          observaciones: this.addressForm.value.observaciones
        }
      ]
    }

    console.log();

    this.faseUnoConstruccionService.createEditObservacionDiagnostico(construccion)
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message);
        if (respuesta.code == "200")
          this.createEditDiagnostico.emit(true);
      })
  };

};