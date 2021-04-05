import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-tabla-lista-contratos',
  templateUrl: './tabla-lista-contratos.component.html',
  styleUrls: ['./tabla-lista-contratos.component.scss']
})
export class TablaListaContratosComponent implements OnInit {

    @Input() listContrato: any[];
    listaTipoSolicitudContrato: Dominio[] = [];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'numContrato',
      'tipoContrato',
      'contratista',
      'gestion'
    ];

    constructor( private commonSvc: CommonService ) {
        this.commonSvc.listaTipoSolicitudContrato()
            .subscribe( response => this.listaTipoSolicitudContrato = response );
    }

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource( this.listContrato );
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    getTipoSolicitudContrato( tipoSolicitudCodigo: string ) {
        if ( this.listaTipoSolicitudContrato.length > 0 ) {
            const solicitud = this.listaTipoSolicitudContrato.find( solicitud => solicitud.codigo === tipoSolicitudCodigo );
            
            if ( solicitud !== undefined ) {
                return solicitud.nombre;
            }
        }
    }

}
