import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';


@Component({
  selector: 'app-table-solicitud-contratacion',
  templateUrl: './table-solicitud-contratacion.component.html',
  styleUrls: ['./table-solicitud-contratacion.component.scss']
})
export class TableSolicitudContratacionComponent implements OnInit {

  displayedColumns: string[] = [
    'fecha',
    'numero',
    'opcionPorContratar',
    'estadoSolicitud',
    'estadoDelIngreso',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
              private projectContractingService: ProjectContractingService
  ) { }

  ngOnInit(): void {

    this.projectContractingService.getListContratacion().subscribe( response => {

      this.dataSource = new MatTableDataSource( response );

      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    })
    
    
  }

  detallarSolicitud(id: number) {
    console.log(id);
  }
  eliminarSolicitud(id: number) {
    console.log(id);
  }

}
