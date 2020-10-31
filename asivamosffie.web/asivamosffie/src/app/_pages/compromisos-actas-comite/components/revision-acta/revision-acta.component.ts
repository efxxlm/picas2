import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CompromisosActasComiteService } from '../../../../core/_services/compromisosActasComite/compromisos-actas-comite.service';
import { TechnicalCommitteSessionService } from '../../../../core/_services/technicalCommitteSession/technical-committe-session.service';
import { CommonService } from '../../../../core/_services/common/common.service';
import { forkJoin } from 'rxjs';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';

@Component({
  selector: 'app-revision-acta',
  templateUrl: './revision-acta.component.html',
  styleUrls: ['./revision-acta.component.scss']
})
export class RevisionActaComponent implements OnInit, OnDestroy {

  acta: any;
  form:FormGroup;
  comentarActa: boolean = false;
  comentarios: boolean = false;
  tablaDecisiones: any[] = [];
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
  fechaComentario: Date = new Date();
  miembrosParticipantes: any[] = [];
  temas: any[] = [];
  proposicionesVarios: any[] = [];
  seRealizoPeticion: boolean = false;

  constructor ( private routes: Router,
                public dialog: MatDialog,
                private fb: FormBuilder,
                private activatedRoute: ActivatedRoute,
                private commonSvc: CommonService,
                private technicalCommitteeSessionSvc: TechnicalCommitteSessionService,
                private compromisoSvc: CompromisosActasComiteService,
                private projectContractingSvc: ProjectContractingService,
                private comiteTecnicoSvc: TechnicalCommitteSessionService ) {
    this.getActa( this.activatedRoute.snapshot.params.id );
    this.crearFormulario();
  }
  
  ngOnDestroy(): void {
    if ( this.form.get( 'comentarioActa' ).value !== null && this.seRealizoPeticion === false ) {
      this.openDialogConfirmar( '', '¿Desea guardar la información registrada?' )
    }
  }

  ngOnInit(): void {
  };

  getActa ( comiteTecnicoId: number ) {
    this.compromisoSvc.getActa( comiteTecnicoId )
      .subscribe( ( resp: any ) => {
        this.acta = resp[0];
        console.log( this.acta );

        for ( let temas of this.acta.sesionComiteTema ) {
          if ( !temas.esProposicionesVarios ) {
            this.temas.push( temas );
          } else {
            this.proposicionesVarios.push( temas );
          }
        };

        for ( let participantes of this.acta.sesionParticipante ) {
          this.miembrosParticipantes.push( `${ participantes.usuario.nombres } ${ participantes.usuario.apellidos }` )
        };

        this.technicalCommitteeSessionSvc.getComiteTecnicoByComiteTecnicoId( this.activatedRoute.snapshot.params.id )
        .subscribe( ( response: any ) => { 
          this.acta.sesionComiteSolicitudComiteTecnico = response.sesionComiteSolicitudComiteTecnico;
        } );

      } );
  };

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
  openDialogConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText, siNoBoton:true }
    });

    confirmarDialog.afterClosed()
      .subscribe( response => {
        if ( response === true ) {
          this.onSubmit();
        }
      } );
  };
  //Submit de la data
  onSubmit () {

    if ( this.form.invalid ) {
      this.openDialog('', '<b>Falta registrar información</b>');
      return;
    };

    const value = this.form.get( 'comentarioActa' ).value;
    const observaciones = {
      comiteTecnicoId: this.acta.comiteTecnicoId,
      observaciones: value
    };

    this.compromisoSvc.postComentariosActa( observaciones )
      .subscribe( ( resp: any ) => {
        this.seRealizoPeticion = true;
        this.openDialog( '', `<b>${ resp.message }</b>` );
        this.routes.navigate( ['/compromisosActasComite'] );
      } );

  };
  //Aprobar acta
  aprobarActa ( comiteTecnicoId ) {
    //Al aprobar acta redirige al componente principal
    this.compromisoSvc.aprobarActa( comiteTecnicoId )
      .subscribe( ( resp: any ) => {
        this.seRealizoPeticion = true;
        this.openDialog( '', `<b>${ resp.message }</b>` );
        this.routes.navigate( ['/compromisosActasComite'] );
      } )
  };
  //Descargar acta en formato pdf
  getActaPdf( comiteTecnicoId, numeroComite ) {
    this.comiteTecnicoSvc.getPlantillaActaBySesionComiteSolicitudId( comiteTecnicoId )
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