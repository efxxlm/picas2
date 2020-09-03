import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AplazarSesionComponent } from '../aplazar-sesion/aplazar-sesion.component';
import { Sesion } from 'src/app/_interfaces/technicalCommitteSession';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-registrar-sesion-comite-fiduciario',
  templateUrl: './registrar-sesion-comite-fiduciario.component.html',
  styleUrls: ['./registrar-sesion-comite-fiduciario.component.scss']
})
export class RegistrarSesionComiteFiduciarioComponent implements OnInit {

  idSesion: number;
  objetoSesion = {
    numeroComite: 'CF _00001',
    responsable: 'Dirección administrativa',
    tiempo: '20 minutos',
    temaSolicitud: 'Elección de presidente',
    id: null
  };

  constructor ( public dialog: MatDialog,
                private activatedRoute: ActivatedRoute ) { 
  }

  openDialogAplazarSesion() {
    this.dialog.open(AplazarSesionComponent, {
      width: '42em'
    });
  }

  ngOnInit(): void {

    //getData de la sesion a registrar
    this.idSesion = Number( this.activatedRoute.snapshot.params.id );
    this.objetoSesion.id = this.idSesion;
    /*
    getSesionBySesionId( this.idSesion )
        .subscribe( response => {
          this.objetoSesion = response;

          setTimeout(() => {

            let btnOtros = document.getElementById( 'btnOtros' )
            btnOtros.click();

          }, 1000);

        })
    */
    

  }

}
