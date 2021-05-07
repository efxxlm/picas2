import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { FuenteFinanciacionService } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';

@Component({
  selector: 'app-saldos-y-rendimientos',
  templateUrl: './saldos-y-rendimientos.component.html',
  styleUrls: ['./saldos-y-rendimientos.component.scss']
})
export class SaldosYRendimientosComponent implements OnInit, AfterViewInit {
  
  aportanteId: number = 0;
  displayedColumns: string[] = ['saldoActual', 'comprometidoEnDdp', 'rendimientosIncorporados'];
  ELEMENT_DATA: any[] = [];
  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  datosTabla = [];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private activatedRoute: ActivatedRoute, private fuenteFinanciacionService: FuenteFinanciacionService) {}

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.aportanteId = param['aportanteId'];

      this.getVSaldosFuenteXaportanteId();
    });
  }

  getVSaldosFuenteXaportanteId() {
    forkJoin([this.fuenteFinanciacionService.getVSaldosFuenteXaportanteId(this.aportanteId)]).subscribe(res => {
      if (res[0].length > 0) {
        res.forEach(element => {
          this.datosTabla.push({
            saldoActual: element[0].saldoActual,
            comprometidoEnDdp: element[0].comprometidoEnDdp,
            rendimientosIncorporados: element[0].rendimientosIncorporados
          });
        });
        this.dataSource.data = this.datosTabla;
      }
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
