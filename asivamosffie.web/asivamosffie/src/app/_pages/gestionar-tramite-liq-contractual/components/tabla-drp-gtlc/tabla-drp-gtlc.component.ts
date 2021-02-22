import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-drp-gtlc',
  templateUrl: './tabla-drp-gtlc.component.html',
  styleUrls: ['./tabla-drp-gtlc.component.scss']
})
export class TablaDrpGtlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'uso',
    'aportante',
    'valorUso',
    'saldoActualUso'
  ];
  dataTable: any[] = [
    {
      uso: 'Diseño Obra Complementaria',
      aportante: [{nombre:'Alcaldía de Susacón'},{nombre:'Gobernación de Boyacá'}],
      valorUso: [{valor:'$ 45.000.000'},{valor:'$ 30.000.000'}],
      saldoActualUso: [{saldo:'$ 45.000.000'},{saldo:'$ 30.000.000'}]
    },
    {
      uso: 'Estudios y diseños',
      aportante: [{nombre:'Alcaldía de Susacón'}],
      valorUso: [{valor:'$ 30.000.000'}],
      saldoActualUso: [{saldo:'$ 15.000.000'}]
    }
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
