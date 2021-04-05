import { ActualizarPolizasService } from './../../../../core/_services/actualizarPolizas/actualizar-polizas.service';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-actualizar-poliza-rapg',
  templateUrl: './actualizar-poliza-rapg.component.html',
  styleUrls: ['./actualizar-poliza-rapg.component.scss']
})
export class ActualizarPolizaRapgComponent implements OnInit {

    contratoPoliza: any;
    esRegistroNuevo: boolean;
    listaTipoSolicitudContrato: Dominio[] = [];
    dataSource = new MatTableDataSource();
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'polizaYSeguros',
        'responsableAprobacion'
    ];
    dataTable: any[] = [
        {
          polizaYSeguros: 'Buen manejo y correcta inversión del anticipo',
          responsableAprobacion: 'Andres Nikolai Montealegre Rojas'
        },
        {
          polizaYSeguros: 'Garantía de estabilidad y calidad de la obra',
          responsableAprobacion: 'Andres Nikolai Montealegre Rojas'
        }
    ]
    acordeonRegistroCompleto = {
        acordeonTipoActualizacion: false,
        acordeonVigenciaValor: false,
        acordeonObsEspecifica: false,
        acordeonListaChequeo: false,
        acordeonRevisionAprobacion: false
    }

    constructor(
        private activatedRoute: ActivatedRoute,
        private routes: Router,
        private dialog: MatDialog,
        private actualizarPolizaSvc: ActualizarPolizasService,
        private commonSvc: CommonService )
    {
        this.getContratoPoliza();
    }

    ngOnInit(): void {
        this.loadDataSource();
    }

    getContratoPoliza() {
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {

            if ( urlSegment.path === 'actualizarPoliza' ) {
                this.esRegistroNuevo = true;
                return;
            }
            if ( urlSegment.path === 'verDetalleEditarPoliza' ) {
                this.esRegistroNuevo = false;
                return;
            }

        } )
        this.commonSvc.listaTipoSolicitudContrato()
            .subscribe( response => this.listaTipoSolicitudContrato = response );
        this.actualizarPolizaSvc.getContratoPoliza( this.activatedRoute.snapshot.params.id )
            .subscribe(
                response => {
                    this.contratoPoliza = response;
                    console.log( this.contratoPoliza );
                }
            )
    }

    getTipoSolicitudContrato( tipoSolicitudCodigo: string ) {
        if ( this.listaTipoSolicitudContrato.length > 0 ) {
            const solicitud = this.listaTipoSolicitudContrato.find( solicitud => solicitud.codigo === tipoSolicitudCodigo );
            
            if ( solicitud !== undefined ) {
                return solicitud.nombre;
            }
        }
    }
  
    loadDataSource() {
        this.dataSource = new MatTableDataSource(this.dataTable);
        this.dataSource.sort = this.sort;
    }

}
