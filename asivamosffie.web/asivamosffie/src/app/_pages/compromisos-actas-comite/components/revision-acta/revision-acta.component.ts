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
    this.form.reset({
      comentarioActa: 'El servicio para comentar y devolver acta se esta enviando la informacion pero el servicio no esta completo en el servidor'
    })
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

  textoLimpioMessage(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
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

    const value = String( this.form.get( 'comentarioActa' ).value );
    const observaciones = {
      comiteTecnicoId: this.acta.comiteTecnicoId,
      observaciones: value,
      fecha: new Date(),
      sesionComentarioId: 0 // no esta llegando este id del servidor
    };

    this.compromisoSvc.postComentariosActa( observaciones )
      .subscribe( ( resp: any ) => {
        this.openDialog( this.textoLimpioMessage( resp.message ), '' );
        this.routes.navigate( ['/compromisosActasComite'] );
      } );

  };
  //Aprobar acta
  aprobarActa ( comiteTecnicoId ) {
    //Al aprobar acta redirige al componente principal
    this.compromisoSvc.aprobarActa( comiteTecnicoId )
      .subscribe( ( resp: any ) => {
        this.openDialog( this.textoLimpioMessage( resp.message ), '' );
        this.routes.navigate( ['/compromisosActasComite'] );
      } )
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
  };

};