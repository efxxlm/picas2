import { Component, OnInit, ViewChild, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SesionComiteSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { ProjectService, Proyecto, ProyectoGrilla } from 'src/app/core/_services/project/project.service';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { ContratacionProyecto, EstadosProyecto, EstadosSolicitud } from 'src/app/_interfaces/project-contracting';
import { Router } from '@angular/router';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { Observable } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ObservacionComponent } from '../observacion/observacion.component'

@Component({
  selector: 'app-tabla-form-solicitud-multiple',
  templateUrl: './tabla-form-solicitud-multiple.component.html',
  styleUrls: ['./tabla-form-solicitud-multiple.component.scss']
})
export class TablaFormSolicitudMultipleComponent implements OnInit, OnChanges {

  @Input() sesionComiteSolicitud: SesionComiteSolicitud;
  @Input() Estadosolicitud: Observable<string>;
  @Output() ActualizarProyectos: EventEmitter<ContratacionProyecto[]> = new EventEmitter();

  cantidadProyecto: Number = 0;
  listaEstados: Dominio[] = [];
  listaEstadosCompleta: Dominio[] = [];
  estadosValidos: string[] = ['3', '5', '7']
  estadosSolicitud = EstadosSolicitud;
  proyectos: ContratacionProyecto[] = []

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
  ) {

  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes.sesionComiteSolicitud) {
      this.cargarRegistro();
      console.log('input', this.Estadosolicitud, changes)
    }


  }

  cambiarEstados(estado: string) {
    
    //this.proyectos.forEach(p => { p.proyecto.estadoProyectoCodigo = {}; })
    this.onChangeEstado();
    if (estado) {
      if (this.cantidadProyecto == 1) {
        switch (estado) {
          case EstadosSolicitud.AprobadaPorComiteTecnico:
            this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.AprobadoComiteTecnico);
            this.proyectos.forEach(p => { p.proyecto.estadoProyectoCodigo = EstadosProyecto.AprobadoComiteTecnico; })
            break;
          case this.estadosSolicitud.RechazadaPorComiteTecnico:
            this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.RechazadoComiteTecnico);
            this.proyectos.forEach(p => { p.proyecto.estadoProyectoCodigo = EstadosProyecto.RechazadoComiteTecnico; })
            break;
          case this.estadosSolicitud.DevueltaPorComiteTecnico:
            this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.DevueltoComiteTecnico);
            this.proyectos.forEach(p => { p.proyecto.estadoProyectoCodigo = EstadosProyecto.DevueltoComiteTecnico; })
            break;
        }
      } else if (this.cantidadProyecto > 1) {
        switch (estado) {
          case EstadosSolicitud.AprobadaPorComiteTecnico:
            this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.AprobadoComiteTecnico);
            break;
          default:
            this.listaEstados = this.listaEstadosCompleta.filter(e => ["3", "5", "7"].includes(e.codigo));
        }

      }
      this.proyectos.forEach(p => {
        let estado = this.listaEstados.find(e => e.codigo == p.proyecto.estadoProyectoCodigo);
        p.proyecto.estadoProyectoCodigo = estado ? estado.codigo : null;
        console.log( this.listaEstados,  p.proyecto.estadoProyectoCodigo)
      });
    }
  }

  ngOnInit(): void {

    this.commonService.listaEstadoProyecto()
      .subscribe(estados => {
        this.listaEstadosCompleta = estados;
        this.listaEstados = this.listaEstadosCompleta.filter(e => this.estadosValidos.includes(e.codigo));

        this.Estadosolicitud.subscribe(estado => {
          console.log('estado', estado)
          this.cambiarEstados( estado );  

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
  Observaciones(contratacionProyectoid: number, contratacionid: number) {
    let idsesionComiteSolicitud = this.sesionComiteSolicitud.sesionComiteSolicitudId;
    let idcomiteTecnico = this.sesionComiteSolicitud.comiteTecnicoId;

    // this.router.navigate(['/comiteTecnico/crearActa',
    //   this.sesionComiteSolicitud.comiteTecnicoId,
    //   'observacion',
    //   this.sesionComiteSolicitud.sesionComiteSolicitudId,
    //   this.sesionComiteSolicitud.comiteTecnicoId,
    //   contratacionProyectoid,
    //   contratacionid
    // ])

    const dialogRef = this.dialog.open(ObservacionComponent, {
      width: '60em',
      data: { contratacionProyectoid, contratacionid, 
            idsesionComiteSolicitud, idcomiteTecnico
             }
    });
  }

  onChangeEstado() {
    this.ActualizarProyectos.emit(this.proyectos);
  }

  cargarRegistro() {

    if (this.sesionComiteSolicitud.contratacion) {
      let promesa = new Promise(resolve => {
        this.projectContractingService.getContratacionByContratacionIdWithGrillaProyecto(this.sesionComiteSolicitud.contratacion.contratacionId)
          .subscribe(contra => {
            console.log( contra );
            this.cantidadProyecto = contra.contratacionProyecto.length;

            this.proyectos = contra.contratacionProyecto;
            this.ActualizarProyectos.emit(this.proyectos);
            this.dataSource = new MatTableDataSource(contra.contratacionProyecto);
            this.cambiarEstados( this.sesionComiteSolicitud.estadoCodigo );
            resolve();
          })

      })

      // promesa.then(() => {
      //   this.proyectos.forEach(p => {
      //     let estado = this.listaEstados.find(e => e.codigo == p.proyecto.estadoProyectoCodigo);
      //     p.proyecto.estadoProyectoCodigo = estado ? estado.codigo : null;

      //     console.log(p.proyecto.proyectoId, this.sesionComiteSolicitud.estadoCodigo)


      //   });
      // })



    }
  }

}
