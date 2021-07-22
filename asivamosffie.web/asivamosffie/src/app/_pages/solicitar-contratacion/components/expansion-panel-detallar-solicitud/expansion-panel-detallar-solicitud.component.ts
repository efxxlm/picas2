import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ContratacionProyecto, Contratacion, PlazoContratacion } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-expansion-panel-detallar-solicitud',
  templateUrl: './expansion-panel-detallar-solicitud.component.html',
  styleUrls: ['./expansion-panel-detallar-solicitud.component.scss']
})
export class ExpansionPanelDetallarSolicitudComponent implements OnInit {
  contratacion: Contratacion = {} as Contratacion;
  estadoSemaforos = {
    sinDiligenciar: 'sin-diligenciar',
    enProceso: 'en-proceso',
    completo: 'completo'
  };
  public plazoProyecto: number = 0;

  constructor(
    private route: ActivatedRoute,
    private projectContractingService: ProjectContractingService,
    public dialog: MatDialog,
    private router: Router
  ) {
    this.getContratacion();
  }

  ngOnInit(): void {}

  getContratacion() {
    this.route.params.subscribe((params: Params) => {
      this.projectContractingService.getContratacionByContratacionId(params.id).subscribe(response => {
        this.contratacion = response;
        this.setPlazoProyecto();
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

  setPlazoProyecto() {
    if (!this.contratacion.plazoContratacion) {
      this.contratacion.plazoContratacion = { plazoMeses: 0, plazoDias: 0 };
    }

    if ((this.contratacion.tipoSolicitudCodigo == '1')) {
      const plazoDiasObras = this.contratacion.contratacionProyecto.map(
        crtPrj => +crtPrj.proyecto.plazoMesesObra * 30 + Number(crtPrj.proyecto.plazoDiasObra)
      );
      this.plazoProyecto = Math.max(...plazoDiasObras);
    } else if (this.contratacion.tipoSolicitudCodigo == '2') {
      const plazoDiasInterventoria = this.contratacion.contratacionProyecto.map(
        crtPrj => +crtPrj.proyecto.plazoMesesInterventoria * 30 + Number(crtPrj.proyecto.plazoDiasInterventoria)
      );
      this.plazoProyecto = Math.max(...plazoDiasInterventoria);
    }
    console.log( this.plazoProyecto );
  }

  semaforoAcordeon(acordeon: string) {
    if (acordeon === 'consideracionEspecial') {
      if (this.contratacion.esObligacionEspecial !== undefined) {
        if (this.contratacion.esObligacionEspecial === false) {
          return this.estadoSemaforos.completo;
        }

        if (this.contratacion.esObligacionEspecial === true) {
          if (this.contratacion.consideracionDescripcion !== undefined) {
            return this.estadoSemaforos.completo;
          }

          return this.estadoSemaforos.enProceso;
        }
      } else {
        return this.estadoSemaforos.sinDiligenciar;
      }
    } else if (acordeon === 'datosContratista') {
      if (this.contratacion['contratista']) {
        return this.estadoSemaforos.completo;
      } else {
        return this.estadoSemaforos.sinDiligenciar;
      }
    } else if (acordeon === 'caracteristicasTecnicas') {
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
          let caracteristicasconalgo = false;

          if (
            contratacionProyecto['tieneMonitoreoWeb'] !== undefined ||
            contratacionProyecto['esReasignacion'] !== undefined ||
            contratacionProyecto['esAvanceobra'] !== undefined ||
            contratacionProyecto['porcentajeAvanceObra'] !== undefined ||
            contratacionProyecto['requiereLicencia'] !== undefined ||
            contratacionProyecto['numeroLicencia'] !== undefined ||
            contratacionProyecto['licenciaVigente'] != undefined ||
            contratacionProyecto['fechaVigencia'] !== undefined
          ) {
            caracteristicasconalgo = true;
          }

          let registroCompleto: boolean = true;

          if (
            contratacionProyecto['tieneMonitoreoWeb'] === undefined ||
            contratacionProyecto['esReasignacion'] === undefined ||
            (contratacionProyecto['esReasignacion'] === true && contratacionProyecto['esAvanceobra'] === undefined) ||
            (contratacionProyecto['esAvanceobra'] === true &&
              contratacionProyecto['porcentajeAvanceObra'] === undefined) ||
            (contratacionProyecto['porcentajeAvanceObra'] !== undefined &&
              contratacionProyecto['requiereLicencia'] === undefined) ||
            (contratacionProyecto['requiereLicencia'] === true &&
              contratacionProyecto['licenciaVigente'] === undefined) ||
            (contratacionProyecto['licenciaVigente'] === true &&
              contratacionProyecto['numeroLicencia'] === undefined) ||
            (contratacionProyecto['licenciaVigente'] === true && contratacionProyecto['fechaVigencia'] === undefined) ||
            (contratacionProyecto['esReasignacion'] === false &&
              contratacionProyecto['requiereLicencia'] === undefined) ||
            (contratacionProyecto['esAvanceobra'] === false && contratacionProyecto['requiereLicencia'] === undefined)
          ) {
            registroCompleto = false;
          }

          if (registroCompleto === false && caracteristicasconalgo === false) {
            cantidadProyectosSinDiligenciar++;
          }
          if (registroCompleto === false && caracteristicasconalgo === true) {
            cantProyectosEnProceso++;
          }
          if (registroCompleto === true) {
            cantProyectosCompletos++;
          }
        }

        let respuesta: string;

        if (cantProyectosEnProceso > 0 || cantProyectosCompletos > 0) {
          respuesta = this.estadoSemaforos.enProceso;
        }
        if (cantidadProyectosSinDiligenciar === this.contratacion.contratacionProyecto.length) {
          respuesta = this.estadoSemaforos.sinDiligenciar;
        }
        if (cantProyectosCompletos === this.contratacion.contratacionProyecto.length) {
          respuesta = this.estadoSemaforos.completo;
        }

        return respuesta;
      }
    } else if (acordeon === 'fuentesUso') {
      let contratacionProyectoAportanteCompleto = 0;
      let contratacionProyectoAportanteEnProceso = 0;
      let contratacionProyectoAportanteSinDiligenciar = 0;
      let esRegistroValido = undefined;

      if (this.contratacion.contratacionProyecto !== undefined) {
        const proyecto = this.contratacion.contratacionProyecto.find( proyecto => proyecto[ 'registroValido' ] === false )

        if ( proyecto !== undefined ) {
          esRegistroValido = false
        }

        for (const contratacionProyecto of this.contratacion.contratacionProyecto) {
          let aportanteCompleto = 0;
          let aportanteEnProceso = 0;
          let aportanteSinDiligenciar = 0;
          for (const contratacionProyectoAportante of contratacionProyecto.contratacionProyectoAportante) {
            let completos = 0;
            let enProceso = 0;
            let sinDiligenciar = 0;
            if (contratacionProyectoAportante.componenteAportante.length === 0) {
              sinDiligenciar++;
            } else {
              for (const componenteAportante of contratacionProyectoAportante.componenteAportante) {
                if (componenteAportante['registroCompleto'] === undefined) {
                  sinDiligenciar++;
                }
                if (componenteAportante['registroCompleto'] === false) {
                  enProceso++;
                }
                if (componenteAportante['registroCompleto'] === true) {
                  completos++;
                }
              }
              if (enProceso < completos || enProceso > sinDiligenciar) {
                aportanteEnProceso++;
              }
              if (sinDiligenciar === contratacionProyectoAportante.componenteAportante.length) {
                aportanteSinDiligenciar++;
              }
              if (completos === contratacionProyectoAportante.componenteAportante.length) {
                aportanteCompleto++;
              }
            }
          }

          if (aportanteSinDiligenciar === contratacionProyecto.contratacionProyectoAportante.length) {
            contratacionProyectoAportanteSinDiligenciar++;
          }
          if (aportanteEnProceso < aportanteCompleto || aportanteEnProceso > aportanteSinDiligenciar) {
            contratacionProyectoAportanteEnProceso++;
          }
          if (aportanteCompleto === contratacionProyecto.contratacionProyectoAportante.length) {
            contratacionProyectoAportanteCompleto++;
          }
        
        }

        if ( esRegistroValido === false ) {
          return this.estadoSemaforos.enProceso;
        }

        if (contratacionProyectoAportanteSinDiligenciar === this.contratacion.contratacionProyecto.length) {
          return this.estadoSemaforos.sinDiligenciar;
        }
        if (contratacionProyectoAportanteCompleto === this.contratacion.contratacionProyecto.length) {
          return this.estadoSemaforos.completo;
        }
        if (
          contratacionProyectoAportanteEnProceso < contratacionProyectoAportanteCompleto ||
          contratacionProyectoAportanteEnProceso > contratacionProyectoAportanteSinDiligenciar
        ) {
          return this.estadoSemaforos.enProceso;
        }
      }
    } else if (acordeon === 'plazoEjecucion') {
      if ( this.contratacion.plazoContratacion && ( this.contratacion.plazoContratacion.plazoDias > 0 || this.contratacion.plazoContratacion.plazoMeses > 0 ) ) {
        return this.estadoSemaforos.completo;
      } else {
        return this.estadoSemaforos.sinDiligenciar;
      }
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onChangeTerm(termLimit: PlazoContratacion) {
    this.contratacion.plazoContratacion.plazoDias = termLimit.plazoDias;
    this.contratacion.plazoContratacion.plazoMeses = termLimit.plazoMeses;
  }

  onSubmitTerm(termLimit: PlazoContratacion) {
    this.contratacion.plazoContratacion.plazoDias = termLimit.plazoDias;
    this.contratacion.plazoContratacion.plazoMeses = termLimit.plazoMeses;
    this.projectContractingService
      .createEditTermLimit(this.contratacion.contratacionId, this.contratacion.plazoContratacion)
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        this.contratacion = null;
        console.log(respuesta);
        this.getContratacion();
      });
  }

  onSubmit() {
    console.log(this.contratacion);
    let contratista = {
      contratistaId: this.contratacion.contratistaId,
      contratacionId: this.contratacion.contratacionId,
      tipoSolicitudCodigo: this.contratacion.tipoSolicitudCodigo,
      EsObligacionEspecial: this.contratacion.esObligacionEspecial,
      ConsideracionDescripcion: this.contratacion.consideracionDescripcion,
      EstadoSolicitudCodigo: this.contratacion.estadoSolicitudCodigo
    };
    this.projectContractingService.createEditContratacion(contratista).subscribe(respuesta => {
      this.openDialog('', `<b>${respuesta.message}</b>`);
      this.contratacion = null;
      console.log(respuesta);
      this.getContratacion();
    });
  }
}
