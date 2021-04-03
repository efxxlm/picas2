import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ListaMenu, ListaMenuId, TipoObservaciones, TipoObservacionesCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-soporte-orden-giro',
  templateUrl: './soporte-orden-giro.component.html',
  styleUrls: ['./soporte-orden-giro.component.scss']
})
export class SoporteOrdenGiroComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    @Output() estadoSemaforo = new EventEmitter<string>();
    listaMenu: ListaMenu = ListaMenuId;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    ordenGiroObservacionId = 0;
    ordenGiroId = 0;
    ordenGiroSoporte: any;
    historialObservaciones: any[] = [];
    tablaHistorial = new MatTableDataSource();
    formObservacion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null, Validators.required ],
        fechaCreacion: [ null ]
    });
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
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

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private obsOrdenGiro: ObservacionesOrdenGiroService,
        private routes: Router )
    { }

    ngOnInit(): void {
        this.getObservaciones();
    }

    async getObservaciones() {
        this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;
        this.ordenGiroSoporte = this.solicitudPago.ordenGiro.ordenGiroDetalle[ 0 ].ordenGiroSoporte[ 0 ];

        // Get observaciones
        const listaObservacionVerificar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
            this.listaMenu.verificarOrdenGiro,
            this.ordenGiroId,
            this.ordenGiroSoporte.ordenGiroSoporteId,
            this.tipoObservaciones.soporteOrdenGiro );
        const listaObservacionAprobar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
            this.listaMenu.aprobarOrdenGiro,
            this.ordenGiroId,
            this.ordenGiroSoporte.ordenGiroSoporteId,
            this.tipoObservaciones.soporteOrdenGiro );
        const listaObservacionTramitar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                this.listaMenu.tramitarOrdenGiro,
                this.ordenGiroId,
                this.ordenGiroSoporte.ordenGiroSoporteId,
                this.tipoObservaciones.soporteOrdenGiro );
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
                this.estadoSemaforo.emit( 'en-proceso' );
            }
            if ( observacion.registroCompleto === true ) {
                this.estadoSemaforo.emit( 'completo' );
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
            tipoObservacionCodigo: this.tipoObservaciones.soporteOrdenGiro,
            menuId: this.listaMenu.verificarOrdenGiro,
            idPadre: this.ordenGiroSoporte.ordenGiroSoporteId,
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
