import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    llaveMen: 'LISA21',
    tipoIntervencion: 'Ampliación',
    departamento: 'BOGOTÁ D.C.',
    municipio: 'BOGOTÁ D.C.	',
    institucionEducativa: 'COLEGIO ROGELIO SALMONA',
    sede: 'SEDE PRINCIPAL'
  }
];

@Component({
  selector: 'app-tabla-datos-solicitud',
  templateUrl: './tabla-datos-solicitud.component.html',
  styleUrls: ['./tabla-datos-solicitud.component.scss']
})
export class TablaDatosSolicitudComponent implements OnInit, AfterViewInit {

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'llaveMen',
    'tipoIntervencion',
    'departamento',
    'municipio',
    'institucionEducativa',
    'sede'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatSort) sort: MatSort;
  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

}