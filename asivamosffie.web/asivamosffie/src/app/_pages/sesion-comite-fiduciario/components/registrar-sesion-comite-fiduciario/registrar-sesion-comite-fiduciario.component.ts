import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AplazarSesionComponent } from '../aplazar-sesion/aplazar-sesion.component';
import { ActivatedRoute } from '@angular/router';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { ComiteTecnico, EstadosComite } from 'src/app/_interfaces/technicalCommitteSession';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
@Component({
  selector: 'app-registrar-sesion-comite-fiduciario',
  templateUrl: './registrar-sesion-comite-fiduciario.component.html',
  styleUrls: ['./registrar-sesion-comite-fiduciario.component.scss']
})
export class RegistrarSesionComiteFiduciarioComponent implements OnInit {

  objetoComiteTecnico: ComiteTecnico = {};

  estadosComite = EstadosComite
  
  estadoAcordeon : string = "";

  constructor(
                public dialog: MatDialog,
                private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
                private activatedRoute: ActivatedRoute,

             ) 
  { 

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

    this.fiduciaryCommitteeSessionService.cambiarEstadoComiteTecnico( comite )
      .subscribe( respuesta => {
        this.openDialog('', '<b>No se cuenta con el Quorum necesario para realizar la sesión.</b>');
        this.ngOnInit();
      })
  }
  
  changeSemaforo(e){
    this.estadoAcordeon = e;
    console.log( e )
  }

  ngOnInit(): void {

    this.activatedRoute.params.subscribe( parametros => {
      this.fiduciaryCommitteeSessionService.getComiteTecnicoByComiteTecnicoId( parametros.id )
        .subscribe( response => {

          console.log( response )

          this.objetoComiteTecnico = response;


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
