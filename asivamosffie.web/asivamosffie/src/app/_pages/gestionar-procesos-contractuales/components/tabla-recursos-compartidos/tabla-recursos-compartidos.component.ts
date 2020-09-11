import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-recursos-compartidos',
  templateUrl: './tabla-recursos-compartidos.component.html',
  styleUrls: ['./tabla-recursos-compartidos.component.scss']
})
export class TablaRecursosCompartidosComponent implements OnInit {

  dataSource                 = new MatTableDataSource();
  displayedColumns: string[] = [ 'nombre', 'valorAportante', 'valorModificacion', 'valorDespuesModificacion' ];
  displayedColumnsFooter: string[] = [ 'nombre', 'valorAportante', 'valorModificacion', 'total' ];
  ELEMENT_DATA    : any[]    = [
    { titulo: 'Valor aportante', name: 'valorAportante' },
    { titulo: 'Valor de la modificación', name: 'valorModificacion' }
  ];
  data: any[] = [
    {
      nombre                  : 'FFIE',
      valorAportante          : '50.000.000',
      valorModificacion       : '76.000.000',
      valorDespuesModificacion: '126.000.000'
    },
    {
      nombre                  : 'Gobernación del Valle Del Cauca',
      valorAportante          : '50.000.000',
      valorModificacion       : '70.000.000',
      valorDespuesModificacion: '100.000.000'
    },
    {
      nombre                  : 'Fundación Pies Descalzos',
      valorAportante          : '0',
      valorModificacion       : '100.000.000',
      valorDespuesModificacion: '100.000.000'
    },
  ]

  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.data )
  }

}
