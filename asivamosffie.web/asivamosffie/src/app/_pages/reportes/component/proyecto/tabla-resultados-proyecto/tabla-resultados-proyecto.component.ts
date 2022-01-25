import { Component, ViewChild, OnInit, Output,Input, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';


@Component({
  selector: 'app-tabla-resultados-proyecto',
  templateUrl: './tabla-resultados-proyecto.component.html',
  styleUrls: ['./tabla-resultados-proyecto.component.scss']
})
export class TablaResultadosProyectoComponent implements OnInit {

  @Output() morstrarFicha = new EventEmitter<{ficha: boolean, proyectoId: number}>();
  @Input() resultados: any[];

  ELEMENT_DATA: any[] = [];
  displayedColumns: string[] = [
    'llaveMen',
    'departamentoMunicipio',
    'institucionEducativaSede',
    'tipoIntervencion',
    'vigencia',
    'proyectoId'
  ];

  dataSource = new MatTableDataSource(this.ELEMENT_DATA);

  @ViewChild(MatSort) sort: MatSort;
  constructor() { }

  ngOnInit(): void {
    this.dataSource.data = this.resultados;
  }

  verFicha(proyectoId: number) {
    this.morstrarFicha.emit({ficha: true, proyectoId: proyectoId});
  }

  descargarFicha() {
    console.log('descargar Ficha');
  }

}
