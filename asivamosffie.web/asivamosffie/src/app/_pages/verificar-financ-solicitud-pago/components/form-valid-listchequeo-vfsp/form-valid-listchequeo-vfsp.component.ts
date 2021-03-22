import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { DialogObservacionesVfspComponent } from '../dialog-observaciones-vfsp/dialog-observaciones-vfsp.component';
import humanize from 'humanize-plus';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-valid-listchequeo-vfsp',
  templateUrl: './form-valid-listchequeo-vfsp.component.html',
  styleUrls: ['./form-valid-listchequeo-vfsp.component.scss']
})
export class FormValidListchequeoVfspComponent implements OnInit {

    @Input() contrato: any;
    @Input() solicitudPago: any;
    @Input() listaChequeoCodigo: string;
    @Input() aprobarSolicitudPagoId: number;
    @Input() esVerDetalle = false;
    @Output() estadoSemaforo = new EventEmitter<string>();
    esExpensas: boolean;
    listaRevisionTecnica: Dominio[] = [];
    noCumpleCodigo = '2';
    seDiligencioCampo = false;
    solicitudPagoModificado: any;
    dataSource = new MatTableDataSource();
    estaEditando = false;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'item',
        'documento',
        'revTecnica',
        'verificacionFinanciera',
        'observaciones'
    ];
    addressForm: FormGroup;
    editorStyle = {
      height: '45px',
      overflow: 'auto'
    };
    config = {
      toolbar: [
        ['bold', 'italic', 'underline'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ indent: '-1' }, { indent: '+1' }],
        [{ align: [] }],
      ]
    };


    get listas() {
        return this.addressForm.get( 'listas' ) as FormArray;
    }

    getRespuestasListaChequeo( index: number ) {
        return this.listas.controls[ index ].get( 'solicitudPagoListaChequeoRespuesta' ).get( 'respuestasListaChequeo' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private activatedRoute: ActivatedRoute,
        private dialog: MatDialog,
        private commonSvc: CommonService )
    {
        this.commonSvc.listaRevisionTecnica()
            .subscribe( listaRevisionTecnica => this.listaRevisionTecnica = listaRevisionTecnica );
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        this.getListasDeChequeo();
    }

    async getListasDeChequeo() {
        if ( this.contrato === undefined && this.solicitudPago !== undefined ) {
            this.esExpensas = true;
            for ( const solicitudPagoListaChequeo of this.solicitudPago.solicitudPagoListaChequeo ) {
                const respuestasListaChequeo = [];
                for ( const solicitudPagoListaChequeoRespuesta of solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta ) {

                    respuestasListaChequeo.push(
                        this.fb.group(
                            {
                                listaChequeoItemNombre: [ solicitudPagoListaChequeoRespuesta.listaChequeoItem.nombre ],
                                respuestaCodigo: [ solicitudPagoListaChequeoRespuesta.respuestaCodigo !== undefined ? solicitudPagoListaChequeoRespuesta.respuestaCodigo : null, Validators.required ],
                                observacion: [ null, Validators.required ],
                                respuestaRevisionTecnicaCodigo: [ null, Validators.required ],
                                tieneSubsanacion: [ null, Validators.required ]
                            }
                        )
                    )

                }

                this.listas.push( this.fb.group(
                    {
                        solicitudPagoId: this.solicitudPago.solicitudPagoId,
                        solicitudPagoListaChequeoId: solicitudPagoListaChequeo.solicitudPagoListaChequeoId,
                        nombre: solicitudPagoListaChequeo.listaChequeo.nombre,
                        solicitudPagoListaChequeoRespuesta: this.fb.group(
                            {
                                respuestasListaChequeo: this.fb.array( respuestasListaChequeo )
                            }
                        )
                    }
                ) );
            }

            this.dataSource = new MatTableDataSource();
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
        }
        if ( this.contrato !== undefined  && this.solicitudPago === undefined ) {
            this.esExpensas = false;
            for ( const solicitudPagoListaChequeo of this.contrato.solicitudPagoOnly.solicitudPagoListaChequeo ) {
                const respuestasListaChequeo = [];
                for ( const solicitudPagoListaChequeoRespuesta of solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta ) {

                    respuestasListaChequeo.push(
                        this.fb.group(
                            {
                                solicitudPagoListaChequeoRespuestaId: [ solicitudPagoListaChequeoRespuesta.solicitudPagoListaChequeoRespuestaId ],
                                listaChequeoItemNombre: [ solicitudPagoListaChequeoRespuesta.listaChequeoItem.nombre ],
                                respuestaCodigo: [ solicitudPagoListaChequeoRespuesta.respuestaCodigo !== undefined ? solicitudPagoListaChequeoRespuesta.respuestaCodigo : null, Validators.required ],
                                observacion: [ null, Validators.required ],
                                respuestaRevisionTecnicaCodigo: [ null, Validators.required ],
                                tieneSubsanacion: [ null, Validators.required ]
                            }
                        )
                    )

                }

                this.listas.push( this.fb.group(
                    {
                        solicitudPagoId: this.contrato.solicitudPagoOnly.solicitudPagoId,
                        solicitudPagoListaChequeoId: solicitudPagoListaChequeo.solicitudPagoListaChequeoId,
                        nombre: solicitudPagoListaChequeo.listaChequeo.nombre,
                        solicitudPagoListaChequeoRespuesta: this.fb.group(
                            {
                                respuestasListaChequeo: this.fb.array( respuestasListaChequeo )
                            }
                        )
                    }
                ) );
            }

            this.dataSource = new MatTableDataSource();
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
        }
    }

    crearFormulario() {
        return this.fb.group(
            {
                listas: this.fb.array( [] )
            }
        )
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    getMatTable( index: number ) {
        return new MatTableDataSource( this.getRespuestasListaChequeo( index ).controls );
    }

    getRevisionTecnica( respuestaCodigo: string ) {
        if ( this.listaRevisionTecnica.length > 0 ) {
            const revision = this.listaRevisionTecnica.find( revision => revision.codigo === respuestaCodigo );

            if ( revision !== undefined ) {
                return revision.nombre;
            };
        }
    }

    getSubsanacion( registro: any, index: number, jIndex: number ) {
        const dialogRef = this.dialog.open( DialogObservacionesVfspComponent, {
            width: '80em',
            data: { registro, jIndex }
        });

        dialogRef.afterClosed()
            .subscribe(
                value => {
                    this.getRespuestasListaChequeo( index ).controls[ jIndex ].get( 'observacion' ).setValue( value.observaciones );
                    this.getRespuestasListaChequeo( index ).controls[ jIndex ].get( 'tieneSubsanacion' ).setValue( value.tieneSubsanacion );
                }
            );
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        this.estaEditando = true;
        this.listas.markAllAsTouched();
        console.log( this.addressForm );
    }

}
