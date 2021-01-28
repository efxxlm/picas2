import { MatTableDataSource } from '@angular/material/table';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-tabla-avance-financiero',
  templateUrl: './tabla-avance-financiero.component.html',
  styleUrls: ['./tabla-avance-financiero.component.scss']
})
export class TablaAvanceFinancieroComponent implements OnInit {

  tablaAvanceFinanciero = new MatTableDataSource();
  displayedColumns: string[] = [
    'item',
    'valor',
    'porcentaje'
  ];
  dataTable: any[] = [
    {
      item: 'Facturación programada en el mes',
      valor: '$18.000.000',
      porcentaje: '16%'
    },
    {
      item: 'Facturación del mes',
      valor: '$18.000.000',
      porcentaje: '16%'
    },
    {
      item: 'Facturación programada acumulada',
      valor: '$18.000.000',
      porcentaje: '16%'
    }
  ];

  constructor() { }

  ngOnInit(): void {
    this.tablaAvanceFinanciero = new MatTableDataSource( this.dataTable );
  }

}
