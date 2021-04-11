import { Component, OnInit, ViewChild, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SesionComiteSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { ProjectService, Proyecto, ProyectoGrilla } from 'src/app/core/_services/project/project.service';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import {
  Contratacion,
  ContratacionObservacion,
  ContratacionProyecto,
  EstadosProyecto,
  EstadosSolicitud
} from 'src/app/_interfaces/project-contracting';
import { Router } from '@angular/router';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { Observable } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ObservacionComponent } from '../observacion/observacion.component';

@Component({
  selector: 'app-tabla-form-solicitud-multiple',
  templateUrl: './tabla-form-solicitud-multiple.component.html',
  styleUrls: ['./tabla-form-solicitud-multiple.component.scss']
})
export class TablaFormSolicitudMultipleComponent implements OnInit, OnChanges {

  @Input() sesionComiteSolicitud: SesionComiteSolicitud;
  @Input() Estadosolicitud: Observable<string>;
  @Input() EstadosolicitudActa: any;
  @Input() estaEditando: boolean;
  @Input() listaEstadosSolicitud: Dominio[];

  @Output() ActualizarProyectos: EventEmitter<ContratacionProyecto[]> = new EventEmitter();

  cantidadProyecto: Number = 0;
  listaEstados: Dominio[] = [];
  listaEstadosCompleta: Dominio[] = [];
  estadosValidos: string[] = ['3', '5', '7'];
  estadosSolicitud = EstadosSolicitud;
  estadosProyecto = EstadosProyecto;
  proyectos: ContratacionProyecto[] = [];
  contratacion: Contratacion;


  displayedColumns: string[] = [
    'idMen',
    'tipoInterventor',
    'departamento',
    'institucionEducativa',
    'sede',
    'estado',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    private projectService: ProjectService,
    private commonService: CommonService,
    private router: Router,
    private projectContractingService: ProjectContractingService,
    public dialog: MatDialog
  ) { }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes.sesionComiteSolicitud) {
      this.cargarRegistro();

    }
  }

  cargarRegistrosInicial() {

  }

  cambiarEstados(estado: string) {
    this.onChangeEstado();

    if (this.EstadosolicitudActa === undefined || this.EstadosolicitudActa === '1') {
      if (estado !== undefined) {
        console.log('perdio')
        if (this.cantidadProyecto == 1) {
          switch (estado) {
            case EstadosSolicitud.AprobadaPorComiteTecnico:
              this.listaEstados = this.listaEstadosCompleta.filter(
                e => e.codigo == EstadosProyecto.AprobadoComiteTecnico
              );
              this.proyectos.forEach(p => {
                console.log(p.proyecto.estadoProyectoInterventoriaCodigo, p);

                if (p.contratacion.tipoSolicitudCodigo === '1')
                  p.proyecto.estadoProyectoObraCodigo = EstadosProyecto.AprobadoComiteTecnico;
                if (p.contratacion.tipoSolicitudCodigo === '2')
                  p.proyecto.estadoProyectoInterventoriaCodigo = EstadosProyecto.AprobadoComiteTecnico;
              });
              break;
            case this.estadosSolicitud.RechazadaPorComiteTecnico:
              this.listaEstados = this.listaEstadosCompleta.filter(
                e => e.codigo == EstadosProyecto.RechazadoComiteTecnico
              );
              this.proyectos.forEach(p => {
                if (p.contratacion.tipoSolicitudCodigo === '1')
                  p.proyecto.estadoProyectoObraCodigo = EstadosProyecto.RechazadoComiteTecnico;
                if (p.contratacion.tipoSolicitudCodigo === '2')
                  p.proyecto.estadoProyectoInterventoriaCodigo = EstadosProyecto.RechazadoComiteTecnico;
              });
              break;
            case this.estadosSolicitud.DevueltaPorComiteTecnico:
              this.listaEstados = this.listaEstadosCompleta.filter(
                e => e.codigo == EstadosProyecto.DevueltoComiteTecnico
              );
              this.proyectos.forEach(p => {
                if (p.contratacion.tipoSolicitudCodigo === '1')
                  p.proyecto.estadoProyectoObraCodigo = EstadosProyecto.DevueltoComiteTecnico;
                if (p.contratacion.tipoSolicitudCodigo === '2')
                  p.proyecto.estadoProyectoInterveCodigo = EstadosProyecto.DevueltoComiteTecnico;
              });
              break;
          }
        } else if (this.cantidadProyecto > 1) {
          switch (estado) {
            case EstadosSolicitud.AprobadaPorComiteTecnico:
              this.listaEstados = this.listaEstadosCompleta.filter(
                e => e.codigo == EstadosProyecto.AprobadoComiteTecnico
              );
              break;
            default:
              this.listaEstados = this.listaEstadosCompleta.filter(e => ['3', '5', '7'].includes(e.codigo));
          }
        }
        this.proyectos.forEach(p => {
          console.log(this.listaEstados, p);
          let estadoObra = this.listaEstados.find(e => e.codigo == p.proyecto.estadoProyectoObraCodigo);
          let estadoInterventoria = this.listaEstados.find(
            e => e.codigo == p.proyecto.estadoProyectoInterventoriaCodigo
          );
          p.proyecto.estadoProyectoObraCodigo = estadoObra ? estadoObra.codigo : null;
          p.proyecto.estadoProyectoInterventoriaCodigo = estadoInterventoria ? estadoInterventoria.codigo : null;
        });
      }
      else {
        /// esta seccion es cuando no se ha seleccionado el estado de la solictud
        this.listaEstados = [];
        console.log(this.listaEstadosSolicitud, EstadosSolicitud.AprobadaPorComiteTecnico, this.listaEstadosCompleta)
        this.listaEstadosSolicitud.forEach(estadoSolicitud => {
          switch  (estadoSolicitud.codigo){
            case EstadosSolicitud.AprobadaPorComiteTecnico:{
              Array.prototype.push.apply(this.listaEstados, this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.AprobadoComiteTecnico));
              break;  
            }
            case EstadosSolicitud.RechazadaPorComiteTecnico:{
              Array.prototype.push.apply(this.listaEstados, this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.RechazadoComiteTecnico));
              break;  
            }
            case EstadosSolicitud.DevueltaPorComiteTecnico:{
              Array.prototype.push.apply(this.listaEstados, this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.DevueltoComiteTecnico));
              break;  
            }
          }
          
        });
      }
    } else {
      this.listaEstados = this.listaEstadosCompleta;
    }
  }

  ngOnInit(): void {

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
      const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
    //this.cargarRegistro();
  }

  Observaciones(
    contratacionProyectoid: number,
    contratacionid: number,
    contratacionObservacion: ContratacionObservacion[],
    proyectoId: number,
    estadoProyectoObraCodigo: number,
    estadoProyectoInterventoriaCodigo: number
  ) {
    let idsesionComiteSolicitud = this.sesionComiteSolicitud.sesionComiteSolicitudId;
    let idcomiteTecnico = this.sesionComiteSolicitud.comiteTecnicoId;

    const dialogRef = this.dialog.open(ObservacionComponent, {
      width: '60em',
      data: {
        contratacionProyectoid,
        contratacionid,
        idsesionComiteSolicitud,
        idcomiteTecnico,
        contratacionObservacion,
        proyectoId,
        estadoProyectoObraCodigo,
        estadoProyectoInterventoriaCodigo
      }
    });

    dialogRef.afterClosed().subscribe(observaciones => {
      this.cargarRegistro();
    });
  }

  onChangeEstado() {
    this.ActualizarProyectos.emit(this.proyectos);
  }

  cargarRegistro() {
    if (this.sesionComiteSolicitud.contratacion) {
      //let promesa = new Promise(resolve => {
      this.commonService.listaEstadoProyecto()
        .subscribe(estados => {
          this.listaEstadosCompleta = estados;
          this.listaEstados = this.listaEstadosCompleta.filter(e => this.estadosValidos.includes(e.codigo));

          this.Estadosolicitud.subscribe(estado => {
            console.log('estado', estado)
            this.cambiarEstados(estado);
          })

          this.projectContractingService.getContratacionByContratacionIdWithGrillaProyecto(this.sesionComiteSolicitud.contratacion.contratacionId)
            .subscribe(contra => {
              this.contratacion = contra;
              this.cantidadProyecto = contra.contratacionProyecto.length;

              contra.contratacionProyecto.forEach(cp => {

                cp.contratacion = JSON.parse(JSON.stringify(contra));
                cp.contratacion.contratacionProyecto = null;
              })

              this.proyectos = contra.contratacionProyecto;
              this.ActualizarProyectos.emit(this.proyectos);
              this.dataSource = new MatTableDataSource(contra.contratacionProyecto);

              this.cambiarEstados(this.sesionComiteSolicitud.estadoCodigo);
              //            resolve();
              //          })

            })

        })


    }
  }

}
