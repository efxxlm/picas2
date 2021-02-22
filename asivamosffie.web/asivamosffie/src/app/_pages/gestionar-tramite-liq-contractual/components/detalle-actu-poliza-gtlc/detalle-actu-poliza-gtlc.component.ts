import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-detalle-actu-poliza-gtlc',
  templateUrl: './detalle-actu-poliza-gtlc.component.html',
  styleUrls: ['./detalle-actu-poliza-gtlc.component.scss']
})
export class DetalleActuPolizaGtlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'polizaYSeguros',
    'responsableAprobacion'
  ];
  dataTable: any[] = [
    {
      polizaYSeguros: 'Buen manejo y correcta inversión del anticipo',
      responsableAprobacion: 'Andres Nikolai Montealegre Rojas'
    },
    {
      polizaYSeguros: 'Garantía de estabilidad y calidad de la obra',
      responsableAprobacion: 'Andres Nikolai Montealegre Rojas'
    }
  ]
  constructor() { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
}
