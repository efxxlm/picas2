import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-desc-dir-tecnica-gog',
  templateUrl: './desc-dir-tecnica-gog.component.html',
  styleUrls: ['./desc-dir-tecnica-gog.component.scss']
})
export class DescDirTecnicaGogComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'tipoDescuento',
    'valorDescuento',
    'valorTotalDescuentos',
    'valorNetoGiro'
  ];
  dataTable: any[] = [
    {
      tipoDescuento: '4 x mil',
      valorDescuento: '$ 60.000',
      valorTotalDescuentos: '$ 60.000',
      valorNetoGiro: '$ 14.940.000'
    }
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
