import { ActualizarPolizasService } from './../../../../core/_services/actualizarPolizas/actualizar-polizas.service';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-actualizar-poliza-rapg',
  templateUrl: './actualizar-poliza-rapg.component.html',
  styleUrls: ['./actualizar-poliza-rapg.component.scss']
})
export class ActualizarPolizaRapgComponent implements OnInit {

    esRegistroNuevo: boolean;
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
        private actualizarPolizaSvc: ActualizarPolizasService )
    {
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
        this.getContratoPoliza();
    }

  ngOnInit(): void {
    this.loadDataSource();
  }

  getContratoPoliza() {
    this.actualizarPolizaSvc.getContratoPoliza( this.activatedRoute.snapshot.params.id )
        .subscribe(
            response => {
                console.log( response );
            }
        )
  }
  
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }

}
