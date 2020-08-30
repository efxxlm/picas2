import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SesionComiteSolicitud, ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-votacion-solicitud-multiple',
  templateUrl: './votacion-solicitud-multiple.component.html',
  styleUrls: ['./votacion-solicitud-multiple.component.scss']
})
export class VotacionSolicitudMultipleComponent implements OnInit {
  miembros: any[] =  ['Juan Lizcano Garcia', 'Fernando José Aldemar Rojas', 'Gonzalo Díaz Mesa'];

  get listaVotacion() {
    return this.addressForm as FormArray;
  }

  addressForm = this.fb.array([]);

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
              public dialogRef: MatDialogRef<VotacionSolicitudMultipleComponent>, 
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

  }

  agregarAprovacion() {
    this.aprobacion.push(this.fb.control(null, Validators.required));
  }

  onSubmit() {
    alert('Thanks!');
  }

}
