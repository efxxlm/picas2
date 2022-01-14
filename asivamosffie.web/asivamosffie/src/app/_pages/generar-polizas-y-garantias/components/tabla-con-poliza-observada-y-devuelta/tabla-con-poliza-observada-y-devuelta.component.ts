import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';
import { Subscription } from 'rxjs';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { EstadoPolizaCodigo } from 'src/app/_interfaces/gestionar-polizas-garantias.interface';


@Component({
  selector: 'app-tabla-con-poliza-observada-y-devuelta',
  templateUrl: './tabla-con-poliza-observada-y-devuelta.component.html',
  styleUrls: ['./tabla-con-poliza-observada-y-devuelta.component.scss']
})
export class TablaConPolizaObservadaYDevueltaComponent implements OnInit {

    @Output() estadoSemaforo2 = new EventEmitter<string>();
    estadoPolizaCodigo = EstadoPolizaCodigo;
    displayedColumns: string[] = ['fechaFirma', 'numeroContrato', 'tipoSolicitud', 'estadoPoliza', 'contratoId'];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    dataTable: any[] = [];

    constructor( private polizaSvc: PolizaGarantiaService ) {
        this.polizaSvc.getListGrillaContratoGarantiaPoliza( this.estadoPolizaCodigo.polizaDevuelta , false)
            .subscribe(
                getListGrillaContratoGarantiaPoliza => {
                    let totalIncompleto = 0;
                    let totalCompleto = 0;

                    if ( getListGrillaContratoGarantiaPoliza.length === 0 ) {
                        this.estadoSemaforo2.emit( 'completo' );
                        return;
                    }

                    getListGrillaContratoGarantiaPoliza.forEach( registro => {
                        registro.fechaFirma = registro.fechaFirma !== undefined ? ( moment( registro.fechaFirma ).format( 'DD/MM/YYYY' ) ) : '';

                        if ( registro.registroCompletoPoliza === true ) {
                            totalCompleto++;
                        }
                        if ( registro.registroCompletoPoliza !== true ) {
                            totalIncompleto++;
                        }
                    } );

                    if ( totalIncompleto > 0 && totalIncompleto === getListGrillaContratoGarantiaPoliza.length ) {
                        this.estadoSemaforo2.emit( 'sin-diligenciar' );
                    }
                    if ( totalIncompleto > 0 && totalIncompleto < getListGrillaContratoGarantiaPoliza.length ) {
                        this.estadoSemaforo2.emit( 'en-proceso' );
                    }
                    if ( totalCompleto > 0 && totalCompleto < getListGrillaContratoGarantiaPoliza.length ) {
                        this.estadoSemaforo2.emit( 'en-proceso' );
                    }
                    if ( totalCompleto > 0 && totalCompleto === getListGrillaContratoGarantiaPoliza.length ) {
                        this.estadoSemaforo2.emit( 'completo' );
                    }

                    this.dataSource = new MatTableDataSource( getListGrillaContratoGarantiaPoliza );
                    this.dataSource.sort = this.sort;
                    this.dataSource.paginator = this.paginator;
                    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
                }
            );
    }

    ngOnInit(): void {
    }

    applyFilter ( event: Event ) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    };

}
