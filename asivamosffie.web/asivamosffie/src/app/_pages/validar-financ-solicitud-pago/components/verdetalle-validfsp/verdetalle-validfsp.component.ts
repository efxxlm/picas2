import { Component, OnInit, ViewChild } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { DialogProyectosAsociadosValidfspComponent } from '../dialog-proyectos-asociados-validfsp/dialog-proyectos-asociados-validfsp.component';

@Component({
  selector: 'app-verdetalle-validfsp',
  templateUrl: './verdetalle-validfsp.component.html',
  styleUrls: ['./verdetalle-validfsp.component.scss']
})
export class VerdetalleValidfspComponent implements OnInit {

    contrato: any;
    idGestion: any;
    solicitud: string;
    modalidadContratoArray: Dominio[] = [];
    tipoPagoArray: Dominio[] = [];
    formaPagoArray: Dominio[] = [];
    tipoSolicitudCodigo: any = {};
    solicitudPagoCargarFormaPago: any;
    estadoSemaforo: string;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'drp',
        'numDrp',
        'valor',
        'saldo'
    ];
    otrosCostosForm = this.fb.group({
        numeroContrato: [null, Validators.required],
        numeroRadicadoSAC: [null, Validators.required],
        numeroFactura: [null, Validators.required],
        valorFacturado: [null, Validators.required],
        tipoPago: [null, Validators.required]
    });

    constructor(
        private activatedRoute: ActivatedRoute,
        private dialog: MatDialog,
        private fb: FormBuilder,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private commonSvc: CommonService )
    {
        this.commonSvc.formasDePago()
            .subscribe( response => this.formaPagoArray = response );
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

                                if ( this.contrato.solicitudPagoOnly.tipoSolicitudCodigo === this.tipoSolicitudCodigo.otrosCostos ) {
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

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.modalidadContratoArray.length > 0 ) {
            const modalidad = this.modalidadContratoArray.filter( modalidad => modalidad.codigo === modalidadCodigo );
            return modalidad[0].nombre;
        }
    }

    getFormaPago( formaPagoCodigo: string ) {
        if ( this.formaPagoArray.length > 0 ) {
            const forma = this.formaPagoArray.filter( forma => forma.codigo === formaPagoCodigo );
            return forma[0].nombre;
        }
    }

    openProyectosAsociados() {
        if ( this.contrato === undefined ) {
            return;
        }

        const dialogRef = this.dialog.open( DialogProyectosAsociadosValidfspComponent, {
            width: '80em',
            data: { contrato: this.contrato }
        });
    }

}
