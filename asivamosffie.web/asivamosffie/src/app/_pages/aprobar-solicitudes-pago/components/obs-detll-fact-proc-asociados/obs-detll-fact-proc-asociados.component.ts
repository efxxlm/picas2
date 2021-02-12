import { Router, ActivatedRoute } from '@angular/router';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-obs-detll-fact-proc-asociados',
  templateUrl: './obs-detll-fact-proc-asociados.component.html',
  styleUrls: ['./obs-detll-fact-proc-asociados.component.scss']
})
export class ObsDetllFactProcAsociadosComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() aprobarSolicitudPagoId: any
    @Input() criteriosPagoProyectoCodigo: string;
    @Output() estadoSemaforo = new EventEmitter<string>();
    solicitudPagoObservacionId = 0;
    esMultiProyecto = false;
    proyectos: any;
    listaCriterios: Dominio[] = [];
    criteriosArraySeleccionados: Dominio[] = [];
    solicitudPagoFaseCriterio: any;
    solicitudPagoFaseCriterioProyecto: any;
    solicitudPagoCargarFormaPago: any;
    solicitudPagoFase: any;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'llaveMen',
      'tipoIntervencion',
      'departamento',
      'municipio',
      'institucionEducativa',
      'sede'
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

    get projects() {
        return this.addressForm.get( 'projects' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private activatedRoute: ActivatedRoute,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private dialog: MatDialog,
        private obsMultipleSvc: ObservacionesMultiplesCuService )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        this.getProyectos();
        if ( this.solicitudPago !== undefined ) {

        }
    };

    getProyectos() {
        if ( this.solicitudPago !== undefined ) {
            this.solicitudPagoCargarFormaPago = this.solicitudPago.solicitudPagoCargarFormaPago[0];
            this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
            this.solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio;
            // Get observaciones
            this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId( this.aprobarSolicitudPagoId, this.solicitudPago.solicitudPagoId, this.solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioProyecto[0].solicitudPagoFaseCriterioProyectoId )
                .subscribe(
                    response => {
                        const obsSupervisor = response.filter( obs => obs.archivada === false )[0];

                        if ( obsSupervisor !== undefined ) {
                            if ( obsSupervisor.registroCompleto === false ) {
                                this.estadoSemaforo.emit( 'en-proceso' );
                            }
                            if ( obsSupervisor.registroCompleto === true ) {
                                this.estadoSemaforo.emit( 'completo' );
                            }
                            this.solicitudPagoObservacionId = obsSupervisor.solicitudPagoObservacionId;
                            this.addressForm.get( 'fechaCreacion' ).setValue( obsSupervisor.fechaCreacion );
                            this.addressForm.get( 'tieneObservaciones' ).setValue( obsSupervisor.tieneObservacion !== undefined ? obsSupervisor.tieneObservacion : null );
                            this.addressForm.get( 'observaciones' ).setValue( obsSupervisor.observacion !== undefined ? ( obsSupervisor.observacion.length > 0 ? obsSupervisor.observacion : null ) : null );
                        }
                    }
                );

            if ( this.solicitudPagoFase.esPreconstruccion === true ) {
                this.registrarPagosSvc.getCriterioByFormaPagoCodigo( this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo )
                    .subscribe(
                        criterios => {
                            this.registrarPagosSvc.getProyectosByIdContrato( this.solicitudPago.contratoId )
                                .subscribe(
                                    proyectos => {
                                        const criteriosArray = [];
                                        const criteriosSeleccionados = [];
                                        let criteriosDiligenciados = [];
                                        this.listaCriterios = criterios;
                                        this.solicitudPagoFaseCriterio.forEach( criterio => {
                                            this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                        } );
                                        if ( proyectos[1].length > 1 ) {
                                            this.esMultiProyecto = true;
                                            this.solicitudPagoFaseCriterio.forEach( criterioValue => {
                                                const criterioSelect = criterios.filter( value => value.codigo === criterioValue.tipoCriterioCodigo );
                                                if ( criterioSelect.length > 0 ) {
                                                    criterioSelect.forEach( value => {
                                                        value[ 'solicitudPagoFaseCriterioId' ] = criterioValue.solicitudPagoFaseCriterioId;
                                                        criteriosArray.push( value );
                                                    } );
                                                }
                                                if ( criterioValue.solicitudPagoFaseCriterioProyecto.length > 0 ) {
                                                    criteriosDiligenciados.push( criterios.filter( value => value.codigo === criterioValue.tipoCriterioCodigo )[0] );
                                                    criterioValue.solicitudPagoFaseCriterioProyecto.forEach( criterioProyectoValue => {
                                                        criterioSelect.forEach( value => {
                                                            criteriosSeleccionados.push(
                                                                this.fb.group(
                                                                    {
                                                                        tipoCriterioCodigo: [ value.codigo ],
                                                                        nombre: [ value.nombre ],
                                                                        contratacionProyectoId: [ criterioProyectoValue.contratacionProyectoId ],
                                                                        solicitudPagoFaseCriterioProyectoId: [ criterioProyectoValue.solicitudPagoFaseCriterioProyectoId ],
                                                                        valorFacturado: [ criterioProyectoValue.valorFacturado ]
                                                                    }
                                                                )
                                                            )
                                                        } )
                                                    } );
                                                }
                                            } );
                                            for ( const proyecto of proyectos[1] ) {
                                                const criteriosProyectoSeleccionados = criteriosSeleccionados.filter( criterioValue => criterioValue.value.contratacionProyectoId === proyecto.contratacionProyectoId );
                                                const criterioPagoArray = [];
                                                if ( criteriosSeleccionados.length > 0 ) {
                                                    criteriosSeleccionados.forEach( criterioValue => {
                                                        if ( criterioValue.value.contratacionProyectoId === proyecto.contratacionProyectoId ) {
                                                            proyecto.check = true;
                                                        }
                                                    } );
                                                    if ( proyecto.check === undefined ) {
                                                        proyecto.check = false;
                                                    }
                                                } else {
                                                    proyecto.check = false;
                                                }
                                                criteriosDiligenciados.forEach( value => {
                                                    criteriosProyectoSeleccionados.forEach( criterioValue => {
                                                        if ( value.codigo === criterioValue.value.tipoCriterioCodigo ) {
                                                            criterioPagoArray.push( value );
                                                        }
                                                    } );
                                                } );
                                                this.projects.push(
                                                    this.fb.group(
                                                        {
                                                            check: [ criteriosProyectoSeleccionados.length > 0 ? true : false ],
                                                            listaCriterios: [ criteriosArray ],
                                                            contratacionProyectoId: [ proyecto.contratacionProyectoId ],
                                                            llaveMen: [ proyecto.llaveMen ],
                                                            criterioPago: [ criterioPagoArray.length > 0 ? criterioPagoArray : null ],
                                                            solicitudPagoFaseCriterioId: [ 0 ],
                                                            criteriosProyecto: this.fb.array( criteriosProyectoSeleccionados.length > 0 ? criteriosProyectoSeleccionados : [] ),
                                                            tieneObservaciones: [null, Validators.required],
                                                            observaciones:[null, Validators.required]
                                                        }
                                                    )
                                                );
                                            }
                                        }
                                        if ( proyectos[1].length < 2 ) {
                                            this.solicitudPagoFaseCriterio.forEach( criterio => {
                                                this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                            } );
                                            this.proyectos = proyectos[1];
                                        }
                                        this.dataSource = new MatTableDataSource( proyectos[1] );
                                        this.dataSource.paginator = this.paginator;
                                        this.dataSource.sort = this.sort;
                                    }
                                );
                        }
                    );
            }
            if ( this.solicitudPagoFase.esPreconstruccion === false ) {
                this.registrarPagosSvc.getCriterioByFormaPagoCodigo( this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo )
                    .subscribe(
                        criterios => {
                            this.registrarPagosSvc.getProyectosByIdContrato( this.solicitudPago.contratoId )
                                .subscribe(
                                    proyectos => {
                                        const criteriosArray = [];
                                        const criteriosSeleccionados = [];
                                        let criteriosDiligenciados = [];
                                        this.listaCriterios = criterios;
                                        this.solicitudPagoFaseCriterio.forEach( criterio => {
                                            this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                        } );
                                        if ( proyectos[1].length > 1 ) {
                                            this.esMultiProyecto = true;
                                            this.solicitudPagoFaseCriterio.forEach( criterioValue => {
                                                const criterioSelect = criterios.filter( value => value.codigo === criterioValue.tipoCriterioCodigo );
                                                if ( criterioSelect.length > 0 ) {
                                                    criterioSelect.forEach( value => {
                                                        value[ 'solicitudPagoFaseCriterioId' ] = criterioValue.solicitudPagoFaseCriterioId;
                                                        criteriosArray.push( value );
                                                    } );
                                                }
                                                if ( criterioValue.solicitudPagoFaseCriterioProyecto.length > 0 ) {
                                                    criteriosDiligenciados.push( criterios.filter( value => value.codigo === criterioValue.tipoCriterioCodigo )[0] );
                                                    criterioValue.solicitudPagoFaseCriterioProyecto.forEach( criterioProyectoValue => {
                                                        criterioSelect.forEach( value => {
                                                            criteriosSeleccionados.push(
                                                                this.fb.group(
                                                                    {
                                                                        tipoCriterioCodigo: [ value.codigo ],
                                                                        nombre: [ value.nombre ],
                                                                        contratacionProyectoId: [ criterioProyectoValue.contratacionProyectoId ],
                                                                        solicitudPagoFaseCriterioProyectoId: [ criterioProyectoValue.solicitudPagoFaseCriterioProyectoId ],
                                                                        valorFacturado: [ criterioProyectoValue.valorFacturado ]
                                                                    }
                                                                )
                                                            )
                                                        } )
                                                    } );
                                                }
                                            } );
                                            for ( const proyecto of proyectos[1] ) {
                                                const criteriosProyectoSeleccionados = criteriosSeleccionados.filter( criterioValue => criterioValue.value.contratacionProyectoId === proyecto.contratacionProyectoId );
                                                const criterioPagoArray = [];
                                                if ( criteriosSeleccionados.length > 0 ) {
                                                    criteriosSeleccionados.forEach( criterioValue => {
                                                        if ( criterioValue.value.contratacionProyectoId === proyecto.contratacionProyectoId ) {
                                                            proyecto.check = true;
                                                        }
                                                    } );
                                                    if ( proyecto.check === undefined ) {
                                                        proyecto.check = false;
                                                    }
                                                } else {
                                                    proyecto.check = false;
                                                }
                                                criteriosDiligenciados.forEach( value => {
                                                    criteriosProyectoSeleccionados.forEach( criterioValue => {
                                                        if ( value.codigo === criterioValue.value.tipoCriterioCodigo ) {
                                                            criterioPagoArray.push( value );
                                                        }
                                                    } );
                                                } );
                                                this.projects.push(
                                                    this.fb.group(
                                                        {
                                                            check: [ criteriosProyectoSeleccionados.length > 0 ? true : false ],
                                                            listaCriterios: [ criteriosArray ],
                                                            contratacionProyectoId: [ proyecto.contratacionProyectoId ],
                                                            llaveMen: [ proyecto.llaveMen ],
                                                            criterioPago: [ criterioPagoArray.length > 0 ? criterioPagoArray : null ],
                                                            solicitudPagoFaseCriterioId: [ 0 ],
                                                            criteriosProyecto: this.fb.array( criteriosProyectoSeleccionados.length > 0 ? criteriosProyectoSeleccionados : [] ),
                                                            tieneObservaciones: [null, Validators.required],
                                                            observaciones:[null, Validators.required]
                                                        }
                                                    )
                                                );
                                            }
                                        } else {
                                            this.solicitudPagoFaseCriterio.forEach( criterio => {
                                                this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                            } );
                                            this.proyectos = proyectos[1];
                                        }
                                        this.dataSource = new MatTableDataSource( proyectos[1] );
                                        this.dataSource.paginator = this.paginator;
                                        this.dataSource.sort = this.sort;
                                    }
                                );
                        }
                    );
            }
        }
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    crearFormulario() {
      return this.fb.group({
        fechaCreacion: [ null ],
        tieneObservaciones: [null, Validators.required],
        observaciones:[null, Validators.required],
        projects: this.fb.array( [] )
      })
    }

    filterCriterios( tipoCriterioCodigo: string ) {
        if ( this.listaCriterios.length > 0 ) {
            const criterio = this.listaCriterios.filter( criterio => criterio.codigo === tipoCriterioCodigo );
            return criterio[0].nombre;
        }
    }

    criteriosProyecto( i: number ) {
        return this.projects.controls[i].get( 'criteriosProyecto' ) as FormArray;
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

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        if ( this.addressForm.get( 'tieneObservaciones' ).value !== null && this.addressForm.get( 'tieneObservaciones' ).value === false ) {
            this.addressForm.get( 'observaciones' ).setValue( '' );
        }

        const pSolicitudPagoObservacion = {
            solicitudPagoObservacionId: this.solicitudPagoObservacionId,
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            observacion: this.addressForm.get( 'observaciones' ).value !== null ? this.addressForm.get( 'observaciones' ).value : this.addressForm.get( 'observaciones' ).value,
            tipoObservacionCodigo: this.criteriosPagoProyectoCodigo,
            menuId: this.aprobarSolicitudPagoId,
            idPadre: this.solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioProyecto[0].solicitudPagoFaseCriterioProyectoId,
            tieneObservacion: this.addressForm.get( 'tieneObservaciones' ).value !== null ? this.addressForm.get( 'tieneObservaciones' ).value : this.addressForm.get( 'tieneObservaciones' ).value
        };

        console.log( pSolicitudPagoObservacion );
        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( pSolicitudPagoObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/verificarSolicitudPago/aprobacionSolicitud',  this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitudPago
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }

}
