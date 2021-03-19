import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-datos-ddp-drp-gbftrec',
  templateUrl: './datos-ddp-drp-gbftrec.component.html',
  styleUrls: ['./datos-ddp-drp-gbftrec.component.scss']
})
export class DatosDdpDrpGbftrecComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'drp',
    'numDRP',
    'valor',
    'saldo'
  ];
  dataTable: any[] = [
    {
      drp: '1',
      numDRP: 'IP_00090',
      valor: '$100.000.000',
      saldo: '$100.000.000'
    },
    {
      drp: '2',
      numDRP: 'IP_00123',
      valor: '$5.000.000',
      saldo: '$5.000.000'
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
