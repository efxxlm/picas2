import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-porc-participacion-gtlc',
  templateUrl: './tabla-porc-participacion-gtlc.component.html',
  styleUrls: ['./tabla-porc-participacion-gtlc.component.scss']
})
export class TablaPorcParticipacionGtlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'drp',
    'numDrp',
    'nomAportante',
    'porcParticipacion'
  ];
  dataTable: any[] = [
    {
      drp: '1',
      numDrp: 'IP_00090',
      nomAportante: [{nombre:'Alcaldía de Susacón'},{nombre:'Gobernación de Boyacá'}],
      porcParticipacion: [{valor:'70%'},{valor:'30%'}]
    },
    {
      drp: '2',
      numDrp: 'IP_00123',
      nomAportante: [{nombre:'Alcaldía de Susacón'}],
      porcParticipacion: [{valor:'100%'}]
    },
  ];
  constructor() { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
}
