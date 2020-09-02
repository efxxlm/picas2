import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { forkJoin } from 'rxjs';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute } from '@angular/router';
import { ComiteTecnico, SesionParticipante, SesionInvitado } from 'src/app/_interfaces/technicalCommitteSession';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MAT_EXPANSION_PANEL_DEFAULT_OPTIONS } from '@angular/material/expansion';
import { ComiteTecnicoComponent } from '../comite-tecnico/comite-tecnico.component';

@Component({
  selector: 'app-form-registrar-participantes',
  templateUrl: './form-registrar-participantes.component.html',
  styleUrls: ['./form-registrar-participantes.component.scss']
})
export class FormRegistrarParticipantesComponent implements OnInit {

  objetoComiteTecnico: ComiteTecnico = {};
  estaTodo: boolean = false;

  addressForm = this.fb.group({
    miembrosParticipantes: [null, Validators.required],
    invitados: this.fb.array([])
  });

  estadoFormulario = {
    sinDiligenciar: 'info-text sin-diligenciar',
    enProceso: 'info-text en-proceso',
    completo: 'expansion-style--title completo'
  }

  estadoSeccion = {
    sinDiligenciar: '1',
    enProceso: '2',
    completo: '3'
  }

  estadoSolicitudes = this.estadoFormulario.sinDiligenciar;
  estadoOtrosTemas = this.estadoFormulario.sinDiligenciar;
  estadoProposiciones = this.estadoFormulario.sinDiligenciar;


  hasUnitNumber = false;

  miembrosArray: SesionParticipante[] = [];

  constructor(
    private fb: FormBuilder,
    private commonService: CommonService,
    private activatedRoute: ActivatedRoute,
    private technicalCommitteSessionService: TechnicalCommitteSessionService,
    public dialog: MatDialog,

  ) {

  }

  ngOnInit(): void {

    this.agregaInvitado();
    let lista: any[] = [];

    this.activatedRoute.params.subscribe(parametros => {
      let id = parametros.id;

      forkJoin([
        this.commonService.getUsuariosByPerfil(1),
        this.commonService.getUsuariosByPerfil(2),
        this.commonService.getUsuariosByPerfil(3),
        this.commonService.getUsuariosByPerfil(4),
        this.commonService.getUsuariosByPerfil(5),
        this.technicalCommitteSessionService.getComiteTecnicoByComiteTecnicoId(id),


      ]).subscribe(response => {

        for (let i = 0; i < 5; i++) {
          lista = lista.concat(response[i])
        }

        this.miembrosArray = lista.map(u => {

          u.sesionParticipanteId = 0;
          u.comiteTecnicoId = 0;

          return u
        })

        this.objetoComiteTecnico = response[5];

        this.onUpdate()

        setTimeout(() => {

          let btnRegistrarSolicitudes = document.getElementById('btnRegistrarSolicitudes');
          let btnOtrosTemas = document.getElementById('btnOtrosTemas');
          let btnProposiciones = document.getElementById('btnProposiciones');


          btnRegistrarSolicitudes.click();
          btnOtrosTemas.click();
          btnProposiciones.click();

        }, 1000);

        let listaSeleccionados = [];
        this.objetoComiteTecnico.sesionParticipante.forEach(p => {
          let participante: any = {}
          participante = this.miembrosArray.find(m => m.usuarioId == p.usuarioId)
          participante.sesionParticipanteId = p.sesionParticipanteId

          listaSeleccionados.push(participante);
        });

        this.addressForm.get('miembrosParticipantes').setValue(listaSeleccionados)


        if (this.objetoComiteTecnico.sesionInvitado.length > 0) {

          this.invitados.clear();

          this.objetoComiteTecnico.sesionInvitado.forEach(i => {
            let grupoInvitado = this.crearInvitado();

            grupoInvitado.get('nombre').setValue(i.nombre)
            grupoInvitado.get('cargo').setValue(i.cargo)
            grupoInvitado.get('entidad').setValue(i.entidad)
            grupoInvitado.get('sesionInvitadoId').setValue(i.sesionInvitadoId)

            this.invitados.push(grupoInvitado);
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

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onUpdate() {
    let comite: ComiteTecnico;
    this.technicalCommitteSessionService.getComiteTecnicoByComiteTecnicoId(this.objetoComiteTecnico.comiteTecnicoId)
      .subscribe(respuesta => {
        let cantidadSolicitudesCompletas = 0;
        let cantidadSolicitudes = 0;
        comite = respuesta;

        console.log(comite)

        comite.sesionComiteSolicitud.forEach(sol => {
          if (sol.requiereVotacion == true) {
              comite.sesionParticipante.forEach(par => {
                if ( par.sesionSolicitudVoto.length == 0 )
                  cantidadSolicitudes++;

                par.sesionSolicitudVoto.forEach(vot => {
                  console.log(cantidadSolicitudes)
                  cantidadSolicitudes++;
                  if ( vot.esAprobado == false || vot.esAprobado == true ){
                    cantidadSolicitudesCompletas++;   
                  }
              })
            })
          }else if (sol.requiereVotacion == false){
            cantidadSolicitudes++;    
            cantidadSolicitudesCompletas++;
          }
          else{
            cantidadSolicitudesCompletas--;
          }
        })

        if (cantidadSolicitudes > 0){
          this.estadoSolicitudes = this.estadoFormulario.enProceso;
          if (cantidadSolicitudes == cantidadSolicitudesCompletas)
            this.estadoSolicitudes = this.estadoFormulario.completo;
        }
      })
  }

  onDelete(i: number) {
    let grupo = this.invitados.controls[i] as FormGroup;
    console.log(grupo, this.invitados, i)
    this.technicalCommitteSessionService.deleteSesionInvitado(grupo.get('sesionInvitadoId').value)
      .subscribe(respuesta => {
        this.openDialog('', 'La información se ha eliminado correctamente.')
        this.borrarArray(this.invitados, i)
      })

  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.onDelete(e)
      }
    });
  }

  onSubmit() {

    if (this.addressForm.valid) {

      let comite: ComiteTecnico = {
        comiteTecnicoId: this.objetoComiteTecnico.comiteTecnicoId,
        sesionParticipante: [],
        sesionInvitado: [],

      }

      let miembros = this.addressForm.get('miembrosParticipantes').value;

      if (miembros) {
        miembros.forEach(m => {
          let sesionParticipante: SesionParticipante = {
            sesionParticipanteId: m.sesionParticipanteId,
            comiteTecnicoId: comite.comiteTecnicoId,
            usuarioId: m.usuarioId,

          }

          comite.sesionParticipante.push(sesionParticipante);
        });
      }

      this.invitados.controls.forEach(control => {
        let sesionInvitado: SesionInvitado = {
          comiteTecnicoId: this.objetoComiteTecnico.comiteTecnicoId,
          sesionInvitadoId: control.get('sesionInvitadoId').value,
          nombre: control.get('nombre').value,
          cargo: control.get('cargo').value,
          entidad: control.get('entidad').value,

        }

        comite.sesionInvitado.push(sesionInvitado);
      })

      console.log(comite)

      this.technicalCommitteSessionService.createEditSesionInvitadoAndParticipante(comite)
        .subscribe(respuesta => {
          this.openDialog('', respuesta.message)
          if (respuesta.code == "200")
            this.ngOnInit();
        })

      console.log(this.addressForm.get('miembrosParticipantes').value);
    }
  }
}
