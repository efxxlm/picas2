import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Contrato, ContratoPerfil } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-form-interventoria-verificacion-requisitos',
  templateUrl: './form-interventoria-verificacion-requisitos.component.html',
  styleUrls: ['./form-interventoria-verificacion-requisitos.component.scss']
})
export class FormInterventoriaVerificacionRequisitosComponent implements OnInit {

  contrato: Contrato;
  fechaPoliza: string;

  constructor(
    private faseUnoConstruccionService: FaseUnoConstruccionService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog )
  {
    this.getContrato();
    if (this.router.getCurrentNavigation().extras.state) {
      this.fechaPoliza = this.router.getCurrentNavigation().extras.state.fechaPoliza;
    }
  }

  ngOnInit(): void {
  }

  getContrato() {
    this.faseUnoConstruccionService.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
    .subscribe( response => {
      this.contrato = response;
      console.log( this.contrato );
    } );
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  estadoSemaforo( index: number, semaforo: string ) {
    this.contrato.contratacion.contratacionProyecto[index].proyecto['estadoSemaforo'] = semaforo;
  }

  recargarContrato( value: boolean = false ) {
    if ( value === true ) {
      this.contrato = undefined;
      this.getContrato();
    }
  }

  getPerfilesContrato( index: number, perfilContrato: ContratoPerfil[] ) {

    const construccionPerfil: any = {
      contratoId: this.contrato.contratoId,
      proyectoId: this.contrato.contratacion.contratacionProyecto[index].proyectoId,
      construccionPerfil: perfilContrato
    };
    if ( this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoConstruccion.length > 0 ) {
      const construccionId = this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoConstruccion[0].contratoConstruccionId;
      construccionPerfil.contratoConstruccionId = construccionId;
    }
    this.faseUnoConstruccionService.createEditConstruccionPerfil( construccionPerfil )
      .subscribe(
        response => {
          this.openDialog( '', response.message );
          this.getContrato();
        },
        err => this.openDialog( '', err.message )
      );

  }

}
