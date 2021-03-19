import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-fuentes-usos-gtlc',
  templateUrl: './tabla-fuentes-usos-gtlc.component.html',
  styleUrls: ['./tabla-fuentes-usos-gtlc.component.scss']
})
export class TablaFuentesUsosGtlcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'aportante',
    'valorAportante',
    'fuenteAportante',
    'uso',
    'valorUso'
  ];
  tablaEjemplo: any[] = [
    {
      aportante: 'Alcaldía de Susacón',
      valorAportante: '$45.000.000',
      fuenteAportante: [
        {
          fuente: 'Recursos propios',
          uso: [
            { nombre: 'Diseño' }
          ],
          valorUso: [
            { valor: '$12.000.000' }
          ],
        },
        {
          fuente: 'Contingencias',
          uso: [
            { nombre: 'Diseño' },
            { nombre: 'Obra principal' }
          ],
          valorUso: [
            { valor: '$12.000.000' },
            { valor: '$21.000.000' }
          ],
        }
      ],
    },
    {
      aportante: 'Gobernación de Boyacá',
      valorAportante: '$40.000.000',
      fuenteAportante: [
        {
          fuente: 'Recursos propios',
          uso: [
            { nombre: 'Obra principal' }
          ],
          valorUso: [
            { valor: '$40.000.000' },
          ],
        }
      ],
    },
    {
      aportante: 'FFIE',
      valorAportante: '$20.000.000',
      fuenteAportante: [
        {
          fuente: 'Contingencias',
          uso: [
            { nombre: 'Obra principal' }
          ],
          valorUso: [
            { valor: '$20.000.000' },
          ],
        }
      ],
    }
    /*
    {
      aportante: 'Gobernación de Boyacá',
      valorAportante: '$40.000.000',
      fuenteAportante: [
        { fuente: 'Recursos propios' },
      ],
      uso: [
        { nombre: 'Obra principal' },
      ],
      valorUso: [
        { valor: '$40.000.000' },
      ],
    },
    {
      aportante: 'FFIE',
      valorAportante: '$20.000.000',
      fuenteAportante: [
        { fuente: 'Contingencias' },
      ],
      uso: [
        { nombre: 'Obra principal' },
      ],
      valorUso: [
        { valor: '$20.000.000' },
      ],
    },
    */
  ];
  constructor() { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.tablaEjemplo);
    this.dataSource.sort = this.sort;
    //this.dataSource.paginator = this.paginator;
    //this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    /*
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
    };*/
  }


}
