import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { SesionComiteSolicitud, SesionSolicitudVoto, ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';
import { EstadosSolicitud } from 'src/app/_interfaces/project-contracting';


@Component({
  selector: 'app-votacion-solicitud',
  templateUrl: './votacion-solicitud.component.html',
  styleUrls: ['./votacion-solicitud.component.scss']
})
export class VotacionSolicitudComponent implements OnInit{
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
      sesionSolicitudVotoId: [],
      sesionParticipanteId: [],
      sesionComiteSolicitudId: [],
      aprobacion: [null, Validators.required],
      observaciones: [null, Validators.required]
    });
  }

  constructor(
              private fb: FormBuilder,
              public dialogRef: MatDialogRef<VotacionSolicitudComponent>,
              @Inject(MAT_DIALOG_DATA) public data: { 
                                                      sesionComiteSolicitud: SesionComiteSolicitud, 
                                                      objetoComiteTecnico: ComiteTecnico 
                                                    },
              private technicalCommitteSessionService: TechnicalCommitteSessionService,
              public dialog: MatDialog,
              private router: Router,

             ) 
  {

  }

  ngOnInit(): void {

    

    this.data.sesionComiteSolicitud.sesionSolicitudVoto.forEach( v => {
      let grupoVotacion = this.crearParticipante();
      
      grupoVotacion.get('nombreParticipante').setValue( v.nombreParticipante );
      grupoVotacion.get('aprobacion').setValue( v.esAprobado );
      grupoVotacion.get('observaciones').setValue( v.observacion );

      grupoVotacion.get('sesionSolicitudVotoId').setValue( v.sesionSolicitudVotoId );
      grupoVotacion.get('sesionParticipanteId').setValue( v.sesionParticipanteId );
      grupoVotacion.get('sesionComiteSolicitudId').setValue( v.sesionComiteSolicitudId );

      this.listaVotacion.push( grupoVotacion )
    })

    console.log( this.addressForm.value, this.data.sesionComiteSolicitud )

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

    let sesionComiteSolicitud: SesionComiteSolicitud = {
      sesionComiteSolicitudId: this.data.sesionComiteSolicitud.sesionComiteSolicitudId,
      comiteTecnicoId: this.data.sesionComiteSolicitud.comiteTecnicoId,
      sesionSolicitudVoto: []
    }

    this.listaVotacion.controls.forEach( control => {
      let sesionSolicitudVoto: SesionSolicitudVoto = {
        sesionSolicitudVotoId: control.get('sesionSolicitudVotoId').value,
        sesionComiteSolicitudId: control.get('sesionComiteSolicitudId').value,
        sesionParticipanteId: control.get('sesionParticipanteId').value,
        esAprobado: control.get('aprobacion').value,
        observacion: control.get('observaciones').value,

      }

      sesionComiteSolicitud.sesionSolicitudVoto.push( sesionSolicitudVoto );
    })
    
    sesionComiteSolicitud.estadoCodigo = EstadosSolicitud.AprobadaPorComiteTecnico;
    sesionComiteSolicitud.sesionSolicitudVoto.forEach( sv => {
      if ( sv.esAprobado != true )
      sesionComiteSolicitud.estadoCodigo = EstadosSolicitud.RechazadaPorComiteTecnico;
    })

    console.log( sesionComiteSolicitud );

    this.technicalCommitteSessionService.createEditSesionSolicitudVoto( sesionComiteSolicitud )
    .subscribe( respuesta => {
      this.openDialog('', respuesta.message)
      if ( respuesta.code == "200" ){
        this.dialogRef.close(this.data.objetoComiteTecnico);
        //this.router.navigate(['/comiteTecnico/registrarSesionDeComiteTecnico',this.data.objetoComiteTecnico.comiteTecnicoId,'registrarParticipantes'])
        
        
      }

    })
    
  }
 
}
