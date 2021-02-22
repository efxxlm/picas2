import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-acordion-recursos-compro-pagados-gtlc',
  templateUrl: './acordion-recursos-compro-pagados-gtlc.component.html',
  styleUrls: ['./acordion-recursos-compro-pagados-gtlc.component.scss']
})
export class AcordionRecursosComproPagadosGtlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'aportante',
    'valorAportante',
    'valorTotalAportantes'
  ];
  tablaEjemplo: any[] = [
    {
      aportante: [
        { nombre: "Alcaldía de Susacón" }, 
        { nombre: "Gobernación de Boyacá" }, 
        { nombre: "FFIE" }
      ],
      valorAportante: [
        { valor: "$45.000.000" }, 
        { valor: "$40.000.000" }, 
        { valor: "$20.000.000" }
      ], 
      valorTotalAportantes: "$105.000.000", 

    },
  ];
  constructor() { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.tablaEjemplo);
    this.dataSource.sort = this.sort;
  }

}
