import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';

const ELEMENT_DATA = [
  {
    saldoActual: 'Saldo actual',
    comprometidoDDP: 'Comprometido en DDP',
    rendimientosIncorporados: 'Rendimientos incorporados'
  },
  {
    saldoActual: 'Saldo actual',
    comprometidoDDP: 'Comprometido en DDP',
    rendimientosIncorporados: 'Rendimientos incorporados'
  }
];

@Component({
  selector: 'app-saldos-y-rendimientos',
  templateUrl: './saldos-y-rendimientos.component.html',
  styleUrls: ['./saldos-y-rendimientos.component.scss']
})
export class SaldosYRendimientosComponent implements OnInit, AfterViewInit {
  idFuente: number = 0;
  idControl: number = 0;

  ELEMENT_DATA: any[];
  displayedColumns: string[] = ['saldoActual', 'comprometidoDDP', 'rendimientosIncorporados'];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idFuente = param['idFuente'];
      this.idControl = param['idControl'];

      console.log(this.idFuente, this.idControl);
    });
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
