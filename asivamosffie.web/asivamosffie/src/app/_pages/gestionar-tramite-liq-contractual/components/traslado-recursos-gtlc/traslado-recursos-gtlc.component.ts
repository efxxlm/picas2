import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import moment from 'moment';

@Component({
  selector: 'app-traslado-recursos-gtlc',
  templateUrl: './traslado-recursos-gtlc.component.html',
  styleUrls: ['./traslado-recursos-gtlc.component.scss']
})
export class TrasladoRecursosGtlcComponent implements OnInit {
  proyectoId = 0;
  proyecto: any;
  balanceFinanciero: any;
  listaEstadoTraslado: Dominio[] = [];
  dataSource = new MatTableDataSource();
  urlDetalle: string;
  displayedColumns: string[] = [
    'fechaTraslado',
    'numTraslado',
    'numContrato',
    'numOrdenGiro',
    'valorTraslado',
    'estadoTraslado',
    'gestion'
  ];

  constructor(
      private activatedRoute: ActivatedRoute,
      private routes: Router,
      private balanceSvc: FinancialBalanceService,
      private commonSvc: CommonService )
  {
      this.proyectoId = this.activatedRoute.snapshot.params.proyectoId
      this.urlDetalle = `${ this.routes.url }/verDetalle`
  }

  async ngOnInit() {
      this.listaEstadoTraslado = await this.commonSvc.listaEstadoTraslado().toPromise()
      const getDataByProyectoId = await this.balanceSvc.getDataByProyectoId(this.proyectoId).toPromise()
      let balanceFinancieroTraslado = []

      if( getDataByProyectoId.length > 0 ){
          this.proyecto = getDataByProyectoId[0]
          this.balanceFinanciero = await this.balanceSvc.getBalanceFinanciero(this.proyectoId).toPromise()
          balanceFinancieroTraslado = this.balanceFinanciero.balanceFinancieroTraslado
      }

      balanceFinancieroTraslado.forEach( registro => registro.fechaCreacion = moment( registro.fechaCreacion ).format( 'DD/MM/YYYY' ) )
      this.dataSource = new MatTableDataSource( balanceFinancieroTraslado )
  }

  getEstadoTraslado( codigo: string ) {
      if ( this.listaEstadoTraslado.length > 0 ) {
          const traslado = this.listaEstadoTraslado.find( traslado => traslado.codigo === codigo )

          if ( traslado !== undefined ) {
              return traslado.nombre
          }
      }
  }

}
