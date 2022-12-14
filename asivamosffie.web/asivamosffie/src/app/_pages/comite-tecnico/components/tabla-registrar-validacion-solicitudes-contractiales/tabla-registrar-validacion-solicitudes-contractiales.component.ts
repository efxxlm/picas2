import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { VotacionSolicitudComponent } from '../votacion-solicitud/votacion-solicitud.component';
import { VotacionSolicitudMultipleComponent } from '../votacion-solicitud-multiple/votacion-solicitud-multiple.component';
import {
  ComiteTecnico,
  SesionComiteSolicitud,
  SesionSolicitudVoto,
  TiposSolicitud,
  SesionSolicitudObservacionProyecto,
  SesionParticipante,
  SesionSolicitudObservacionActualizacionCronograma
} from 'src/app/_interfaces/technicalCommitteSession';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ProjectService, Proyecto } from 'src/app/core/_services/project/project.service';
import { forkJoin } from 'rxjs';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { VotacionSolicitudActualizaCronogramaComponent } from '../votacion-solicitud-actualiza_cronograma/votacion-solicitud-actualiza_cronograma.component';

@Component({
  selector: 'app-tabla-registrar-validacion-solicitudes-contractiales',
  templateUrl: './tabla-registrar-validacion-solicitudes-contractiales.component.html',
  styleUrls: ['./tabla-registrar-validacion-solicitudes-contractiales.component.scss']
})
export class TablaRegistrarValidacionSolicitudesContractialesComponent implements OnInit {
  @Input() ObjetoComiteTecnico: ComiteTecnico;
  @Input() esVerDetalle: boolean;
  @Output() validar: EventEmitter<any> = new EventEmitter();

  listaMiembros: Usuario[];
  tiposSolicitud = TiposSolicitud;

  displayedColumns: string[] = ['fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'requiereVotacion', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    public dialog: MatDialog,
    private commonService: CommonService,
    private technicalCommitteSessionService: TechnicalCommitteSessionService,
    private projectService: ProjectService,
    private projectContractingService: ProjectContractingService
  ) {}

  openDialogValidacionSolicitudes(elemento: SesionComiteSolicitud) {
    let promesa = new Promise<void>(resolve => {
      if (elemento.tipoSolicitudCodigo == this.tiposSolicitud.Contratacion) {
        this.projectContractingService
          .getContratacionByContratacionId(elemento.contratacion.contratacionId)
          .subscribe(respuesta => {
            elemento.contratacion = respuesta;
            resolve();
          });
      }

      if (elemento.tipoSolicitudCodigo == this.tiposSolicitud.ActualizacionCronogramaProcesoseleccion) {
        this.technicalCommitteSessionService
          .getProcesoSeleccionMonitoreo(elemento.procesoSeleccionMonitoreo.procesoSeleccionMonitoreoId)
          .subscribe(respuesta => {
            // console.log(respuesta);
            elemento.procesoSeleccionMonitoreo = respuesta;
            resolve();
          });
      }
    });

    let sesionComiteSolicitud: SesionComiteSolicitud = {
      sesionComiteSolicitudId: elemento.sesionComiteSolicitudId,
      tipoSolicitudCodigo: elemento.tipoSolicitudCodigo,
      numeroSolicitud: elemento.numeroSolicitud,
      fechaSolicitud: elemento.fechaSolicitud,
      tipoSolicitud: elemento.tipoSolicitud,

      contratacion: elemento.contratacion,
      procesoSeleccionMonitoreo: elemento.procesoSeleccionMonitoreo,

      sesionSolicitudObservacionProyecto: [],
      sesionSolicitudVoto: [],
      sesionSolicitudObservacionActualizacionCronograma: []
    };

    // console.log(elemento);

    this.ObjetoComiteTecnico.sesionParticipante.forEach(p => {
      let solicitudVoto: SesionSolicitudVoto = elemento.sesionSolicitudVoto.find(
        v =>
          v.sesionComiteSolicitudId == elemento.sesionComiteSolicitudId &&
          v.sesionParticipanteId == p.sesionParticipanteId
      );

      let usuario: Usuario = this.listaMiembros.find(m => m.usuarioId == p.usuarioId);

      if (solicitudVoto) {
        solicitudVoto.nombreParticipante = `${usuario.primerNombre} ${usuario.primerApellido}`;
      } else {
        solicitudVoto = {
          sesionComiteSolicitudId: elemento.sesionComiteSolicitudId,
          sesionParticipanteId: p.sesionParticipanteId,
          sesionSolicitudVotoId: 0,
          nombreParticipante: `${usuario.primerNombre} ${usuario.primerApellido}`,
          esAprobado: null,
          observacion: null
        };
        solicitudVoto.sesionComiteSolicitud = elemento;
        //solicitudVoto.nombreParticipante = `${usuario.primerNombre} ${usuario.primerApellido}`;
      }

      if (elemento.tipoSolicitudCodigo == this.tiposSolicitud.Contratacion) {
        promesa.then(() => {
          // console.log(elemento.contratacion);
          elemento.contratacion.contratacionProyecto.forEach(c => {
            let observacion = p.sesionSolicitudObservacionProyecto //elemento.sesionSolicitudObservacionProyecto
              .find(
                o =>
                  o.contratacionProyectoId == c.contratacionProyectoId &&
                  o.sesionComiteSolicitudId == elemento.sesionComiteSolicitudId
              );

            let sesionSolicitudObservacionProyecto: SesionSolicitudObservacionProyecto = {
              sesionSolicitudObservacionProyectoId: observacion ? observacion.sesionSolicitudObservacionProyectoId : 0,
              sesionComiteSolicitudId: elemento.sesionComiteSolicitudId,
              sesionParticipanteId: p.sesionParticipanteId,
              contratacionProyectoId: c.contratacionProyectoId,
              observacion: observacion ? observacion.observacion : null,
              nombreParticipante: `${usuario.primerNombre} ${usuario.primerApellido}`,

              proyecto: c.proyecto
            };

            sesionComiteSolicitud.sesionSolicitudObservacionProyecto.push(sesionSolicitudObservacionProyecto);
          });
        });
      }

      if (elemento.tipoSolicitudCodigo == this.tiposSolicitud.ActualizacionCronogramaProcesoseleccion) {
        promesa.then(() => {
          //console.log(elemento.procesoSeleccionMonitoreo)
          elemento.procesoSeleccionMonitoreo.procesoSeleccionCronogramaMonitoreo.forEach(c => {
            let observacion = c.sesionSolicitudObservacionActualizacionCronograma.find(
              o =>
                p.sesionParticipanteId == o.sesionParticipanteId &&
                o.sesionComiteSolicitudId == elemento.sesionComiteSolicitudId
            );

            let sesionSolicitudObservacionActualizacionCronograma: SesionSolicitudObservacionActualizacionCronograma = {
              sesionSolicitudObservacionActualizacionCronogramaId: observacion
                ? observacion.sesionSolicitudObservacionActualizacionCronogramaId
                : 0,
              sesionComiteSolicitudId: elemento.sesionComiteSolicitudId,
              sesionParticipanteId: p.sesionParticipanteId,
              procesoSeleccionCronogramaMonitoreoId: c.procesoSeleccionCronogramaMonitoreoId,
              observacion: observacion ? observacion.observacion : null,
              nombreParticipante: `${usuario.primerNombre} ${usuario.primerApellido}`,

              procesoSeleccionCronograma: c.procesoSeleccionCronograma
            };

            sesionComiteSolicitud.sesionSolicitudObservacionActualizacionCronograma.push(
              sesionSolicitudObservacionActualizacionCronograma
            );
            //console.log( sesionComiteSolicitud.sesionSolicitudObservacionActualizacionCronograma )
          });
        });
      }

      sesionComiteSolicitud.sesionSolicitudVoto.push(solicitudVoto);
    });

    // console.log(sesionComiteSolicitud);

    this.abrirPopupVotacion(sesionComiteSolicitud);
  }

  changeRequiere(check: boolean, solicitud: SesionComiteSolicitud) {
    this.ObjetoComiteTecnico.sesionComiteSolicitudComiteTecnico.forEach(sc => {
      if (sc.sesionComiteSolicitudId == solicitud.sesionComiteSolicitudId) {
        solicitud.requiereVotacion = check;
        this.technicalCommitteSessionService.noRequiereVotacionSesionComiteSolicitud(solicitud).subscribe(respuesta => {
          sc.completo = !check;
          this.validar.emit(null);
        });
      }
    });
  }

  abrirPopupVotacion(elemento: SesionComiteSolicitud) {
    // console.log(elemento.tipoSolicitudCodigo);
    if (elemento.tipoSolicitudCodigo == this.tiposSolicitud.Contratacion) {
      const dialog = this.dialog.open(VotacionSolicitudMultipleComponent, {
        width: '70em',
        data: { sesionComiteSolicitud: elemento, objetoComiteTecnico: this.ObjetoComiteTecnico, esVerDetalle: this.esVerDetalle },
        maxHeight: '90em'
      });

      dialog.afterClosed().subscribe(c => {
        if (c && c.comiteTecnicoId) {
          this.technicalCommitteSessionService
            .getComiteTecnicoByComiteTecnicoId(c.comiteTecnicoId)
            .subscribe(response => {
              this.ObjetoComiteTecnico = response;
              this.validarRegistros();
              this.validar.emit(null);
            });
        }
      });
    } else if (elemento.tipoSolicitudCodigo == this.tiposSolicitud.ActualizacionCronogramaProcesoseleccion) {
      const dialog = this.dialog.open(VotacionSolicitudActualizaCronogramaComponent, {
        width: '70em',
        data: { sesionComiteSolicitud: elemento, objetoComiteTecnico: this.ObjetoComiteTecnico, esVerDetalle: this.esVerDetalle },
        maxHeight: '90em'
      });

      dialog.afterClosed().subscribe(c => {
        if (c && c.comiteTecnicoId) {
          this.technicalCommitteSessionService
            .getComiteTecnicoByComiteTecnicoId(c.comiteTecnicoId)
            .subscribe(response => {
              this.ObjetoComiteTecnico = response;
              this.validarRegistros();
              this.validar.emit(null);
            });
        }
      });
    } else {
      const dialog = this.dialog.open(VotacionSolicitudComponent, {
        width: '70em',
        data: { sesionComiteSolicitud: elemento, objetoComiteTecnico: this.ObjetoComiteTecnico,  esVerDetalle: this.esVerDetalle }
      });

      dialog.afterClosed().subscribe(c => {
        if (c && c.comiteTecnicoId) {
          this.technicalCommitteSessionService
            .getComiteTecnicoByComiteTecnicoId(c.comiteTecnicoId)
            .subscribe(response => {
              this.ObjetoComiteTecnico = response;
              this.validarRegistros();
              this.validar.emit(null);
            });
        }
      });
    }
  }

  ngOnInit(): void {
    this.commonService.listaUsuarios().then(respuesta => {
      this.listaMiembros = respuesta;
    });
  }

  paginadorTabla() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por p??gina';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  validarRegistros() {
    if (this.ObjetoComiteTecnico.sesionComiteSolicitudComiteTecnico) {
      this.ObjetoComiteTecnico.sesionComiteSolicitudComiteTecnico.forEach(sc => {

        if (sc.requiereVotacion == true && sc.sesionSolicitudVoto.length == 0) {
          sc.completo = false;
        }
        else sc.completo = true;

        sc.sesionSolicitudVoto.forEach(ss => {
          if (ss.esAprobado != true && ss.esAprobado != false) {
            sc.completo = false;
            return
          }
          else sc.completo = true;
        });
      });
    }
  }

  cargarRegistro() {
    this.validarRegistros();

    this.dataSource = new MatTableDataSource(this.ObjetoComiteTecnico.sesionComiteSolicitudComiteTecnico);
    //console.log( this.ObjetoComiteTecnico.sesionComiteSolicitudComiteTecnico );
    this.paginadorTabla();
  }
}
