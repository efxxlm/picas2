import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { VotacionSolicitudComponent } from '../votacion-solicitud/votacion-solicitud.component';
import { VotacionSolicitudMultipleComponent } from '../votacion-solicitud-multiple/votacion-solicitud-multiple.component';
import { ComiteTecnico, SesionComiteSolicitud, SesionSolicitudVoto, TiposSolicitud, SesionSolicitudObservacionProyecto } from 'src/app/_interfaces/technicalCommitteSession';
import { Usuario } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { ProjectService, Proyecto } from 'src/app/core/_services/project/project.service';

@Component({
  selector: 'app-tabla-registrar-validacion-solicitudes-contractiales',
  templateUrl: './tabla-registrar-validacion-solicitudes-contractiales.component.html',
  styleUrls: ['./tabla-registrar-validacion-solicitudes-contractiales.component.scss']
})
export class TablaRegistrarValidacionSolicitudesContractialesComponent implements OnInit {

  @Input() ObjetoComiteTecnico: ComiteTecnico;
  @Output() validar: EventEmitter<any> = new EventEmitter();

  listaMiembros: Usuario[];
  tiposSolicitud = TiposSolicitud
  

  displayedColumns: string[] = ['fecha', 'numero', 'tipo', 'votacion', 'id'];
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

  ) {

  }

  openDialogValidacionSolicitudes(elemento: SesionComiteSolicitud) {

    elemento.sesionSolicitudVoto = [];
    elemento.sesionSolicitudObservacionProyecto = [];

    console.log(this.ObjetoComiteTecnico);

    this.ObjetoComiteTecnico.sesionParticipante.forEach(p => {
      let votacion: SesionSolicitudVoto = p.sesionSolicitudVoto.find(v => v.sesionComiteSolicitudId == elemento.sesionComiteSolicitudId);
      let usuario: Usuario = this.listaMiembros.find(m => m.usuarioId == p.usuarioId)

      let solicitudVoto: SesionSolicitudVoto = {
        sesionComiteSolicitudId: elemento.sesionComiteSolicitudId,
        sesionParticipanteId: p.sesionParticipanteId,
        sesionSolicitudVotoId: votacion ? votacion.sesionSolicitudVotoId : 0,
        nombreParticipante: `${usuario.nombres} ${usuario.apellidos}`,
        esAprobado: votacion ? votacion.esAprobado : false,
        observacion: votacion ? votacion.observacion : null,

        sesionComiteSolicitud: elemento,

      }

      if (elemento.contratacion && elemento.contratacion.contratacionProyecto) {

        elemento.contratacion.contratacionProyecto.forEach(c => {

          let observacion = p.sesionSolicitudObservacionProyecto //elemento.sesionSolicitudObservacionProyecto
            .find(o => o.contratacionProyectoId == c.contratacionProyectoId
              && o.sesionComiteSolicitudId == elemento.sesionComiteSolicitudId)

          let sesionSolicitudObservacionProyecto: SesionSolicitudObservacionProyecto = {
            sesionSolicitudObservacionProyectoId: observacion ? observacion.sesionSolicitudObservacionProyectoId : 0,
            sesionComiteSolicitudId: elemento.sesionComiteSolicitudId,
            sesionParticipanteId: p.sesionParticipanteId,
            contratacionProyectoId: c.contratacionProyectoId,
            observacion: observacion ? observacion.observacion : null,
            nombreParticipante: `${usuario.nombres} ${usuario.apellidos}`,

            proyecto: c.proyecto,
          }

          elemento.sesionSolicitudObservacionProyecto.push(sesionSolicitudObservacionProyecto)
        })
      }


      elemento.sesionSolicitudVoto.push(solicitudVoto)
    })


    //console.log(elemento)

    this.abrirPopupVotacion(elemento);
  }

  abrirPopupVotacion(elemento: SesionComiteSolicitud) {

    if (elemento.tipoSolicitudCodigo == this.tiposSolicitud.Contratacion) {

      const dialog = this.dialog.open(VotacionSolicitudMultipleComponent, {
        width: '70em',
        data: { sesionComiteSolicitud: elemento, objetoComiteTecnico: this.ObjetoComiteTecnico },
        maxHeight: '90em',

      });

      

      dialog.afterClosed().subscribe(c => {
        if (c && c.comiteTecnicoId) {
          this.technicalCommitteSessionService.getComiteTecnicoByComiteTecnicoId(c.comiteTecnicoId)
            .subscribe(response => {
              this.ObjetoComiteTecnico = response;
              this.validar.emit(null);        
            })
        }
      })

    } else {

      const dialog = this.dialog.open(VotacionSolicitudComponent, {
        width: '70em', data: { sesionComiteSolicitud: elemento, objetoComiteTecnico: this.ObjetoComiteTecnico }
      });

      

      dialog.afterClosed().subscribe(c => {
        if (c && c.comiteTecnicoId) {
          this.technicalCommitteSessionService.getComiteTecnicoByComiteTecnicoId(c.comiteTecnicoId)
            .subscribe(response => {
              this.ObjetoComiteTecnico = response;
              this.validar.emit(null);        
            })
        }
      })
    }


  }


  ngOnInit(): void {

    this.commonService.listaUsuarios().then((respuesta) => {
      this.listaMiembros = respuesta;
    })



    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }

  cargarRegistro() {
    console.log(this.ObjetoComiteTecnico.sesionComiteSolicitud)
    this.dataSource = new MatTableDataSource(this.ObjetoComiteTecnico.sesionComiteSolicitud);
  }

}
