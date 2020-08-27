import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { forkJoin } from 'rxjs';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute } from '@angular/router';
import { ComiteTecnico, SesionParticipante, SesionInvitado } from 'src/app/_interfaces/technicalCommitteSession';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';

@Component({
  selector: 'app-form-registrar-participantes',
  templateUrl: './form-registrar-participantes.component.html',
  styleUrls: ['./form-registrar-participantes.component.scss']
})
export class FormRegistrarParticipantesComponent implements OnInit {

  objetoComiteTecnico: ComiteTecnico = {};

  addressForm = this.fb.group({
    miembrosParticipantes: [null, Validators.required],
    invitados: this.fb.array([])
  });

  hasUnitNumber = false;

  miembrosArray: SesionParticipante[] = [];

  constructor(
              private fb: FormBuilder,
              private commonService: CommonService,
              private activatedRoute: ActivatedRoute,
              private technicalCommitteSessionService: TechnicalCommitteSessionService
             ) 
  {

  }

  ngOnInit(): void {

    this.agregaInvitado();
    let lista: any[] = [];
    
    this.activatedRoute.params.subscribe( parametros => {
      let id = parametros.id;

      forkJoin([
        this.commonService.getUsuariosByPerfil(1),
        this.commonService.getUsuariosByPerfil(2),
        this.commonService.getUsuariosByPerfil(3),
        this.commonService.getUsuariosByPerfil(4),
        this.commonService.getUsuariosByPerfil(5),
        this.technicalCommitteSessionService.getComiteTecnicoByComiteTecnicoId( id ),

  
      ]).subscribe( response => {
  
        for (let i = 0; i < 5; i++)
        {
          lista = lista.concat(response[i])  
        }

        this.miembrosArray = lista.map( u => {

          u.sesionParticipanteId = 0;
          u.comiteTecnicoId = 0;

          return u
        })

        this.objetoComiteTecnico = response[5];

        let listaSeleccionados = [];
        this.objetoComiteTecnico.sesionParticipante.forEach( p => {
          let participante: any = {}
          participante = this.miembrosArray.find( m => m.usuarioId == p.usuarioId )
          participante.sesionParticipanteId = p.sesionParticipanteId

          listaSeleccionados.push( participante );
        });

        this.addressForm.get('miembrosParticipantes').setValue( listaSeleccionados )


        if (this.objetoComiteTecnico.sesionInvitado.length > 0)
        {
          
          this.invitados.clear();

          this.objetoComiteTecnico.sesionInvitado.forEach( i => {
            let grupoInvitado = this.crearInvitado();

            grupoInvitado.get('nombre').setValue( i.nombre )
            grupoInvitado.get('cargo').setValue( i.cargo )
            grupoInvitado.get('entidad').setValue( i.entidad )
            grupoInvitado.get('sesionInvitadoId').setValue( i.sesionInvitadoId )
            
            this.invitados.push( grupoInvitado );
          })
        }
  
      })

    })

    
  }

  get invitados() {
    return this.addressForm.get('invitados') as FormArray;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregaInvitado() {
    this.invitados.push(this.crearInvitado());
  }

  crearInvitado() {
    return this.fb.group({
      sesionInvitadoId: [],
      nombre: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      cargo: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(50)])
      ],
      entidad: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ]
    });
  }

  onSubmit() {

    if (this.addressForm.valid){

      let comite: ComiteTecnico = {
        comiteTecnicoId: this.objetoComiteTecnico.comiteTecnicoId,
        sesionParticipante: [],
        sesionInvitado: [],

      }

      let miembros = this.addressForm.get('miembrosParticipantes').value;

      if (miembros)
      {
        miembros.forEach( m => {
          let sesionParticipante: SesionParticipante = {
            sesionParticipanteId: m.sesionParticipanteId,
            comiteTecnicoId: comite.comiteTecnicoId,
            usuarioId: m.usuarioId,
            
          }

          comite.sesionParticipante.push( sesionParticipante );
        });
      }

      this.invitados.controls.forEach( control => {
        let sesionInvitado: SesionInvitado = {
          comiteTecnicoId: this.objetoComiteTecnico.comiteTecnicoId,
          sesionInvitadoId: control.get('sesionInvitadoId').value,
          nombre: control.get('nombre').value,
          cargo: control.get('cargo').value,
          entidad: control.get('entidad').value,

        }

        comite.sesionInvitado.push( sesionInvitado );
      })

      console.log( comite )

      this.technicalCommitteSessionService.createSesionInvitadoAndParticipante( comite )
        .subscribe( respuesta => {
          console.log( respuesta )
        })

      console.log( this.addressForm.get('miembrosParticipantes').value );
    }
  }
}
