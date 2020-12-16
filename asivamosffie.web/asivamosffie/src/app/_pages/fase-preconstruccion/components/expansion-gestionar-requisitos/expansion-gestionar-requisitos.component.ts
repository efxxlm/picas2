import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FaseUnoPreconstruccionService } from '../../../../core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { ContratoModificado, Contrato, ContratoPerfil } from '../../../../_interfaces/faseUnoPreconstruccion.interface';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-expansion-gestionar-requisitos',
  templateUrl: './expansion-gestionar-requisitos.component.html',
  styleUrls: ['./expansion-gestionar-requisitos.component.scss']
})
export class ExpansionGestionarRequisitosComponent implements OnInit {

  contrato: Contrato;
  fechaPoliza: string;
  estadosPreconstruccion = {
    sinAprobacionReqTecnicos: '1',
    enProcesoAprobacionReqTecnicos: '2',
    enviadoAlInterventor: '10'
  };
  estado: string;

  constructor ( private activatedRoute: ActivatedRoute,
                private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService,
                private dialog: MatDialog,
                private routes: Router ) {
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
    if ( this.routes.getCurrentNavigation().extras.replaceUrl ) {
      this.routes.navigateByUrl('/preconstruccion');
      return;
    };
    if ( this.routes.getCurrentNavigation().extras.state ) {
      this.fechaPoliza = this.routes.getCurrentNavigation().extras.state.fechaPoliza;
      this.estado = this.routes.getCurrentNavigation().extras.state.estado;
    };
  };

  ngOnInit(): void {
  };

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  };

  getContratacionByContratoId ( pContratoId: string ) {
    this.faseUnoPreconstruccionSvc.getContratacionByContratoId( pContratoId )
      .subscribe( contrato => {
        this.contrato = contrato;
        console.log( this.contrato );
      } );
  };

  estadoSemaforo ( index: number, semaforo: string ) {
    this.contrato.contratacion.contratacionProyecto[index].proyecto['estadoSemaforo'] = semaforo;
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  };

  getPerfilesContrato ( index: number, perfilContrato: ContratoPerfil[] ) {
    this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoPerfil = perfilContrato;

    console.log( this.contrato );
    this.faseUnoPreconstruccionSvc.createEditContratoPerfil( this.contrato )
      .subscribe( 
        response => {
          this.openDialog( '', response['message'] );
          this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
        },
        err => {
          this.openDialog( '', err.message );
        }
      );
  };

};