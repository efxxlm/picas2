import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-accordion-info-general-gog',
  templateUrl: './accordion-info-general-gog.component.html',
  styleUrls: ['./accordion-info-general-gog.component.scss']
})
export class AccordionInfoGeneralGogComponent implements OnInit {

    @Input() solicitudPago: any;
    listaTipoSolicitudContrato: Dominio[] = [];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'drp',
      'numDrp',
      'valor',
      'saldo'
    ];

    constructor( private commonSvc: CommonService )
    {
        this.commonSvc.listaTipoSolicitudContrato()
            .subscribe( response => this.listaTipoSolicitudContrato = response );
    }

    ngOnInit(): void {
      this.dataSource = new MatTableDataSource( this.solicitudPago.contratoSon.contratacion.disponibilidadPresupuestal );
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    };

    getTipoSolicitudContrato( tipoSolicitudCodigo: string ) {
        if ( this.listaTipoSolicitudContrato.length > 0 ) {
            const solicitud = this.listaTipoSolicitudContrato.filter( solicitud => solicitud.codigo === tipoSolicitudCodigo );
            return solicitud[0].nombre;
        }
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    };

}
