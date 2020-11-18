import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ProgramacionPersonalObraService } from 'src/app/core/_services/programacionPersonalObra/programacion-personal-obra.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadosProgramacion } from 'src/app/_interfaces/programacionPersonal.interface';
import { DialogRegistroProgramacionComponent } from '../dialog-registro-programacion/dialog-registro-programacion.component';

@Component({
  selector: 'app-tabla-registro-programacion',
  templateUrl: './tabla-registro-programacion.component.html',
  styleUrls: ['./tabla-registro-programacion.component.scss']
})
export class TablaRegistroProgramacionComponent implements OnInit {

  tablaRegistro              = new MatTableDataSource();
  estadosProgramacion: EstadosProgramacion = {
    sinProgramacionPersonal: '0',
    enRegistroProgramacion: '1',
    sinAprobacionProgramacionPersonal: '2',
    conAprobacionProgramacionPersonal: '3'
  };
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
                private programacionPersonalSvc: ProgramacionPersonalObraService,
                private routes: Router ) {
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

  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  openRegistroProgramacion ( contrato: any ) {
    const dialogProgramacion = this.dialog.open( DialogRegistroProgramacionComponent, {
      width: '80em',
      data: { contrato }
    });

    dialogProgramacion.afterClosed()
      .subscribe( value => {
        if ( value === true ) {
          this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
            () => this.routes.navigate( [ '/registrarProgramacionPersonalObra' ] )
          );
        }
      } )
  };

  aprobarProgramacion ( contratoConstruccionId: number ) {
    this.programacionPersonalSvc.changeStatusProgramacionContratoPersonal( contratoConstruccionId, this.estadosProgramacion.conAprobacionProgramacionPersonal )
      .subscribe(
        response => {
          this.openDialog( '', response.message );
          this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
            () => this.routes.navigate( [ '/registrarProgramacionPersonalObra' ] )
          );
        },
        err => this.openDialog( '', err.message )
      )
  };

};