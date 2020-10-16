import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-control-y-tabla-controversias-contractuales',
  templateUrl: './control-y-tabla-controversias-contractuales.component.html',
  styleUrls: ['./control-y-tabla-controversias-contractuales.component.scss']
})
export class ControlYTablaControversiasContractualesComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaSolicitud',
    'numeroSolicitud',
    'tipoControversia',
    'estadoControversia',
    'gestion',
  ];
  dataTable: any[] = [
    {
      fechaSolicitud: '20/08/2020',
      numeroSolicitud: 'CO001',
      tipoControversia: '1',
      estadoControversia: '1',
      id: 1
    },
    {
      fechaSolicitud: '10/08/2020',
      numeroSolicitud: 'CO002',
      tipoControversia: '1',
      estadoControversia: '3',
      id: 2
    }
  ];  
  constructor() {
   }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
}
