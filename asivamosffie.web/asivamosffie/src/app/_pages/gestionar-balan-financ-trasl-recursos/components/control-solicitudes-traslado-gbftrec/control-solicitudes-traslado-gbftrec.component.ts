import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Validators, FormControl, FormBuilder, FormArray } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-control-solicitudes-traslado-gbftrec',
  templateUrl: './control-solicitudes-traslado-gbftrec.component.html',
  styleUrls: ['./control-solicitudes-traslado-gbftrec.component.scss']
})
export class ControlSolicitudesTrasladoGbftrecComponent implements OnInit {

    @Input() proyectoId: number;
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
        tipoSolicitud: [ null, Validators.compose( [ Validators.minLength(3), Validators.maxLength(100) ] ) ],
        numeroOrdenGiro: [ null, Validators.compose( [ Validators.minLength(3), Validators.maxLength(100) ] ) ]
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
        private commonSvc: CommonService )
    { }

    async ngOnInit() {
        this.listaTipoSolicitudContrato = await this.commonSvc.listaTipoSolicitudContrato().toPromise()
    }

    loadDataSource() {
    }

    getParamsSearch() {
        if ( this.addressForm.get( 'tipoSolicitud' ).value !== null || this.addressForm.get( 'numeroOrdenGiro' ).value !== null ) {
            this.dataTable = [];

            this.dataTable.push(
                {
                    tipoSolicitudGiro: 'Obra',
                    fechaAprobacionFiduciaria: new Date(),
                    fechaPagoFiduciaria: new Date(),
                    numeroOrdendeGiro: 'ODG_Obra_001',
                    modalidadContrato: 'Modalidad 1',
                    numeroContrato: 'N801801',
                    contratacionProyectoId: Math.round( Math.random() * 100 )
                },
                {
                    tipoSolicitudGiro: 'Obra',
                    fechaAprobacionFiduciaria: new Date(),
                    fechaPagoFiduciaria: new Date(),
                    numeroOrdendeGiro: 'ODG_Obra_326',
                    modalidadContrato: 'Modalidad 2',
                    numeroContrato: 'N801802',
                    contratacionProyectoId: Math.round( Math.random() * 100 )
                },
                {
                    tipoSolicitudGiro: 'Obra',
                    fechaAprobacionFiduciaria: new Date(),
                    fechaPagoFiduciaria: new Date(),
                    numeroOrdendeGiro: 'ODG_Obra_327',
                    modalidadContrato: 'Modalidad 3',
                    numeroContrato: 'N801803',
                    contratacionProyectoId: Math.round( Math.random() * 100 )
                }
            )
        }
    }

}
