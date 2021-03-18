import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { SesionComentario } from 'src/app/_interfaces/compromisos-actas-comite.interfaces';
import { ComiteTecnico, EstadosActaComite, SesionComiteTema } from 'src/app/_interfaces/technicalCommitteSession';

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
    private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
    private activatedRoute: ActivatedRoute,
    private commonService: CommonService,
    private router: Router,
    private technicalCommitteeSessionService: TechnicalCommitteSessionService,

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
          this.fiduciaryCommitteeSessionService.getComiteTecnicoByComiteTecnicoId(parametros.id),
          this.fiduciaryCommitteeSessionService.getSesionParticipantesByIdComite(parametros.id),
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

              this.nombresParticipantes = `${this.nombresParticipantes} ${usuario.primerNombre} ${usuario.primerApellido} , `

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

    //this.solicitudesCompletas = null;
    this.temasCompletos = null;
    this.proposicionesCompletos = null;

    let cantidadCompletos = 0;
    let cantidadVacios = 0;

     if (this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnicoFiduciario && this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnicoFiduciario.length > 0) {
      
      this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnicoFiduciario.forEach(cs => {

        if (cs.registroCompletoActa === undefined){
          cantidadVacios++;
        }

        if (cs.registroCompletoActa === true )
           cantidadCompletos++;
      });

      if ( this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnicoFiduciario.length === cantidadCompletos ){
        this.solicitudesCompletas = true;
      }else 
      if ( this.objetoComiteTecnico.sesionComiteSolicitudComiteTecnicoFiduciario.length === cantidadVacios ){
        this.solicitudesCompletas = null;
      }else{
        this.solicitudesCompletas = false;
      }

      console.log(this.solicitudesCompletas);

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
      this.router.navigate(['/comiteFiduciario'])
    } else {
      this.ngOnInit();
    }
  }

}
