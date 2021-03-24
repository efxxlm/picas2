import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro, TipoSolicitud, TipoSolicitudes } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { DialogRechazarSolicitudValidfspComponent } from '../dialog-rechazar-solicitud-validfsp/dialog-rechazar-solicitud-validfsp.component';

@Component({
  selector: 'app-validar-financ-solicitud-pago',
  templateUrl: './validar-financ-solicitud-pago.component.html',
  styleUrls: ['./validar-financ-solicitud-pago.component.scss']
})
export class ValidarFinancSolicitudPagoComponent implements OnInit {

    verAyuda = false;
    tipoSolicitud: TipoSolicitud = TipoSolicitudes;
    listaEstadoSolicitudPago: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'fechaSolicitud',
      'numeroSolicitud',
      'modalidadContrato',
      'numeroContrato',
      'estadoValidacion',
      'estadoRegistro',
      'gestion'
    ];

    constructor(
        private router: Router,
        private dialog: MatDialog,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService )
    {
        this.obsMultipleSvc.listaMenu()
            .subscribe(
                listaMenuId => {
                    this.obsMultipleSvc.getListSolicitudPago( listaMenuId.validarFinancieramenteId )
                        .subscribe(
                            getListSolicitudPago => {
                                console.log( getListSolicitudPago );
                                this.dataSource = new MatTableDataSource(getListSolicitudPago);
                                this.dataSource.paginator = this.paginator;
                                this.dataSource.sort = this.sort;
                                this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                            }
                        );
                }
            );
    }

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource();
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    openRechazo(){
      const dialogConfig = new MatDialogConfig();
      dialogConfig.height = 'auto';
      dialogConfig.width = '1020px';
      //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
      const dialogRef = this.dialog.open(DialogRechazarSolicitudValidfspComponent, dialogConfig);
      //dialogRef.afterClosed().subscribe(value => {});
    }

}
