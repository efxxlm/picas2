import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AplazarSesionComponent } from '../aplazar-sesion/aplazar-sesion.component';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { ComiteTecnico, EstadosComite } from 'src/app/_interfaces/technicalCommitteSession';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
@Component({
  selector: 'app-registrar-sesion-comite-tecnico',
  templateUrl: './registrar-sesion-comite-tecnico.component.html',
  styleUrls: ['./registrar-sesion-comite-tecnico.component.scss']
})
export class RegistrarSesionComiteTecnicoComponent implements OnInit {

  objetoComiteTecnico: ComiteTecnico = {};
  cargoRegistro: boolean = false

  estadosComite = EstadosComite
  estadoAcordeon : string = "";
  esRegistroNuevo: boolean;
  esVerDetalle: boolean;

  constructor(
                public dialog: MatDialog,
                private technicalCommitteeSessionService: TechnicalCommitteSessionService,
                private activatedRoute: ActivatedRoute,
                private router: Router,
                private route: ActivatedRoute
             )
  {
    this.route.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
      if ( urlSegment.path === 'registrarSesionDeComiteTecnico' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = true;
          return;
      }
      if ( urlSegment.path === 'verDetalleEditarComiteTecnico' ) {
          this.esVerDetalle = false;
          this.esRegistroNuevo = false;
          return;
      }
      if ( urlSegment.path === 'verDetalleComiteTecnico' ) {
          this.esVerDetalle = true;
          return;
      }
    });
  }

  openDialogAplazarSesion() {
    this.dialog.open(AplazarSesionComponent, {
      width: '42em', data: { comite: this.objetoComiteTecnico }
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  fallida(){
    let comite: ComiteTecnico = {
      comiteTecnicoId: this.objetoComiteTecnico.comiteTecnicoId,
      estadoComiteCodigo: EstadosComite.fallida,
    }

    this.technicalCommitteeSessionService.cambiarEstadoComiteTecnico( comite )
      .subscribe( respuesta => {
        this.openDialog('', '<b>No se cuenta con el Quorum necesario para realizar la sesi??n.</b>');
        this.router.navigate(["/comiteTecnico"]);
      })
  }

  changeSemaforo(e){
    this.estadoAcordeon = e;
    console.log( e )
  }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe( parametros => {
      this.technicalCommitteeSessionService.getComiteTecnicoByComiteTecnicoId( parametros.id )
        .subscribe( response => {
          this.objetoComiteTecnico = response;
          this.cargoRegistro = true

          console.log( response )

          setTimeout(() => {

            let btnOtros = document.getElementById( 'btnOtros' )
            let btnTablaValidaciones = document.getElementById( 'btnTablaValidaciones' )
            let btnProposiciones = document.getElementById( 'btnProposiciones' )


            btnOtros.click();
            btnTablaValidaciones.click();
            btnProposiciones.click();

          }, 1000);

        })
    })


  }

}
