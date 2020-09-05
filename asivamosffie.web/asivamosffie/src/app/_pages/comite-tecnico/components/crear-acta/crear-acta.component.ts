import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ActivatedRoute } from '@angular/router';
import { ComiteTecnico, SesionComiteTema } from 'src/app/_interfaces/technicalCommitteSession';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { CommonService } from 'src/app/core/_services/common/common.service';

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

  constructor(
    public dialog: MatDialog,
    private technicalCommitteeSessionService: TechnicalCommitteSessionService,
    private activatedRoute: ActivatedRoute,
    private commonService: CommonService,

  ) {

  }

  callChildren( elements : NodeListOf<HTMLElement> ){
    elements.forEach( control => {
      control.click();
    })
  }

  ngOnInit(): void {

    this.objetoComiteTecnico.fechaOrdenDia

    this.activatedRoute.params.subscribe(parametros => {

      this.commonService.listaUsuarios().then((respuesta) => {
        this.listaMiembros = respuesta;

        this.technicalCommitteeSessionService.getComiteTecnicoByComiteTecnicoId(parametros.id)
          .subscribe(response => {
            this.objetoComiteTecnico = response;

            this.listaTemas = response.sesionComiteTema.filter( t => t.esProposicionesVarios != true )
            this.listaProposiciones = response.sesionComiteTema.filter( t => t.esProposicionesVarios == true )

            console.log(response)

            setTimeout(() => {

              

              this.objetoComiteTecnico.sesionParticipante.forEach(p => {
                let usuario: Usuario = this.listaMiembros.find(m => m.usuarioId == p.usuarioId)
                
                this.nombresParticipantes = `${ this.nombresParticipantes } ${usuario.nombres} ${usuario.apellidos} , `
          
                });

               let btnSolicitud = document.getElementsByName( 'btnSolicitud' )
               let btnOtros = document.getElementsByName( 'btnOtros' )
               let btnProposiciones = document.getElementsByName( 'btnProposiciones' )

               this.callChildren( btnSolicitud );
               this.callChildren( btnOtros );
               this.callChildren( btnProposiciones );

            }, 1000);

          })
      })
    })


  }

}
