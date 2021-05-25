import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    uso : 'Interventoría de Obra principal',
    fuente : 'Regalías',
    aportante : 'Alcaldía BOGOTÁ D.C.',
    valorUso : '$15.000.000',
    saldoActualUso : '$15.000.000'
  }
];

@Component({
  selector: 'app-tabla-datos-ddp-gog',
  templateUrl: './tabla-datos-ddp-gog.component.html',
  styleUrls: ['./tabla-datos-ddp-gog.component.scss']
})
export class TablaDatosDdpGogComponent implements OnInit, AfterViewInit {

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'uso',
    'fuente',
    'aportante',
    'valorUso',
    'saldoActualUso'
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
