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
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    idContrato = null;
    dataSource: MatTableDataSource<any>;
    addressForm = this.fb.group({
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

    get ordenesDeGiro() {
        return this.addressForm.get( 'ordenesDeGiro' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder )
    { }

    ngOnChanges( changes: SimpleChanges ): void {
        let listaResultado = [];

        if ( changes.listaBusqueda.currentValue.length > 0 ) {
            listaResultado = changes.listaBusqueda.currentValue;
            this.ordenesDeGiro.clear();

            for ( const ordenGiro of listaResultado ) {
                this.ordenesDeGiro.push(
                    this.fb.group(
                        {
                            tipoSolicitudGiro: [ ordenGiro.tipoSolicitudGiro ],
                            fechaAprobacionFiduciaria: [ moment( ordenGiro.fechaAprobacionFiduciaria ).format( 'DD/MM/YYYY' ) ],
                            fechaPagoFiduciaria: [ moment( ordenGiro.fechaPagoFiduciaria ).format( 'DD/MM/YYYY' ) ],
                            numeroOrdendeGiro: [ ordenGiro.numeroOrdendeGiro ],
                            modalidadContrato: [ ordenGiro.modalidadContrato ],
                            numeroContrato: [ ordenGiro.numeroContrato ],
                            contratacionProyectoId: [ ordenGiro.contratacionProyectoId ],
                            check: [ null ]
                        }
                    )
                )
            }

            this.dataSource = new MatTableDataSource( this.ordenesDeGiro.controls );
            setTimeout(() => {
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
            }, 200);
        }
    }

    ngOnInit(): void {
    }

    addProject( contratacionProyectoId: number, index: number ) {
        console.log( contratacionProyectoId, this.ordenesDeGiro.controls[ index ] );
    }

}
