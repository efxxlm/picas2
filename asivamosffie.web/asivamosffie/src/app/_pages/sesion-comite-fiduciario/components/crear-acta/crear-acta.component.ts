import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { ComiteTecnico, SesionComiteTema } from 'src/app/_interfaces/technicalCommitteSession';

@Component({
  selector: 'app-crear-acta',
  templateUrl: './crear-acta.component.html',
  styleUrls: ['./crear-acta.component.scss']
})
export class CrearActaComponent implements OnInit {

  objetoComiteTecnico: ComiteTecnico = {};
  listaMiembros: Usuario[];
  nombresParticipantes: string = '';
  listaTemas: SesionComiteTema[] = [];
  listaProposiciones: SesionComiteTema[] = [];
  temasCompletos: boolean = false;
  proposicionesCompletos: boolean = false;
  solicitudesCompletas: boolean = false;

  constructor(
    public dialog: MatDialog,
    private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
    private activatedRoute: ActivatedRoute,
    private commonService: CommonService,
    private router: Router,

  ) {

  }

  callChildren(elements: NodeListOf<HTMLElement>) {
    elements.forEach(control => {
      control.click();
    })
  }

  ngOnInit(): void {

    this.listaMiembros = [];
    this.objetoComiteTecnico.fechaOrdenDia
    this.nombresParticipantes = '';

    this.activatedRoute.params.subscribe(parametros => {

      this.commonService.listaUsuarios().then((respuesta) => {
        this.listaMiembros = respuesta;

        forkJoin([
          this.fiduciaryCommitteeSessionService.getComiteTecnicoByComiteTecnicoId(parametros.id),
          this.fiduciaryCommitteeSessionService.getSesionParticipantesByIdComite(parametros.id),

        ]).subscribe(response => {
          response[0].sesionParticipante = response[1];
          this.objetoComiteTecnico = response[0];


          this.listaTemas = this.objetoComiteTecnico.sesionComiteTema.filter(t => t.esProposicionesVarios != true)
          this.listaProposiciones = this.objetoComiteTecnico.sesionComiteTema.filter(t => t.esProposicionesVarios == true)

          console.log(response)

          setTimeout(() => {

            this.objetoComiteTecnico.sesionParticipante.forEach(p => {
              let usuario: Usuario = this.listaMiembros.find(m => m.usuarioId == p.usuarioId)

              this.nombresParticipantes = `${this.nombresParticipantes} ${usuario.nombres} ${usuario.apellidos} , `

            });

            let btnSolicitud = document.getElementsByName('btnSolicitud')
            let btnOtros = document.getElementsByName('btnOtros')
            let btnProposiciones = document.getElementsByName('btnProposiciones')

            this.callChildren(btnSolicitud);
            this.callChildren(btnOtros);
            this.callChildren(btnProposiciones);

            this.validarCompletos();

          }, 1000);

        })
      })
    })


  }

  validarCompletos() {
    this.solicitudesCompletas = true;
    this.temasCompletos = true;
    this.proposicionesCompletos = true;

    if (this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnicoFiduciario) {
      this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnicoFiduciario.forEach(cs => {
        if (!cs.registroCompleto)
          this.solicitudesCompletas = false;
      })
    }

    this.listaTemas.forEach(t => {
      if (!t.registroCompleto)
        this.temasCompletos = false;
    })

    this.listaProposiciones.forEach(p => {
      if (!p.registroCompleto)
        this.proposicionesCompletos = false;
    })

  }

  habilitar(e) {

    if (e) {
      this.router.navigate(['/comiteTecnico'])
    } else {
      this.ngOnInit();
    }
  }

}
