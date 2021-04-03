import { Router } from '@angular/router';
import { CommonService } from './../../../../core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { ListaMenu, ListaMenuId, TipoObservaciones, TipoObservacionesCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';

@Component({
  selector: 'app-detalle-giro',
  templateUrl: './detalle-giro.component.html',
  styleUrls: ['./detalle-giro.component.scss']
})
export class DetalleGiroComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    @Output() estadoSemaforo = new EventEmitter<string>();
    listaMenu: ListaMenu = ListaMenuId;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    ordenGiroObservacionId = 0;
    ordenGiroDetalleEstrategiaPago: any
    ordenGiroId = 0;
    tieneDireccionTecnica = true;
    listaEstrategiaPago: Dominio[] = [];
    historialObservaciones: any[] = [];
    dataSource = new MatTableDataSource();
    dataSourceFuentes = new MatTableDataSource();
    tablaHistorial = new MatTableDataSource();
    formObservacion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null, Validators.required ],
        fechaCreacion: [ null ]
    });
    listaSemaforos = {
        semaforoEstrategiaPago: 'sin-diligenciar',
        semaforoDireccionTecnica: 'sin-diligenciar',
        semaforoTerceroCausacion: 'sin-diligenciar',
        semaforoObservaciones: 'sin-diligenciar',
        semaforoSoporte: 'sin-diligenciar'
    };
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    displayedColumns: string[]  = [
        'drp',
        'numeroDrp',
        'nombreAportante',
        'porcentaje'
    ];
    displayedColumnsFuentes: string[]  = [
        'nombre',
        'fuenteRecursos',
        'saldoActualRecursos',
        'saldoValorFacturado'
    ];
    editorStyle = {
        height: '100px'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };
    dataTable = [
        {
            numeroDrp: 'IP_00090',
            aportantes: [
                {
                    nombre: 'Alcaldía de Susacón',
                    porcentaje: 70

                },
                {
                    nombre: 'Fundación Pies Descalzos',
                    porcentaje: 30

                }
            ]
        },
        {
            numeroDrp: 'IP_00123',
            aportantes: [
                {
                    nombre: 'Alcaldía de Susacón',
                    porcentaje: 100

                }
            ]
        }
    ];
    dataTableFuentes = [
        {
            nombre: 'Alcaldía de Susacón',
            fuenteRecursos: 'Contingencias',
            saldoActualRecursos: 75000000,
            saldoValorFacturado: 75000000
        }
    ];

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private obsOrdenGiro: ObservacionesOrdenGiroService,
        private routes: Router )
    {
        this.commonSvc.listaEstrategiasPago()
            .subscribe( response => this.listaEstrategiaPago = response );
    }

    ngOnInit(): void {
        this.getObservacion()
        this.dataSource = new MatTableDataSource( this.dataTable );
        this.dataSourceFuentes = new MatTableDataSource( this.dataTableFuentes );
    }

    async getObservacion() {
        this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;
        this.ordenGiroDetalleEstrategiaPago = this.solicitudPago.ordenGiro.ordenGiroDetalle[ 0 ].ordenGiroDetalleEstrategiaPago[ 0 ];

        if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase[ 0 ].solicitudPagoFaseFactura[ 0 ].tieneDescuento === false ) {
            this.tieneDireccionTecnica = false;
            delete this.listaSemaforos.semaforoDireccionTecnica;
        }

        // Get observaciones
        const listaObservacionVerificar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
            this.listaMenu.verificarOrdenGiro,
            this.ordenGiroId,
            this.ordenGiroDetalleEstrategiaPago.ordenGiroDetalleEstrategiaPagoId,
            this.tipoObservaciones.estrategiaPago );
        const listaObservacionAprobar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
            this.listaMenu.aprobarOrdenGiro,
            this.ordenGiroId,
            this.ordenGiroDetalleEstrategiaPago.ordenGiroDetalleEstrategiaPagoId,
            this.tipoObservaciones.estrategiaPago );
        const listaObservacionTramitar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                this.listaMenu.tramitarOrdenGiro,
                this.ordenGiroId,
                this.ordenGiroDetalleEstrategiaPago.ordenGiroDetalleEstrategiaPagoId,
                this.tipoObservaciones.estrategiaPago );
        if ( listaObservacionVerificar.length > 0 ) {
            listaObservacionVerificar.forEach( obs => obs.menuId = this.listaMenu.verificarOrdenGiro );
        }
        if ( listaObservacionAprobar.length > 0 ) {
            listaObservacionAprobar.forEach( obs => obs.menuId = this.listaMenu.aprobarOrdenGiro );
        }
        if ( listaObservacionTramitar.length > 0 ) {
            listaObservacionTramitar.forEach( obs => obs.menuId = this.listaMenu.tramitarOrdenGiro )
        }
        // Get lista de observacion y observacion actual    
        const observacion = listaObservacionVerificar.find( obs => obs.archivada === false )
        if ( observacion !== undefined ) {
            this.ordenGiroObservacionId = observacion.ordenGiroObservacionId;
            if ( observacion.registroCompleto === false ) {
                this.listaSemaforos.semaforoEstrategiaPago = 'en-proceso';
            }
            if ( observacion.registroCompleto === true ) {
                this.listaSemaforos.semaforoEstrategiaPago = 'completo';
            }
            this.formObservacion.setValue(
                {
                    tieneObservaciones: observacion.tieneObservacion,
                    observaciones: observacion.observacion !== undefined ? ( observacion.observacion.length > 0 ? observacion.observacion : null ) : null,
                    fechaCreacion: observacion.fechaCreacion
                }
            )
        }
        const obsArchivadasVerificar = listaObservacionVerificar.filter( obs => obs.archivada === true );
        const obsArchivadasAprobar = listaObservacionAprobar.filter( obs => obs.archivada === true );
        const obsArchivadasTramitar = listaObservacionTramitar.filter( obs => obs.archivada === true );
        if ( obsArchivadasVerificar.length > 0 ) {
            obsArchivadasVerificar.forEach( obs => this.historialObservaciones.push( obs ) );
        }
        if ( obsArchivadasAprobar.length > 0 ) {
            obsArchivadasAprobar.forEach( obs => this.historialObservaciones.push( obs ) );
        }
        if ( obsArchivadasTramitar.length > 0 ) {
            obsArchivadasTramitar.forEach( obs => this.historialObservaciones.push( obs ) );
        }
        this.tablaHistorial = new MatTableDataSource( this.historialObservaciones );

        // Check semaforo principal
        setTimeout(() => {
            const tieneSinDiligenciar = Object.values( this.listaSemaforos ).includes( 'sin-diligenciar' );
            const tieneEnProceso = Object.values( this.listaSemaforos ).includes( 'en-proceso' );
            const tieneCompleto = Object.values( this.listaSemaforos ).includes( 'completo' );
    
            if ( tieneEnProceso === true ) {
                this.estadoSemaforo.emit( 'en-proceso' );
            }
            if ( tieneSinDiligenciar === true && tieneCompleto === true ) {
                this.estadoSemaforo.emit( 'en-proceso' );
            }
            if ( tieneSinDiligenciar === false && tieneEnProceso === false && tieneCompleto === true ) {
                this.estadoSemaforo.emit( 'completo' );
            }
        }, 6000);
    }

    getEstrategiaPago( codigo: string ) {
        if ( this.listaEstrategiaPago.length > 0 ) {
            const estrategia = this.listaEstrategiaPago.find( estrategia => estrategia.codigo === codigo );

            if ( estrategia !== undefined ) {
                return estrategia.nombre;
            }
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

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        if ( this.formObservacion.get( 'tieneObservaciones' ).value === false && this.formObservacion.get( 'observaciones' ).value !== null ) {
            this.formObservacion.get( 'observaciones' ).setValue( '' );
        }

        const pOrdenGiroObservacion = {
            ordenGiroObservacionId: this.ordenGiroObservacionId,
            ordenGiroId: this.ordenGiroId,
            tipoObservacionCodigo: this.tipoObservaciones.estrategiaPago,
            menuId: this.listaMenu.verificarOrdenGiro,
            idPadre: this.ordenGiroDetalleEstrategiaPago.ordenGiroDetalleEstrategiaPagoId,
            observacion: this.formObservacion.get( 'observaciones' ).value,
            tieneObservacion: this.formObservacion.get( 'tieneObservaciones' ).value
        }

        this.obsOrdenGiro.createEditSpinOrderObservations( pOrdenGiroObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                this.esRegistroNuevo === true ? '/verificarOrdenGiro/verificarOrdenGiro' : '/verificarOrdenGiro/editarOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
