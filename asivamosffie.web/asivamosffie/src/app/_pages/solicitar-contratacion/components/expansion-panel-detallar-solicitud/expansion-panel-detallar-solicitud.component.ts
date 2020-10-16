import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ContratacionProyecto, Contratacion } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-expansion-panel-detallar-solicitud',
  templateUrl: './expansion-panel-detallar-solicitud.component.html',
  styleUrls: ['./expansion-panel-detallar-solicitud.component.scss']
})
export class ExpansionPanelDetallarSolicitudComponent implements OnInit {

  contratacion: Contratacion = {};
  estadoSemaforos = {
    sinDiligenciar: 'sin-diligenciar',
    enProceso: 'en-proceso',
    completo: 'completo'
  };

  constructor(
    private route: ActivatedRoute,
    private projectContractingService: ProjectContractingService,
    public dialog: MatDialog,
    private router: Router,

  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.contratacion.contratacionId = params.id;

      this.projectContractingService.getContratacionByContratacionId(this.contratacion.contratacionId)
        .subscribe(response => {
          this.contratacion = response;

          setTimeout(() => {

            const btnTablaProyectos = document.getElementById('btnTablaProyectos');
            const btnTablaContratistas = document.getElementById('btnTablaContratistas');
            const btnTablacaracteristicas = document.getElementById('btnTablacaracteristicas');
            const btnconsideraciones = document.getElementById('btnconsideraciones');
            const btnFuentesUsos = document.getElementById('btnFuentesUsos');

            btnTablaProyectos.click();
            btnTablaContratistas.click();
            btnTablacaracteristicas.click();
            btnconsideraciones.click();
            btnFuentesUsos.click();

          }, 1000);

          console.log(response);
        });

    });
  }

  semaforoAcordeon(acordeon: string) {

    if (acordeon === 'consideracionEspecial') {
      if (this.contratacion.esObligacionEspecial !== null) {
        return this.estadoSemaforos.completo;
      } else {
        return this.estadoSemaforos.enProceso;
      }
    } else if (acordeon === 'datosContratista') {
      if (this.contratacion['contratista']) {
        return this.estadoSemaforos.completo;
      } else {
        return this.estadoSemaforos.enProceso;
      }
    } else if (acordeon === 'caracteristicasTecnicas') {
      let contProyectos = 0;
      if (this.contratacion.contratacionProyecto) {
        for (const contratacionProyecto of this.contratacion.contratacionProyecto) {
          if (contratacionProyecto.tieneMonitoreoWeb !== null) {
            contProyectos += 1;
          }
        }
        if (contProyectos === this.contratacion.contratacionProyecto.length) {
          return this.estadoSemaforos.completo;
        } else {
          return this.estadoSemaforos.enProceso;
        }
      }
    }

  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.contratacion);

    this.projectContractingService.createEditContratacion(this.contratacion)
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message);

        console.log(respuesta);

        if (respuesta.code === '200') {
          this.router.navigate(['/solicitarContratacion/solicitud', this.contratacion.contratacionId]);
        }

      });

  }

}
