import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AplazarSesionComponent } from '../aplazar-sesion/aplazar-sesion.component';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ActivatedRoute } from '@angular/router';
import { ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';
@Component({
  selector: 'app-registrar-sesion-comite-tecnico',
  templateUrl: './registrar-sesion-comite-tecnico.component.html',
  styleUrls: ['./registrar-sesion-comite-tecnico.component.scss']
})
export class RegistrarSesionComiteTecnicoComponent implements OnInit {

  objetoComiteTecnico: ComiteTecnico = {  }

  constructor(
                public dialog: MatDialog,
                private technicalCommitteeSessionService: TechnicalCommitteSessionService,
                private activatedRoute: ActivatedRoute,

             ) 
  { 

  }

  openDialogAplazarSesion() {
    this.dialog.open(AplazarSesionComponent, {
      width: '42em'
    });
  }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe( parametros => {
      this.technicalCommitteeSessionService.getComiteTecnicoByComiteTecnicoId( parametros.id )
        .subscribe( response => {
          this.objetoComiteTecnico = response;

          setTimeout(() => {

            let btnOtros = document.getElementById( 'btnOtros' )
            btnOtros.click();

          }, 1000);

        })
    })
    

  }

}
