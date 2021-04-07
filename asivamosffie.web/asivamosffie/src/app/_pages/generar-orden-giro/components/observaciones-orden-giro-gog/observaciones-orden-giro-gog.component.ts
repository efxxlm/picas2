import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ListaMenu, ListaMenuId, TipoObservaciones, TipoObservacionesCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-observaciones-orden-giro-gog',
  templateUrl: './observaciones-orden-giro-gog.component.html',
  styleUrls: ['./observaciones-orden-giro-gog.component.scss']
})
export class ObservacionesOrdenGiroGogComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Output() tieneObservacion = new EventEmitter<boolean>();
    obsVerificar: any;
    obsAprobar: any;
    listaMenu: ListaMenu = ListaMenuId;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;
    ordenGiroObservacionId = 0;
    ordenGiroDetalle: any;
    addressForm = this.fb.group({});
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
    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private ordenPagoSvc: OrdenPagoService,
        private dialog: MatDialog,
        private routes: Router,
        private obsOrdenGiro: ObservacionesOrdenGiroService )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        this.getObservacion();
    }

    async getObservacion() {
        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;
            
            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    this.ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];
                    this.ordenGiroDetalleId = this.ordenGiroDetalle.ordenGiroDetalleId;
    
                    if ( this.ordenGiroDetalle.ordenGiroDetalleObservacion !== undefined ) {
                        if ( this.ordenGiroDetalle.ordenGiroDetalleObservacion.length > 0 ) {
                            this.ordenGiroObservacionId = this.ordenGiroDetalle.ordenGiroDetalleObservacion[0].ordenGiroObservacionId;

                            this.addressForm.setValue(
                                {
                                    observaciones: this.ordenGiroDetalle.ordenGiroDetalleObservacion[0].observacion !== undefined ? this.ordenGiroDetalle.ordenGiroDetalleObservacion[0].observacion : null
                                }
                            )

                            // Get observaciones
                            const listaObservacionVerificar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                this.listaMenu.verificarOrdenGiro,
                                this.ordenGiroId,
                                this.ordenGiroDetalle.ordenGiroDetalleId,
                                this.tipoObservaciones.observaciones );
                            const listaObservacionAprobar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                this.listaMenu.aprobarOrdenGiro,
                                this.ordenGiroId,
                                this.ordenGiroDetalle.ordenGiroDetalleId,
                                this.tipoObservaciones.observaciones );
                            // Get lista de observacion y observacion actual    
                            if ( listaObservacionVerificar.find( obs => obs.archivada === false ) !== undefined ) {
                                if ( listaObservacionVerificar.find( obs => obs.archivada === false ).tieneObservacion === true ) {
                                    this.obsVerificar = listaObservacionVerificar.find( obs => obs.archivada === false );
                                }
                            }
                            if ( listaObservacionAprobar.find( obs => obs.archivada === false ) !== undefined ) {
                                if ( listaObservacionAprobar.find( obs => obs.archivada === false ).tieneObservacion === true ) {
                                    this.obsAprobar = listaObservacionAprobar.find( obs => obs.archivada === false );
                                }
                            }

                            if ( this.obsVerificar !== undefined ) {
                                this.tieneObservacion.emit( true );
                            }
                            if ( this.obsAprobar !== undefined ) {
                                this.tieneObservacion.emit( true );
                            }
                        }
                    }
                }
            }
        }
    }

    crearFormulario() {
      return this.fb.group({
        observaciones:[null, Validators.required]
      })
    }

    maxLength( e: any, n: number ) {
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
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        const pOrdenGiro = {
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            ordenGiroId: this.ordenGiroId,
            ordenGiroDetalle: [
                {
                    ordenGiroId: this.ordenGiroId,
                    ordenGiroDetalleId: this.ordenGiroDetalleId,
                    ordenGiroDetalleObservacion: [
                        {
                            ordenGiroDetalleId: this.ordenGiroDetalleId,
                            ordenGiroObservacionId: this.ordenGiroObservacionId,
                            observacion: this.addressForm.get( 'observaciones' ).value
                        }
                    ]
                }
            ]
        }

        this.ordenPagoSvc.createEditOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    if ( this.obsVerificar !== undefined ) {
                        this.obsVerificar.archivada = !this.obsVerificar.archivada;
                        this.obsOrdenGiro.createEditSpinOrderObservations( this.obsVerificar )
                            .subscribe();
                    }
                    if ( this.obsAprobar !== undefined ) {
                        this.obsAprobar.archivada = !this.obsAprobar.archivada;
                        this.obsOrdenGiro.createEditSpinOrderObservations( this.obsAprobar )
                            .subscribe();
                    }
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/generarOrdenDeGiro/generacionOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
