import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-traslado-recursos-gtlc',
  templateUrl: './traslado-recursos-gtlc.component.html',
  styleUrls: ['./traslado-recursos-gtlc.component.scss']
})
export class TrasladoRecursosGtlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaTraslado',
    'numTraslado',
    'numContrato',
    'numOrdenGiro',
    'valorTraslado',
    'estadoTraslado',
    'gestion'
  ];
  dataTable: any[] = [{
    fechaTraslado: '09/08/2021',
    numTraslado: 'Tras_001',
    numContrato: 'N000000',
    numOrdenGiro: 'ODG_obra_222',
    valorTraslado: '$5.000.000',
    estadoTraslado: 'Con registro',
    id: 1
  }];
  constructor(public dialog: MatDialog,private router: Router) { }

  ngOnInit(): void {
    this.loadTable();
  }

  loadTable(){
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  } 
  verDetalleTraslado(id){
    this.router.navigate(['/gestionarTramiteLiquidacionContractual/detalleTraslado', id]);
  }

}
