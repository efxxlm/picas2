import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-revision-acta',
  templateUrl: './revision-acta.component.html',
  styleUrls: ['./revision-acta.component.scss']
})
export class RevisionActaComponent implements OnInit {

  acta;
  form:FormGroup;
  comentarActa: boolean = false;
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

  constructor ( private routes: Router,
                public dialog: MatDialog,
                private fb: FormBuilder ) {
    this.getActa();
    this.crearFormulario();
    console.log( this.acta );
  }

  ngOnInit(): void {
  }

  crearFormulario(){
    this.form = this.fb.group({
      comentarioActa: [ null, Validators.required ]
    })
  };

  getActa () {
    if ( this.routes.getCurrentNavigation().extras.replaceUrl ) {
      this.routes.navigateByUrl( '/compromisosActasComite' );
      return;
    };
    this.acta = this.routes.getCurrentNavigation().extras.state.acta;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit () {
    console.log( this.form );
    if ( this.form.invalid ) {
      this.openDialog('Falta registrar información', '');
      return;
    };

    const value = String( this.form.get( 'comentarioActa' ).value );

    const comentario = value.slice( 0, value.length - 1 );

    console.log( comentario );

    this.openDialog('La información ha sido guardada exitosamente', '');

  }

}
