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
      'valor',
      'saldo'
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

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private commonSvc: CommonService )
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

    getContratos() {
        if ( this.addressForm.get( 'searchContrato' ).value !== null ) {
            if ( this.addressForm.get( 'searchContrato' ).value.length > 0 ) {
                this.contratosArray = [];
                this.registrarPagosSvc.getContratos( this.addressForm.get( 'tipoSolicitud' ).value.codigo, this.addressForm.get( 'modalidadContrato' ).value.codigo, this.addressForm.get( 'searchContrato' ).value )
                    .subscribe( response => {
                      this.contratosArray = response;
                      console.log( response );
                    } );
            }
        }
    }

    seleccionAutocomplete( contrato: any ) {
      this.addressForm.get( 'contratoSeleccionado' ).setValue( contrato );
      this.registrarPagosSvc.getContratoByContratoId( contrato.contratoId )
        .subscribe(
            contrato => {
                this.contrato = contrato;
                console.log( this.contrato );
            }
        );
    }

    openProyectosAsociados() {
        const dialogRef = this.dialog.open( DialogProyectosAsociadosComponent, {
            width: '80em',
            data: { contrato: this.addressForm.get( 'contratoSeleccionado' ).value }
        });
    }

}
