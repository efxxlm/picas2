import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { DialogDevolverSolicitudComponent } from '../dialog-devolver-solicitud/dialog-devolver-solicitud.component';

@Component({
  selector: 'app-registrar-validar-requisitos-pago',
  templateUrl: './registrar-validar-requisitos-pago.component.html',
  styleUrls: ['./registrar-validar-requisitos-pago.component.scss']
})
export class RegistrarValidarRequisitosPagoComponent implements OnInit {

    verAyuda = false;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'fechaSolicitud',
        'numeroSolicitud',
        'modalidadContrato',
        'numeroContrato',
        'estadoRegistroPago',
        'gestion'
    ];

    constructor(
        private router: Router,
        private dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService, )
    {
        this.registrarPagosSvc.getListSolicitudPago()
            .subscribe(
                response => {
                    console.log( response );
                    this.dataSource = new MatTableDataSource( response );
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                }
            );
    }

    ngOnInit(): void {
    };

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    };

    registrarNuevaSolicitud(){
        this.router.navigate(['registrarValidarRequisitosPago/registrarNuevaSolicitudPago'])
    }

    devolverSolictud(){
        const dialogConfig = new MatDialogConfig();
        dialogConfig.height = 'auto';
        dialogConfig.width = '865px';
        //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
        const dialogRef = this.dialog.open(DialogDevolverSolicitudComponent, dialogConfig);
        //dialogRef.afterClosed().subscribe(value => {});
    }

}
