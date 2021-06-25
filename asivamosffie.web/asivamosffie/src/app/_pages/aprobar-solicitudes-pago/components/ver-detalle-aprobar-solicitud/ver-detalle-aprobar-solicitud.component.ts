import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { DialogProyectosAsociadosAprobComponent } from '../dialog-proyectos-asociados-aprob/dialog-proyectos-asociados-aprob.component';

@Component({
  selector: 'app-ver-detalle-aprobar-solicitud',
  templateUrl: './ver-detalle-aprobar-solicitud.component.html',
  styleUrls: ['./ver-detalle-aprobar-solicitud.component.scss']
})
export class VerDetalleAprobarSolicitudComponent implements OnInit {

    contrato: any;
    idGestion: any;
    solicitud: string;
    esVerDetalle = true;
    tipoSolicitudCodigo: any = {};
    modalidadContratoArray: Dominio[] = [];
    tipoPagoArray: Dominio[] = [];
    addressForm: FormGroup;
    otrosCostosObsForm: FormGroup;
    solicitudPagoCargarFormaPago: any;
    tieneFormaPago = true;
    menusIdPath: any; // Se obtienen los ID de los respectivos PATH de cada caso de uso que se implementaran observaciones.
    listaTipoObservacionSolicitudes: any; // Interfaz lista tipos de observaciones.
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    otrosCostosForm = this.fb.group({
        numeroContrato: [null, Validators.required],
        numeroRadicadoSAC: [null, Validators.required],
        numeroFactura: [null, Validators.required],
        valorFacturado: [null, Validators.required],
        tipoPago: [null, Validators.required]
    });
    displayedColumns: string[] = [
        'drp',
        'numDrp',
        'ProyectoLLaveMen',
        'NombreUso',
        'valor',
        'saldo'
      ];
    displayedColumnsRegistrarSolicitud: string[] = [
        'faseContrato',
        'valorFacturado',
        'porcentajeFacturado',
        'saldoPorPagar',
        'porcentajePorPagar'
    ];
    dataTable: any[] = [
      {
        drp: '1',
        numDrp: 'IP_00090',
        valor: '$100.000.000',
        saldo: '$100.000.000'
      },
      {
        drp: '2',
        numDrp: 'IP_00123',
        valor: '$5.000.000',
        saldo: '$5.000.000'
      }
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
                                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                    this.menusIdPath.aprobarSolicitudPagoId,
                                    this.activatedRoute.snapshot.params.idSolicitudPago,
                                    this.contrato.solicitudPagoOnly.solicitudPagoSoporteSolicitud[0].solicitudPagoSoporteSolicitudId,
                                    this.listaTipoObservacionSolicitudes.soporteSolicitudCodigo )
                                    .subscribe(
                                        response => {
                                            const obsSupervisor = response.filter( obs => obs.archivada === false )[0];

                                            if ( obsSupervisor !== undefined ) {
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
                                    this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                        this.menusIdPath.aprobarSolicitudPagoId,
                                        this.activatedRoute.snapshot.params.idSolicitudPago,
                                        this.contrato.solicitudPagoOnly.solicitudPagoOtrosCostosServicios[0].solicitudPagoOtrosCostosServiciosId,
                                        this.listaTipoObservacionSolicitudes.otrosCostosCodigo )
                                        .subscribe(
                                            response => {
                                                const obsSupervisor = response.filter( obs => obs.archivada === false )[0];

                                                if ( obsSupervisor !== undefined ) {
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


                                    if ( this.contrato.solicitudPago.length > 1 ) {
                                        this.solicitudPagoCargarFormaPago = this.contrato.solicitudPago[0].solicitudPagoCargarFormaPago[0];
                                        this.tieneFormaPago = false;
                                    } else {
                                        this.solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
                                    }

                                    this.dataSource = new MatTableDataSource( this.contrato.tablaDRP );
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

    openProyectosAsociados() {
        if ( this.contrato === undefined ) {
            return;
        }

        const dialogRef = this.dialog.open( DialogProyectosAsociadosAprobComponent, {
            width: '80em',
            data: { contrato: this.contrato }
        });
    }

}
