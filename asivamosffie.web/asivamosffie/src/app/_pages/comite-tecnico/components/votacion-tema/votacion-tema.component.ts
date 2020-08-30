import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { SesionComiteSolicitud, SesionSolicitudVoto, ComiteTecnico, SesionComiteTema, SesionTemaVoto } from 'src/app/_interfaces/technicalCommitteSession';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';


@Component({
  selector: 'app-votacion-tema',
  templateUrl: './votacion-tema.component.html',
  styleUrls: ['./votacion-tema.component.scss']
})
export class VotacionTemaComponent implements OnInit{
  miembros: any[] =  ['Juan Lizcano Garcia', 'Fernando José Aldemar Rojas', 'Gonzalo Díaz Mesa'];

  addressForm = this.fb.array([]);

  get listaVotacion() {
    return this.addressForm as FormArray;
  }

  get aprobacion() {
    return this.addressForm.get('aprobacion') as FormArray;
  }

  get observaciones() {
    return this.addressForm.get('observaciones') as FormArray;
  }

  editorStyle = {
    height: '100px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  crearParticipante() {
    return this.fb.group({
      nombreParticipante: [],
      sesionTemaVotoId: [],
      sesionTemaId: [],
      sesionParticipanteId: [],
      aprobacion: [null, Validators.required],
      observaciones: [null, Validators.required]
    });
  }

  constructor(
              private fb: FormBuilder,
              public dialogRef: MatDialogRef<VotacionTemaComponent>, 
              @Inject(MAT_DIALOG_DATA) public data: { 
                                                      sesionComiteTema: SesionComiteTema, 
                                                      //objetoComiteTecnico: ComiteTecnico 
                                                    },
              private technicalCommitteSessionService: TechnicalCommitteSessionService,
              public dialog: MatDialog,
              private router: Router,

             ) 
  {

  }

  ngOnInit(): void {

    

    this.data.sesionComiteTema.sesionTemaVoto.forEach( v => {
      let grupoVotacion = this.crearParticipante();
      
      grupoVotacion.get('nombreParticipante').setValue( v.nombreParticipante );
      grupoVotacion.get('aprobacion').setValue( v.esAprobado );
      grupoVotacion.get('observaciones').setValue( v.observacion );

      grupoVotacion.get('sesionTemaVotoId').setValue( v.sesionTemaVotoId );
      grupoVotacion.get('sesionTemaId').setValue( v.sesionTemaId );
      grupoVotacion.get('sesionParticipanteId').setValue( v.sesionParticipanteId );

      this.listaVotacion.push( grupoVotacion )
    })

    console.log( this.addressForm.value )

  }

  agregarAprovacion() {
    this.aprobacion.push(this.fb.control(null, Validators.required));
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {

    let sesionComiteTema: SesionComiteTema = {

      sesionTemaId: this.data.sesionComiteTema.sesionTemaId,
      comiteTecnicoId: this.data.sesionComiteTema.comiteTecnicoId,
      sesionTemaVoto: []
    }

    this.listaVotacion.controls.forEach( control => {
      let sesionTemaVoto: SesionTemaVoto = {
        sesionTemaVotoId: control.get('sesionTemaVotoId').value,
        sesionTemaId: control.get('sesionTemaId').value,
        sesionParticipanteId: control.get('sesionParticipanteId').value,
        esAprobado: control.get('aprobacion').value,
        observacion: control.get('observaciones').value,

      }

      sesionComiteTema.sesionTemaVoto.push( sesionTemaVoto );
    })


    this.technicalCommitteSessionService.createEditSesionTemaVoto( sesionComiteTema )
    .subscribe( respuesta => {
      this.openDialog('Comité técnico', respuesta.message)
      if ( respuesta.code == "200" ){
        this.dialogRef.close(this.data.sesionComiteTema);
        //this.router.navigate(['/comiteTecnico/registrarSesionDeComiteTecnico',this.data.objetoComiteTecnico.comiteTecnicoId,'registrarParticipantes'])
        
        
      }

    })
    
  }
 
}
