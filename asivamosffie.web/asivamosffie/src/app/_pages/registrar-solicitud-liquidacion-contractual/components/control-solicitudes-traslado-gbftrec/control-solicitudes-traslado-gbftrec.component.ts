import { ActivatedRoute } from '@angular/router';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Validators, FormControl, FormBuilder, FormArray } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-control-solicitudes-traslado-gbftrec',
  templateUrl: './control-solicitudes-traslado-gbftrec.component.html',
  styleUrls: ['./control-solicitudes-traslado-gbftrec.component.scss']
})
export class ControlSolicitudesTrasladoGbftrecComponent implements OnInit {

    @Input() proyectoId: number;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    @Input() proyecto: any;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    listaTipoSolicitudContrato: Dominio[] = [];
    myFilter = new FormControl();
    myFilter2 = new FormControl();
    var1: any;
    var2: any;
    idContrato = null;
    dataSource: MatTableDataSource<any>;
    addressForm = this.fb.group({
        tipoSolicitud: [ null ],
        numeroOrdenGiro: [ null ]
    });
    displayedColumns: string[] = [
        'tipoSolicitudGiro',
        'fechaAprobacionFiduciaria',
        'fechaPagoFiduciaria',
        'numeroOrdendeGiro',
        'modalidadContrato',
        'numeroContrato',
        'seleccion',
    ];
    dataTable: any[] = [];

    constructor(
        private fb: FormBuilder,
        private balanceSvc: FinancialBalanceService,
        private activatedRoute: ActivatedRoute,
        private commonSvc: CommonService )
    {
    }

    async ngOnInit() {
        this.listaTipoSolicitudContrato = await this.commonSvc.listaTipoSolicitudContrato().toPromise()

        if ( this.esRegistroNuevo === false || this.esVerDetalle === true ) {
            const numeroOrdenGiro = this.activatedRoute.snapshot.params.numeroOrdenGiro;
            this.addressForm.get( 'numeroOrdenGiro' ).setValue( numeroOrdenGiro )
            this.getParamsSearch()
        }
    }

    loadDataSource() {
    }

    async getParamsSearch() {
        if ( this.addressForm.get( 'tipoSolicitud' ).value !== null || this.addressForm.get( 'numeroOrdenGiro' ).value !== null ) {
            this.dataTable = [];
            let listaOrdenGiro: any[];

            if ( this.esRegistroNuevo === true ) {
                listaOrdenGiro = await this.balanceSvc.getOrdenGiroBy(
                    this.addressForm.get( 'tipoSolicitud' ).value !== null ? this.addressForm.get( 'tipoSolicitud' ).value : '',
                    this.addressForm.get( 'numeroOrdenGiro').value !== null ? this.addressForm.get( 'numeroOrdenGiro').value : '',
                    this.proyecto.llaveMen ).toPromise()
            }
            if ( this.esRegistroNuevo === false || this.esVerDetalle === true ) {
                listaOrdenGiro = await this.balanceSvc.getOrdenGiroByNumeroOrdenGiro( this.addressForm.get( 'numeroOrdenGiro').value, this.proyecto.llaveMen ).toPromise()
            }

            if ( listaOrdenGiro.length > 0 ) {
                listaOrdenGiro.forEach( ordenGiro => {
                    this.dataTable.push(
                        {
                            tipoSolicitudGiro: ordenGiro.tipoSolicitud,
                            fechaAprobacionFiduciaria: moment( ordenGiro.fechaAprobacionFinanciera ).format( 'DD/MM/YYYY' ),
                            fechaPagoFiduciaria: moment( ordenGiro.fechaAprobacionFinanciera ).format( 'DD/MM/YYYY' ),
                            numeroOrdendeGiro: ordenGiro.numeroSolicitudOrdenGiro,
                            modalidadContrato: ordenGiro.contrato.modalidadCodigo,
                            numeroContrato: ordenGiro.contrato.numeroContrato,
                            solicitudPagoId: ordenGiro.solicitudPago[ 0 ].solicitudPagoId
                        }
                    )
                } )
            }
        }
    }

}
