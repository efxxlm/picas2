import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, UrlSegment, Router } from '@angular/router';
import { Location } from '@angular/common';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-validar-balance-gbftrec',
  templateUrl: './validar-balance-gbftrec.component.html',
  styleUrls: ['./validar-balance-gbftrec.component.scss']
})
export class ValidarBalanceGbftrecComponent implements OnInit {

    proyectoId: number;
    opcion1 = false;
    opcion2 = false;
    opcion3 = false;
    opcion4 = false;
    data : any;
    esRegistroNuevo: boolean;
    esVerDetalle: boolean;
    cumpleCondicionesTai: boolean = false;

    constructor(
        private activatedRoute: ActivatedRoute,
        private location: Location,
        private routes: Router,
        private financialBalanceService: FinancialBalanceService )
    {
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
            if ( urlSegment.path === 'validarBalance' ) {
                this.esVerDetalle = false;
                this.esRegistroNuevo = true;
                return;
            }
            if ( urlSegment.path === 'verDetalleEditarBalance' ) {
                this.esVerDetalle = false;
                this.esRegistroNuevo = false;
                return;
            }
            if ( urlSegment.path === 'verDetalleBalance' ) {
                this.esVerDetalle = true;
                return;
            }
        } );

        if( this.activatedRoute.snapshot.params.id !== undefined ){
            this.proyectoId = this.activatedRoute.snapshot.params.id;

            this.financialBalanceService.getDataByProyectoId( this.activatedRoute.snapshot.params.id )
                .subscribe( getDataByProyectoId => {
                    if( getDataByProyectoId.length > 0 ){
                        this.data = getDataByProyectoId[0];
                        this.cumpleCondicionesTai = this.data?.cumpleCondicionesTai;
                    }
                } );
        }
    }

    ngOnInit(): void {
    }

    getRutaAnterior() {
        this.location.back();
    }

}
