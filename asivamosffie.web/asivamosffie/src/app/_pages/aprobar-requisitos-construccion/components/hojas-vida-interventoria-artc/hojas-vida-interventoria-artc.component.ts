import { Component, Input, OnInit } from '@angular/core';
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
  @Input() observacionesPerfil;
  @Input() tieneObservacion;
  @Input() construccionPerfilId: number;
  observacionSupervisor: string = '3';
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

  constructor ( private dialog: MatDialog, 
                private fb: FormBuilder,
                private faseDosAprobarConstruccionSvc: FaseDosAprobarConstruccionService )
  { }

  ngOnInit(): void {
    if ( this.observacionesPerfil !== undefined ) {
      const observacionTipo3 = [];
      for ( let observacion of this.observacionesPerfil ) {             
        if ( observacion.tipoObservacionCodigo === this.observacionSupervisor ) {
          observacionTipo3.push( observacion );
        };
      };
      if ( observacionTipo3.length > 0 ) {
        console.log( observacionTipo3[ observacionTipo3.length -1 ] );
        this.addressForm.get( 'tieneObservaciones' ).setValue( observacionTipo3[ observacionTipo3.length -1 ].esSupervision !== undefined ? observacionTipo3[ observacionTipo3.length -1 ].esSupervision : null )
        if ( this.addressForm.get( 'tieneObservaciones' ).value === true && observacionTipo3[ observacionTipo3.length -1 ].observacion === undefined ) {
          this.addressForm.get( 'construccionPerfilObservacionId' ).setValue( observacionTipo3[ observacionTipo3.length -1 ].construccionPerfilObservacionId );
        };
        if ( this.addressForm.get( 'tieneObservaciones' ).value === true && observacionTipo3[ observacionTipo3.length -1 ].observacion !== undefined ) {
          this.addressForm.get( 'construccionPerfilObservacionId' ).setValue( observacionTipo3[ observacionTipo3.length -1 ].construccionPerfilObservacionId );
          this.addressForm.get( 'observaciones' ).setValue( observacionTipo3[ observacionTipo3.length -1 ].observacion );
        };
      };
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }
  
  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  onSubmit (){
    const observacionPerfil = {
      construccionPerfilId: this.construccionPerfilId,
      observacion: this.addressForm.get( 'observaciones' ).value !== null ? this.addressForm.get( 'observaciones' ).value : null,
      esSupervision: this.addressForm.get( 'tieneObservaciones' ).value !== null ? this.addressForm.get( 'tieneObservaciones' ).value : null,
      tipoObservacionCodigo: this.observacionSupervisor
    };
    if ( this.addressForm.get( 'construccionPerfilObservacionId' ).value !== null ) {
      observacionPerfil[ 'construccionPerfilObservacionId' ] = this.addressForm.get( 'construccionPerfilObservacionId' ).value;
    };

    console.log( observacionPerfil );

    this.faseDosAprobarConstruccionSvc.createEditObservacionConstruccionPerfil( observacionPerfil )
      .subscribe(
        response => {
          console.log( response );
          //this.openDialog( '', `<b>${ response.message }</b>` )
        },
        err => {
          console.log( err );
          //this.openDialog( '', `<b>${ err.error !== undefined || err.error !== null ? err.error.message : err.message }</b>` )
        }
      )
  }

}
