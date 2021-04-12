import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { CommonService } from '../../../../core/_services/common/common.service';
import { Router } from '@angular/router';
import { estadosPreconstruccion } from '../../../../_interfaces/faseUnoPreconstruccion.interface';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

export interface PeriodicElement {
  id: number;
  fecha: string;
  numContrato: string;
  proyAsociados: number;
  proyConRequisitosAprovados: number;
  proyConRequisitosPendientes: number;
  estado: string;
}

@Component({
  selector: 'app-tabla-registrar-requisitos',
  templateUrl: './tabla-registrar-requisitos.component.html',
  styleUrls: ['./tabla-registrar-requisitos.component.scss']
})
export class TablaRegistrarRequisitosComponent implements OnInit {

  verAyuda = false;
  dataSource = new MatTableDataSource();
  displayedColumns: string[] = [
    'fechaAprobacion',
    'numeroContrato',
    'cantidadProyectosAsociados',
    'cantidadProyectosRequisitosAprobados',
    'cantidadProyectosConPerfilesPendientes',
    'estadoNombre',
    'gestion'
  ];
  estadosPreconstruccion: estadosPreconstruccion;

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService,
    private commonSvc: CommonService,
    private dialog: MatDialog,
    private routes: Router )
  {
    faseUnoPreconstruccionSvc.listaEstadosVerificacionContrato()
      .subscribe(
        response => {
          this.estadosPreconstruccion = response;
          this.faseUnoPreconstruccionSvc.getListContratacion()
            .subscribe( listas => {
              if ( listas.length > 0 ) {
                listas.forEach(
                  registro =>
                  (registro.fechaAprobacion = registro.fechaAprobacion
                    ? registro.fechaAprobacion.split('T')[0].split('-').reverse().join('/')
                    : '')
                    );
              }

              this.dataSource = new MatTableDataSource( listas );
              this.dataSource.sort = this.sort;
              this.dataSource.paginator = this.paginator;
              this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
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
              };
            } );
        }
      );
  }

  ngOnInit(): void {
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  aprobarInicio( contratoId: number ) {
    this.faseUnoPreconstruccionSvc.changeStateContrato( contratoId, this.estadosPreconstruccion.conReqTecnicosAprobados.codigo )
      .subscribe(
        response => {
          this.openDialog( '', response.message );
          this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
            () => this.routes.navigate( [ '/preconstruccion' ] )
          );
        },
        err => this.openDialog( '', err.message )
      );
  }

  getForm( id: number, fechaPoliza: string, estado: string ) {
    this.routes.navigate( [ '/preconstruccion/gestionarRequisitos', id ], { state: { fechaPoliza, estado } } );
  }

}
