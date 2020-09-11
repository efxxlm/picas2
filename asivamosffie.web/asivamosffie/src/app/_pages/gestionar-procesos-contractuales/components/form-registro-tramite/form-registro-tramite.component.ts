import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ProcesosContractualesService } from '../../../../core/_services/procesosContractuales/procesos-contractuales.service';
import { Contratacion } from '../../../../_interfaces/project-contracting';

@Component({
  selector: 'app-form-registro-tramite',
  templateUrl: './form-registro-tramite.component.html',
  styleUrls: ['./form-registro-tramite.component.scss']
})
export class FormRegistroTramiteComponent implements OnInit {

  archivo                : string;
  @Input() dataFormulario: FormGroup;
  @Input() contratacion  : Contratacion;
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
                private procesosContractualesSvc: ProcesosContractualesService ) {
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

    this.contratacion.observaciones = this.dataFormulario.get( 'observaciones' ).value;
    this.contratacion.fechaEnvioDocumentacion = this.dataFormulario.get( 'fechaEnvioTramite' ).value;

    this.procesosContractualesSvc.sendTramite( this.contratacion, this.dataFormulario.get( 'minutaFile' ).value )
      .subscribe( console.log );

    //this.openDialog( 'La información ha sido guardada exitosamente.', '' );
  };

};