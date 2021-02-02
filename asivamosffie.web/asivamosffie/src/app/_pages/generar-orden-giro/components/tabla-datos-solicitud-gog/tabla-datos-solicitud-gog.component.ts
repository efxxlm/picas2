import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-datos-solicitud-gog',
  templateUrl: './tabla-datos-solicitud-gog.component.html',
  styleUrls: ['./tabla-datos-solicitud-gog.component.scss']
})
export class TablaDatosSolicitudGogComponent implements OnInit {

    @Input() listProyectos: any[];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'llaveMen',
      'tipoIntervencion',
      'departamento',
      'municipio',
      'institucionEducativa',
      'sede'
    ];

    constructor() { }

    ngOnInit(): void {
      this.dataSource = new MatTableDataSource( this.listProyectos );
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    };

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    };

}
