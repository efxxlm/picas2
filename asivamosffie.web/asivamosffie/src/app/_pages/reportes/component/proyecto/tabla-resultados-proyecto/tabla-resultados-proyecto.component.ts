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

  @Output() morstrarFicha = new EventEmitter<{ficha: boolean, contratacionProyectoId: number}>();
  @Input() resultados: any[];

  ELEMENT_DATA: any[] = [];
  displayedColumns: string[] = [
    'llaveMen',
    'departamentoMunicipio',
    'institucionEducativaSede',
    'nombreTipoContrato',
    'tipoIntervencion',
    'vigencia',
    'contratacionProyectoId'
  ];

  dataSource = new MatTableDataSource(this.ELEMENT_DATA);

  @ViewChild(MatSort) sort: MatSort;
  constructor() { }

  ngOnInit(): void {
    this.dataSource.data = this.resultados;
  }

  verFicha(contratacionProyectoId: number) {
    this.morstrarFicha.emit({ficha: true, contratacionProyectoId: contratacionProyectoId});
  }

  descargarFicha() {
    console.log('descargar Ficha');
  }

}
