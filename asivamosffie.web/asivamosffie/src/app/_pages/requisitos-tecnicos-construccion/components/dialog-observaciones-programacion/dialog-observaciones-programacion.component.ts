import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { FaseUnoConstruccionService } from '../../../../core/_services/faseUnoConstruccion/fase-uno-construccion.service';

@Component({
  selector: 'app-dialog-observaciones-programacion',
  templateUrl: './dialog-observaciones-programacion.component.html',
  styleUrls: ['./dialog-observaciones-programacion.component.scss']
})
export class DialogObservacionesProgramacionComponent implements OnInit {

  formObservacion: FormGroup;
  editorStyle = {
    height: '70px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  constructor(private fb: FormBuilder,
    private dialog: MatDialog,
    private dialogRef: MatDialogRef<DialogObservacionesProgramacionComponent>,
    private faseUnoConstruccionSvc: FaseUnoConstruccionService,
    @Inject(MAT_DIALOG_DATA) public data) {
    this.crearFormulario();
  }

  ngOnInit(): void {
    this.formObservacion.get( 'observaciones' ).setValue( this.data.observaciones !== null ? this.data.observaciones : null )
  }
  crearFormulario() {
    this.formObservacion = this.fb.group({
      observaciones: [null]
    });
  };

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

  textoLimpioMessage(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };
  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  guardar() {
    if ( this.formObservacion.get( 'observaciones' ).value === null ) {
      this.openDialog( '', '<b>Falta registrar información.</b>' );
      return;
    };
    this.faseUnoConstruccionSvc.createEditObservacionesCarga( this.data.pArchivoCargueId, this.formObservacion.get( 'observaciones' ).value )
    .subscribe(
      response => {
        this.openDialog( '', response.message );
        this.dialogRef.close({ realizoObservacion: true });
      },
      err => this.openDialog( '', err.message )
    )
    //this.dialogRef.close();
  };

};
