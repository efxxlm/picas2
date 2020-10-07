import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-form-registro-tramite',
  templateUrl: './form-registro-tramite.component.html',
  styleUrls: ['./form-registro-tramite.component.scss']
})
export class FormRegistroTramiteComponent implements OnInit {

  archivo                : string;
  @Input() dataFormulario: FormGroup;
  @Input() estadoCodigo  : string;
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
                private routes: Router ) { }

  ngOnInit(): void {
    console.log( this.estadoCodigo );
  };

  fileName ( event: any ) {
    
    if ( event.target.files.length > 0) {
      const file   = event.target.files[0];
      this.archivo = event.target.files[0].name;
      this.dataFormulario.patchValue({
        documentoFile: file
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

    this.openDialog( 'La información ha sido guardada exitosamente.', '' );
    this.routes.navigate( [ '/contratosModificacionesContractuales' ] );
  };

}
