import { Router } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ListaMenu, ListaMenuId, TipoObservaciones, TipoObservacionesCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';

@Component({
  selector: 'app-form-estrateg-pagos-gog',
  templateUrl: './form-estrateg-pagos-gog.component.html',
  styleUrls: ['./form-estrateg-pagos-gog.component.scss']
})
export class FormEstrategPagosGogComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    obsVerificar: any;
    obsAprobar: any;
    obsTramitar: any;
    listaMenu: ListaMenu = ListaMenuId;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;
    ordenGiro: any;
    estrategiaPagoArray: Dominio[] = [];
    addressForm: FormGroup;
    estaEditando = true;

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private ordenPagoSvc: OrdenPagoService,
        private dialog: MatDialog,
        private routes: Router,
        private obsOrdenGiro: ObservacionesOrdenGiroService )
    {
        this.commonSvc.listaEstrategiasPago()
            .subscribe( response => this.estrategiaPagoArray = response );
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        this.getEstrategia();
    }

    async getEstrategia() {
        if ( this.solicitudPago.ordenGiro !== undefined ) {

            this.ordenGiro = this.solicitudPago.ordenGiro;
            this.ordenGiroId = this.ordenGiro.ordenGiroId;

            if ( this.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    const ordenGiroDetalle = this.ordenGiro.ordenGiroDetalle[0];
                    this.ordenGiroDetalleId = ordenGiroDetalle.ordenGiroDetalleId;
                    
                    if ( ordenGiroDetalle.ordenGiroDetalleEstrategiaPago !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroDetalleEstrategiaPago.length > 0 ) {
                            const ordenGiroDetalleEstrategiaPago = ordenGiroDetalle.ordenGiroDetalleEstrategiaPago[0];

    
                            this.addressForm.setValue(
                                {
                                    ordenGiroDetalleId: this.ordenGiroDetalleId,
                                    ordenGiroDetalleEstrategiaPagoId: ordenGiroDetalleEstrategiaPago.ordenGiroDetalleEstrategiaPagoId,
                                    estrategiaPagoCodigo: ordenGiroDetalleEstrategiaPago.estrategiaPagoCodigo !== undefined ? ordenGiroDetalleEstrategiaPago.estrategiaPagoCodigo : null
                                }
                            );

                            // Get observaciones
                            const listaObservacionVerificar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                this.listaMenu.verificarOrdenGiro,
                                this.ordenGiroId,
                                ordenGiroDetalleEstrategiaPago.ordenGiroDetalleEstrategiaPagoId,
                                this.tipoObservaciones.estrategiaPago );
                            const listaObservacionAprobar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                this.listaMenu.aprobarOrdenGiro,
                                this.ordenGiroId,
                                ordenGiroDetalleEstrategiaPago.ordenGiroDetalleEstrategiaPagoId,
                                this.tipoObservaciones.estrategiaPago );
                            const listaObservacionTramitar = await this.obsOrdenGiro.getObservacionOrdenGiroByMenuIdAndSolicitudPagoId(
                                    this.listaMenu.tramitarOrdenGiro,
                                    this.ordenGiroId,
                                    ordenGiroDetalleEstrategiaPago.ordenGiroDetalleEstrategiaPagoId,
                                    this.tipoObservaciones.estrategiaPago );
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
                        }
                    }
                }
            }
        }
    }

    crearFormulario() {
      return this.fb.group({
        ordenGiroDetalleId: [ 0 ],
        ordenGiroDetalleEstrategiaPagoId: [ 0 ],
        estrategiaPagoCodigo: [ null, Validators.required ]
      })
    }

    getEstrategiaPago( codigo: string ) {
        if ( this.estrategiaPagoArray.length > 0 ) {
            const estrategia = this.estrategiaPagoArray.find( estrategia => estrategia.codigo === codigo );

            if ( estrategia !== undefined ) {
                return estrategia.nombre;
            }
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
                    ordenGiroDetalleEstrategiaPago: [
                        {
                            ordenGiroDetalleId: this.ordenGiroDetalleId,
                            ordenGiroDetalleEstrategiaPagoId: this.addressForm.get( 'ordenGiroDetalleEstrategiaPagoId' ).value,
                            estrategiaPagoCodigo: this.addressForm.get( 'estrategiaPagoCodigo' ).value
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
                                '/generarOrdenDeGiro/verDetalleEditarOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
