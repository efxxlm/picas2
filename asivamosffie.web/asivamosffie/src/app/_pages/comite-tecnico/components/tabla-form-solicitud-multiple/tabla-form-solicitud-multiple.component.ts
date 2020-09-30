import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
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


@Component({
  selector: 'app-tabla-form-solicitud-multiple',
  templateUrl: './tabla-form-solicitud-multiple.component.html',
  styleUrls: ['./tabla-form-solicitud-multiple.component.scss']
})
export class TablaFormSolicitudMultipleComponent implements OnInit {

  @Input() sesionComiteSolicitud: SesionComiteSolicitud;
  @Input() Estadosolicitud: Observable<Dominio>;
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

  ) {

  }

  ngOnInit(): void {

    this.commonService.listaEstadoProyecto()
      .subscribe(estados => {
        this.listaEstadosCompleta = estados;
        this.listaEstados = this.listaEstadosCompleta.filter(e => this.estadosValidos.includes(e.codigo));

        this.Estadosolicitud.subscribe(estado => {
          this.proyectos.forEach( p => { p.proyecto.estadoProyecto = {}; } )
          this.onChangeEstado();
          if (estado)
          if (this.cantidadProyecto > 0)
          {
            switch (estado.codigo) {
              case EstadosSolicitud.AprobadaPorComiteTecnico:
                this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.AprobadoComiteTecnico);
                break;
              case this.estadosSolicitud.RechazadaPorComiteTecnico:
                this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.RechazadoComiteTecnico);
                break;
              case this.estadosSolicitud.DevueltaPorComiteTecnico:
                this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.DevueltoComiteTecnico);
                break;
            }
          }else{
            switch (estado.codigo) {
              case EstadosSolicitud.AprobadaPorComiteTecnico:
                this.listaEstados = this.listaEstadosCompleta.filter(e => e.codigo == EstadosProyecto.AprobadoComiteTecnico);
                break;
              default:
                this.listaEstados = this.listaEstadosCompleta.filter(e => ["3","5"].includes(e.codigo));
            }

          }
                      
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
    this.router.navigate(['/comiteTecnico/crearActa',
      this.sesionComiteSolicitud.comiteTecnicoId,
      'observacion',
      this.sesionComiteSolicitud.sesionComiteSolicitudId,
      this.sesionComiteSolicitud.comiteTecnicoId,
      contratacionProyectoid,
      contratacionid
    ])
  }

  onChangeEstado(){
    this.ActualizarProyectos.emit( this.proyectos );
  }

  cargarRegistro() {

    //this.proyectos = [];

    if (this.sesionComiteSolicitud.contratacion) {
      this.projectContractingService.getContratacionByContratacionId(this.sesionComiteSolicitud.contratacion.contratacionId)
        .subscribe(contra => {
          this.cantidadProyecto = contra.contratacionProyecto.length;

          contra.contratacionProyecto.forEach(cp => {
            this.projectService.getProyectoGrillaByProyectoId(cp.proyectoId)
              .subscribe(proy => {
                cp.proyecto = proy;
                
              })
        
            this.proyectos = contra.contratacionProyecto;  
            this.ActualizarProyectos.emit( this.proyectos );
            this.dataSource = new MatTableDataSource(contra.contratacionProyecto);
           
          })

        })

    }
  }

}
