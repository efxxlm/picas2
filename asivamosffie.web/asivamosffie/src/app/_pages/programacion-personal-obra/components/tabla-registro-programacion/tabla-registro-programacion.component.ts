import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProgramacionPersonalObraService } from 'src/app/core/_services/programacionPersonalObra/programacion-personal-obra.service';
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
  displayedColumns: string[]  = [
    'fechaFirmaActaInicio',
    'llaveMen',
    'numeroContrato',
    'tipoIntervencion',
    'institucionEducativaSede',
    'sede',
    'estadoProgramacionInicial',
    'gestion' 
  ];

  constructor ( private dialog: MatDialog,
                private programacionPersonalSvc: ProgramacionPersonalObraService ) {
  }

  ngOnInit(): void {
    this.programacionPersonalSvc.getListProyectos()
      .subscribe(
        response => {
          console.log( response );
          this.tablaRegistro = new MatTableDataSource( response );
          this.tablaRegistro.paginator              = this.paginator;
          this.tablaRegistro.sort                   = this.sort;
          this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        }
      );
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.tablaRegistro.filter = filterValue.trim().toLowerCase();
  };

  openRegistroProgramacion ( contratoConstruccionId: number ) {
    this.dialog.open( DialogRegistroProgramacionComponent, {
      width: '70em',
      data: { contratoConstruccionId }
    });
  };

};