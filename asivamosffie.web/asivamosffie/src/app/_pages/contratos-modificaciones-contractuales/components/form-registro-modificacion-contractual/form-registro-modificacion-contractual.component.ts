import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registro-modificacion-contractual',
  templateUrl: './form-registro-modificacion-contractual.component.html',
  styleUrls: ['./form-registro-modificacion-contractual.component.scss']
})
export class FormRegistroModificacionContractualComponent implements OnInit {

  @Input() dataFormulario: FormGroup;
  archivo                : string;
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
      this.openDialog( '', '<b>Falta registrar información</b>' );
      return;
    };

    this.openDialog( '', '<b>La información ha sido guardada exitosamente.</b>' );
    this.routes.navigate( [ '/contratosModificacionesContractuales' ] );
  };

}
