import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { forkJoin } from 'rxjs';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ComiteTecnico, SesionParticipante, SesionInvitado, EstadosComite } from 'src/app/_interfaces/technicalCommitteSession';
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
    private router: Router,

  ) {

  }

  ngOnInit(): void {

    //this.agregaInvitado();
    let lista: any[] = [];

    this.activatedRoute.params.subscribe(parametros => {
      let id = parametros.id;

      this.commonService.listaUsuarios().then((respuesta) => {
        lista = respuesta;

        this.miembrosArray = lista.map(u => {

          u.sesionParticipanteId = 0;
          u.comiteTecnicoId = 0;

          return u
        })

        forkJoin([
          this.technicalCommitteSessionService.getComiteTecnicoByComiteTecnicoId(id),
          this.technicalCommitteSessionService.getSesionParticipantesByIdComite(id),

        ]).subscribe(response => {
          response[0].sesionParticipante = response[1];
          this.objetoComiteTecnico = response[0];

          console.log(this.objetoComiteTecnico)


          setTimeout(() => {

            this.onUpdate();
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
        Validators.required, Validators.minLength(1), Validators.maxLength(100)])
      ],
      cargo: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(50)])
      ],
      entidad: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(100)])
      ]
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  validarSolicitudes() {

    if (this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnico.length == 0) {

        this.estadoSolicitudes = this.estadoFormulario.completo;

      return true;
    }

    let cantidadSolicitudesCompletas = 0;
    let cantidadSolicitudes = 0;

    if (this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnico) {
      this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnico.forEach(sol => {
        sol.completo = true;
        if (sol.requiereVotacion == true) {
          if (sol.sesionSolicitudVoto.length == 0)
            cantidadSolicitudes++;

          sol.sesionSolicitudVoto.forEach(vot => {
            cantidadSolicitudes++;
            if (vot.esAprobado == false || vot.esAprobado == true) {
              cantidadSolicitudesCompletas++;
            } else {
              sol.completo = false;
            }
          })
        } else if (sol.requiereVotacion == false) {
          cantidadSolicitudes++;
          cantidadSolicitudesCompletas++;
        }
        else {
          cantidadSolicitudesCompletas--;
        }
      })

      if (this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnico.length > 0) {
        if (cantidadSolicitudes > 0) {
          this.estadoSolicitudes = this.estadoFormulario.enProceso;
          if (cantidadSolicitudes == cantidadSolicitudesCompletas)
            this.estadoSolicitudes = this.estadoFormulario.completo;
        }
      } else {
        this.estadoSolicitudes = '';
      }
    }

  }

  validarTemas(esProposicion: boolean) {

    if (this.objetoComiteTecnico.sesionComiteTema
      .filter(t => (t.esProposicionesVarios ? t.esProposicionesVarios : false) == esProposicion).length == 0) {

      if (esProposicion)
        this.estadoProposiciones = this.estadoFormulario.completo;
      else
        this.estadoOtrosTemas = this.estadoFormulario.completo;

      return true;
    } 

    let cantidadTemasCompletas = 0;
    let cantidadTemas = 0;
    let sinDiligenciar = true;

    this.objetoComiteTecnico.sesionComiteTema
      .filter(t => (t.esProposicionesVarios ? t.esProposicionesVarios : false) == esProposicion).forEach(tem => {
        tem.completo = true;

        if (tem.requiereVotacion == true) {
          //this.objetoComiteTecnico.sesionParticipante.forEach(par => {
          if (tem.sesionTemaVoto.length == 0)
            cantidadTemas++;

          tem.sesionTemaVoto.forEach(vot => {
            cantidadTemas++;

            if (vot.esAprobado == false || vot.esAprobado == true) {
              cantidadTemasCompletas++;
            } else {
              tem.completo = false;

            }
          })
          sinDiligenciar = false;
          //})
        } else if (tem.requiereVotacion == false) {
          cantidadTemas++;
          cantidadTemasCompletas++;
          sinDiligenciar = false;
        }
        else {
          cantidadTemas++;
        }
      })



    if (cantidadTemas > 0) {
      if (esProposicion)
        this.estadoProposiciones = this.estadoFormulario.enProceso;
      else
        this.estadoOtrosTemas = this.estadoFormulario.enProceso;

        if (sinDiligenciar) // no se ha llenado nada
        if (esProposicion)
          this.estadoProposiciones = this.estadoFormulario.sinDiligenciar;
        else
          this.estadoOtrosTemas = this.estadoFormulario.sinDiligenciar;

      if (cantidadTemas == cantidadTemasCompletas)
        if (esProposicion)
          this.estadoProposiciones = this.estadoFormulario.completo;
        else
          this.estadoOtrosTemas = this.estadoFormulario.completo;
    } else {

    }

    console.log(cantidadTemas, this.estadoOtrosTemas, this.estadoProposiciones)

  }

  onUpdate() {

    this.estaTodo = false;

    this.validarSolicitudes();
    this.validarTemas(true);
    this.validarTemas(false);


    let btnRegistrarSolicitudes = document.getElementById('btnRegistrarSolicitudes');
    let btnOtrosTemas = document.getElementById('btnOtrosTemas');
    let btnProposiciones = document.getElementById('btnProposiciones');


    if (this.objetoComiteTecnico.sesionParticipante && this.objetoComiteTecnico.sesionParticipante.length > 0) {
      btnRegistrarSolicitudes.click();
      btnOtrosTemas.click();
      btnProposiciones.click();
    }

    if (this.estadoSolicitudes == this.estadoFormulario.completo &&
      this.estadoOtrosTemas == this.estadoFormulario.completo &&
      this.estadoProposiciones == this.estadoFormulario.completo
    ) {
      this.estaTodo = true;
    }
  }

  onDelete(i: number) {
    let grupo = this.invitados.controls[i] as FormGroup;
    console.log(grupo, this.invitados, i)
    let idInvitado = grupo.get('sesionInvitadoId').value ? grupo.get('sesionInvitadoId').value : 0;
    this.technicalCommitteSessionService.deleteSesionInvitado(idInvitado)
      .subscribe(respuesta => {
        this.openDialog('', '<b>La información se ha eliminado correctamente.</b>')
        this.borrarArray(this.invitados, i)
      })

  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.onDelete(e)
      }
    });
  }

  CambiarEstado() {
    let comite: ComiteTecnico = {
      comiteTecnicoId: this.objetoComiteTecnico.comiteTecnicoId,
      estadoComiteCodigo: EstadosComite.desarrolladaSinActa,

    }
    this.technicalCommitteSessionService.cambiarEstadoComiteTecnico(comite)
      .subscribe(respuesta => {
        this.openDialog('', '<b>La sesión ha sido registrada exitosamente.</b>');
        if (respuesta.code == "200")
          this.router.navigate(['/comiteTecnico']);
      })
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
          this.openDialog('', `<b>${respuesta.message}</b>`)
          if (respuesta.code == "200")
            this.ngOnInit();
        })

      console.log(this.addressForm.get('miembrosParticipantes').value);
    }
  }
}
