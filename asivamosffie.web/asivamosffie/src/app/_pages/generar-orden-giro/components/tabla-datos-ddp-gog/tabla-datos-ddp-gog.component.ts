import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-datos-ddp-gog',
  templateUrl: './tabla-datos-ddp-gog.component.html',
  styleUrls: ['./tabla-datos-ddp-gog.component.scss']
})
export class TablaDatosDdpGogComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['componente', 'uso', 'valorUso', 'valorTotal'];
  dataTable: any[] = [
    {
      componente: "Obra", 
      uso: [
        { nombre: "Diseño" }, 
        { nombre: "Diagnostico" }, 
        { nombre: "Obra Principal" }
      ], 
      valorUso: [
        { valor: "$ 8.000.000" }, 
        { valor: "$ 12.000.000" }, 
        { valor: "$ 60.000.000" }
      ], valorTotal: '$80.000.000'
    },
  ];
  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
}
