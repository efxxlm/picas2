import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ComiteTecnico, EstadosActaComite, EstadosComite, SesionComiteTema } from 'src/app/_interfaces/technicalCommitteSession';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { forkJoin } from 'rxjs';
import { SesionComentario } from 'src/app/_interfaces/compromisos-actas-comite.interfaces';

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
  listaComentarios: SesionComentario[] = []

  constructor(
    public dialog: MatDialog,
    private technicalCommitteeSessionService: TechnicalCommitteSessionService,
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

  getStyle(){
    if (EstadosActaComite.EnProcesoAprobacion != this.objetoComiteTecnico.estadoActaCodigo)
    return 'auto'
  else
    return 'none'
  }

  ngOnInit(): void {

    this.listaMiembros = [];
    this.objetoComiteTecnico.fechaOrdenDia
    this.nombresParticipantes = '';

    this.activatedRoute.params.subscribe(parametros => {

      this.commonService.listaUsuarios().then((respuesta) => {
        this.listaMiembros = respuesta;

        forkJoin([
          this.technicalCommitteeSessionService.getComiteTecnicoByComiteTecnicoId(parametros.id),
          this.technicalCommitteeSessionService.getSesionParticipantesByIdComite(parametros.id),
          this.technicalCommitteeSessionService.getCometariosDelActa( parametros.id ),

        ]).subscribe(response => {
          response[0].sesionParticipante = response[1];
          this.objetoComiteTecnico = response[0];
          this.listaComentarios = response[2];


          this.listaTemas = this.objetoComiteTecnico.sesionComiteTema.filter(t => t.esProposicionesVarios != true)
          this.listaProposiciones = this.objetoComiteTecnico.sesionComiteTema.filter(t => t.esProposicionesVarios == true)

          console.log(response)

          setTimeout(() => {

            this.objetoComiteTecnico.sesionParticipante.forEach(p => {
              let usuario: Usuario = this.listaMiembros.find(m => m.usuarioId == p.usuarioId)
              if ( usuario )
                this.nombresParticipantes = `${this.nombresParticipantes} ${usuario.nombres} ${usuario.apellidos} , `

            });

            let btnSolicitud = document.getElementsByName('btnSolicitud')
            let btnOtros = document.getElementsByName('btnOtros')
            let btnProposiciones = document.getElementsByName('btnProposiciones')

            //this.callChildren(btnSolicitud);
            this.callChildren(btnOtros);
            this.callChildren(btnProposiciones);

            this.validarCompletos();

          }, 1000);

        })
      })
    })


  }

  validarCompletos() {
    this.solicitudesCompletas = null;
    this.temasCompletos = null;
    this.proposicionesCompletos = null;

    if (this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnico && this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnico.length > 0 ) {
      this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnico.forEach(cs => {
        if (cs.registroCompletoActa === true)
          this.solicitudesCompletas = true;
      })

      this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnico.forEach(cs => {
        if (cs.registroCompletoActa === false)
          this.solicitudesCompletas = false;
      })
    }else{
      this.solicitudesCompletas = true;
    }


    if (this.listaTemas && this.listaTemas.length>0){
      this.listaTemas.forEach(t => {
        if (t.registroCompletoActa === true)
          this.temasCompletos = true;
       });
  
       this.listaTemas.forEach(t => {
        if (t.registroCompletoActa === false)
          this.temasCompletos = false;
       });
    }else{
      this.temasCompletos = true;
    }
 

    if (this.listaProposiciones && this.listaProposiciones.length > 0){
      this.listaProposiciones.forEach(p => {
        if (p.registroCompletoActa === true)
          this.proposicionesCompletos = true;
      })
  
      this.listaProposiciones.forEach(p => {
        if (p.registroCompletoActa === false)
          this.proposicionesCompletos = false;
      })
    }else{
      this.proposicionesCompletos = true;
    }
    

  }

  habilitar(e) {

    if (e) {
      this.router.navigate(['/comiteTecnico'])
    } else {
      this.ngOnInit();
    }
  }

}
