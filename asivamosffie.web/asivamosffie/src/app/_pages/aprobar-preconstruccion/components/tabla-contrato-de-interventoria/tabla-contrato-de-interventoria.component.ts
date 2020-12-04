import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { FaseUnoVerificarPreconstruccionService } from 'src/app/core/_services/faseUnoVerificarPreconstruccion/fase-uno-verificar-preconstruccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { estadosPreconstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { FaseUnoAprobarPreconstruccionService } from '../../../../core/_services/faseUnoAprobarPreconstruccion/fase-uno-aprobar-preconstruccion.service';

@Component({
  selector: 'app-tabla-contrato-de-interventoria',
  templateUrl: './tabla-contrato-de-interventoria.component.html',
  styleUrls: ['./tabla-contrato-de-interventoria.component.scss']
})
export class TablaContratoDeInterventoriaComponent implements OnInit {

  displayedColumns: string[] = [
    'fechaAprobacion',
    'numeroContrato',
    'cantidadProyectosAsociados',
    'cantidadProyectosRequisitosAprobados',
    'cantidadProyectosRequisitosPendientes',
    'estadoNombre',
    'gestion'
  ];
  tipoSolicitudCodigoInterventoria = '2';
  estadosPreconstruccionInterventoria: estadosPreconstruccion;
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    private faseUnoVerificarPreConstruccionSvc: FaseUnoVerificarPreconstruccionService,
    private faseUnoAprobarPreconstruccionSvc: FaseUnoAprobarPreconstruccionService,
    private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService,
    private dialog: MatDialog,
    private routes: Router )
  {
    this.faseUnoAprobarPreconstruccionSvc.listaEstadosAprobarContrato( 'interventoria' )
      .subscribe(
        response => {
          this.estadosPreconstruccionInterventoria = response;
          this.faseUnoAprobarPreconstruccionSvc.getListContratacion()
          .subscribe( listas => {
            const dataTable = [];
            listas.forEach( lista => {
              if (  Number( lista[ 'estadoCodigo' ] ) >= Number( this.estadosPreconstruccionInterventoria.enviadoAlSupervisor.codigo )
                    && lista[ 'tipoSolicitudCodigo' ] === this.tipoSolicitudCodigoInterventoria )
              {
                dataTable.push( lista );
              }
              if (  lista[ 'estaDevuelto' ] === true
                    && Number( lista[ 'estadoCodigo' ] ) < Number( this.estadosPreconstruccionInterventoria.enviadoAlSupervisor.codigo )
                    && lista[ 'tipoSolicitudCodigo' ] === this.tipoSolicitudCodigoInterventoria )
              {
                dataTable.push( lista );
              }
            } );
            this.dataSource = new MatTableDataSource( dataTable );
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

  openDialog( modalTitle: string, modalText: string ) {
    const dialogRef = this.dialog.open( ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  aprobarInicio( contratoId: number ) {
    this.faseUnoPreconstruccionSvc.changeStateContrato(
      contratoId, this.estadosPreconstruccionInterventoria.conReqTecnicosAprobadosPorSupervisor.codigo
    )
    .subscribe(
      response => {
        this.openDialog( '', response.message );
        this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
          () => this.routes.navigate( [ '/aprobarPreconstruccion' ] )
        );
      },
      err => this.openDialog( '', err.message )
    );
  }

  enviarAlApoyo( contratoId: number ) {
    this.faseUnoPreconstruccionSvc.changeStateContrato( contratoId, this.estadosPreconstruccionInterventoria.enviadoAlApoyo.codigo )
    .subscribe(
      response => {
        this.openDialog( '', response.message );
        this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
          () => this.routes.navigate( [ '/aprobarPreconstruccion' ] )
        );
      },
      err => this.openDialog( '', err.message )
    );
  }

}
