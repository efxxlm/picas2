import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FaseUnoConstruccionService } from '../../../../core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ActivatedRoute } from '@angular/router';
import { Contrato, ContratoPerfil } from '../../../../_interfaces/faseUnoPreconstruccion.interface';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-requisitos-tecnicos-construccion',
  templateUrl: './form-requisitos-tecnicos-construccion.component.html',
  styleUrls: ['./form-requisitos-tecnicos-construccion.component.scss']
})
export class FormRequisitosTecnicosConstruccionComponent implements OnInit {

  probBoolean: boolean = false;
  contrato: Contrato;

  constructor ( private dialog: MatDialog,
                private faseUnoConstruccionSvc: FaseUnoConstruccionService,
                private activatedRoute: ActivatedRoute )
  {
    this.faseUnoConstruccionSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
      .subscribe( response => {
        this.contrato = response;
        console.log( this.contrato );
      } );
  }

  ngOnInit(): void {
  };

  openDialog (modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  };

  getDiagnostico ( index: number, diagnostico: any ) {

    const diagnosticoForm = {
      contratoId: this.contrato.contratoId,
      proyectoId: this.contrato.contratacion.contratacionProyecto[index].proyectoId,
      esInformeDiagnostico: diagnostico.esInformeDiagnostico,
      rutaInforme: diagnostico.rutaInforme,
      costoDirecto: diagnostico.costoDirecto,
      administracion: diagnostico.administracion,
      imprevistos: diagnostico.imprevistos,
      utilidad: diagnostico.utilidad,
      valorTotalFaseConstruccion: diagnostico.valorTotalFaseConstruccion,
      requiereModificacionContractual: diagnostico.requiereModificacionContractual,
      numeroSolicitudModificacion: diagnostico.numeroSolicitudModificacion
    };
    this.faseUnoConstruccionSvc.createEditDiagnostico( diagnosticoForm )
      .subscribe( 
        response => this.openDialog( '', response.message ),
        err => this.openDialog( '', err.message )
      );

  };

  getAnticipo ( index: number, anticipo: any ) {
    const anticipoForm = {
      contratoId: this.contrato.contratoId,
      proyectoId: this.contrato.contratacion.contratacionProyecto[index].proyectoId,
      manejoAnticipoRequiere: anticipo.requiereAnticipo,
      manejoAnticipoPlanInversion: anticipo.planInversionAnticipo,
      manejoAnticipoCronogramaAmortizacion: anticipo.cronogramaAmortizacionAprobado,
      manejoAnticipoRutaSoporte: anticipo.urlSoporte
    };
    console.log( anticipoForm );
    this.faseUnoConstruccionSvc.createEditManejoAnticipo( anticipoForm )
      .subscribe(
        response => this.openDialog( '', response.message ),
        err => this.openDialog( '', err.message )
      )
  };

  getPerfilesContrato ( index: number, perfilContrato: ContratoPerfil[] ) {

    const construccionPerfil = {
      contratoId: this.contrato.contratoId,
      proyectoId: this.contrato.contratacion.contratacionProyecto[index].proyectoId,
      construccionPerfil: perfilContrato
    };
    this.faseUnoConstruccionSvc.createEditConstruccionPerfil( construccionPerfil )
      .subscribe( 
        response => this.openDialog( '', response.message ),
        err => this.openDialog( '', err.message )
      );

  };

}
