import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TipoSolicitud, TipoSolicitudes } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-accordion-info-general-gog',
  templateUrl: './accordion-info-general-gog.component.html',
  styleUrls: ['./accordion-info-general-gog.component.scss']
})
export class AccordionInfoGeneralGogComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Output() tieneObservacion = new EventEmitter<boolean>();
    listaTipoSolicitud: TipoSolicitud = TipoSolicitudes;
    listaTipoSolicitudContrato: Dominio[] = [];
    valorTotalFactura = 0;
    solicitudPagoFase: any;
    ordenGiroTercero: any;
    semaforoInfoGeneral = 'sin-diligenciar';
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'drp',
      'numeroDrp',
      'valorSolicitudDdp',
      'saldoPresupuestal'
    ];

    constructor( private commonSvc: CommonService )
    {
        this.commonSvc.listaTipoSolicitudContrato()
            .subscribe( response => this.listaTipoSolicitudContrato = response );
    }

    ngOnInit(): void {
        this.getSolicitudPago();
    };

    getSolicitudPago() {
        if ( this.solicitudPago.tipoSolicitudCodigo !== this.listaTipoSolicitud.expensas && this.solicitudPago.tipoSolicitudCodigo !== this.listaTipoSolicitud.otrosCostos ) {
            this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];

            this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( criterio => this.valorTotalFactura += criterio.valorFacturado );
    
            // Get semaforo informacion general
            if ( this.solicitudPago.ordenGiro !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroTercero !== undefined ) {
                    if ( this.solicitudPago.ordenGiro.ordenGiroTercero.length > 0 ) {
                        this.ordenGiroTercero = this.solicitudPago.ordenGiro.ordenGiroTercero[0];
    
                        if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica !== undefined ) {
                            if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica.length > 0 ) {
                                const ordenGiroTerceroTransferenciaElectronica = this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica[0];
    
                                if ( ordenGiroTerceroTransferenciaElectronica.registroCompleto === false ) {
                                    this.semaforoInfoGeneral = 'en-proceso';
                                }
                                if ( ordenGiroTerceroTransferenciaElectronica.registroCompleto === true ) {
                                    this.semaforoInfoGeneral = 'completo';
                                }
                            }
                        }
    
                        if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia !== undefined ) {
                            if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia.length > 0 ) {
                                const ordenGiroTerceroChequeGerencia = this.ordenGiroTercero.ordenGiroTerceroChequeGerencia[0];
    
                                if ( ordenGiroTerceroChequeGerencia.registroCompleto === false ) {
                                    this.semaforoInfoGeneral = 'en-proceso';
                                }
                                if ( ordenGiroTerceroChequeGerencia.registroCompleto === true ) {
                                    this.semaforoInfoGeneral = 'completo';
                                }
                            }
                        }
                    }
                }
            }
    
            this.dataSource = new MatTableDataSource( this.solicitudPago.contratoSon.valorFacturadoContrato );
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
        }
    }

    getTipoSolicitudContrato( tipoSolicitudCodigo: string ) {
        if ( tipoSolicitudCodigo === this.listaTipoSolicitud.otrosCostos ) {
            return 'Otros costos y servicios';
        }

        if ( tipoSolicitudCodigo === this.listaTipoSolicitud.expensas ) {
            return 'Expensas';
        }

        if ( this.listaTipoSolicitudContrato.length > 0 ) {
            const solicitud = this.listaTipoSolicitudContrato.find( solicitud => solicitud.codigo === tipoSolicitudCodigo );
            
            if ( solicitud !== undefined ) {
                return solicitud.nombre;
            }
        }
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    };

}
