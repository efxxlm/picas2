import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';
import { Subscription } from 'rxjs';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { EstadoPolizaCodigo } from 'src/app/_interfaces/gestionar-polizas-garantias.interface';

@Component({
  selector: 'app-tabla-sin-radicacion-de-polizas',
  templateUrl: './tabla-sin-radicacion-de-polizas.component.html',
  styleUrls: ['./tabla-sin-radicacion-de-polizas.component.scss']
})
export class TablaSinRadicacionDePolizasComponent implements OnInit {

    @Output() estadoSemaforo = new EventEmitter<string>();
    estadoPolizaCodigo = EstadoPolizaCodigo;
    displayedColumns: string[] = ['fechaFirma', 'numeroContrato', 'tipoSolicitud', 'estadoPoliza', 'contratoId'];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    dataTable: any[] = [];

    constructor( private polizaSvc: PolizaGarantiaService ) {
        this.polizaSvc.getListGrillaContratoGarantiaPoliza( this.estadoPolizaCodigo.sinRadicacion , false)
        .subscribe(
            getListGrillaContratoGarantiaPoliza => {
                if ( getListGrillaContratoGarantiaPoliza.length === 0 ) {
                    this.estadoSemaforo.emit( 'completo' );
                    return;
                } else {
                    this.estadoSemaforo.emit( 'sin-diligenciar' );
                }

                getListGrillaContratoGarantiaPoliza.forEach( registro => registro.fechaFirma = registro.fechaFirma !== undefined ? ( moment( registro.fechaFirma ).format( 'DD/MM/YYYY' ) ) : '' );

                this.dataSource = new MatTableDataSource( getListGrillaContratoGarantiaPoliza );
                this.dataSource.sort = this.sort;
                this.dataSource.paginator = this.paginator;
                this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
            }
        );
    }

    ngOnInit(): void {
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    };

}
