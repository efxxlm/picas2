import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-observacion-secretario',
  templateUrl: './observacion-secretario.component.html',
  styleUrls: ['./observacion-secretario.component.scss']
})
export class ObservacionSecretarioComponent implements OnInit {

  reporte: FormGroup;
  disabledBtn: boolean = false;
  editorStyle = {
    height: '60px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  constructor ( @Inject(MAT_DIALOG_DATA) public compromisoSeguimiento,
                private fb: FormBuilder,
                private dialog: MatDialog,
                private dialogRef: MatDialogRef<ObservacionSecretarioComponent>,
                private technicalCommitteeSessionService: TechnicalCommitteSessionService )
  {
    this.crearFormulario();
  }

  ngOnInit(): void {
  };

  crearFormulario() {
    this.reporte = this.fb.group({
      reporteEstado: [null, Validators.required]
    });
  };

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  };

  maxLength ( e: any, n: number ) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  textoLimpio ( texto: string ) {
    if ( texto ) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  onSubmit () {

    this.disabledBtn = true;
    if ( this.reporte.invalid ) {
      this.openDialog( '', '<b>Debe registrar la observación para continuar.</b>' );
      this.disabledBtn = false;
      return;
    };
    const pObservacionComentario = {
      sesionSolicitudCompromisoId: this.compromisoSeguimiento.compromisos.sesionSolicitudCompromisoId !== undefined ? this.compromisoSeguimiento.compromisos.sesionSolicitudCompromisoId : null,
      temaCompromisoId: this.compromisoSeguimiento.compromisos.temaCompromisoId !== undefined ? this.compromisoSeguimiento.compromisos.temaCompromisoId : null,
      observacion: this.reporte.value.reporteEstado
    };

    console.log( pObservacionComentario );
    this.technicalCommitteeSessionService.observacionesCompromisos( pObservacionComentario )
      .subscribe(
        response => {
          this.disabledBtn = false;
          this.openDialog( '', `<b>${ response.message }</b>` );
          this.dialogRef.close( true );
        },
        err => {
          this.openDialog( '', `<b>${ err.message }</b>` );
          this.disabledBtn = false;
        }
      )
  };

};