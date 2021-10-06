import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { DialogProyectosAsociadosComponent } from '../dialog-proyectos-asociados/dialog-proyectos-asociados.component';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-verdetalle-solicitud-pago',
  templateUrl: './verdetalle-solicitud-pago.component.html',
  styleUrls: ['./verdetalle-solicitud-pago.component.scss']
})
export class VerdetalleSolicitudPagoComponent implements OnInit {

    listaEstadoSolicitudPago: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    dataSource = new MatTableDataSource();
    esVerDetalle = true;
    dataSourceRegistrarSolicitud = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
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
    addressForm = this.fb.group({
        criterios: this.fb.array( [] )
    });
    montoMaximoPendiente: { montoMaximo: number, valorPendientePorPagar: number };
    listaCriterios: Dominio[] = [];
    formaPagoArray: Dominio[] = [];
    modalidadContratoArray: Dominio[] = [];
    criteriosArray: { codigo: string, nombre: string }[] = [];
    criteriosArraySeleccionados: Dominio[] = [];
    tienePreconstruccion = false;
    tieneConstruccion = false;
    tipoSolicitudCodigo: any = {};
    contrato: any;
    solicitudPagoCargarFormaPago: any;
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoFase: any;
    manejoAnticipoRequiere: boolean;
    idSolicitud: any;
    

    get criterios() {
        return this.addressForm.get( 'criterios' ) as FormArray;
    }

    getConceptos( index: number ) {
        return this.criterios.controls[ index ].get( 'conceptos' ) as FormArray;
    }

    constructor(
        private activatedRoute: ActivatedRoute,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private commonSvc: CommonService )
    {
        this.commonSvc.formasDePago()
            .subscribe( response => this.formaPagoArray = response );
        this.getContrato();
    }

    ngOnInit(): void {
        this.idSolicitud = this.activatedRoute.snapshot.params.idSolicitud
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

    getContrato() {
        this.registrarPagosSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitud )
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

                            this.manejoAnticipoRequiere = this.contrato.contratoConstruccion.length > 0 ? this.contrato.contratoConstruccion[0].manejoAnticipoRequiere : false;
                            if ( this.contrato.solicitudPagoOnly.tipoSolicitudCodigo !== this.tipoSolicitudCodigo.otrosCostos ) {
                                console.log( this.contrato.tablaDRP );
                                if ( this.contrato.solicitudPago.length > 1 ) {
                                    this.solicitudPagoCargarFormaPago = this.contrato.solicitudPago[0].solicitudPagoCargarFormaPago[0];
                                } else {
                                    this.solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
                                }
                                this.solicitudPagoRegistrarSolicitudPago = this.contrato.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[0];
                                this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];

                                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
                                    if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                                        for ( const solicitudPagoFase of this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase ) {
                                            if ( solicitudPagoFase.esPreconstruccion === true ) {
                                                this.tienePreconstruccion = true;
                                            }
    
                                            if ( solicitudPagoFase.esPreconstruccion === false ) {
                                                this.tieneConstruccion = true;
                                            }
                                        }
                                    }
                                }
    
                                this.dataSource = new MatTableDataSource( this.contrato.tablaDRP );
                                this.dataSourceRegistrarSolicitud = new MatTableDataSource( this.contrato.vContratoPagosRealizados );
                            }
                            // console.log( this.contrato );
                          }
                        );
                }
            );
    }

    getFormaPago( formaPagoCodigo: string ) {
        if ( this.formaPagoArray.length > 0 ) {
            const forma = this.formaPagoArray.filter( forma => forma.codigo === formaPagoCodigo );
            return forma[0].nombre;
        }
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.modalidadContratoArray.length > 0 ) {
            const modalidad = this.modalidadContratoArray.filter( modalidad => modalidad.codigo === modalidadCodigo );
            return modalidad[0].nombre;
        }
    }

    filterCriterios( tipoCriterioCodigo: string ) {
        if ( this.listaCriterios.length > 0 ) {
            const criterio = this.listaCriterios.filter( criterio => criterio.codigo === tipoCriterioCodigo );
            return criterio[0].nombre;
        }
    }

}
