import { CommonService } from './../../../../core/_services/common/common.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, OnChanges, SimpleChanges, Input, ViewChild } from '@angular/core';
import { FormArray, FormBuilder } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';
import { Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-form-orden-giro',
  templateUrl: './form-orden-giro.component.html',
  styleUrls: ['./form-orden-giro.component.scss']
})
export class FormOrdenGiroComponent implements OnInit, OnChanges {

    @Input() listaBusqueda = [];
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    listaModalidad: Dominio[] = [];
    listaTipoSolicitud: Dominio[] = [];
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
        'numeroContrato',
        'seleccion',
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

            for ( const ordenGiro of listaResultado ) {
                if ( this.esRegistroNuevo === true ) {
                    this.resultadoBusqueda.push(
                        this.fb.group(
                            {
                                tipoSolicitudGiro: [ ordenGiro.tipoSolicitudGiro ],
                                fechaAprobacionFiduciaria: [ ordenGiro.fechaAprobacionFiduciaria ],
                                fechaPagoFiduciaria: [ ordenGiro.fechaPagoFiduciaria ],
                                numeroOrdendeGiro: [ ordenGiro.numeroOrdendeGiro ],
                                modalidadContrato: [ ordenGiro.modalidadContrato ],
                                numeroContrato: [ ordenGiro.numeroContrato ],
                                solicitudPagoId: [ ordenGiro.solicitudPagoId ],
                                check: [ null ],
    
                            }
                        )
                    )
                }

                if ( this.esRegistroNuevo === false || this.esVerDetalle === true ) {
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
            }

            if ( this.esRegistroNuevo === true && this.esVerDetalle === false ) {
                this.dataSource = new MatTableDataSource( this.resultadoBusqueda.controls );
                setTimeout(this.paginador, 400);
            }
        }
    }

    private paginador() {
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
            if (length === 0 || pageSize === 0) {
                return '0 de ' + length;
            }
            length = Math.max(length, 0);
            const startIndex = page * pageSize;
            // If the start index exceeds the list length, do not try and fix the end index to the end.
            const endIndex = startIndex < length ?
                Math.min(startIndex + pageSize, length) :
                startIndex + pageSize;
            return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
        };
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

    addProject( index: number ) {
        if ( this.resultadoBusqueda.controls[ index ].get( 'check' ).value === true ) {
            this.ordenesGiro.push(
                this.fb.group(
                    {
                        solicitudPagoId: [ this.resultadoBusqueda.controls[ index ].get( 'solicitudPagoId' ).value ],
                        numeroOrdendeGiro: [ this.resultadoBusqueda.controls[ index ].get( 'numeroOrdendeGiro' ).value ],
                        modalidadContrato: [ this.resultadoBusqueda.controls[ index ].get( 'modalidadContrato' ).value ],
                        numeroContrato: [ this.resultadoBusqueda.controls[ index ].get( 'numeroContrato' ).value ]
                    }
                )
            )
        }

        if ( this.resultadoBusqueda.controls[ index ].get( 'check' ).value === false ) {
            if ( this.ordenesGiro.length > 0 ) {
                const ordenGiroIndex = this.ordenesGiro.controls.findIndex( control => control.get( 'contratacionProyectoId' ).value === this.resultadoBusqueda.controls[ index ].get( 'contratacionProyectoId' ).value );

                if ( ordenGiroIndex !== -1 ) {
                    this.ordenesGiro.removeAt( ordenGiroIndex );
                }
            }
        }
    }

}
