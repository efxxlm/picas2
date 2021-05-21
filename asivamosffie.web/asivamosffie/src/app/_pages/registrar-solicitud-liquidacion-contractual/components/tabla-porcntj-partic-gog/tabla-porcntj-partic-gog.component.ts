import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    drp : '1',
    numeroDRP : 'DRP_PI_91',
    nombreAportante : 'Alcaldía BOGOTÁ D.C.',
    porcParticipacion : '100%'
  }
];

@Component({
  selector: 'app-tabla-porcntj-partic-gog',
  templateUrl: './tabla-porcntj-partic-gog.component.html',
  styleUrls: ['./tabla-porcntj-partic-gog.component.scss']
})
export class TablaPorcntjParticGogComponent implements OnInit, AfterViewInit {

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'drp',
    'numeroDRP',
    'nombreAportante',
    'porcParticipacion'
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
