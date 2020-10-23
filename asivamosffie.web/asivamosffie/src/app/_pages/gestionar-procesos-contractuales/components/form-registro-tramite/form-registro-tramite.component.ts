import { Component, Input, OnDestroy, OnInit } from '@angular/core';
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
export class FormRegistroTramiteComponent implements OnInit, OnDestroy {
  fechaSesionString: string;
  fechaSesion: Date;
  @Input() minDate: Date;
  archivo                : string;
  @Input() dataFormulario: FormGroup;
  @Input() contratacion  : DataSolicitud;
  seRealizoPeticion: boolean = false;
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
                private routes: Router ) 
  {
  }

  ngOnDestroy(): void {
    if ( this.dataFormulario.dirty === true && this.seRealizoPeticion === false ) {
      this.openDialogConfirmar( '', '¿Desea guardar la información registrada?' );
    };
  };

  ngOnInit(): void {
    this.archivo = this.dataFormulario.get( 'minutaName' ).value;
  }

  openDialogConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    confirmarDialog.afterClosed()
      .subscribe( response => {
        if ( response === true ) {
          this.guardar();
        }
      } );
  };

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

    this.contratacion.observaciones = this.dataFormulario.get( 'observaciones' ).value;

    let fecha = Date.parse(this.dataFormulario.get( 'fechaEnvioTramite' ).value);
    this.fechaSesion = new Date(fecha);
    this.fechaSesionString = `${ this.fechaSesion.getFullYear() }/${ this.fechaSesion.getMonth() + 1 }/${ this.fechaSesion.getDate() }` 
    this.contratacion.fechaEnvioDocumentacion =   this.fechaSesionString 
    let documento: any = document.getElementById('file');
    if ( documento !== null ) {
      let pFile = this.dataFormulario.get( 'minutaFile' ).value;
      pFile = pFile.name.split( '.' )
      pFile = pFile[ pFile.length -1 ]
      console.log( pFile );
      if ( pFile === 'docx' || pFile === 'doc' ) {
        this.procesosContractualesSvc.sendTramite( this.contratacion, documento.files[0] )
        .subscribe( 
          response => {
            this.seRealizoPeticion = true;
            this.openDialog( '' , response.message );
            this.routes.navigate( [ '/procesosContractuales' ] );
          },
          err => this.openDialog( '' , err.message )
        ); 
      } else {
        this.openDialog( '', 'El tipo de documento soportado es .doc y .docx' );
        return;
      }
    } else {
      this.procesosContractualesSvc.sendTramite( this.contratacion, undefined )
      .subscribe( 
        response => {
          this.seRealizoPeticion = true;
          this.openDialog( '' , response.message );
          this.routes.navigate( [ '/procesosContractuales' ] );
        },
        err => this.openDialog( '' , err.message )
      );
    }

  };

};