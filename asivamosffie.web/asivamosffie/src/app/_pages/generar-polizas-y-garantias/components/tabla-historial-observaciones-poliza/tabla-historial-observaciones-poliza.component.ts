import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { EstadosRevision } from 'src/app/_interfaces/estados-actualizacion-polizas.interface';

@Component({
  selector: 'app-tabla-historial-observaciones-poliza',
  templateUrl: './tabla-historial-observaciones-poliza.component.html',
  styleUrls: ['./tabla-historial-observaciones-poliza.component.scss']
})
export class TablaHistorialObservacionesPolizaComponent implements OnInit {

    @Input() polizaObservacion: any[];
    estadosRevision = EstadosRevision;
    displayedColumns: string[] = ['fechaRevision', 'observacion', 'estadoRevisionCodigo'];
    dataSource = new MatTableDataSource();
    @ViewChild( MatPaginator, {static: true} ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;

    constructor() { }

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource(this.polizaObservacion);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter( event: Event ) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
