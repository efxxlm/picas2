import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DialogProyectosAsociadosAutorizComponent } from '../dialog-proyectos-asociados-autoriz/dialog-proyectos-asociados-autoriz.component';

@Component({
  selector: 'app-form-autorizar-solicitud',
  templateUrl: './form-autorizar-solicitud.component.html',
  styleUrls: ['./form-autorizar-solicitud.component.scss']
})
export class FormAutorizarSolicitudComponent implements OnInit {

    contrato: any;
    idGestion: any;
    solicitud: string;
    solicitudPagoObservacionId = 0;
    solicitudPagoObsOtrosCostosId = 0;
    tipoSolicitudCodigo: any = {};
    modalidadContratoArray: Dominio[] = [];
    tipoPagoArray: Dominio[] = [];
    addressForm: FormGroup;
    otrosCostosObsForm: FormGroup;
    dataSource = new MatTableDataSource();
    menusIdPath: any; // Se obtienen los ID de los respectivos PATH de cada caso de uso que se implementaran observaciones.
    listaTipoObservacionSolicitudes: any; // Interfaz lista tipos de observaciones.
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    otrosCostosForm = this.fb.group({
        numeroContrato: [null, Validators.required],
        numeroRadicadoSAC: [null, Validators.required],
        numeroFactura: [null, Validators.required],
        valorFacturado: [null, Validators.required],
        tipoPago: [null, Validators.required]
    });
    estadoAcordeones = {
        estadoFormaPago: 'sin-diligenciar',
        estadoSolicitudPago: 'sin-diligenciar',
        soporteSolicitud: 'sin-diligenciar'
    }
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
    displayedColumns: string[] = [
        'drp',
        'numDrp',
        'valor',
        'saldo'
    ];

    constructor(
        private activatedRoute: ActivatedRoute,
        private dialog: MatDialog,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private fb: FormBuilder,
        private commonSvc: CommonService )
    {
        this.obsMultipleSvc.listaMenu()
            .subscribe( response => this.menusIdPath = response );
        this.obsMultipleSvc.listaTipoObservacionSolicitudes()
            .subscribe( response => this.listaTipoObservacionSolicitudes = response );
        this.addressForm = this.crearFormulario();
        this.otrosCostosObsForm = this.crearFormulario();
        this.getContrato();
    }

    ngOnInit(): void {
    }

    getContrato() {
        this.registrarPagosSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitudPago )
            .subscribe(
                response => {
                    this.commonSvc.tiposDeSolicitudes()
                        .subscribe(
                            solicitudes => {
                                this.commonSvc.modalidadesContrato()
                                    .subscribe( response => this.modalidadContratoArray = response );
                                for ( const solicitud of solicitudes ) {
                                    if ( solicitud.codigo === '1' ) {
                                      this.tipoSolicitudCodigo.contratoObra = solicitud.codigo;
                                    }
                                    if ( solicitud.codigo === '2' ) {
                                      this.tipoSolicitudCodigo.contratoInterventoria = solicitud.codigo;
                                    }
                                    if ( solicitud.codigo === '3' ) {
                                      this.tipoSolicitudCodigo.expensas = solicitud.codigo;
                                    }
                                    if ( solicitud.codigo === '4' ) {
                                      this.tipoSolicitudCodigo.otrosCostos = solicitud.codigo;
                                    }
                                }
                                this.contrato = response;
                                console.log( this.contrato );
                                // Get observaciones Soporte de la solicitud
                                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId( this.menusIdPath.autorizarSolicitudPagoId, this.activatedRoute.snapshot.params.idSolicitudPago, this.contrato.solicitudPagoOnly.solicitudPagoSoporteSolicitud[0].solicitudPagoSoporteSolicitudId )
                                    .subscribe(
                                        response => {
                                            const obsSupervisor = response.filter( obs => obs.archivada === false )[0];

                                            if ( obsSupervisor !== undefined ) {
                                                if ( obsSupervisor.registroCompleto === false ) {
                                                    this.estadoAcordeones.soporteSolicitud = 'en-proceso';
                                                }
                                                if ( obsSupervisor.registroCompleto === true ) {
                                                    this.estadoAcordeones.soporteSolicitud = 'completo';
                                                }
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

                                if ( this.contrato.solicitudPagoOnly.tipoSolicitudCodigo === this.tipoSolicitudCodigo.otrosCostos ) {
                                    // Get observaciones otros costos
                                    this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId( this.menusIdPath.autorizarSolicitudPagoId, this.activatedRoute.snapshot.params.idSolicitudPago, this.contrato.solicitudPagoOnly.solicitudPagoOtrosCostosServicios[0].solicitudPagoOtrosCostosServiciosId )
                                        .subscribe(
                                            response => {
                                                const obsSupervisor = response.filter( obs => obs.archivada === false )[0];

                                                if ( obsSupervisor !== undefined ) {
                                                    console.log( obsSupervisor );
                                                    this.solicitudPagoObsOtrosCostosId = obsSupervisor.solicitudPagoObservacionId;
                                                    this.otrosCostosObsForm.setValue(
                                                        {
                                                            fechaCreacion: obsSupervisor.fechaCreacion,
                                                            tieneObservaciones: obsSupervisor.tieneObservacion !== undefined ? obsSupervisor.tieneObservacion : null,
                                                            observaciones: obsSupervisor.observacion !== undefined ? ( obsSupervisor.observacion.length > 0 ? obsSupervisor.observacion : null ) : null
                                                        }
                                                    );
                                                }
                                            }
                                        );
                                    this.commonSvc.tiposDePagoExpensas()
                                        .subscribe( response => {
                                            this.tipoPagoArray = response;
                                            if ( this.contrato !== undefined ) {
                                                const solicitudPagoOtrosCostosServicios = this.contrato.solicitudPagoOnly.solicitudPagoOtrosCostosServicios[0];
                                                this.otrosCostosForm.setValue(
                                                    {
                                                        numeroContrato: this.contrato.numeroContrato,
                                                        numeroRadicadoSAC: solicitudPagoOtrosCostosServicios.numeroRadicadoSac !== undefined ? solicitudPagoOtrosCostosServicios.numeroRadicadoSac : null,
                                                        numeroFactura: solicitudPagoOtrosCostosServicios.numeroFactura !== undefined ? solicitudPagoOtrosCostosServicios.numeroFactura : null,
                                                        valorFacturado: solicitudPagoOtrosCostosServicios.valorFacturado !== undefined ? solicitudPagoOtrosCostosServicios.valorFacturado : null,
                                                        tipoPago: solicitudPagoOtrosCostosServicios.tipoPagoCodigo !== undefined ? this.tipoPagoArray.filter( tipoPago => tipoPago.codigo === solicitudPagoOtrosCostosServicios.tipoPagoCodigo )[0] : null
                                                    }
                                                );
                                            }
                                        } );
                                } else {
                                    this.dataSource = new MatTableDataSource( this.contrato.contratacion.disponibilidadPresupuestal );
                                    this.dataSource.paginator = this.paginator;
                                    this.dataSource.sort = this.sort;
                                }
                            }
                        );
                }
            );
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.modalidadContratoArray.length > 0 ) {
            const modalidad = this.modalidadContratoArray.filter( modalidad => modalidad.codigo === modalidadCodigo );
            return modalidad[0].nombre;
        }
    }

    crearFormulario() {
        return this.fb.group({
            fechaCreacion: [ null ],
            tieneObservaciones: [null, Validators.required],
            observaciones:[null, Validators.required]
        })
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
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

    openProyectosAsociados() {
        if ( this.contrato === undefined ) {
            return;
        }

        const dialogRef = this.dialog.open( DialogProyectosAsociadosAutorizComponent, {
            width: '80em',
            data: { contrato: this.contrato }
        });
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
            solicitudPagoId: Number( this.activatedRoute.snapshot.params.idSolicitudPago ),
            observacion: this.addressForm.get( 'observaciones' ).value !== null ? this.addressForm.get( 'observaciones' ).value : this.addressForm.get( 'observaciones' ).value,
            tipoObservacionCodigo: this.listaTipoObservacionSolicitudes.soporteSolicitudCodigo,
            menuId: this.menusIdPath.autorizarSolicitudPagoId,
            idPadre: this.contrato.solicitudPagoOnly.solicitudPagoSoporteSolicitud[0].solicitudPagoSoporteSolicitudId,
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
                                '/autorizarSolicitudPago/autorizacionSolicitud',  this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitudPago
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }

    guardar() {
        if ( this.otrosCostosObsForm.get( 'tieneObservaciones' ).value !== null && this.otrosCostosObsForm.get( 'tieneObservaciones' ).value === false ) {
            this.otrosCostosObsForm.get( 'observaciones' ).setValue( '' );
        }

        const pSolicitudPagoObservacion = {
            solicitudPagoObservacionId: this.solicitudPagoObsOtrosCostosId,
            solicitudPagoId: Number( this.activatedRoute.snapshot.params.idSolicitudPago ),
            observacion: this.otrosCostosObsForm.get( 'observaciones' ).value !== null ? this.otrosCostosObsForm.get( 'observaciones' ).value : this.otrosCostosObsForm.get( 'observaciones' ).value,
            tipoObservacionCodigo: this.listaTipoObservacionSolicitudes.otrosCostosCodigo,
            menuId: this.menusIdPath.autorizarSolicitudPagoId,
            idPadre: this.contrato.solicitudPagoOnly.solicitudPagoOtrosCostosServicios[0].solicitudPagoOtrosCostosServiciosId,
            tieneObservacion: this.otrosCostosObsForm.get( 'tieneObservaciones' ).value !== null ? this.otrosCostosObsForm.get( 'tieneObservaciones' ).value : this.otrosCostosObsForm.get( 'tieneObservaciones' ).value
        };

        console.log( pSolicitudPagoObservacion );
        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( pSolicitudPagoObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/autorizarSolicitudPago/autorizacionSolicitud',  this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitudPago
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }

}
