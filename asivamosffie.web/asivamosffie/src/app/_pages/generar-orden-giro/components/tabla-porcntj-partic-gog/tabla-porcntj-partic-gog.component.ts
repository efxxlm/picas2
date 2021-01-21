import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-porcntj-partic-gog',
  templateUrl: './tabla-porcntj-partic-gog.component.html',
  styleUrls: ['./tabla-porcntj-partic-gog.component.scss']
})
export class TablaPorcntjParticGogComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['drp', 'numeroDRP', 'nombreAportante', 'porcParticipacion'];
  dataTable: any[] = [
    /*
    {
      drp: "1", 
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
    {
      drp: "2"
    }*/
    {
      drp: "1", 
      numeroDRP: "IP_00090",
      nombreAportante : [
        {nombre: 'Alcaldía de Susacón'},
        {nombre: 'Fundación Pies Descalzos'}
      ],
      porcParticipacion : [
        {numero: '70'},
        {numero: '30'}
      ]
    },
    {
      drp: "2",
      numeroDRP: "IP_00123",
      nombreAportante : [
        {nombre: 'Alcaldía de Susacón'}
      ],
      porcParticipacion : [
        {numero: '100'}
      ]
    }
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
