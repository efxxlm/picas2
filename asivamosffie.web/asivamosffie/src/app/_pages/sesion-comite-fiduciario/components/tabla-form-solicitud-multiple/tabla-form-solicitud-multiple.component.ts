import { Component, OnInit, ViewChild, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { Contratacion, ContratacionObservacion, ContratacionProyecto, EstadosProyecto, EstadosSolicitud } from 'src/app/_interfaces/project-contracting';
import { SesionComiteSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { ObservacionComponent } from 'src/app/_pages/comite-tecnico/components/observacion/observacion.component';


@Component({
  selector: 'app-tabla-form-solicitud-multiple',
  templateUrl: './tabla-form-solicitud-multiple.component.html',
  styleUrls: ['./tabla-form-solicitud-multiple.component.scss']
})
export class TablaFormSolicitudMultipleComponent implements OnInit, OnChanges {

  @Input() sesionComiteSolicitud: SesionComiteSolicitud;
  @Input() Estadosolicitud: Observable<string>;
  @Input() EstadosolicitudActa: any;
  @Output() ActualizarProyectos: EventEmitter<ContratacionProyecto[]> = new EventEmitter();
  @Input() estaEditando: boolean;
  @Input() listaEstadosSolicitud: Dominio[];

  cantidadProyecto: Number = 0;
  listaEstados: Dominio[] = [];
  listaEstadosCompleta: Dominio[] = [];
  estadosValidos: string[] = ['4', '6', '8']
  estadosSolicitud = EstadosSolicitud;
  estadosProyecto = EstadosProyecto;
  proyectos: ContratacionProyecto[] = []
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
    public dialog: MatDialog,

  ) {

  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes.sesionComiteSolicitud) {
      this.cargarRegistro();
      console.log('input', this.Estadosolicitud, changes)
    }


  }

  cambiarEstados(estado: string) {

    this.onChangeEstado();
    console.log( this.EstadosolicitudActa, estado )
    if (this.EstadosolicitudActa === undefined || this.EstadosolicitudActa === "1" || this.EstadosolicitudActa === "0")  {
      // se valida que no venga el estado o que no se ha cambiado en fiduciario
      if (estado && estado !== '1') {
        if (this.cantidadProyecto == 1) {
          switch (estado) {
            case EstadosSolicitud.AprobadaPorComiteFiduciario:
              this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.AprobadoComiteFiduciario);
              this.proyectos.forEach(p => {
                if (p.contratacion.tipoSolicitudCodigo === '1')
                  p.proyecto.estadoProyectoObraCodigo = EstadosProyecto.AprobadoComiteFiduciario;
                if (p.contratacion.tipoSolicitudCodigo === '2')
                  p.proyecto.estadoProyectoInterventoriaCodigo = EstadosProyecto.AprobadoComiteFiduciario;
              })
              break;
            case this.estadosSolicitud.RechazadaPorComiteFiduciario:
              this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.RechazadoComiteFiduciario);
              this.proyectos.forEach(p => {
                if (p.contratacion.tipoSolicitudCodigo === '1')
                  p.proyecto.estadoProyectoObraCodigo = EstadosProyecto.RechazadoComiteFiduciario;
                if (p.contratacion.tipoSolicitudCodigo === '2')
                  p.proyecto.estadoProyectoInterventoriaCodigo = EstadosProyecto.RechazadoComiteFiduciario;
              })
              break;
            case this.estadosSolicitud.DevueltaPorComiteFiduciario:
              this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.DevueltoComiteFiduciario);
              this.proyectos.forEach(p => {
                if (p.contratacion.tipoSolicitudCodigo === '1')
                  p.proyecto.estadoProyectoObraCodigo = EstadosProyecto.DevueltoComiteFiduciario;
                if (p.contratacion.tipoSolicitudCodigo === '2')
                  p.proyecto.estadoProyectoInterventoriaCodigo = EstadosProyecto.DevueltoComiteFiduciario;
              })
              break;
          }
        } else if (this.cantidadProyecto > 1) {
          switch (estado) {
            case EstadosSolicitud.AprobadaPorComiteFiduciario:
              this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.AprobadoComiteFiduciario);
              break;
            default:
              this.listaEstados = this.listaEstadosCompleta.filter(e => ["4", "6", "8"].includes(e.codigo));

          }

        }
        this.proyectos.forEach(p => {
          let estadoObra = this.listaEstados.find(e => e.codigo == p.proyecto.estadoProyectoObraCodigo);
          let estadoInterventoria = this.listaEstados.find(e => e.codigo == p.proyecto.estadoProyectoInterventoriaCodigo);
          p.proyecto.estadoProyectoObraCodigo = estadoObra ? estadoObra.codigo : null;
          p.proyecto.estadoProyectoInterventoriaCodigo = estadoInterventoria ? estadoInterventoria.codigo : null;
        });
      }else{
        /// esta seccion es cuando no se ha seleccionado el estado de la solictud
        this.listaEstados = [];
        console.log(this.listaEstadosSolicitud, EstadosSolicitud.AprobadaPorComiteTecnico, this.listaEstadosCompleta)
        this.listaEstadosSolicitud.forEach(estadoSolicitud => {
          switch  (estadoSolicitud.codigo){
            case EstadosSolicitud.AprobadaPorComiteFiduciario:{
              Array.prototype.push.apply(this.listaEstados, this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.AprobadoComiteFiduciario));
              break;
            }
            case EstadosSolicitud.RechazadaPorComiteFiduciario:{
              Array.prototype.push.apply(this.listaEstados, this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.RechazadoComiteFiduciario));
              break;
            }
            case EstadosSolicitud.DevueltaPorComiteFiduciario:{
              Array.prototype.push.apply(this.listaEstados, this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.DevueltoComiteFiduciario));
              break;
            }
          }

        });
      }
    } else {
      this.listaEstados = this.listaEstadosCompleta;
      console.log('entro', this.EstadosolicitudActa)
    }
  }

  ngOnInit(): void {

    this.commonService.listaEstadoProyecto()
      .subscribe(estados => {
        this.listaEstadosCompleta = estados;
        this.listaEstados = this.listaEstadosCompleta.filter(e => this.estadosValidos.includes(e.codigo));

        this.Estadosolicitud.subscribe(estado => {
          console.log('estado', estado)
          this.cambiarEstados(estado);

        })

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
    this.cargarRegistro();
  }

  Observaciones(contratacionProyectoid: number,
    contratacionid: number,
    contratacionObservacion: ContratacionObservacion[],
    proyectoId: number,
    estadoProyectoObraCodigo: number,
    estadoProyectoInterventoriaCodigo: number,
    verDetalle: boolean) {

    let idsesionComiteSolicitud = this.sesionComiteSolicitud.sesionComiteSolicitudId;
    let idcomiteTecnico = this.sesionComiteSolicitud.comiteTecnicoFiduciarioId;

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
        estadoProyectoInterventoriaCodigo,
        verDetalle
      }
    });

    dialogRef.afterClosed().subscribe(observaciones => {
      this.cargarRegistro();
    })
  }

  // Observaciones(contratacionProyectoid: number, contratacionid: number) {
  //   this.router.navigate(['/comiteTecnico/crearActa',
  //     this.sesionComiteSolicitud.comiteTecnicoId,
  //     'observacion',
  //     this.sesionComiteSolicitud.sesionComiteSolicitudId,
  //     this.sesionComiteSolicitud.comiteTecnicoId,
  //     contratacionProyectoid,
  //     contratacionid
  //   ])
  // }

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
