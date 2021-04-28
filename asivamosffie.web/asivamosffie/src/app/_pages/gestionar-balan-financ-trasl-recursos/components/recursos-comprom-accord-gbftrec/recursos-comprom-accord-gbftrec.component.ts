import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-recursos-comprom-accord-gbftrec',
  templateUrl: './recursos-comprom-accord-gbftrec.component.html',
  styleUrls: ['./recursos-comprom-accord-gbftrec.component.scss']
})
export class RecursosCompromAccordGbftrecComponent implements OnInit {

  @Input() contratacionProyecto: any[] = [];
  dataTable: any[] = [];
  aportante: any[] = [];
  valorAportante: any[] = [];
  valorTotalAportantes : number = 0;
  dataSource = new MatTableDataSource();
  //@ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
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
    if(this.contratacionProyecto.length > 0){
      this.contratacionProyecto.forEach(contratacionProyecto => {
        contratacionProyecto.contratacionProyectoAportante.forEach(element => {
          if (element['cofinanciacionAportante'].tipoAportanteId === 6) {
            this.aportante.push({
              nombre: 'FFIE'
            });
          } else if (element['cofinanciacionAportante'].tipoAportanteId === 9) {
          if (element['cofinanciacionAportante'].departamento !== undefined && element['cofinanciacionAportante'].municipio === undefined) {
            this.aportante.push({
              nombre: `Gobernación de ${element['cofinanciacionAportante'].departamento.descripcion}`
            });
          };
          if (element['cofinanciacionAportante'].departamento !== undefined && element['cofinanciacionAportante'].municipio !== undefined) {
            this.aportante.push({
              nombre: `Alcaldía de ${element['cofinanciacionAportante'].municipio.descripcion}`
            });
          };
          } else if (element['cofinanciacionAportante'].tipoAportanteId === 10) {
            this.aportante.push({
              nombre: `${element['cofinanciacionAportante'].nombreAportante.nombre}`
            });
          }
            this.valorAportante.push({
              valor: element.valorAporte
            });
          });
      });
    }

    this.valorAportante.forEach(element => {
      this.valorTotalAportantes = this.valorTotalAportantes + element.valor;
    });
    this.dataTable.push({
      aportante: this.aportante,
      valorAportante: this.valorAportante, 
      valorTotalAportantes: this.valorTotalAportantes, 
    });
    this.dataSource = new MatTableDataSource(this.dataTable);
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.tablaEjemplo);
    this.dataSource.sort = this.sort;
    //this.dataSource.paginator = this.paginator;
    //this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    /*
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
    };*/
  }
}
