import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, OnChanges, SimpleChanges, Input, ViewChild } from '@angular/core';
import { FormArray, FormBuilder } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';

@Component({
  selector: 'app-form-orden-giro',
  templateUrl: './form-orden-giro.component.html',
  styleUrls: ['./form-orden-giro.component.scss']
})
export class FormOrdenGiroComponent implements OnInit, OnChanges {

    @Input() listaBusqueda = [];
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    listaModalidad: Dominio[] = [];
    listaTipoSolicitud: Dominio[] = [];
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    dataSource: MatTableDataSource<any>;
    addressForm = this.fb.group({
        resultadoBusqueda: this.fb.array( [] ),
        ordenesDeGiro: this.fb.array( [] )
    });
    displayedColumns: string[] = [
        'tipoSolicitudGiro',
        'fechaAprobacionFiduciaria',
        'fechaPagoFiduciaria',
        'numeroOrdendeGiro',
        'modalidadContrato',
        'numeroContrato'
    ];

    get resultadoBusqueda() {
        return this.addressForm.get( 'resultadoBusqueda' ) as FormArray;
    }

    get ordenesGiro() {
        return this.addressForm.get( 'ordenesDeGiro' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private activatedRoute: ActivatedRoute,
        private commonSvc: CommonService )
    { }

    ngOnChanges( changes: SimpleChanges ): void {
        let listaResultado = [];

        if ( changes.listaBusqueda.currentValue.length > 0 ) {
            listaResultado = changes.listaBusqueda.currentValue;
            this.resultadoBusqueda.clear();
            console.log(listaResultado);
            for ( const ordenGiro of listaResultado ) {
                this.resultadoBusqueda.push(
                    this.fb.group(
                        {
                            tipoSolicitudGiro: [ ordenGiro.tipoSolicitudGiro ],
                            fechaAprobacionFiduciaria: [ ordenGiro.fechaAprobacionFiduciaria ],
                            fechaPagoFiduciaria: [ ordenGiro.fechaPagoFiduciaria ],
                            numeroOrdendeGiro: [ ordenGiro.numeroOrdendeGiro ],
                            modalidadContrato: [ ordenGiro.modalidadContrato ],
                            numeroContrato: [ ordenGiro.numeroContrato ],
                            solicitudPagoId: [ ordenGiro.solicitudPagoId ]

                        }
                    )
                )

                this.ordenesGiro.push(
                    this.fb.group(
                        {
                            solicitudPagoId: [ ordenGiro.solicitudPagoId ],
                            numeroOrdendeGiro: [ this.activatedRoute.snapshot.params.numeroOrdenGiro ],
                            modalidadContrato: [ ordenGiro.modalidadContrato ],
                            numeroContrato: [ ordenGiro.numeroContrato ]
                        }
                    )
                )
            }

            this.dataSource = new MatTableDataSource( this.resultadoBusqueda.controls )
        }
    }

    async ngOnInit() {
        this.listaModalidad = await this.commonSvc.modalidadesContrato().toPromise()
        this.listaTipoSolicitud = await this.commonSvc.listaTipoSolicitudContrato().toPromise()
    }

    getTipoSolicitud( codigo: string ) {
        if ( this.listaTipoSolicitud.length > 0 ) {
            const solicitud = this.listaTipoSolicitud.find( solicitud => solicitud.codigo === codigo )
            
            if ( solicitud !== undefined ) {
                return solicitud.nombre
            }
        }
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.listaModalidad.length > 0 ) {
            const modalidad = this.listaModalidad.find( modalidad => modalidad.codigo === modalidadCodigo )
            
            if ( modalidad !== undefined ) {
                return modalidad.nombre
            }
        }
    }

}