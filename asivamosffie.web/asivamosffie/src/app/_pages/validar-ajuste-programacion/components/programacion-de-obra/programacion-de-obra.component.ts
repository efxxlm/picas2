import { Route } from '@angular/compiler/src/core';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ReprogrammingService } from 'src/app/core/_services/reprogramming/reprogramming.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component';

@Component({
  selector: 'app-programacion-de-obra',
  templateUrl: './programacion-de-obra.component.html',
  styleUrls: ['./programacion-de-obra.component.scss']
})
export class ProgramacionDeObraComponent implements OnInit, OnChanges {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required],
    ajustePragramacionObservacionId: [null, Validators.required],
    ajusteProgramacionId: [null, Validators.required],
  });

  estaEditando = false;

  editorStyle = {
    height: '75px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  ajusteProgramacionId: number;
  @Input() ajusteProgramacion: any;
  @Input() esVerDetalle: boolean;
  @Output() estadoSemaforo = new EventEmitter<string>();

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private reprogrammingsvc: ReprogrammingService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private commonSvc: CommonService,
    ) { }

  ngOnChanges(changes: SimpleChanges): void {
    if ( changes.ajusteProgramacion )
      {
        this.addressForm.get('tieneObservaciones').setValue( this.ajusteProgramacion?.tieneObservacionesProgramacionObra )
        this.addressForm.get('observaciones').setValue( this.ajusteProgramacion?.observacionObra ? this.ajusteProgramacion?.observacionObra?.observaciones : '' )
        this.addressForm.get('ajustePragramacionObservacionId').setValue( this.ajusteProgramacion?.observacionObra ? this.ajusteProgramacion?.observacionObra?.ajustePragramacionObservacionId : 0 )
        this.addressForm.get('ajusteProgramacionId').setValue( this.ajusteProgramacionId)
        //semaforo
        if(this.esVerDetalle == true){
          this.estadoSemaforo.emit('');
        }else if((this.ajusteProgramacion.observacionObra != undefined && this.ajusteProgramacion.tieneObservacionesProgramacionObra != null && this.ajusteProgramacion.tieneObservacionesProgramacionObra != undefined) || this.ajusteProgramacion.tieneObservacionesProgramacionObra == false){
          this.estadoSemaforo.emit( 'completo' );
        }else if((this.ajusteProgramacion.tieneObservacionesProgramacionObra == true && (this.ajusteProgramacion.observacionObra?.observaciones == null || this.ajusteProgramacion.observacionObra?.observaciones == ''))){
          this.estadoSemaforo.emit( 'en-proceso' );
        }else{
          this.estadoSemaforo.emit( 'sin-diligenciar' );
        }
      }
  }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe( parametros => {
      this.ajusteProgramacionId = parametros.id;
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
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

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    let ajuste = {
      ajusteProgramacionId: this.ajusteProgramacionId,
      tieneObservacionesProgramacionObra: this.addressForm.value.tieneObservaciones,
      AjustePragramacionObservacion: [
        {
          ajusteProgramacionId: this.ajusteProgramacionId,
          ajustePragramacionObservacionId: this.addressForm.value.ajustePragramacionObservacionId,
          observaciones: this.addressForm.value.observaciones,
          esSupervisor: true,
          esObra: true,
          archivoCargueId: this.ajusteProgramacion.archivoCargueIdProgramacionObra
        }
      ]
    }

    this.reprogrammingsvc.createEditObservacionAjusteProgramacion( ajuste, true )
      .subscribe( respuesta => {
        this.openDialog('', respuesta.message);
        if (respuesta.code === "200")
          this.router.navigate(["/validarAjusteProgramacion"]);
      });


  }

  descargar(element: any) {
    if(element.estadoCodigo == "4" && (element.archivoCargueIdProgramacionObra == null || element.archivoCargueIdProgramacionObra == undefined)){
      this.reprogrammingsvc.getFileReturn(element.ajusteProgramacionId, true).subscribe(respuesta => {
        console.log(respuesta);
        if(respuesta != null)
          this.downloadFile(respuesta.archivoCargueId);
      });
    }else{
      this.downloadFile(element.archivoCargueIdProgramacionObra);
    }
  }

  downloadFile(idFile:  number){
    this.commonSvc.getFileById(idFile)
    .subscribe(respuesta => {
      const documento = 'ProgramacionObra.xlsx';
      const  blob = new Blob([respuesta], { type: 'application/octet-stream' });
      const  anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/octet-stream', anchor.download, anchor.href].join(':');
      anchor.click();
    });
  }

  openObservaciones(dataFile: any) {
    const dialogCargarProgramacion = this.dialog.open(DialogObservacionesComponent, {
      width: '75em',
       data: { ajusteProgramacion: this.ajusteProgramacion, esInterventor: true, esObra: true}
    });
    dialogCargarProgramacion.afterClosed()
      .subscribe(response => {
        if (response) {
          console.log(response);
        };
      })
  }

}
