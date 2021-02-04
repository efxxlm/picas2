import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-gestionar-balan-financ-trasl-rec',
  templateUrl: './gestionar-balan-financ-trasl-rec.component.html',
  styleUrls: ['./gestionar-balan-financ-trasl-rec.component.scss']
})
export class GestionarBalanFinancTraslRecComponent implements OnInit {
  verAyuda = false;
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaTerminacionProyecto',
    'llaveMEN',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'numeroTraslados',
    'estadoBalance',
    'gestion'
  ];
  dataTable: any[] = [
    {
      fechaTerminacionProyecto: '09/08/2021',
      llaveMEN: 'LL457326',
      tipoIntervencion: 'Remodelación',
      institucionEducativa: 'I.E Nuestra Señora Del Carmen/Única sede',
      sede: 'Única sede',
      numeroTraslados: '---',
      estadoBalance: 'Sin balance validado',
      gestion: 1
    },
  ];
  constructor(
    private routes: Router
  ) { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }
  validarBalance(id){
    this.routes.navigate(['/gestionarBalanceFinancieroTrasladoRecursos/validarBalance', id]);
  }
  verDetalleEditarBalance(id){
    this.routes.navigate(['/gestionarBalanceFinancieroTrasladoRecursos/verDetalleEditarBalance', id]);
  }
  verDetalleBalance(id){
    this.routes.navigate(['/gestionarBalanceFinancieroTrasladoRecursos/verDetalleBalance', id]);
  }
}
