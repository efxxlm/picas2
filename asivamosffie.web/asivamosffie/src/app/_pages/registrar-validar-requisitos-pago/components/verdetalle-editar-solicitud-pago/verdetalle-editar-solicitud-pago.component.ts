import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { DialogProyectosAsociadosComponent } from '../dialog-proyectos-asociados/dialog-proyectos-asociados.component';

@Component({
  selector: 'app-verdetalle-editar-solicitud-pago',
  templateUrl: './verdetalle-editar-solicitud-pago.component.html',
  styleUrls: ['./verdetalle-editar-solicitud-pago.component.scss']
})
export class VerdetalleEditarSolicitudPagoComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    contrato: any;
    tipoSolicitudCodigo: any = {};
    // Semaforos
    semaforoFormaDePago = 'sin-diligenciar';
    // Acordeones habilitados
    registroCompletoAcordeones: any = {
        registroCompletoFormaDePago: false,
        registroCompletoSolicitudPago: false,
        registroCompletoListaChequeo: false,
        registroCompletoOtrosCostos: false
    }
    displayedColumns: string[] = [
      'drp',
      'numDrp',
      'valor',
      'saldo'
    ];

    constructor(
        private activatedRoute: ActivatedRoute,
        public dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private commonSvc: CommonService )
    {
        this.getContrato();
    }

    ngOnInit(): void {
    }

    getContrato() {
        this.registrarPagosSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitud )
            .subscribe(
                response => {
                    this.commonSvc.tiposDeSolicitudes()
                        .subscribe(
                          solicitudes => {
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
                            if ( this.contrato.solicitudPagoOnly.tipoSolicitudCodigo !== this.tipoSolicitudCodigo.otrosCostos ) {
                                this.dataSource = new MatTableDataSource( this.contrato.contratacion.disponibilidadPresupuestal );
                                this.dataSource.paginator = this.paginator;
                                this.dataSource.sort = this.sort;

                                // Get semaforo forma de pago y registro completo
                                const solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];

                                if ( solicitudPagoCargarFormaPago !== undefined ) {

                                    // Get semaforo
                                    if ( solicitudPagoCargarFormaPago.registroCompleto === false ) {
                                        this.semaforoFormaDePago = 'en-proceso';
                                    }
                                    if ( solicitudPagoCargarFormaPago.registroCompleto === true ) {
                                        this.semaforoFormaDePago = 'completo';
                                        //Get registro completo
                                        this.registroCompletoAcordeones.registroCompletoFormaDePago = true;
                                    }
                                }
                            } else {
                                this.registroCompletoAcordeones.registroCompletoSolicitudPago = true;
                            }
                          }
                        );
                }
            );
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    };

    openProyectosAsociados() {
        if ( this.contrato === undefined ) {
            return;
        }

        const dialogRef = this.dialog.open( DialogProyectosAsociadosComponent, {
            width: '80em',
            data: { contrato: this.contrato }
        });
    }

    enabledAcordeon( nombreAcordeon: string, tieneRegistroAnterior: boolean ) {

        // Acordeon solicitud de pago
        if ( nombreAcordeon === 'solicitudDePago' && tieneRegistroAnterior === false ) {
            return 'en-alerta';
        }
        if ( nombreAcordeon === 'solicitudDePago' && tieneRegistroAnterior === true ) {

            const solicitudPagoRegistrarSolicitudPago = this.contrato.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[0];
            let semaforoSolicitudPago = 'sin-diligenciar';

            if ( solicitudPagoRegistrarSolicitudPago !== undefined ) {
                if ( solicitudPagoRegistrarSolicitudPago.registroCompleto === false ) {
                    semaforoSolicitudPago = 'en-proceso';
                }
                if ( solicitudPagoRegistrarSolicitudPago.registroCompleto === true ) {
                    semaforoSolicitudPago = 'completo';
                    // Get registro completo
                    this.registroCompletoAcordeones.registroCompletoSolicitudPago = true;
                }
            }
            return semaforoSolicitudPago;
        }

        // Acordeon lista de chequeo
        if ( nombreAcordeon === 'listaChequeo' && tieneRegistroAnterior === false ) {
            return 'en-alerta';
        }
        if ( nombreAcordeon === 'listaChequeo' && tieneRegistroAnterior === true ) {
            // Get semaforo se coloca con la clase 'en-alerta' ya que el CU lista de chequeo no esta terminado
            return 'en-alerta';
        }

        // Acordeon soporte de la solicitud
        if ( nombreAcordeon === 'soporteSolicitud' && tieneRegistroAnterior === false ) {
            return 'en-alerta';
        }
        if ( nombreAcordeon === 'soporteSolicitud' && tieneRegistroAnterior === true ) {

            const solicitudPagoSoporteSolicitud = this.contrato.solicitudPagoOnly.solicitudPagoSoporteSolicitud[0];
            let semaforoSolicitudPago = 'sin-diligenciar';

            if ( solicitudPagoSoporteSolicitud !== undefined ) {
                if ( solicitudPagoSoporteSolicitud.registroCompleto === false ) {
                    semaforoSolicitudPago = 'en-proceso';
                }
                if ( solicitudPagoSoporteSolicitud.registroCompleto === true ) {
                    semaforoSolicitudPago = 'completo';
                }
            }

            return semaforoSolicitudPago;
        }
    }

}
