import { Component, ViewChild, OnInit, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    id: '1',
    llaveMen: 'LL457326',
    departamento: 'Boyacá',
    municipio: 'Susacón',
    institucionEducativa: 'I.E Nuestra Señora Del Carmen',
    sede: 'Única sede',
    tipoContrato: 'Obra',
    vigencia: '2021'
  }
];


@Component({
  selector: 'app-tabla-resultados-proyecto',
  templateUrl: './tabla-resultados-proyecto.component.html',
  styleUrls: ['./tabla-resultados-proyecto.component.scss']
})
export class TablaResultadosProyectoComponent implements OnInit {

  @Output() morstrarFicha = new EventEmitter();

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'llaveMen',
    'departamento',
    'institucionEducativa',
    'tipoContrato',
    'vigencia',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatSort) sort: MatSort;
  constructor() { }

  ngOnInit(): void {
  }

  verFicha() {
    this.morstrarFicha.emit( true );
  }

  descargarFicha() {
    console.log('descargar Ficha');
  }

}