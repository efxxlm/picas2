import { Component, ViewChild, OnInit } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

const ELEMENT_DATA = [
  {
    id: '1',
    numeroOrdenGiro: 'ODG_Obra_001',
    contratista: 'Construir futuro',
    facturado: '$67.000.000',
    ANSaplicado: '$500.000',
    reteGarantia: '$3.500.000',
    otrosDescuentos: '$2.110.000',
    pagarAntes: '$61.890.000'
  }
];

@Component({
  selector: 'app-tabla-total-ordenes-de-giro',
  templateUrl: './tabla-total-ordenes-de-giro.component.html',
  styleUrls: ['./tabla-total-ordenes-de-giro.component.scss']
})
export class TablaTotalOrdenesDeGiroComponent implements OnInit {

  ELEMENT_DATA: any[];
  displayedColumns: string[] = [
    'numeroOrdenGiro',
    'contratista',
    'facturado',
    'ANSaplicado',
    'reteGarantia',
    'otrosDescuentos',
    'pagarAntes',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatSort) sort: MatSort;
  constructor() { }

  ngOnInit(): void {
  }

}