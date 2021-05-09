import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Validators, FormControl, FormBuilder, FormArray } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import moment from 'moment';

@Component({
  selector: 'app-control-solicitudes-traslado-gbftrec',
  templateUrl: './control-solicitudes-traslado-gbftrec.component.html',
  styleUrls: ['./control-solicitudes-traslado-gbftrec.component.scss']
})
export class ControlSolicitudesTrasladoGbftrecComponent implements OnInit {

    @Input() proyectoId: number;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    myFilter = new FormControl();
    myFilter2 = new FormControl();
    var1: any;
    var2: any;
    idContrato:any = null;
    dataSource: MatTableDataSource<any>;
    addressForm = this.fb.group({
        tipoSolicitud: [ null, Validators.compose( [ Validators.minLength(3), Validators.maxLength(100) ] ) ],
        numeroOrdenGiro: [ null, Validators.compose( [ Validators.minLength(3), Validators.maxLength(100) ] ) ],
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
    dataTable: any[] = [];

    get ordenesDeGiro() {
        return this.addressForm.get( 'ordenesDeGiro' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder )
    { }

    ngOnInit(): void {
    }

    loadDataSource() {
    }
  
    addProject( contratacionProyectoId: number, index: number ) {
      console.log( contratacionProyectoId, this.ordenesDeGiro.controls[ index ] );
    }

    getParamsSearch() {
        this.ordenesDeGiro.clear();

        if ( this.addressForm.get( 'tipoSolicitud' ).value === null ) {
            this.dataTable = [];
            return;
        } else {
            if ( this.addressForm.get( 'tipoSolicitud' ).value.length === 0 ) {
                this.dataTable = [];
                this.addressForm.get( 'tipoSolicitud' ).setValue( null )
                return;
            }
        }
        if ( this.addressForm.get( 'numeroOrdenGiro' ).value === null ) {
            this.dataTable = [];
            return;
        } else {
            if ( this.addressForm.get( 'numeroOrdenGiro' ).value.length === 0 ) {
                this.dataTable = [];
                this.addressForm.get( 'numeroOrdenGiro' ).setValue( null )
                return;
            }
        }

        if ( this.addressForm.get( 'tipoSolicitud' ).value !== null && this.addressForm.get( 'numeroOrdenGiro' ).value !== null ) {
            this.dataTable = [];
            console.log( this.addressForm.get( 'tipoSolicitud' ).value, this.addressForm.get( 'numeroOrdenGiro' ).value.length )

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
                    modalidadContrato: 'Modalidad 1',
                    numeroContrato: 'N801801',
                    contratacionProyectoId: Math.round( Math.random() * 100 )
                },
                {
                    tipoSolicitudGiro: 'Obra',
                    fechaAprobacionFiduciaria: new Date(),
                    fechaPagoFiduciaria: new Date(),
                    numeroOrdendeGiro: 'ODG_Obra_326',
                    modalidadContrato: 'Modalidad 1',
                    numeroContrato: 'N801801',
                    contratacionProyectoId: Math.round( Math.random() * 100 )
                }
            )

            for ( const ordenGiro of this.dataTable ) {
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

}
