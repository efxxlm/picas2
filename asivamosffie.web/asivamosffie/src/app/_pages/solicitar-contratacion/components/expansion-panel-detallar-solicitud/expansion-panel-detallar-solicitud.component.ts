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
    this.getContratacion();
  }

  ngOnInit(): void {
  }

  getContratacion () {
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
          // if (contratacionProyecto[ 'registroCompleto' ] === undefined) {
          //   cantidadProyectosSinDiligenciar++;
          // }
          // if ( contratacionProyecto[ 'registroCompleto' ] === true ) {
          //   cantProyectosCompletos++;
          // }
          //podria tener algun otro campo lleno, no solo tiene monitoreo, por lo que toca 
          let caracteristicasconalgo=false;

          if (
            contratacionProyecto['tieneMonitoreoWeb'] !== undefined ||
            contratacionProyecto['esReasignacion'] !== undefined ||
            contratacionProyecto['esAvanceobra'] !== undefined ||
            contratacionProyecto['porcentajeAvanceObra'] !== undefined || 
            contratacionProyecto['requiereLicencia'] !== undefined ||
            contratacionProyecto['numeroLicencia'] !== undefined ||
            contratacionProyecto['licenciaVigente'] != undefined || 
            contratacionProyecto['fechaVigencia'] !== undefined
            
          ) 
          {
            caracteristicasconalgo = true;
          }

          let registroCompleto: boolean = true;

          if( 
              contratacionProyecto[ 'tieneMonitoreoWeb' ] === undefined || 
              contratacionProyecto[ 'esReasignacion' ] === undefined ||
              ( contratacionProyecto[ 'esReasignacion' ] === true && contratacionProyecto[ 'esAvanceobra' ] === undefined ) ||
              ( contratacionProyecto[ 'esAvanceobra' ] === true && contratacionProyecto[ 'porcentajeAvanceObra' ] === undefined ) ||
              ( contratacionProyecto[ 'porcentajeAvanceObra' ] !== undefined && contratacionProyecto[ 'requiereLicencia' ] === undefined ) ||
              ( contratacionProyecto[ 'requiereLicencia' ] === true && contratacionProyecto[ 'licenciaVigente' ] === undefined ) ||
              ( contratacionProyecto[ 'licenciaVigente' ] === true && contratacionProyecto[ 'numeroLicencia' ] === undefined ) ||
              ( contratacionProyecto[ 'licenciaVigente' ] === true && contratacionProyecto[ 'fechaVigencia' ] === undefined ) ||

              ( contratacionProyecto[ 'esReasignacion' ] === false && contratacionProyecto[ 'requiereLicencia' ] === undefined ) ||
              ( contratacionProyecto[ 'esAvanceobra' ] === false && contratacionProyecto[ 'requiereLicencia' ] === undefined ) 
            )
            {
              registroCompleto = false;
            }

            console.log( caracteristicasconalgo, registroCompleto )

          if ( registroCompleto === false && caracteristicasconalgo===false) {
            cantidadProyectosSinDiligenciar++;
          }
          if ( registroCompleto === false && caracteristicasconalgo===true ) {
            cantProyectosEnProceso++;
          }
          if ( registroCompleto === true ) {
            cantProyectosCompletos++;
          }
        }

        console.log( cantProyectosCompletos, this.contratacion.contratacionProyecto.length, cantidadProyectosSinDiligenciar )

        let respuesta: string;
        
        if ( cantProyectosEnProceso > 0 || cantProyectosCompletos > 0 ) {
          respuesta = this.estadoSemaforos.enProceso;
        }
        if ( cantidadProyectosSinDiligenciar === this.contratacion.contratacionProyecto.length ) {
          respuesta = this.estadoSemaforos.sinDiligenciar;
        }
        if ( cantProyectosCompletos === this.contratacion.contratacionProyecto.length ) {
          respuesta = this.estadoSemaforos.completo;
        }
        

        return respuesta;
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
              sinDiligenciar++;
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
              if ( enProceso < completos || enProceso > sinDiligenciar ) {
                aportanteEnProceso++;
              }
              if ( sinDiligenciar === contratacionProyectoAportante.componenteAportante.length ) {
                aportanteSinDiligenciar++;
              }
              if ( completos === contratacionProyectoAportante.componenteAportante.length ) {
                aportanteCompleto++;
              };
            } 
          }

          if ( aportanteSinDiligenciar === contratacionProyecto.contratacionProyectoAportante.length ) {
            contratacionProyectoAportanteSinDiligenciar++;
          };
          if ( aportanteEnProceso < aportanteCompleto || aportanteEnProceso > aportanteSinDiligenciar ) {
            contratacionProyectoAportanteEnProceso++;
          };
          if ( aportanteCompleto === contratacionProyecto.contratacionProyectoAportante.length ) {
            contratacionProyectoAportanteCompleto++;
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
        this.openDialog('', `<b>${respuesta.message}</b>`);
        this.contratacion = null;
        console.log(respuesta);
        this.getContratacion();

      });

  }

}
