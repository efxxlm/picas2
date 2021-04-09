import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ListaMenu, ListaMenuId, TipoObservaciones, TipoObservacionesCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-soporte-orden-giro-gog',
  templateUrl: './soporte-orden-giro-gog.component.html',
  styleUrls: ['./soporte-orden-giro-gog.component.scss']
})
export class SoporteOrdenGiroGogComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Output() tieneObservacion = new EventEmitter<boolean>();
    obsVerificar: any;
    obsAprobar: any;
    obsTramitar: any;
    listaMenu: ListaMenu = ListaMenuId;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;
    ordenGiroSoporteId = 0;
    ordenGiroDetalle: any;
    addressForm: FormGroup;
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
        this.getSoporte();
    }

    async getSoporte() {
        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;
            
            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    this.ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];
                    this.ordenGiroDetalleId = this.ordenGiroDetalle.ordenGiroDetalleId;
    
                    if ( this.ordenGiroDetalle.ordenGiroSoporte !== undefined ) {
                        if ( this.ordenGiroDetalle.ordenGiroSoporte.length > 0 ) {
                            this.ordenGiroSoporteId = this.ordenGiroDetalle.ordenGiroSoporte[0].ordenGiroSoporteId;
    
                            this.addressForm.setValue( { urlSoporte: this.ordenGiroDetalle.ordenGiroSoporte[0].urlSoporte !== undefined ? this.ordenGiroDetalle.ordenGiroSoporte[0].urlSoporte : null } )
                            // Get observaciones
                            const listaObservacionVerificar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                this.listaMenu.verificarOrdenGiro,
                                this.ordenGiroId,
                                this.ordenGiroSoporteId,
                                this.tipoObservaciones.soporteOrdenGiro );
                            const listaObservacionAprobar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                this.listaMenu.aprobarOrdenGiro,
                                this.ordenGiroId,
                                this.ordenGiroSoporteId,
                                this.tipoObservaciones.soporteOrdenGiro );
                            const listaObservacionTramitar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                    this.listaMenu.tramitarOrdenGiro,
                                    this.ordenGiroId,
                                    this.ordenGiroSoporteId,
                                    this.tipoObservaciones.soporteOrdenGiro );
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
                            if ( listaObservacionTramitar.find( obs => obs.archivada === false ) !== undefined ) {
                                if ( listaObservacionTramitar.find( obs => obs.archivada === false ).tieneObservacion === true ) {
                                    this.obsTramitar = listaObservacionTramitar.find( obs => obs.archivada === false );
                                }
                            }

                            if ( this.obsVerificar !== undefined ) {
                                this.tieneObservacion.emit( true );
                            }
                            if ( this.obsAprobar !== undefined ) {
                                this.tieneObservacion.emit( true );
                            }
                            if ( this.obsTramitar !== undefined ) {
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
        urlSoporte:[ null, Validators.required ]
      })
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
                    ordenGiroSoporte: [
                        {
                            ordenGiroDetalleId: this.ordenGiroDetalleId,
                            ordenGiroSoporteId: this.ordenGiroSoporteId,
                            urlSoporte: this.addressForm.get( 'urlSoporte' ).value
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
                    if ( this.obsTramitar !== undefined ) {
                        this.obsTramitar.archivada = !this.obsTramitar.archivada;
                        this.obsOrdenGiro.createEditSpinOrderObservations( this.obsTramitar )
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
