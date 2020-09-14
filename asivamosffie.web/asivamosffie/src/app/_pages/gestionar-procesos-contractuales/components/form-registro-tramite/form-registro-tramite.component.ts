import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ProcesosContractualesService } from '../../../../core/_services/procesosContractuales/procesos-contractuales.service';
import { DataSolicitud } from '../../../../_interfaces/procesosContractuales.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-form-registro-tramite',
  templateUrl: './form-registro-tramite.component.html',
  styleUrls: ['./form-registro-tramite.component.scss']
})
export class FormRegistroTramiteComponent implements OnInit {

  archivo                : string;
  @Input() dataFormulario: FormGroup;
  @Input() contratacion  : DataSolicitud;
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

  constructor ( private dialog: MatDialog,
                private procesosContractualesSvc: ProcesosContractualesService,
                private routes: Router ) {
  }

  ngOnInit(): void {
  }

  fileName ( event: any ) {
    
    if ( event.target.files.length > 0) {
      const file   = event.target.files[0];
      this.archivo = event.target.files[0].name;
      this.dataFormulario.patchValue({
        minutaFile: file
      });
    };

  };

  textoLimpio (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  maxLength (e: any, n: number) {
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

  guardar () {
    console.log( this.dataFormulario );

    if ( this.dataFormulario.invalid ) {
      this.openDialog( 'Falta registrar información.', '' );
      return;
    };

    if ( this.dataFormulario.get( 'minutaFile' ).value ) {
      let documento = this.dataFormulario.get( 'minutaFile' ).value;
      let formData = new FormData();
      formData.append( 'file', documento, documento.name );
      this.contratacion.pFile = formData.get( 'file' );
    };

    this.contratacion.observaciones = this.dataFormulario.get( 'observaciones' ).value;
    this.contratacion.fechaEnvioDocumentacion = this.dataFormulario.get( 'fechaEnvioTramite' ).value;

    this.procesosContractualesSvc.sendTramite( this.contratacion )
      .subscribe( 
        () => {
        this.openDialog( 'La información ha sido guardada exitosamente.', '' );
        this.routes.navigate( [ '/procesosContractuales' ] );
        },
        () => {
          this.openDialog( 'Ha ocurrido un error.', '' );
        } 
      );

    //
  };

};