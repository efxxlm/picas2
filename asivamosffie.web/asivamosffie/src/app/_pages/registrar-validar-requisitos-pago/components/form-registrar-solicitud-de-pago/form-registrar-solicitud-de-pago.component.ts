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
      },
      {
        faseContrato: 'Fase 2 - Construcción',
        pagosRealizados: '0',
        valorFacturado: '0',
        porcentajeFacturado: '0',
        saldoPorPagar: '$75.000.000',
        porcentajePorPagar: '100%',
      }
    ];
    addressForm = this.fb.group({
      fechaSolicitud: [null, Validators.required],
      numeroRadicado: [null, Validators.required],
      faseContrato: [null, Validators.required]
    });
    fasesArray = [
      { name: 'Fase 1 - Preconstrucción', value: '1' },
      { name: 'Fase 2 - Construcción', value: '2' }
    ];
    solicitudPagoId = 0;
    obj1: boolean;
    obj2: boolean;

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    { }

    ngOnInit(): void {
        if ( this.contrato.solicitudPago.length > 0 ) {
            this.solicitudPagoId = this.contrato.solicitudPago[0].solicitudPagoId;
        }
        this.dataSource = new MatTableDataSource(this.dataTable);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    };

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    };

    getvalues(values: any[]) {
        console.log(values);
        const fase1Preconstruccion = values.find(value => value.value == "1");
        const fase2Construccion = values.find(value => value.value == "2");
        fase1Preconstruccion ? this.obj1 = true : this.obj1 = false;
        fase2Construccion ? this.obj2 = true : this.obj2 = false;
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    guardar() {
        const pSolicitudPago = {
            solicitudPagoId: this.solicitudPagoId,
            contratoId: this.contrato.contratoId,
            solicitudPagoRegistrarSolicitudPago: [
                {
                  solicitudPagoRegistrarSolicitudPagoId: 0,
                  solicitudPagoId: this.solicitudPagoId,
                  tieneFasePreconstruccion: this.obj1,
                  tieneFaseConstruccion: this.obj2,
                  fechaSolicitud: new Date( this.addressForm.get( 'fechaSolicitud' ).value ).toISOString(),
                  numeroRadicadoSAC: this.addressForm.get( 'numeroRadicado' ).value
                }
            ]
        }
        console.log( pSolicitudPago );
        this.registrarPagosSvc.createEditNewPayment( pSolicitudPago )
          .subscribe(
              response => {
                  this.openDialog( '', `<b>${ response.message }</b>` );
                  this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                      () =>   this.routes.navigate(
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
