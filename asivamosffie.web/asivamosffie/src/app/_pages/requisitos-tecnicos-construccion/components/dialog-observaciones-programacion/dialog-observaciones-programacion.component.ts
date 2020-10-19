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
    height: '45px'
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
  }
  crearFormulario() {
    this.formObservacion = this.fb.group({
      observaciones: [null]
    });
  };

  textoLimpio(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

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
      width: '40em',
      data : { modalTitle, modalText }
    });
  };

  guardar() {
    if ( this.formObservacion.get( 'observaciones' ).value === null ) {
      this.openDialog( '', 'Falta registrar información.' );
      return;
    };
    this.faseUnoConstruccionSvc.createEditObservacionesCarga( this.data.pArchivoCargueId, this.formObservacion.get( 'observaciones' ).value )
    .subscribe(
      response => console.log( response ),
      err => this.openDialog( '', err.message )
    )
    //this.dialogRef.close();
  };

};