import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    drp: '1',
    numeroDRP: 'DRP_PI_91',
    valor: '$16.000.000',
    saldo: '$15.999.800'
  }
];

@Component({
  selector: 'app-tabla-datos-ddp',
  templateUrl: './tabla-datos-ddp.component.html',
  styleUrls: ['./tabla-datos-ddp.component.scss']
})
export class TablaDatosDdpComponent implements OnInit, AfterViewInit {

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'drp',
    'numeroDRP',
    'valor',
    'saldo'
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
