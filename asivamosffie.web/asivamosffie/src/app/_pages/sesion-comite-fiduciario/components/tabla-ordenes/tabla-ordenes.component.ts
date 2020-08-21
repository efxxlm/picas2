import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ColumnasTabla } from 'src/app/_interfaces/comiteFiduciario.interfaces';
import { ComiteFiduciario } from '../../../../_interfaces/comiteFiduciario.interfaces';

@Component({
  selector: 'app-tabla-ordenes',
  templateUrl: './tabla-ordenes.component.html',
  styleUrls: ['./tabla-ordenes.component.scss']
})
export class TablaOrdenesComponent implements OnInit {

  dataSource = new MatTableDataSource();
  //Decoradores ViewChild para controlar "MatPaginator" y "MatSort" del componente
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true }) sort: MatSort;
  //Inputs para recibir la data de la tabla
  @Input() displayedColumns: string[] = [];
  @Input() dataOrden: ComiteFiduciario[];
  @Input() columnas: ColumnasTabla[] = [];

  constructor( private routes: Router ) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.dataOrden );
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  applyFilter ( event ) {
    console.log( event );
  };

  convocarComite( element ) {
    console.log( element );
  };

  editOrden ( sesion: ComiteFiduciario ) {
    this.routes.navigate( ['/comiteFiduciario/editarOrden'], { state: { sesion } } );
  };

  deleteComite( comite ) {
    console.log( comite );
  }

};