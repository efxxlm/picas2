import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DisponibilidadPresupuestal } from '../../../../_interfaces/budgetAvailability';

@Component({
  selector: 'app-ver-detalle-ddp-especial',
  templateUrl: './ver-detalle-ddp-especial.component.html',
  styleUrls: ['./ver-detalle-ddp-especial.component.scss']
})
export class VerDetalleDdpEspecialComponent implements OnInit {

  disponibilidadPresupuestal: DisponibilidadPresupuestal;
  tipoSolicitudCodigos = {
    solicitudExpensas: '1',
    solicitudOtrosCostos: '2'
  };
  tipoAportante = {
    aportanteFfie: 6,
    aportanteEt: 9,
    aportanteTercero: 10
  };

  constructor ( private activatedRoute: ActivatedRoute,
                private budgetAvailabilitySvc: BudgetAvailabilityService,
                private dialog: MatDialog )
  {
    this.getDDP( this.activatedRoute.snapshot.params.id );
  };

  ngOnInit(): void {
  };

  getDDP ( disponibilidadId: number ) {
    this.budgetAvailabilitySvc.getDetailInfoAdditionalById( disponibilidadId )
      .subscribe(
        disponibilidad => {
          this.budgetAvailabilitySvc.getNumeroContrato( disponibilidad.numeroContrato )
            .subscribe( 
              response => {
                this.disponibilidadPresupuestal = disponibilidad;
                this.disponibilidadPresupuestal[ 'contratista' ] = response[ 'contratacion' ].contratista;
                if ( this.disponibilidadPresupuestal[ 'aportante' ].tipoAportanteId === this.tipoAportante.aportanteFfie ) {
                  this.disponibilidadPresupuestal[ 'tipoAportante' ] = 'FFIE';
                  this.disponibilidadPresupuestal[ 'nombreAportante' ] = 'FFIE';
                  console.log( this.disponibilidadPresupuestal );
                }
              } 
            )
        },
        err => this.openDialog( '', err.message )
      );
  };

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  };

  innerObservacion ( observacion: string ) {
    const observacionHtml = observacion.replace( '"', '' );
    return observacionHtml;
  };

};