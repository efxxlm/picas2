import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { Location } from '@angular/common';
import { MatTableDataSource } from '@angular/material/table';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-registrar-traslado-gbftrec',
  templateUrl: './registrar-traslado-gbftrec.component.html',
  styleUrls: ['./registrar-traslado-gbftrec.component.scss']
})
export class RegistrarTrasladoGbftrecComponent implements OnInit {

    proyecto: any;
    proyectoId = 0;
    esRegistroNuevo = false;
    esVerDetalle = true;
    numeroOrdenGiro: string;
    numeroTraslado: string;
    dataSource = new MatTableDataSource();
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'drp',
      'numDRP',
      'valor',
      'saldo'
    ];
    dataTable: any[] = [
      {
        drp: '1',
        numDRP: 'IP_00090',
        valor: '$100.000.000',
        saldo: '$100.000.000'
      },
      {
        drp: '2',
        numDRP: 'IP_00123',
        valor: '$5.000.000',
        saldo: '$5.000.000'
      },
    ];

    constructor(
        private activatedRoute: ActivatedRoute,
        private routes: Router,
        private location: Location,
        private balanceSvc: FinancialBalanceService )
    {
        this.getProyectoId()
        this.numeroOrdenGiro = this.activatedRoute.snapshot.params.numeroOrdenGiro
    }

    ngOnInit(): void {
        this.loadDataSource();
    }

    loadDataSource() {
        this.dataSource = new MatTableDataSource(this.dataTable);
        this.dataSource.sort = this.sort;
    }

    async getProyectoId() {
        this.proyectoId = this.activatedRoute.snapshot.params.proyectoId;
        const getDataByProyectoId = await this.balanceSvc.getDataByProyectoId( 557 /* this.proyectoId */ ).toPromise()
        let balanceFinancieroTraslado = []

        if( getDataByProyectoId.length > 0 ){
            this.proyecto = getDataByProyectoId[0]
            console.log( this.proyecto )
            const balanceFinanciero: any = await this.balanceSvc.getBalanceFinanciero( 557 /* this.proyectoId */ ).toPromise()
            balanceFinancieroTraslado = balanceFinanciero.balanceFinancieroTraslado

            const traslado = balanceFinancieroTraslado.find( traslado => traslado.numeroOrdenGiro === this.numeroOrdenGiro )

            if ( traslado !== undefined ) {
                this.numeroTraslado = traslado.numeroTraslado
            }
        }
    }

    getRutaAnterior() {
        this.location.back();
    }

}
