import { Component, ViewChild, OnInit, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-recursos-comprometidos',
  templateUrl: './tabla-recursos-comprometidos.component.html',
  styleUrls: ['./tabla-recursos-comprometidos.component.scss']
})
export class TablaRecursosComprometidosComponent implements OnInit {

  @Input() contratacionProyecto: any[] = [];
  dataTable: any[] = [];
  aportante: any[] = [];
  valorAportante: any[] = [];
  valorTotalAportantes : number = 0;
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'aportante',
    'valorAportante',
    'valorTotalAportantes'
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
    this.dataSource.sort = this.sort;
  }


}