import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-datos-solicitud-gtlc',
  templateUrl: './datos-solicitud-gtlc.component.html',
  styleUrls: ['./datos-solicitud-gtlc.component.scss']
})
export class DatosSolicitudGtlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'llaveMen',
    'tipoIntervencion',
    'departamento',
    'municipio',
    'institucionEducativa',
    'sede'
  ];
  dataTable: any[] = [{
    llaveMen: 'LL457326',
    tipoIntervencion: 'Remodelación',
    departamento: 'Boyacá',
    municipio: 'Susacón',
    institucionEducativa: 'I.E Nuestra Señora Del Carmen',
    sede: 'Única sede'
  }]
  constructor() { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }

}
