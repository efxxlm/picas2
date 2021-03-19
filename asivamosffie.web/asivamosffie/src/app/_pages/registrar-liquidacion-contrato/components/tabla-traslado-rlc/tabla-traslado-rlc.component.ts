import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-traslado-rlc',
  templateUrl: './tabla-traslado-rlc.component.html',
  styleUrls: ['./tabla-traslado-rlc.component.scss']
})
export class TablaTrasladoRlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'tipoSolicitud',
    'fechaAprobacionFiduciaria',
    'fechaPagoFiduciaria',
    'numeroOrdenGiro',
    'modalidadContrato',
    'numeroContrato'
  ];
  dataTable: any[] = [{
    tipoSolicitud: 'Obra',
    fechaAprobacionFiduciaria: '15/11/2020',
    fechaPagoFiduciaria: '22/11/2020',
    numeroOrdenGiro: 'ODG_Obra_222',
    modalidadContrato: 'Modalidad 1',
    numeroContrato: 'N801801'
    }];
  constructor() { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
}
