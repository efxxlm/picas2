import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SesionComiteSolicitud } from 'src/app/_interfaces/technicalCommitteSession';
import { ProjectService } from 'src/app/core/_services/project/project.service';


@Component({
  selector: 'app-tabla-form-solicitud-multiple',
  templateUrl: './tabla-form-solicitud-multiple.component.html',
  styleUrls: ['./tabla-form-solicitud-multiple.component.scss']
})
export class TablaFormSolicitudMultipleComponent implements OnInit {

  @Input() sesionComiteSolicitud: SesionComiteSolicitud;

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
                private projectService: ProjectService
             ) 
  {

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
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }

  cargarRegistro(){

    this.sesionComiteSolicitud.contratacion.contratacionProyecto.forEach( cp => {
      this.projectService.getProjectById( cp.proyectoId )
        .subscribe( proy => {
          cp.proyecto = proy; 
        })
    })

    console.log( this.sesionComiteSolicitud.contratacion.contratacionProyecto );

    this.dataSource = new MatTableDataSource( this.sesionComiteSolicitud.contratacion.contratacionProyecto );


  }

}
