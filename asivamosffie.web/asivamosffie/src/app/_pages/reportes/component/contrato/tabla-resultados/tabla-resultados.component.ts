import { Component, ViewChild, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-resultados',
  templateUrl: './tabla-resultados.component.html',
  styleUrls: ['./tabla-resultados.component.scss']
})
export class TablaResultadosComponent implements OnInit {
  @Output() morstrarFicha = new EventEmitter<{ ficha: boolean; contratoId: number }>();
  @Input() resultados: any;

  ELEMENT_DATA: any[] = [];
  displayedColumns: string[] = [
    'numeroContrato',
    'contratista',
    'departamento',
    'institucionEducativa',
    'tipoContrato',
    'vigencia',
    'id'
  ];
  dataSource = new MatTableDataSource(this.ELEMENT_DATA);

  @ViewChild(MatSort) sort: MatSort;
  constructor() {}

  ngOnInit(): void {
    let infoDataTable = [];
    infoDataTable[0] = this.resultados.informacion;
    this.dataSource.data = infoDataTable;
  }

  verFicha(contratoId: number) {
    this.morstrarFicha.emit({ ficha: true, contratoId: contratoId });
  }

  descargarFicha() {
    console.log('descargar Ficha');
  }
}
