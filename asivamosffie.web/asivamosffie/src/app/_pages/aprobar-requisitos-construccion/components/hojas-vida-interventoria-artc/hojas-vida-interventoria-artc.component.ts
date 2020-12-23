import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { FaseDosAprobarConstruccionService } from 'src/app/core/_services/faseDosAprobarConstruccion/fase-dos-aprobar-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-hojas-vida-interventoria-artc',
  templateUrl: './hojas-vida-interventoria-artc.component.html',
  styleUrls: ['./hojas-vida-interventoria-artc.component.scss']
})
export class HojasVidaInterventoriaArtcComponent implements OnInit {

  @Input() observacionesCompleted;
  @Input() observacionDevolucion;
  @Input() observacionesPerfil;
  @Input() tieneObservacion;
  @Input() fechaModificacion;
  @Input() construccionPerfilId: number;
  @Output() seRealizoPeticion = new EventEmitter<boolean>();
  observacionSupervisor = '3';
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
  addressForm = this.fb.group({
    tieneObservaciones: [ null, Validators.required ],
    observaciones: [ null ],
    construccionPerfilObservacionId: [ null ]
  });

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private faseDosAprobarConstruccionSvc: FaseDosAprobarConstruccionService )
  { }

  ngOnInit(): void {
    this.addressForm.get('tieneObservaciones')
      .setValue( this.tieneObservacion !== undefined ? this.tieneObservacion : null );
    this.addressForm.get('observaciones')
      .setValue(  this.observacionesPerfil !== undefined
                  && this.observacionesPerfil.observacion !== undefined ? this.observacionesPerfil.observacion : null );
    this.addressForm.get('construccionPerfilObservacionId')
      .setValue( this.observacionesPerfil !== undefined
        ? this.observacionesPerfil.construccionPerfilObservacionId : null );
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n - 1, e.editor.getLength());
    }
  }

  textoLimpio( evento: any, n: number ) {
    if ( evento !== undefined ) {
      return evento.getLength() > n ? n : evento.getLength();
    } else {
      return 0;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  }

  onSubmit(){

    const ConstruccionPerfil = {
      construccionPerfilId: this.construccionPerfilId,
      tieneObservacionesSupervisor: this.addressForm.get( 'tieneObservaciones' ).value !== null ?
                                    this.addressForm.get( 'tieneObservaciones' ).value : null,
      construccionPerfilObservacion: [
        {
          ConstruccionPerfilObservacionId: this.addressForm.value.construccionPerfilObservacionId,
          construccionPerfilId: this.construccionPerfilId,
          esSupervision: true,
          esActa: false,
          observacion:  this.addressForm.get( 'observaciones' ).value !== null
                        || this.addressForm.get( 'observaciones' ).value !== undefined
                        && this.addressForm.get( 'tieneObservaciones' ).value === true ?
                        this.addressForm.get( 'observaciones' ).value : null
        }
      ]
    };

    console.log( ConstruccionPerfil );
    this.faseDosAprobarConstruccionSvc.createEditObservacionPerfilSupervisor( ConstruccionPerfil )
      .subscribe(
        response => {
          this.openDialog( '', `<b>${ response.message }</b>` );
          this.seRealizoPeticion.emit(true);
        },
        err => {
          this.openDialog( '', `<b>${ err.error !== undefined || err.error !== null ? err.error.message : err.message }</b>` );
          this.seRealizoPeticion.emit(true);
        }
      );
  }

}
