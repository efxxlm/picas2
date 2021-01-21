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
    acordeones: any = {
        tieneFormaDePago: false,
        tieneRegistroSolicitudPago: false,
        tieneListaChequeo: false
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
        this.registrarPagosSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
            .subscribe(
                response => {
                    this.contrato = response;
                    console.log( this.contrato );
                    this.dataSource = new MatTableDataSource( this.contrato.contratacion.disponibilidadPresupuestal );
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                    // Get semaforo forma de pago
                    const solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
                    if ( solicitudPagoCargarFormaPago.registroCompleto === false ) {
                        this.semaforoFormaDePago = 'en-proceso';
                    }
                    if ( solicitudPagoCargarFormaPago.registroCompleto === true ) {
                        this.semaforoFormaDePago = 'completo';
                        this.acordeones.tieneFormaDePago = true;
                    }
                }
            );
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
              }
            );
    }

    ngOnInit(): void {
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
            return 'sin-diligenciar';
        }
        // Acordeon lista de chequeo
        if ( nombreAcordeon === 'listaChequeo' && tieneRegistroAnterior === false ) {
            return 'en-alerta';
        }
        if ( nombreAcordeon === 'listaChequeo' && tieneRegistroAnterior === true ) {
            return 'sin-diligenciar';
        }
        // Acordeon soporte de la solicitud
        if ( nombreAcordeon === 'soporteSolicitud' && tieneRegistroAnterior === false ) {
            return 'en-alerta';
        }
        if ( nombreAcordeon === 'soporteSolicitud' && tieneRegistroAnterior === true ) {
            return 'sin-diligenciar';
        }
    }

}
