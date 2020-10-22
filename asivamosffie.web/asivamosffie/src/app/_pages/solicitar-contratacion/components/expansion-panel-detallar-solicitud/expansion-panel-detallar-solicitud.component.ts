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

  ) {
    this.getSolicitud();
  }

  ngOnInit(): void {
  }

  getSolicitud () {
    this.route.params.subscribe((params: Params) => {
      this.projectContractingService.getContratacionByContratacionId(params.id)
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

    if ( acordeon === 'consideracionEspecial' ) {
      if (this.contratacion.esObligacionEspecial !== undefined) {
        return this.estadoSemaforos.completo;
      } else {
        return this.estadoSemaforos.sinDiligenciar;
      }
    } else if ( acordeon === 'datosContratista' ) {
      if (this.contratacion['contratista']) {
        return this.estadoSemaforos.completo;
      } else {
        return this.estadoSemaforos.sinDiligenciar;
      }
    } else if ( acordeon === 'caracteristicasTecnicas' ) {
      let cantProyectosCompletos = 0;
      let cantProyectosEnProceso = 0;
      let cantidadProyectosSinDiligenciar = 0;
      if (this.contratacion.contratacionProyecto) {
        for (const contratacionProyecto of this.contratacion.contratacionProyecto) {
          if (contratacionProyecto[ 'registroCompleto' ] === undefined) {
            cantidadProyectosSinDiligenciar++;
          }
          if ( contratacionProyecto[ 'registroCompleto' ] === true ) {
            cantProyectosCompletos++;
          } 
          if ( contratacionProyecto[ 'registroCompleto' ] === false ) {
            cantProyectosEnProceso++;
          }
        }
        if ( cantidadProyectosSinDiligenciar === this.contratacion.contratacionProyecto.length ) {
          return this.estadoSemaforos.sinDiligenciar;
        }
        if ( cantProyectosCompletos === this.contratacion.contratacionProyecto.length ) {
          return this.estadoSemaforos.completo;
        }
        if ( cantProyectosEnProceso < cantProyectosCompletos || cantProyectosEnProceso > cantidadProyectosSinDiligenciar ) {
          return this.estadoSemaforos.enProceso;
        }
      }
    } else if ( acordeon === 'fuentesUso' ) {
      let contratacionProyectoAportanteCompleto = 0;
      let contratacionProyectoAportanteEnProceso = 0;
      let contratacionProyectoAportanteSinDiligenciar = 0;
      if ( this.contratacion.contratacionProyecto !== undefined ) {
        for ( const contratacionProyecto of this.contratacion.contratacionProyecto ) {
          let aportanteCompleto = 0;
          let aportanteEnProceso = 0;
          let aportanteSinDiligenciar = 0;
          for ( const contratacionProyectoAportante of contratacionProyecto.contratacionProyectoAportante ) {
            let completos = 0;
            let enProceso = 0;
            let sinDiligenciar = 0;
            if ( contratacionProyectoAportante.componenteAportante.length === 0 ) {
              aportanteSinDiligenciar++;
            } else {
              for ( const componenteAportante of contratacionProyectoAportante.componenteAportante ) {
                if ( componenteAportante[ 'registroCompleto' ] === undefined ) {
                  sinDiligenciar++;
                }
                if ( componenteAportante[ 'registroCompleto' ] === false ) {
                  enProceso++;
                } 
                if ( componenteAportante[ 'registroCompleto' ] === true ) {
                  completos++;
                }
              };
              if ( completos === contratacionProyectoAportante.componenteAportante.length ) {
                aportanteCompleto++;
              };
              if ( enProceso < completos || enProceso > sinDiligenciar ) {
                aportanteEnProceso++;
              }
              if ( sinDiligenciar === contratacionProyectoAportante.componenteAportante.length ) {
                aportanteSinDiligenciar++;
              }
            }
          }
          if ( aportanteSinDiligenciar === contratacionProyecto.contratacionProyectoAportante.length ) {
            contratacionProyectoAportanteSinDiligenciar++;
          };
          if ( aportanteCompleto === contratacionProyecto.contratacionProyectoAportante.length ) {
            contratacionProyectoAportanteCompleto++;
          };
          if ( aportanteEnProceso < aportanteCompleto || aportanteEnProceso > aportanteSinDiligenciar ) {
            contratacionProyectoAportanteEnProceso++;
          };
        }

        if ( contratacionProyectoAportanteSinDiligenciar === this.contratacion.contratacionProyecto.length ) {
          return this.estadoSemaforos.sinDiligenciar;
        }
        if ( contratacionProyectoAportanteCompleto === this.contratacion.contratacionProyecto.length ) {
          return this.estadoSemaforos.completo;
        }
        if ( contratacionProyectoAportanteEnProceso < contratacionProyectoAportanteCompleto || contratacionProyectoAportanteEnProceso > contratacionProyectoAportanteSinDiligenciar ) {
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
          this.contratacion = null;
          this.getSolicitud();
          if ( this.contratacion !== null ) {
            this.router.navigate(['/solicitarContratacion/solicitud/', this.contratacion.contratacionId]);
          }
        }

      });

  }

}
