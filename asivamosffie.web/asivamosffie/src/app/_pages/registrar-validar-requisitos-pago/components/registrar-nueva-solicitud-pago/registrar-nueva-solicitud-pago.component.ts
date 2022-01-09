import { Router } from '@angular/router';
import { RegistrarRequisitosPagoService } from './../../../../core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { CommonService } from './../../../../core/_services/common/common.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DialogProyectosAsociadosComponent } from '../dialog-proyectos-asociados/dialog-proyectos-asociados.component';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatAutocomplete, MatAutocompleteTrigger } from '@angular/material/autocomplete';

@Component({
  selector: 'app-registrar-nueva-solicitud-pago',
  templateUrl: './registrar-nueva-solicitud-pago.component.html',
  styleUrls: ['./registrar-nueva-solicitud-pago.component.scss']
})
export class RegistrarNuevaSolicitudPagoComponent implements OnInit {

    dataSource = new MatTableDataSource();
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
    addressForm = this.fb.group({
      tipoSolicitud: [null, Validators.required],
      modalidadContrato: [null, Validators.required],
      numeroContrato: [null, Validators.required],
      searchContrato: [ null, Validators.required ],
      contratoSeleccionado: [ null ]
    });
    tiposSolicitudArray: Dominio[] = [];
    tipoSolicitudCodigo: any = {};
    modalidadContratoArray: Dominio[] = [];
    contratosArray: any[] = [];
    contrato: any;
    estaEditando = false;
    solicitudPagoId = 0;

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private commonSvc: CommonService,
        private routes: Router )
    {
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
                this.tiposSolicitudArray = solicitudes;
              }
            );
        this.commonSvc.modalidadesContrato()
            .subscribe( response => this.modalidadContratoArray = response );
    }

    ngOnInit(): void {
    };

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    };

    getContratos( trigger: MatAutocompleteTrigger ) {
        if ( this.addressForm.get( 'searchContrato' ).value !== null ) {
            if ( this.addressForm.get( 'searchContrato' ).value.length > 0 ) {
                this.contratosArray = [];
                this.registrarPagosSvc.getContratos( this.addressForm.get( 'tipoSolicitud' ).value.codigo, this.addressForm.get( 'modalidadContrato' ).value.codigo, this.addressForm.get( 'searchContrato' ).value )
                    .subscribe( response => {
                        this.contratosArray = response;
                        console.log( response );
                        if ( response.length === 0 ) {
                            this.openDialog( '', '<b>No se encontraron contratos relacionados.</b>' );
                            this.addressForm.get( 'searchContrato' ).setValue( null );
                        } else {
                            trigger.openPanel();
                        }
                    } );
            }
        }
    }

    seleccionAutocomplete( contrato: any ) {
      this.addressForm.get( 'contratoSeleccionado' ).setValue( contrato );
      this.registrarPagosSvc.getContratoByContratoId( contrato.contratoId, 0 , true)
        .subscribe(
            contrato => {
                this.contrato = contrato;
                console.log( this.contrato );
                this.dataSource = new MatTableDataSource( this.contrato.tablaDRP );
                this.dataSource.paginator = this.paginator;
                this.dataSource.sort = this.sort;
            }
        );
    }

    openProyectosAsociados() {
        const dialogRef = this.dialog.open( DialogProyectosAsociadosComponent, {
            width: '80em',
            data: { contrato: this.addressForm.get( 'contratoSeleccionado' ).value }
        });
    }

    getTipoSolicitud( tipoSolicitud: Dominio ) {
        if ( tipoSolicitud.codigo === this.tipoSolicitudCodigo.contratoObra || this.tipoSolicitudCodigo.contratoInterventoria ) {

            if ( this.contrato ) {
                this.addressForm.get( 'modalidadContrato' ).setValue( null )
                this.addressForm.get( 'numeroContrato' ).setValue( null )
                this.addressForm.get( 'searchContrato' ).setValue( null )
                this.addressForm.get( 'contratoSeleccionado' ).setValue( null )
                this.contrato = undefined;
            }

        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        const pSolicitudPago = {
            esFactura: this.contrato && this.contrato.solicitudPagoOnly && this.contrato.solicitudPagoOnly.esFactura ? this.contrato.solicitudPagoOnly.esFactura : null,
            solicitudPagoId: this.solicitudPagoId,
            tipoSolicitudCodigo: this.addressForm.get( 'tipoSolicitud' ).value.codigo,
            contratoId: this.contrato.contratoId
        }
        this.registrarPagosSvc.createEditNewPayment(pSolicitudPago)
            .subscribe(
                response => {
                    this.openDialog('', `<b>${response.message}</b>`);

                    this.routes.navigateByUrl('/', { skipLocationChange: true }).then(
                        () => this.routes.navigate( [ '/registrarValidarRequisitosPago/verDetalleEditar', response.data.contratoId, response.data.solicitudPagoId ] )
                    );
                },
                err => this.openDialog('', `<b>${err.message}</b>`)
            );
    }

}
