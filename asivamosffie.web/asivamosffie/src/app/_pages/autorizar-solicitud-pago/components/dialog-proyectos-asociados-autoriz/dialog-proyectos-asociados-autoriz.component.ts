import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { DialogProyectosAsociadosComponent } from 'src/app/_pages/registrar-validar-requisitos-pago/components/dialog-proyectos-asociados/dialog-proyectos-asociados.component';

@Component({
  selector: 'app-dialog-proyectos-asociados-autoriz',
  templateUrl: './dialog-proyectos-asociados-autoriz.component.html',
  styleUrls: ['./dialog-proyectos-asociados-autoriz.component.scss']
})
export class DialogProyectosAsociadosAutorizComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'llaveMen',
        'tipoIntervencion',
        'departamento',
        'municipio',
        'institucionEducativa',
        'sede'
    ];
    contrato: any;
    dataTable: any[] = [];

    constructor(
        public matDialogRef: MatDialogRef<DialogProyectosAsociadosComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private registrarPagoSvc: RegistrarRequisitosPagoService )
    { }

    ngOnInit(): void {
        this.registrarPagoSvc.getProyectosByIdContrato( this.data.contrato.contratoId )
            .subscribe(
                response => {
                    this.contrato = response[0];
                    this.dataSource = new MatTableDataSource( response[1] );
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                }
            );
    };

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    };

    close() {
        this.matDialogRef.close('aceptado');
    }

}
