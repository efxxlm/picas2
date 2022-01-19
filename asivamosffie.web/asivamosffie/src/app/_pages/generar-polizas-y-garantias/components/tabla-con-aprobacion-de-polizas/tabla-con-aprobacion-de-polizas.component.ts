import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';
import { Subscription } from 'rxjs';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { EstadoPolizaCodigo } from 'src/app/_interfaces/gestionar-polizas-garantias.interface';

@Component({
  selector: 'app-tabla-con-aprobacion-de-polizas',
  templateUrl: './tabla-con-aprobacion-de-polizas.component.html',
  styleUrls: ['./tabla-con-aprobacion-de-polizas.component.scss']
})
export class TablaConAprobacionDePolizasComponent implements OnInit {

    @Output() estadoSemaforo3 = new EventEmitter<string>();
    @Input() esCancelada: boolean;
    estadoPolizaCodigo = EstadoPolizaCodigo;
    displayedColumns: string[] = ['fechaFirma', 'numeroContrato', 'tipoSolicitud', 'estadoPoliza', 'contratoId'];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    dataTable: any[] = [];

    constructor( private polizaSvc: PolizaGarantiaService) {

    }

    ngOnInit(): void {
      this.polizaSvc.getListGrillaContratoGarantiaPoliza( this.estadoPolizaCodigo.conAprobacion , this.esCancelada)
      .subscribe(
          getListGrillaContratoGarantiaPoliza => {
              this.estadoSemaforo3.emit( 'completo' );

              getListGrillaContratoGarantiaPoliza.forEach( registro => registro.fechaFirma = registro.fechaFirma !== undefined ? ( moment( registro.fechaFirma ).format( 'DD/MM/YYYY' ) ) : '' );

              this.dataSource = new MatTableDataSource( getListGrillaContratoGarantiaPoliza );
              this.dataSource.sort = this.sort;
              this.dataSource.paginator = this.paginator;
              this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
          }
      );
    }

    applyFilter ( event: Event ) {
        const filterValue      = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
