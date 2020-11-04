import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DialogRegistroProgramacionComponent } from '../dialog-registro-programacion/dialog-registro-programacion.component';

@Component({
  selector: 'app-tabla-registro-programacion',
  templateUrl: './tabla-registro-programacion.component.html',
  styleUrls: ['./tabla-registro-programacion.component.scss']
})
export class TablaRegistroProgramacionComponent implements OnInit {

  tablaRegistro              = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  dataTable: any[] = [
    {
      fechaFirmaInicio: new Date(),
      llaveMen: 'LJ776554',
      numeroContratoObra: 'C223456789',
      tipoIntervencion: 'Remodelación',
      institucionEducativa: 'I.E. María Villa Campo',
      sede: 'Única sede',
      estadoProgramacion :'Sin programación de personal',
      contratoId: 2
    }
  ];
  displayedColumns: string[]  = [ 
    'fechaFirmaInicio',
    'llaveMen',
    'numeroContratoObra',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'estadoProgramacion',
    'gestion' 
  ];

  constructor ( private dialog: MatDialog ) {
  }

  ngOnInit(): void {
    this.tablaRegistro = new MatTableDataSource( this.dataTable );
    this.tablaRegistro.paginator              = this.paginator;
    this.tablaRegistro.sort                   = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.tablaRegistro.filter = filterValue.trim().toLowerCase();
  };

  openRegistroProgramacion ( ) {
    this.dialog.open( DialogRegistroProgramacionComponent, {
      width: '70em'
    });
  };

};