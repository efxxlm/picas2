import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from './../../../../core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { Router } from '@angular/router';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-form-registrar-solicitud-de-pago',
  templateUrl: './form-registrar-solicitud-de-pago.component.html',
  styleUrls: ['./form-registrar-solicitud-de-pago.component.scss']
})
export class FormRegistrarSolicitudDePagoComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @Input() contrato: any;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    solicitudPagoId = 0;
    solicitudPagoRegistrarSolicitudPagoId = 0;
    solicitudPagofaseId = 0;
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoFase: any;
    displayedColumns: string[] = [
      'faseContrato',
      'pagosRealizados',
      'valorFacturado',
      'porcentajeFacturado',
      'saldoPorPagar',
      'porcentajePorPagar'
    ];
    dataTable: any[] = [
      {
        faseContrato: 'Fase 1 - Preconstrucción',
        pagosRealizados: '0',
        valorFacturado: '0',
        porcentajeFacturado: '0',
        saldoPorPagar: '$30.000.000',
        porcentajePorPagar: '100%',
      }
    ];
    addressForm = this.fb.group({
      fechaSolicitud: [null, Validators.required],
      numeroRadicado: [null, Validators.required],
      faseContrato: [null, Validators.required]
    });
    fasesArray: Dominio[] = [];
    faseContrato: any = {};
    postConstruccion = '3';

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    { }

    ngOnInit(): void {
        this.commonSvc.listaFases()
            .subscribe(
                response => {
                    response.forEach( ( fase, index ) => {
                        if ( fase.codigo === this.postConstruccion ) {
                            response.splice( index, 1 );
                        }
                    } );
                    response.forEach( fase => {
                        if ( fase.codigo === '1' ) {
                            this.faseContrato.preConstruccion = fase.codigo;
                        }
                        if ( fase.codigo === '2' ) {
                            this.faseContrato.construccion = fase.codigo;
                        }
                    } );
                    this.fasesArray = response;

                    if ( this.contrato.solicitudPago.length > 0 ) {
                        this.solicitudPagoId = this.contrato.solicitudPago[0].solicitudPagoId;
                    }
                    if ( this.contrato.solicitudPagoOnly !== undefined ) {
                        this.solicitudPagoRegistrarSolicitudPago = this.contrato.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[0];
            
                        if ( this.solicitudPagoRegistrarSolicitudPago !== undefined ) {
                            this.solicitudPagoRegistrarSolicitudPagoId = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoRegistrarSolicitudPagoId;
                            let faseSeleccionada: Dominio;
                            this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];
                            if ( this.solicitudPagoFase !== undefined ) {
                                this.solicitudPagofaseId = this.solicitudPagoFase.solicitudPagoFaseId;
            
                                if ( this.solicitudPagoFase.esPreconstruccion === true ) {
                                    const fase = this.fasesArray.filter( fase => fase.codigo === this.faseContrato.preConstruccion );
                                    faseSeleccionada = fase[0];
                                }
                                if ( this.solicitudPagoFase.esPreconstruccion === false ) {
                                    const fase = this.fasesArray.filter( fase => fase.codigo === this.faseContrato.construccion );
                                    faseSeleccionada = fase[0];
                                }
                            }
                            this.addressForm.setValue(
                                {
                                    fechaSolicitud: this.solicitudPagoRegistrarSolicitudPago.fechaSolicitud !== undefined ? new Date( this.solicitudPagoRegistrarSolicitudPago.fechaSolicitud ) : null,
                                    numeroRadicado: this.solicitudPagoRegistrarSolicitudPago.numeroRadicadoSac !== undefined ? this.solicitudPagoRegistrarSolicitudPago.numeroRadicadoSac : null,
                                    faseContrato: faseSeleccionada !== undefined ? faseSeleccionada : null
                                }
                            );
                        }
                    }
                    // Tabla pendiente por integrar
                    this.dataSource = new MatTableDataSource(this.dataTable);
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                }
            );
    };

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    };

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    guardar() {
        let tieneFasePreConstruccion = false;
        let tieneFaseConstruccion = false;
        const faseSeleccionada: Dominio = this.addressForm.get( 'faseContrato' ).value;
        const pago = () => {
            if ( faseSeleccionada !== null ) {
                const fase: Dominio[] = this.fasesArray.filter( fase => fase.codigo === faseSeleccionada.codigo );
                if ( fase[0].codigo === this.faseContrato.preConstruccion ) {
                    return [
                        {
                            solicitudPagofaseId: this.solicitudPagofaseId,
                            solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId,
                            esPreconstruccion: true
                        }
                    ]
                }
                if ( fase[0].codigo === this.faseContrato.construccion ) {
                    return [
                        {
                            solicitudPagofaseId: this.solicitudPagofaseId,
                            solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId,
                            esPreconstruccion: false
                        }
                    ]
                }
            } else {
                return null;
            }
        };

        if ( faseSeleccionada !== null ) {
            const fase: Dominio[] = this.fasesArray.filter( fase => fase.codigo === faseSeleccionada.codigo );
            if ( fase[0].codigo === this.faseContrato.preConstruccion ) {
                tieneFasePreConstruccion = true;
            }
            if ( fase[0].codigo === this.faseContrato.construccion ) {
                tieneFaseConstruccion = true;
            }
        }
        const pSolicitudPago = {
            solicitudPagoId: this.solicitudPagoId,
            contratoId: this.contrato.contratoId,
            solicitudPagoRegistrarSolicitudPago: [
                {
                  solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId,
                  solicitudPagoId: this.solicitudPagoId,
                  tieneFasePreconstruccion: tieneFasePreConstruccion,
                  tieneFaseConstruccion: tieneFaseConstruccion,
                  fechaSolicitud: new Date( this.addressForm.get( 'fechaSolicitud' ).value ).toISOString(),
                  numeroRadicadoSAC: this.addressForm.get( 'numeroRadicado' ).value,
                  solicitudPagofase: pago()
                }
            ]
        }
        console.log( pSolicitudPago );
        this.registrarPagosSvc.createEditNewPayment( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/registrarValidarRequisitosPago/verDetalleEditar', this.contrato.contratoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
