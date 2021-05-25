import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    nombreAportante : 'Alcaldía BOGOTÁ D.C.',
    fuenteRecursos : 'Regalías',
    saldoActual : '$16,000,000',
    saldoAfectado : '$14,200,950'
  }
];

@Component({
  selector: 'app-tabla-info-fuenterec-gog',
  templateUrl: './tabla-info-fuenterec-gog.component.html',
  styleUrls: ['./tabla-info-fuenterec-gog.component.scss']
})
export class TablaInfoFuenterecGogComponent implements OnInit, AfterViewInit {

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'nombreAportante',
    'fuenteRecursos',
    'saldoActual',
    'saldoAfectado'
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
