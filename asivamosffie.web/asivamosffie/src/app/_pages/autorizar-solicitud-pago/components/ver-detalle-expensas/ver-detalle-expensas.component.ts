import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-ver-detalle-expensas',
  templateUrl: './ver-detalle-expensas.component.html',
  styleUrls: ['./ver-detalle-expensas.component.scss']
})
export class VerDetalleExpensasComponent implements OnInit {

    solicitudPago: any;
    solicitudPagoObservacionExpensasId = 0;
    solicitudPagoObservacionId = 0;
    solicitudPagoCertificadoObsId = 0;
    menusIdPath: any; // Se obtienen los ID de los respectivos PATH de cada caso de uso que se implementaran observaciones.
    listaTipoObservacionSolicitudes: any; // Interfaz lista tipos de observaciones.
    tipoPagoArray: Dominio[] = [];
    conceptoPagoCriterioArray: Dominio[] = [];
    addressForm: FormGroup;
    expensasForm: FormGroup;
    certificadoObsForm: FormGroup;
    detalleForm = this.fb.group({
        llaveMen: [null, Validators.required],
        llaveMenSeleccionada: [ null, Validators.required ],
        numeroRadicadoSAC: [null, Validators.required],
        numeroFactura: [null, Validators.required],
        valorFacturado: [null, Validators.required],
        tipoPago: [null, Validators.required],
        conceptoPagoCriterio: [null, Validators.required],
        valorFacturadoConcepto: [null, Validators.required]
    });

    constructor(
        private routes: Router,
        private activatedRoute: ActivatedRoute,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.addressForm = this.crearFormulario();
        this.expensasForm = this.crearFormulario();
        this.certificadoObsForm = this.crearFormulario();
        this.getSolicitudExpensas();
    }

    ngOnInit(): void {
    }

    getSolicitudExpensas() {
        this.obsMultipleSvc.listaMenu()
            .subscribe( response => this.menusIdPath = response );
        this.obsMultipleSvc.listaTipoObservacionSolicitudes()
            .subscribe( response => this.listaTipoObservacionSolicitudes = response );
        this.registrarPagosSvc.getSolicitudPago( this.activatedRoute.snapshot.params.id )
            .subscribe(
                response => {
                    console.log( response );
                    this.solicitudPago = response;
                    this.commonSvc.tiposDePagoExpensas()
                        .subscribe( tipoPago => {
                            this.tipoPagoArray = tipoPago;
                            this.commonSvc.conceptosDePagoExpensas()
                                .subscribe( conceptoPago => {
                                    this.conceptoPagoCriterioArray = conceptoPago;
                                    if ( this.solicitudPago !== undefined ) {
                                        // Get observacion expensas
                                        this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                            this.menusIdPath.autorizarSolicitudPagoId,
                                            this.activatedRoute.snapshot.params.id,
                                            this.solicitudPago.solicitudPagoExpensas[0].solicitudPagoExpensasId,
                                            this.listaTipoObservacionSolicitudes.expensasCodigo )
                                            .subscribe(
                                                response => {
                                                    const obsSupervisor = response.filter( obs => obs.archivada === false )[0];

                                                    if ( obsSupervisor !== undefined ) {
                                                        this.solicitudPagoObservacionExpensasId = obsSupervisor.solicitudPagoObservacionId;
                                                        this.expensasForm.setValue(
                                                            {
                                                                fechaCreacion: obsSupervisor.fechaCreacion,
                                                                tieneObservaciones: obsSupervisor.tieneObservacion !== undefined ? obsSupervisor.tieneObservacion : null,
                                                                observaciones: obsSupervisor.observacion !== undefined ? ( obsSupervisor.observacion.length > 0 ? obsSupervisor.observacion : null ) : null
                                                            }
                                                        );
                                                    }
                                                }
                                            );

                                        // Get observacion soporte de la solicitud
                                        this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                            this.menusIdPath.autorizarSolicitudPagoId,
                                            this.activatedRoute.snapshot.params.id,
                                            this.solicitudPago.solicitudPagoSoporteSolicitud[0].solicitudPagoSoporteSolicitudId,
                                            this.listaTipoObservacionSolicitudes.soporteSolicitudCodigo )
                                            .subscribe(
                                                response => {
                                                    const obsSupervisor = response.filter( obs => obs.archivada === false )[0];

                                                    if ( obsSupervisor !== undefined ) {
                                                        this.solicitudPagoObservacionId = obsSupervisor.solicitudPagoObservacionId;
                                                        this.addressForm.setValue(
                                                            {
                                                                fechaCreacion: obsSupervisor.fechaCreacion,
                                                                tieneObservaciones: obsSupervisor.tieneObservacion !== undefined ? obsSupervisor.tieneObservacion : null,
                                                                observaciones: obsSupervisor.observacion !== undefined ? ( obsSupervisor.observacion.length > 0 ? obsSupervisor.observacion : null ) : null
                                                            }
                                                        );
                                                    }
                                                }
                                            );

                                        // Get observaciones certificado de la solicitud
                                        this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                            this.menusIdPath.autorizarSolicitudPagoId,
                                            this.activatedRoute.snapshot.params.id,
                                            this.solicitudPago.solicitudPagoSoporteSolicitud[0].solicitudPagoSoporteSolicitudId,
                                            this.listaTipoObservacionSolicitudes.certificadoCodigo )
                                            .subscribe(
                                                response => {
                                                    const obsSupervisor = response.filter(obs => obs.archivada === false)[0];

                                                    if (obsSupervisor !== undefined) {
                                                        this.certificadoObsForm.markAllAsTouched();
                                                        this.solicitudPagoCertificadoObsId = obsSupervisor.solicitudPagoObservacionId;
                                                        this.certificadoObsForm.setValue(
                                                            {
                                                                fechaCreacion: obsSupervisor.fechaCreacion,
                                                                tieneObservaciones: obsSupervisor.tieneObservacion !== undefined ? obsSupervisor.tieneObservacion : null,
                                                                observaciones: obsSupervisor.observacion !== undefined ? (obsSupervisor.observacion.length > 0 ? obsSupervisor.observacion : null) : null
                                                            }
                                                        );
                                                    }
                                                }
                                            );

                                        const solicitudPagoExpensas = this.solicitudPago.solicitudPagoExpensas[0];
                                        this.detalleForm.setValue(
                                            {
                                                llaveMen: this.solicitudPago.contratacionProyecto.proyecto.llaveMen,
                                                llaveMenSeleccionada: {
                                                    contratacionProyectoId: this.solicitudPago.contratacionProyectoId,
                                                    llaveMen: this.solicitudPago.contratacionProyecto.proyecto.llaveMen
                                                },
                                                numeroRadicadoSAC: solicitudPagoExpensas.numeroRadicadoSac !== undefined ? solicitudPagoExpensas.numeroRadicadoSac : null,
                                                numeroFactura: solicitudPagoExpensas.numeroFactura !== undefined ? solicitudPagoExpensas.numeroFactura : null,
                                                valorFacturado: solicitudPagoExpensas.valorFacturado !== undefined ? solicitudPagoExpensas.valorFacturado : null,
                                                tipoPago: solicitudPagoExpensas.tipoPagoCodigo !== undefined ? this.tipoPagoArray.filter( tipoPago => tipoPago.codigo === solicitudPagoExpensas.tipoPagoCodigo )[0] : null,
                                                conceptoPagoCriterio: solicitudPagoExpensas.conceptoPagoCriterioCodigo !== undefined ? this.conceptoPagoCriterioArray.filter( conceptoPago => conceptoPago.codigo === solicitudPagoExpensas.conceptoPagoCriterioCodigo )[0] : null,
                                                valorFacturadoConcepto: solicitudPagoExpensas.valorFacturadoConcepto !== undefined ? solicitudPagoExpensas.valorFacturadoConcepto : null
                                            }
                                        );
                                    }
                                } );
                        } );
                }
            );
    }

    crearFormulario() {
        return this.fb.group({
            fechaCreacion: [ null ],
            tieneObservaciones: [null, Validators.required],
            observaciones:[null, Validators.required]
        })
    }

}
