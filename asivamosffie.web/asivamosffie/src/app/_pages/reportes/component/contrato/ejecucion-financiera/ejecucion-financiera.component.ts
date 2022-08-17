import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { forkJoin, Subscription } from 'rxjs';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { FichaContratoService } from 'src/app/core/_services/fichaContrato/ficha-contrato.service';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
    selector: 'app-ejecucion-financiera',
    templateUrl: './ejecucion-financiera.component.html',
    styleUrls: ['./ejecucion-financiera.component.scss']
})
export class EjecucionFinancieraComponent implements OnInit, OnDestroy {
    listaModificaciones = [
        {
            numeroActualizacion: 'ACT-DDP 0001',
            tipoModificacion: 'Adición',
            valorAdicional: '$5.000.000',
            numeroComite: 'CT_0060',
            fechaComite: '20/11/2020',
            urlSoporte: 'http://AdicionDDP_0001.onedrive.com'
        }
    ];

    listaDRP = [
        {
            drp: '1',
            numero: 'IP_00090',
            Valor: '$100.000.000'
        }
    ];

    pContratoId: string;
    contratacionId = 0
    openAcordeon = false;
    contrato: any;
    dataTableEjpresupuestal: any[] = [];
    totalEjpresupuestal = {
        totalComprometido: 0,
        facturadoAntesImpuestos: 0,
        saldo: 0,
        porcentajeEjecucionPresupuestal: 0
    };
    dataTableEjfinanciera: any[] = [];
    totalEjfinanciera = {
        totalComprometido: 0,
        ordenadoGirarAntesImpuestos: 0,
        saldo: 0,
        porcentajeEjecucionFinanciera: 0
    };
    contratacion!: any;
    ejecucionFinanciera!: any;
    subscription: Subscription = new Subscription()

    constructor(
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private financialBalanceService: FinancialBalanceService,
        private fichaContratoService: FichaContratoService,
        private _contratosModificacionesSvc: ContratosModificacionesContractualesService )
    {
        this._activatedRoute.params.subscribe((params: Params) => (this.pContratoId = params.id))

        this.getInitialData()
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe()
    }

    ngOnInit(): void {
    }

    getInitialData() {
        if ( this._activatedRoute.snapshot.queryParamMap.get( 'contratacionId' ) ) this.contratacionId = Number( this._activatedRoute.snapshot.queryParamMap.get( 'contratacionId' ) )
        if ( !this._activatedRoute.snapshot.queryParamMap.get( 'contratacionId' ) ) {
            this._router.navigate( [ 'reportes/fichaContratosProyectos' ] )
            return
        }

        const subscriber: any = {
            ejecucionFinanciera: this.financialBalanceService.getEjecucionFinancieraXProyectoId(this.pContratoId),
        }

        if ( this.contratacionId > 0 ) {
            subscriber.contratacion = this._contratosModificacionesSvc.getContratacionId( this.contratacionId )
        }

        this.subscription.add(
            forkJoin(
                subscriber
            )
            .subscribe(
                (response: any) => {
                    this.contratacion = response.contratacion
                    this.ejecucionFinanciera = response.ejecucionFinanciera
                    console.log( this.contratacion, this.ejecucionFinanciera )
    
                    this.ejecucionFinanciera[0].forEach(element => {
                        this.dataTableEjpresupuestal.push({
                            facturadoAntesImpuestos: element.facturadoAntesImpuestos,
                            nombre: element.nombre,
                            porcentajeEjecucionPresupuestal: element.porcentajeEjecucionPresupuestal,
                            proyectoId: element.proyectoId,
                            saldo: element.saldo,
                            tipoSolicitudCodigo: element.tipoSolicitudCodigo,
                            totalComprometido: element.totalComprometido
                        });
                    });
                    this.ejecucionFinanciera[1].forEach(element => {
                        this.dataTableEjfinanciera.push({
                            nombre: element.nombre,
                            ordenadoGirarAntesImpuestos: element.ordenadoGirarAntesImpuestos,
                            porcentajeEjecucionFinanciera: element.porcentajeEjecucionFinanciera,
                            proyectoId: element.proyectoId,
                            descuento: element.descuento,
                            saldo: element.saldo,
                            totalComprometido: element.totalComprometido
                        });
                    });
        
                    this.totalPresupuestal();
                    this.totalFinanciera();
                }
            )
        )
    }

    totalPresupuestal() {
        this.totalEjpresupuestal = {
            totalComprometido: 0,
            facturadoAntesImpuestos: 0,
            saldo: 0,
            porcentajeEjecucionPresupuestal: 0
        };
        this.dataTableEjpresupuestal.forEach(element => {
            this.totalEjpresupuestal.totalComprometido += element.totalComprometido;
            this.totalEjpresupuestal.facturadoAntesImpuestos =
                this.totalEjpresupuestal.facturadoAntesImpuestos + element.facturadoAntesImpuestos;
            this.totalEjpresupuestal.saldo = this.totalEjpresupuestal.saldo + element.saldo;
            this.totalEjpresupuestal.porcentajeEjecucionPresupuestal =
                this.totalEjpresupuestal.porcentajeEjecucionPresupuestal + element.porcentajeEjecucionPresupuestal;
        });
        if (this.totalEjpresupuestal.porcentajeEjecucionPresupuestal > 0)
            this.totalEjpresupuestal.porcentajeEjecucionPresupuestal =
                this.totalEjpresupuestal.porcentajeEjecucionPresupuestal / 2;
    }

    totalFinanciera() {
        this.totalEjfinanciera = {
            totalComprometido: 0,
            ordenadoGirarAntesImpuestos: 0,
            saldo: 0,
            porcentajeEjecucionFinanciera: 0
        };
        this.dataTableEjfinanciera.forEach(element => {
            this.totalEjfinanciera.totalComprometido += element.totalComprometido;
            this.totalEjfinanciera.ordenadoGirarAntesImpuestos =
                this.totalEjfinanciera.ordenadoGirarAntesImpuestos + element.ordenadoGirarAntesImpuestos;
            this.totalEjfinanciera.saldo = this.totalEjfinanciera.saldo + element.saldo;
            this.totalEjfinanciera.porcentajeEjecucionFinanciera =
                this.totalEjfinanciera.porcentajeEjecucionFinanciera + element.porcentajeEjecucionFinanciera;
        });
        if (this.totalEjfinanciera.porcentajeEjecucionFinanciera > 0) {
            this.totalEjfinanciera.porcentajeEjecucionFinanciera = this.totalEjfinanciera.porcentajeEjecucionFinanciera / 2;
        }
    }

    downloadPDF() {
        this.openAcordeon = true;
        setTimeout(() => {
            // document.title = 'Ejecución financiera ' + this.contrato.numeroContrato;
            document.title = 'Ejecución financiera ' + this.pContratoId;
            window.print();
        }, 300);
        setTimeout(() => (this.openAcordeon = true), 400);
        // window.onafterprint = function () {
        //   window.location.reload();
        // };
    }
}
