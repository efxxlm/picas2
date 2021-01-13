import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-accordion-info-general-gog',
  templateUrl: './accordion-info-general-gog.component.html',
  styleUrls: ['./accordion-info-general-gog.component.scss']
})
export class AccordionInfoGeneralGogComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'drp',
    'numDrp',
    'valor',
    'saldo'
  ];
  dataTable: any[] = [
    {
      drp: 1,
      numDrp: 'IP_00090',
      valor: '$ 100.000.000',
      saldo: '$ 100.000.000'
    },
    {
      drp: 2,
      numDrp: 'IP_00123',
      valor: '$ 5.000.000',
      saldo: '$ 5.000.000'
    },
  ];
  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

}
