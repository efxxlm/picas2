import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ActivatedRoute } from '@angular/router';
import { ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';
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

  constructor(
    public dialog: MatDialog,
    private technicalCommitteeSessionService: TechnicalCommitteSessionService,
    private activatedRoute: ActivatedRoute,
    private commonService: CommonService,

  ) {

  }

  ngOnInit(): void {

    this.objetoComiteTecnico.fechaOrdenDia

    this.activatedRoute.params.subscribe(parametros => {

      this.commonService.listaUsuarios().then((respuesta) => {
        this.listaMiembros = respuesta;

        this.technicalCommitteeSessionService.getComiteTecnicoByComiteTecnicoId(parametros.id)
          .subscribe(response => {
            this.objetoComiteTecnico = response;

            console.log(response)

            setTimeout(() => {

              this.objetoComiteTecnico.sesionParticipante.forEach(p => {
                let usuario: Usuario = this.listaMiembros.find(m => m.usuarioId == p.usuarioId)
                
                this.nombresParticipantes = `${ this.nombresParticipantes } ${usuario.nombres} ${usuario.apellidos} , `
          
                });

              // let btnOtros = document.getElementById( 'btnOtros' )
              // let btnTablaValidaciones = document.getElementById( 'btnTablaValidaciones' )
              // let btnProposiciones = document.getElementById( 'btnProposiciones' )


              // btnOtros.click();
              // btnTablaValidaciones.click();
              // btnProposiciones.click();

            }, 1000);

          })
      })
    })


  }

}
