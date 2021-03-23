import { CommonService } from './../../../../core/_services/common/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { DialogObservacionesItemListchequeoComponent } from '../dialog-observaciones-item-listchequeo/dialog-observaciones-item-listchequeo.component';
import humanize from 'humanize-plus';
import { Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-obs-valid-listachequeo',
  templateUrl: './obs-valid-listachequeo.component.html',
  styleUrls: ['./obs-valid-listachequeo.component.scss']
})
export class ObsValidListachequeoComponent implements OnInit {

    @Input() contrato: any;
    @Input() solicitudPago: any;
    @Input() listaChequeoCodigo: string;
    @Input() autorizarSolicitudPagoId: number;
    @Input() esVerDetalle = false;
    @Output() estadoSemaforo = new EventEmitter<string>();
    esExpensas: boolean;
    listaRevisionTecnica: Dominio[] = [];
    noCumpleCodigo = '2';
    seDiligencioCampo = false;
    solicitudPagoModificado: any;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'item',
        'documento',
        'revTecnica'
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
    estaEditando = false;

    get listas() {
        return this.addressForm.get( 'listas' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
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
            let enProceso = 0;
            let completo = 0;
            for ( const solicitudPagoListaChequeo of this.solicitudPago.solicitudPagoListaChequeo ) {
                for ( const solicitudPagoListaChequeoRespuesta of solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta ) {
                    solicitudPagoListaChequeoRespuesta.respuestaCodigo = solicitudPagoListaChequeoRespuesta.respuestaCodigo !== undefined ? solicitudPagoListaChequeoRespuesta.respuestaCodigo : null;
                    solicitudPagoListaChequeoRespuesta.observacion = solicitudPagoListaChequeoRespuesta.observacion !== undefined ? solicitudPagoListaChequeoRespuesta.observacion : null;
                }

                const listaObservaciones = await this.obsMultipleSvc.asyncGetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                    this.autorizarSolicitudPagoId,
                    this.activatedRoute.snapshot.params.id,
                    solicitudPagoListaChequeo.solicitudPagoListaChequeoId,
                    this.listaChequeoCodigo )

                const observacionApoyo = listaObservaciones.find( obs => obs.archivada === false );
                let semaforo = 'sin-diligenciar';

                if ( observacionApoyo !== undefined ) {
                    if ( observacionApoyo.registroCompleto === false ) {
                        semaforo = 'en-proceso';
                        enProceso++;
                    }
                    if ( observacionApoyo.registroCompleto === true ) {
                        semaforo = 'completo';
                        completo++;
                    }
                }

                this.listas.push( this.fb.group(
                    {
                        semaforo,
                        solicitudPagoListaChequeo: [ solicitudPagoListaChequeo ],
                        fechaCreacion: [ observacionApoyo !== undefined ? observacionApoyo.fechaCreacion : null ],
                        solicitudPagoObservacionId: [ observacionApoyo !== undefined ? observacionApoyo.solicitudPagoObservacionId : 0 ],
                        tieneObservaciones: [ observacionApoyo !== undefined ? observacionApoyo.tieneObservacion : null, Validators.required ],
                        observaciones:[ observacionApoyo !== undefined ? ( observacionApoyo.observacion !== undefined ? ( observacionApoyo.observacion.length > 0 ? observacionApoyo.observacion : null ) : null ) : null, Validators.required ]
                    }
                ) );
            }

            if ( enProceso === 0 && completo === 0 ) {
                this.estadoSemaforo.emit( 'sin-diligenciar' );
            }

            if ( enProceso > 0 && ( enProceso < this.solicitudPago.solicitudPagoListaChequeo.length || enProceso === this.solicitudPago.solicitudPagoListaChequeo.length ) ) {
                this.estadoSemaforo.emit( 'en-proceso' );
            }

            if ( enProceso === 0 && completo > 0 && completo < this.solicitudPago.solicitudPagoListaChequeo.length ) {
                this.estadoSemaforo.emit( 'en-proceso' );
            }

            if ( completo > 0 && completo === this.solicitudPago.solicitudPagoListaChequeo.length ) {
                this.estadoSemaforo.emit( 'completo' );
            }

            this.solicitudPagoModificado = this.solicitudPago;
            this.dataSource = new MatTableDataSource();
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
        }
        if ( this.contrato !== undefined  && this.solicitudPago === undefined ) {
            this.esExpensas = false;
            let enProceso = 0;
            let completo = 0;
            for ( const solicitudPagoListaChequeo of this.contrato.solicitudPagoOnly.solicitudPagoListaChequeo ) {
                for ( const solicitudPagoListaChequeoRespuesta of solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta ) {
                    solicitudPagoListaChequeoRespuesta.respuestaCodigo = solicitudPagoListaChequeoRespuesta.respuestaCodigo !== undefined ? solicitudPagoListaChequeoRespuesta.respuestaCodigo : null;
                    solicitudPagoListaChequeoRespuesta.observacion = solicitudPagoListaChequeoRespuesta.observacion !== undefined ? solicitudPagoListaChequeoRespuesta.observacion : null;
                }

                const listaObservaciones = await this.obsMultipleSvc.asyncGetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                    this.autorizarSolicitudPagoId,
                    this.activatedRoute.snapshot.params.idSolicitudPago,
                    solicitudPagoListaChequeo.solicitudPagoListaChequeoId,
                    this.listaChequeoCodigo )

                const observacionApoyo = listaObservaciones.find( obs => obs.archivada === false );
                let semaforo = 'sin-diligenciar';

                if ( observacionApoyo !== undefined ) {
                    if ( observacionApoyo.registroCompleto === false ) {
                        semaforo = 'en-proceso';
                        enProceso++;
                    }
                    if ( observacionApoyo.registroCompleto === true ) {
                        semaforo = 'completo';
                        completo++;
                    }
                }

                this.listas.push( this.fb.group(
                    {
                        semaforo,
                        solicitudPagoListaChequeo: [ solicitudPagoListaChequeo ],
                        fechaCreacion: [ observacionApoyo !== undefined ? observacionApoyo.fechaCreacion : null ],
                        solicitudPagoObservacionId: [ observacionApoyo !== undefined ? observacionApoyo.solicitudPagoObservacionId : 0 ],
                        tieneObservaciones: [ observacionApoyo !== undefined ? observacionApoyo.tieneObservacion : null, Validators.required ],
                        observaciones:[ observacionApoyo !== undefined ? ( observacionApoyo.observacion !== undefined ? ( observacionApoyo.observacion.length > 0 ? observacionApoyo.observacion : null ) : null ) : null, Validators.required ]
                    }
                ) );
            }

            if ( enProceso === 0 && completo === 0 ) {
                this.estadoSemaforo.emit( 'sin-diligenciar' );
            }

            if ( enProceso > 0 && ( enProceso < this.contrato.solicitudPagoOnly.solicitudPagoListaChequeo.length || enProceso === this.contrato.solicitudPagoOnly.solicitudPagoListaChequeo.length ) ) {
                this.estadoSemaforo.emit( 'en-proceso' );
            }

            if ( enProceso === 0 && completo > 0 && completo < this.contrato.solicitudPagoOnly.solicitudPagoListaChequeo.length ) {
                this.estadoSemaforo.emit( 'en-proceso' );
            }

            if ( completo > 0 && completo === this.contrato.solicitudPagoOnly.solicitudPagoListaChequeo.length ) {
                this.estadoSemaforo.emit( 'completo' );
            }

            this.solicitudPagoModificado = this.contrato.solicitudPagoOnly;
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

    getMatTable( solicitudPagoListaChequeoRespuesta: any[] ) {
        return new MatTableDataSource( solicitudPagoListaChequeoRespuesta );
    }

    getRevisionTecnica( respuestaCodigo: string ) {
        if ( this.listaRevisionTecnica.length > 0 ) {
            const revision = this.listaRevisionTecnica.find( revision => revision.codigo === respuestaCodigo );

            if ( revision !== undefined ) {
                return revision.nombre;
            };
        }
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }

    getObservacion( registro: any, index: number, jIndex: number ) {
        const dialogRef = this.dialog.open(DialogObservacionesItemListchequeoComponent, {
            width: '80em',
            data: { dataSolicitud: this.esExpensas === true ? this.solicitudPago : this.contrato, registro, jIndex, esExpensas: this.esExpensas }
        });
    }

    // callSubsanacion(){
    //     const dialogConfig = new MatDialogConfig();
    //     dialogConfig.height = 'auto';
    //     dialogConfig.width = '865px';
    //     //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
    //     const dialogRef = this.dialog.open(DialogSubsanacionComponent, dialogConfig);
    //     //dialogRef.afterClosed().subscribe(value => {});
    // }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar( lista: FormGroup ) {
        this.estaEditando = true;
        this.listas.markAllAsTouched();
        if ( lista.get( 'observaciones' ).value !== null && lista.get( 'tieneObservaciones' ).value === false ) {
            lista.get( 'observaciones' ).setValue( '' );
        }
        const pSolicitudPagoObservacion = {
            solicitudPagoObservacionId: lista.get( 'solicitudPagoObservacionId' ).value,
            solicitudPagoId: Number( this.activatedRoute.snapshot.params.idSolicitudPago || this.activatedRoute.snapshot.params.id ),
            observacion: lista.get( 'observaciones' ).value,
            tipoObservacionCodigo: this.listaChequeoCodigo,
            menuId: this.autorizarSolicitudPagoId,
            idPadre: lista.get( 'solicitudPagoListaChequeo' ).value.solicitudPagoListaChequeoId,
            tieneObservacion: lista.get( 'tieneObservaciones' ).value
        };

        console.log( pSolicitudPagoObservacion );
        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( pSolicitudPagoObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    if ( this.esExpensas === false ) {
                        this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                            () => this.routes.navigate(
                                [
                                    '/autorizarSolicitudPago/autorizacionSolicitud',  this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitudPago
                                ]
                            )
                        );
                    } else {
                        this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                            () => this.routes.navigate( [ '/autorizarSolicitudPago/observacionExpensas', this.activatedRoute.snapshot.params.id ] )
                        );
                    }
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }

}
