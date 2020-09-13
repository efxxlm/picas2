import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CompromisosActasComiteService } from '../../../../core/_services/compromisosActasComite/compromisos-actas-comite.service';

@Component({
  selector: 'app-revision-acta',
  templateUrl: './revision-acta.component.html',
  styleUrls: ['./revision-acta.component.scss']
})
export class RevisionActaComponent implements OnInit {

  acta: any;
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
                private fb: FormBuilder,
                private activatedRoute: ActivatedRoute,
                private compromisoSvc: CompromisosActasComiteService ) {
    this.getActa( this.activatedRoute.snapshot.params.id );
    this.crearFormulario();
  };

  ngOnInit(): void { };

  getActa ( comiteTecnicoId: number ) {
    this.compromisoSvc.getActa( comiteTecnicoId )
      .subscribe( resp => {
        this.acta = resp[0];
        console.log( resp );
      } )
  }

  //Formulario comentario de actas
  crearFormulario(){
    this.form = this.fb.group({
      comentarioActa: [ null, Validators.required ]
    });
  };

  //Limite maximo Quill Editor
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  textoLimpio(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };
  //Modal
  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  };
  //Submit de la data
  onSubmit () {

    if ( this.form.invalid ) {
      this.openDialog('Falta registrar información', '');
      return;
    };

    const value      = String( this.form.get( 'comentarioActa' ).value );
    const comentario = value.slice( 0, value.length - 1 );

    console.log( comentario );

    this.compromisoSvc.postComentariosActa()
      .subscribe( console.log )

    //this.openDialog('La información ha sido guardada exitosamente', '');

  };
  //Aprobar acta
  aprobarActa () {
    //Al aprobar acta redirige al componente principal
    this.routes.navigate( ['/compromisosActasComite'] );
  };
  //Descargar acta en formato pdf
  getActaPdf( comiteTecnicoId, numeroComite ) {
    this.compromisoSvc.getActaPdf( comiteTecnicoId )
    .subscribe( ( resp: any ) => {

      const documento = `Acta Preliminar ${ numeroComite }.pdf`;
      const text = documento,
      blob = new Blob([resp], { type: 'application/pdf' }),
      anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
      
    } )
  }

};