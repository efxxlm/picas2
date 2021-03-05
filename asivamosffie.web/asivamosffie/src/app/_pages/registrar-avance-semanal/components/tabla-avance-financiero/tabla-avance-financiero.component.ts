import { MatTableDataSource } from '@angular/material/table';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-tabla-avance-financiero',
  templateUrl: './tabla-avance-financiero.component.html',
  styleUrls: ['./tabla-avance-financiero.component.scss']
})
export class TablaAvanceFinancieroComponent implements OnInit {

  @Input() tablaFinanciera: any[];
  tablaAvanceFinanciero = new MatTableDataSource();
  displayedColumns: string[] = [
    'item',
    'valor',
    'porcentaje'
  ];

  constructor() { }

  ngOnInit(): void {
    this.tablaAvanceFinanciero = new MatTableDataSource( this.tablaFinanciera );
  }

}
