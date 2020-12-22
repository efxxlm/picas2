import { DialogCargarActaComponent } from './../dialog-cargar-acta/dialog-cargar-acta.component';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-tabla-registrar-avance-semanal',
  templateUrl: './tabla-registrar-avance-semanal.component.html',
  styleUrls: ['./tabla-registrar-avance-semanal.component.scss']
})
export class TablaRegistrarAvanceSemanalComponent implements OnInit {

    tablaRegistro              = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[]  = [
      'llaveMen',
      'numeroContrato',
      'tipoIntervencion',
      'institucionEducativa',
      'sede',
      'fechaUltimoReporte',
      'estadoObra',
      'gestion'
    ];

    constructor(
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private dialog: MatDialog )
    {
      this.avanceSemanalSvc.getVRegistrarAvanceSemanal()
        .subscribe(
          listas => {
            console.log( listas );
            this.tablaRegistro = new MatTableDataSource( listas );
            this.tablaRegistro.sort = this.sort;
            this.tablaRegistro.paginator = this.paginator;
            this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
          }
          );
    }

    ngOnInit(): void {
    }

    applyFilter( event: Event ) {
      const filterValue      = (event.target as HTMLInputElement).value;
      this.tablaRegistro.filter = filterValue.trim().toLowerCase();
    }

    openDialogCargarActa( registro: any ) {
        this.dialog.open( DialogCargarActaComponent, {
          width: '70em',
          data : { registro }
        });
    }

}
