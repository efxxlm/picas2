import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { FaseUnoVerificarPreconstruccionService } from '../../../../core/_services/faseUnoVerificarPreconstruccion/fase-uno-verificar-preconstruccion.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { FaseUnoPreconstruccionService } from '../../../../core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-expansion-gestionar-interventoria',
  templateUrl: './expansion-gestionar-interventoria.component.html',
  styleUrls: ['./expansion-gestionar-interventoria.component.scss']
})
export class ExpansionGestionarInterventoriaComponent implements OnInit {

  contrato: Contrato;
  estado: FormControl;
  cantidadPerfiles: FormControl;
  fechaPoliza: string;

  estadoProyectoArray = [
    {
      name: 'Estudios y Diseños',
      value: 1
    },
    {
      name: 'Diagnóstico',
      value: 2
    }
  ];

  constructor ( private faseUnoVerificarPreconstruccionSvc: FaseUnoVerificarPreconstruccionService,
                private activatedRoute: ActivatedRoute,
                private dialog: MatDialog,
                private routes: Router,
                private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService ) {
    this.declararEstado();
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
    if (this.routes.getCurrentNavigation().extras.replaceUrl) {
      this.routes.navigateByUrl('/verificarPreconstruccion');
      return;
    };
    if (this.routes.getCurrentNavigation().extras.state)
      this.fechaPoliza = this.routes.getCurrentNavigation().extras.state.fechaPoliza;
  }

  ngOnInit(): void {
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  };

  getContratacionByContratoId ( pContratoId: number ) {
    this.faseUnoVerificarPreconstruccionSvc.getContratacionByContratoId( pContratoId )
    .subscribe( contrato => {
      this.contrato = contrato;
      console.log( this.contrato );
    } );
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  private declararEstado() {
    this.cantidadPerfiles = new FormControl('', Validators.required);
  };

  estadoSemaforo ( index: number, semaforo: string ) {
    this.contrato.contratacion.contratacionProyecto[index].proyecto['estadoSemaforo'] = semaforo;
  }

  getPerfilesContrato ( index: number, evento: any ) {
    console.log( evento );
    this.contrato.contratacion.contratacionProyecto[index].proyecto[ 'tieneEstadoFase1EyD' ] = evento.tieneEstadoFase1EyD;
    this.contrato.contratacion.contratacionProyecto[index].proyecto[ 'tieneEstadoFase1Diagnostico' ] = evento.tieneEstadoFase1Diagnostico;
    this.contrato.contratacion.contratacionProyecto[index].proyecto.contratoPerfil = evento.perfiles;
    console.log( this.contrato.contratacion.contratacionProyecto[index] );

    console.log( this.contrato );
    this.faseUnoPreconstruccionSvc.createEditContratoPerfil( this.contrato )
      .subscribe( 
        response => {
          this.openDialog( '', response['message'] );
        },
        err => {
          this.openDialog( '', err.message );
        }
      );
  }

}
